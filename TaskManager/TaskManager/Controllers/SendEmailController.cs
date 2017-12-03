using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Context;


namespace TaskManager.Controllers
{
    public class SendEmailController : Controller
    {

        public EFContext Context { get; set; }

        public SendEmailController(EFContext context)
        {
            Context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userName = User.Identity.Name;
            var user = Context.Users.Include(x => x.Tasks).Single(x => x.UserName == userName);
            string to = user.Email.ToString();
            //subject = user.Tasks.Single(x => x.Title == subject).ToString();
            var task = user.Tasks.First(); //Do zmiany
            string body = $"Remember about task: {task.Content}, Start date:";
                try
                {
                    MailMessage msg = new MailMessage();
                    msg.From = new MailAddress("task_manager@wp.com");
                    msg.To.Add(new MailAddress(to));
                    msg.Subject = task.Title;
                    msg.SubjectEncoding = System.Text.Encoding.UTF8;
                    msg.Body = body;
                    msg.BodyEncoding = System.Text.Encoding.UTF8;
                    msg.Priority = MailPriority.Normal;
                    msg.IsBodyHtml = true;



                    SmtpClient smtp = new SmtpClient("smtp.wp.pl", 465);
                    

                    
                    smtp.Credentials = new NetworkCredential("task_manager@wp.com", "Taskmanager");
                    
                    smtp.Send(msg);
                    return Content("wysłano");
                }
                catch (Exception ex)
                {
                    return Content("nie udało się");
                }

            return View();
        }
    }
}
