using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;

[assembly: OwinStartupAttribute(typeof(Helpdesk.Startup))]
namespace Helpdesk
{
    

    public partial class Startup
    {
        //private IScheduler _quartsScheduler;
        public void Configuration(IAppBuilder app )
        {
            ConfigureAuth(app);
            //_quartsScheduler = ConfigureQuartz();
            
        }


            //}

            //public IScheduler ConfigureQuartz()
            //{
            //    NameValueCollection props = new NameValueCollection
            //{
            //    {"quartz.serializer.type","binary" },
            //};
            //    StdSchedulerFactory factory = new StdSchedulerFactory(props);
            //    var scheduler = factory.GetScheduler().Result;
            //    scheduler.Start().Wait();
            //    return scheduler;
            //}
        }

    
}
