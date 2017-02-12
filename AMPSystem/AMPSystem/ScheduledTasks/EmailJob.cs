using System.Net;
using System.Net.Mail;
using AMPSystem.DAL;
using Quartz;

namespace AMPSchedules.ScheduledTasks
{
    public class EmailJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            using (var message = new MailMessage("2054313@student.uma.pt", dataMap.GetString("Email")))
            {
                message.Subject = "Lembrete Universidade da Madeira";
                message.Body = "Relembramos que possui o seguinte evento académico: " + dataMap.GetString("Name") +
                               " entre as " + dataMap.GetString("StartTime") + " e as " + dataMap.GetString("EndTime") + ".";

                if (dataMap.GetString("Reminder") != "")
                {
                    message.Body += " Não se esqueça de: " + dataMap.GetString("Reminder");
                }
                using (var client = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.office365.com",
                    Port = 587,
                    Credentials = new NetworkCredential(EMAIL, PASSWORD)
                })
                {
                    client.Send(message);
                }
            }
            var mAlert = DbManager.Instance.ReturnAlert(dataMap.GetIntValue("Id"));
            DbManager.Instance.RemoveAlert(mAlert);
        }
    }
}
