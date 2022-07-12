using Quartz;
using System;
using System.Configuration;
using System.Threading.Tasks;
namespace Helpdesk.Models
{
    public class ExecuteTaskServiceCallJob : IJob
    {
        public static readonly string SchedulingStatus = ConfigurationManager.AppSettings["ExecuteTaskServiceCallSchedulingStatus"];
        public Task Execute(IJobExecutionContext context)
        {
            var task = Task.Run(() =>
            {
                if (SchedulingStatus.Equals("ON"))
                {
                    try
                    {
                        //Do whatever stuff you want
                        Controllers.ImportEmail zgranieEmaili = new Controllers.ImportEmail();
                        zgranieEmaili.Import();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            });
            return task;
        }
    }
}