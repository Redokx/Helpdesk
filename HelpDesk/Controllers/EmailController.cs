using Microsoft.AspNet.Identity;
using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Helpdesk.Controllers
{
    public class EmailController
    {
        //przesyłanie powiadomień
        public void SendEmail(string Email,string Body,string Subject)
        {
            string strEmail = ConfigurationManager.AppSettings["Email"];
            string strPassword = ConfigurationManager.AppSettings["EmailPassword"];
            SmtpClient client = new SmtpClient("smtp-insertEmailAddress.pl", 587)
            {
                EnableSsl = true
            };

        MailAddress from = new MailAddress("pomoc@insertEmailAddress.pl", "pomoc@insertEmailAddress.pl");
        MailAddress to = new MailAddress(Email, Email);
        MailMessage message = new MailMessage(from, to)
        {
            Body = Body,
            Subject = Subject
        };

        message.IsBodyHtml = true;
        NetworkCredential myCreds = new NetworkCredential(strEmail, strPassword);
        client.Credentials = myCreds;
         client.Send(message);
        }

        
    }

}