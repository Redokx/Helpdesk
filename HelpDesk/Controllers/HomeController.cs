using Helpdesk.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Helpdesk.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly HelpdeskContext db = new HelpdeskContext();

        public ActionResult Index()
        {
            try
            {
                //pobierz id zalogowanego użytkownika
                string UserId = User.Identity.GetUserId();
                //znajdź login użytkownika po jego id
                var FindUser = db.Uzytkownicy.Find(UserId);
                if (FindUser != null)
                {
                    //
                    var Name = FindUser.PelneImie;
                }

                var daneNowe = (from c in db.Zgloszenia
                                where c.IdStatusu == 1
                                select c).Count();

                var daneOtwarte = (from c in db.Zgloszenia
                                   where c.IdStatusu == 2
                                   select c).Count();

                var daneOczekujace = (from c in db.Zgloszenia
                                      where c.IdStatusu == 3
                                      select c).Count();

                var daneWstrzymane = (from c in db.Zgloszenia
                                      where c.IdStatusu == 4
                                      select c).Count();

                var daneZakonczone = (from c in db.Zgloszenia
                                      where c.IdStatusu == 5
                                      select c).Count();
            
                ViewBag.repNowe = daneNowe;
                ViewBag.repOtwarte = daneOtwarte;
                ViewBag.repOczekujace = daneOczekujace;
                ViewBag.repWstrzymane = daneWstrzymane;
                ViewBag.repZakonczone = daneZakonczone;

                var UserPrzypisane = 0;
                var UserNowe = 0;
                var UserOtwarte = 0;
                var UserOczekujace = 0;
                var UserWstrzymane = 0;
                var UserZakonczone = 0;
                var UserAnulowane = 0;
                if (User.IsInRole("Autor"))
                {
                    //pobranie ilości zgłoszeń dla użytkowników
                    //Ilość zgłoszeń użytkownika
                     UserPrzypisane = (from c in db.Zgloszenia
                                          where c.UzytkownikId == FindUser.Id
                                          select c).Count();   
                    
                    //Ilość nowcyh zgłoszeń
                    UserNowe = (from c in db.Zgloszenia
                                       where (c.UzytkownikId == FindUser.Id && c.IdStatusu == 1)
                                       select c).Count();
                    //Ilość otwartych zgłoszeń
                     UserOtwarte = (from c in db.Zgloszenia
                                       where (c.UzytkownikId == FindUser.Id && c.IdStatusu == 2)
                                       select c).Count();
                    //Ilość oczekujących zgłoszeń
                     UserOczekujace = (from c in db.Zgloszenia
                                       where ( c.UzytkownikId == FindUser.Id && c.IdStatusu == 3)
                                       select c).Count();
                    //Ilość wstrzymanych zgłoszeń
                     UserWstrzymane = (from c in db.Zgloszenia
                                          where (c.UzytkownikId == FindUser.Id && c.IdStatusu == 4)
                                          select c).Count();
                    //Ilość zakończonych zgłoszeń
                     UserZakonczone = (from c in db.Zgloszenia
                                          where (c.UzytkownikId == FindUser.Id && c.IdStatusu == 5)
                                          select c).Count();
                    //Ilość anulowanych zgłoszeń
                     UserAnulowane = (from c in db.Zgloszenia
                                          where (c.UzytkownikId == FindUser.Id && c.IdStatusu == 6)
                                          select c).Count();


                }
                else
                {
                    //pobranie ilości zgłoszeń dla pracowników
                     UserPrzypisane = (from c in db.Zgloszenia
                                          where c.PracownikId == FindUser.Id
                                          select c).Count();                   
                     UserNowe = (from c in db.Zgloszenia
                                    where (c.PracownikId == FindUser.Id && c.IdStatusu == 1)
                                    select c).Count();                    
                     UserOtwarte = (from c in db.Zgloszenia
                                       where (c.PracownikId == FindUser.Id && c.IdStatusu == 2)
                                       select c).Count();          
                     UserOczekujace = (from c in db.Zgloszenia
                                       where (c.PracownikId == FindUser.Id && c.IdStatusu == 3)
                                       select c).Count();
                     UserWstrzymane = (from c in db.Zgloszenia
                                          where (c.PracownikId == FindUser.Id && c.IdStatusu == 4)
                                          select c).Count();         
                     UserZakonczone = (from c in db.Zgloszenia
                                          where (c.PracownikId == FindUser.Id && c.IdStatusu == 5)
                                          select c).Count();
                }
                
                ViewBag.UserPrzypisane = UserPrzypisane;
                ViewBag.UserNowe = UserNowe;
                if (User.IsInRole("Autor"))
                {
                    ViewBag.UserOtwarte = UserOtwarte + UserOczekujace;
                }
                else
                {
                    ViewBag.UserOtwarte = UserOtwarte;
                }    
                ViewBag.UserOczekujace = UserOczekujace;
                ViewBag.UserWstrzymane = UserWstrzymane;
                ViewBag.UserZakonczone = UserZakonczone;
                ViewBag.UserAnulowane = UserAnulowane;

                var TopUser = (from c in db.Uzytkownicy
                               from name in db.Roles
                                   //from i in db.Zgloszenia where i.IdStatusu == 5 
                               join role in db.Roles
                             on name.Id equals role.Id
                               where name.Name == "Pracownik"

                               select new
                               {
                                   c.Nazwisko,
                                   c.Imie
                               }).ToList();

                ViewBag.TopUser = TopUser;

            }
            catch
            {

            }


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Na temat firmy.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Dane kontaktowe.";

            return View();
        }

        //test raportowania
        //public void GetZgloszenieRaport()
        //{
        //    ParametryReportu objParametryReportu = new ParametryReportu();
        //    var data = GetZgloszenieInfo();
        //    objParametryReportu.DataSource = data.Tables[0];
        //    objParametryReportu.RaportTemat = "Raport zgłoszeń pracowników";
        //    objParametryReportu.RaportNazwa = "Zgloszenia.rdlc";
        //    objParametryReportu.RaportTyp = "ZgloszenieRaport";
        //    objParametryReportu.DataSetNazwa = "DataSet1";
        //    this.HttpContext.Session["ParametryReportu"] = objParametryReportu;
        //}

        //public DataSet GetZgloszenieInfo()
        //{
        //    string constr = ConfigurationManager.ConnectionStrings["HelpdeskConnection"].ConnectionString;
        //    DataSet ds = new DataSet();
        //    string sql = "Select * from Zgloszenia";
        //    SqlConnection con = new SqlConnection(constr);
        //    SqlCommand cmd = new SqlCommand(sql, con);
        //    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
        //    adpt.Fill(ds);
        //    return ds;
        //}


        public ActionResult Email(Zgloszenie zgloszenie)
        {
            string strEmail = ConfigurationManager.AppSettings["Email"];
            string strPassword = ConfigurationManager.AppSettings["EmailPassword"];
            Pop3Client pop3Client = new Pop3Client();
            pop3Client.Connect("pop3-insertEmailAddress.pl", 995, true); //SSL is true or false
            pop3Client.Authenticate(strEmail, strPassword);
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


            foreach(var item in newMessages)
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
                try
                {
                    db.Zgloszenia.Add(email);
                    db.SaveChanges();
                    ListaDodanych.Add("Zgłoszenie id: " + email.IdZgloszenia + " zostało dodane. Temat = " + email.Temat);
                }
                catch(Exception e)
                {

                    ListaDodanych.Add("Błąd przy dodawaniu zgłoszenia; " +e);
                    TempData["Potwierdzenie"] = ListaDodanych;
                    return View();
                }
                


            }


            TempData["Potwierdzenie"] = ListaDodanych;
            //return RedirectToAction("Index", "ZgloszenieController");
            return View();
        }

    }
    }