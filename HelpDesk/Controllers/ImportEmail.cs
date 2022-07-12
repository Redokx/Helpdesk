 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helpdesk.Models;
using OpenPop.Mime;
using OpenPop.Pop3;

namespace Helpdesk.Controllers
{
     
    public class ImportEmail
    {
        private readonly HelpdeskContext db = new HelpdeskContext();
       
        public void Import( )
        {
            
            Pop3Client pop3Client = new Pop3Client();
            pop3Client.Connect("pop3-insertEmailAddress.pl", 995, true); //SSL is true or false
            pop3Client.Authenticate("pomoc@insertEmailAddress.pl", "insertEmailAddressPassword");
            int count = pop3Client.GetMessageCount(); //total count of email in MessageBox
            var Emails = new List<Zgloszenie>(); //POPEmail type

            // Fetch all the current uids seen
            //create list from db.emailuids
            var SeenUid = db.EmailsUids.Select(x => x.Uids).ToList();


            List<string> uids = pop3Client.GetMessageUids();
            //foreach (var item in uids)
            //{
            //    EmailUids emailuids = new EmailUids();
            //    emailuids.Uids = item;
            //    db.EmailsUids.Add(emailuids);
            //    db.SaveChanges();
            //}


            // Create a list we can return with all new messages
            List<Message> newMessages = new List<Message>();

            //lista dodawanych zgłoszeń
            List<string> ListaDodanych = new List<string>();

            // All the new messages not seen by the POP3 client
            for (int i = 0; i < uids.Count(); i++)
            {
                string currentUidOnServer = uids[i];
                if (!SeenUid.Contains(currentUidOnServer))
                {
                    // We have not seen this message before.
                    // Download it and add this new uid to seen uids

                    // the uids list is in messageNumber order - meaning that the first
                    // uid in the list has messageNumber of 1, and the second has 
                    // messageNumber 2. Therefore we can fetch the message using
                    // i + 1 since messageNumber should be in range [1, messageCount]
                    Message unseenMessage = pop3Client.GetMessage(i + 1);

                    // Add the message to the new messages
                    newMessages.Add(unseenMessage);

                    // Add the uid to the seen uids, as it has now been seen
                    SeenUid.Add(currentUidOnServer);
                    EmailUids emailuids = new EmailUids();
                    emailuids.Uids = currentUidOnServer;
                    db.EmailsUids.Add(emailuids);
                    db.SaveChanges();
                }
            }


            foreach (var item in newMessages)
            {
                Zgloszenie email = new Zgloszenie()
                {

                    Temat = item.Headers.Subject,
                    DataDodania = item.Headers.DateSent,
                    DataAktualizacji = item.Headers.DateSent,
                    DataZakonczenia = item.Headers.DateSent.AddDays(3),
                    IdStatusu = 1,
                    IdKategorii = 1,
                    IdPriorytet = 1,
                    //test z wybranym uzytkownikiem
                    UzytkownikId = "024bfb88-c893-4a18-9488-817787713eed",
                    //Uzytkowni = string.Format("<a href = 'mailto:{1}'>{0}</a>",
                    //   message.Headers.From.DisplayName, message.Headers.From.Address),
                };
                MessagePart body = item.FindFirstHtmlVersion();

                if (body != null)
                {

                    //ograniczenie do 2000 znaków z modelu
                    string trescEmaila = body.GetBodyAsText();
                    trescEmaila = trescEmaila.Length > 1000 ? trescEmaila.Substring(0, 2000) : trescEmaila;
                    email.Tresc = trescEmaila;
                }
                else
                {
                    body = item.FindFirstPlainTextVersion();
                    if (body != null)
                    {
                        //ograniczenie do 2000 znaków z modelu
                        string trescEmaila = body.GetBodyAsText();
                        trescEmaila = trescEmaila.Length > 1000 ? trescEmaila.Substring(0, 2000) : trescEmaila;
                        email.Tresc = trescEmaila;

                    }
                }

                db.Zgloszenia.Add(email);
                db.SaveChanges();
                ListaDodanych.Add("Zgłoszenie id: " + email.IdZgloszenia + " zostało dodane. Temat = " + email.Temat);

                 
            }

        }
    }
}
    
