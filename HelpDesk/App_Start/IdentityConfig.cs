using Helpdesk.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Helpdesk
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            //// Plug in your email service here to send an email.
            //SmtpClient client = new SmtpClient();
            //return client.SendMailAsync("HelpdeskU67@gmail.com",
            //                            message.Destination,
            //                            message.Subject,
            //                            message.Body);


            SmtpClient client = new SmtpClient();
            NetworkCredential myCreds = new NetworkCredential("HelpdeskU67@gmail.com", "123456789Q!", "");
            client.Credentials = myCreds;

            //var msg = new MailMessage();
            //msg.Subject = message.Subject;
            //msg.Body = message.Body;
            //msg.IsBodyHtml = true;
            //msg.To.Add(message.Destination);


            //return client.SendMailAsync("Helpdesk",
            //                            message.Destination,
            //                            message.Subject,
            //                            message.Body);



            MailMessage msg = new MailMessage();
            msg.IsBodyHtml = true;
            msg.To.Add(message.Destination);
            msg.Body = message.Body;
            msg.Subject = message.Subject;
            // Plug in your email service here to send an email.

            return client.SendMailAsync(msg);
             







        }

    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class UzytkownikManager : UserManager<Uzytkownik>
    {
        public UzytkownikManager(IUserStore<Uzytkownik> store)
            : base(store)
        {
        }

        public static UzytkownikManager Create(IdentityFactoryOptions<UzytkownikManager> options, IOwinContext context) 
        {
            var manager = new UzytkownikManager(new UserStore<Uzytkownik>(context.Get<HelpdeskContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<Uzytkownik>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<Uzytkownik>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<Uzytkownik>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<Uzytkownik>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<Uzytkownik, string>
    {
        public ApplicationSignInManager(UzytkownikManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(Uzytkownik user)
        {
            return user.GenerateUserIdentityAsync((UzytkownikManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<UzytkownikManager>(), context.Authentication);
        }
    }
}
