using Carts.Data;
using Carts.Data.POCO;
using Carts.Helpers.Interface;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Carts.Helpers
{
    public interface IHangfire
    {
        void ConfigureHangfire(IServiceCollection services, IConfiguration configuration);
        Task Configure();
    }
    public class Hangfire : IHangfire
    {
        private static int LoginDateEpilson = 21;
        private int CartDays;
        private int CheckoutDay;
        private IEmailSender _emailSender;
        private IUnitOfWork _unitOfWork;

        public Hangfire(IEmailSender emailSender,IUnitOfWork unitOfWork)
        {
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            
        }

        //internal EmailSender(IDbContext dbContext, IEmailService emailService)
        //{
        //    _dbContext = dbContext;
        //    _emailService = emailService;
        //}

        public void ConfigureHangfire(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));
        }

        public async Task Configure()
        {
            //_app.UseHangfireServer();
            //_app.UseHangfireDashboard("/hangfire");

            RecurringJob.AddOrUpdate( () =>  InitializeMailJobs(), Cron.Hourly);
            RecurringJob.AddOrUpdate(() => InitializeCheckoutMailJobs(), Cron.Hourly);
        }

        public async Task SendHangFireEmail(UsersTb user,string Subject, string Msg,string MailReference,string OtherReference)
        {
            await _emailSender.SendEmailAsync(user.Email, Subject, Msg);
            _unitOfWork.MailLog.Add(new MailLogTb { CreatedFor=user,MailReference= MailReference , OtherReference = OtherReference });
        }
        
        public async Task InitializeMailJobs()
        {
            CartDays = Convert.ToInt32(Startup.StaticConfig["XDays"]);
            var mailingUsersForCarts = (await _unitOfWork.Order.GetOrderByUser())
                                         .Where(x=>(DateTime.Now.Date - x.DateCreated.Date).TotalDays >= CartDays
                                                    && (DateTime.Now.Date - x.CreatedBy.LastLoginDate.Value.Date).TotalDays < LoginDateEpilson
                                                )
                                        //.Select(x=>x.CreatedBy)
                                        .ToList();
            foreach(var cart in mailingUsersForCarts)
            {
                var msg = $@"<b>{cart?.CreatedBy?.FullName}</b> kindly login at <b>Carts</b> to complete your cart items purchase.";
                var mailRef =  $"Ord{cart?.OrderReference}{cart?.CreatedBy?.UserId.ToString()}" ;
                var user = cart?.CreatedBy;
                BackgroundJob.Enqueue(() => SendHangFireEmail(user, "Carts Reminder", msg, mailRef, cart.OrderId.ToString()));
                //await _emailSender.SendEmailAsync(user.Email, "Carts Reminder", msg);
            }
        }

        public async Task InitializeCheckoutMailJobs()
        {
            CheckoutDay = Convert.ToInt32(Startup.StaticConfig["YDays"]);
            var mailingUsersForCarts = (await _unitOfWork.Order.GetOrderByUser())
                                        .Where(x => (DateTime.Now.Date - x.DateCreated.Date).TotalDays >= CartDays
                                                    && (DateTime.Now.Date-x.CreatedBy.LastLoginDate.Value.Date).TotalDays<LoginDateEpilson
                                                )
                                        //.Select(x => x.CreatedBy)
                                        .ToList();
            foreach (var cart in mailingUsersForCarts)
            {
                var msg = $@"<b>{cart?.CreatedBy?.FullName}</b> kindly login at <b>Carts</b> to complete your checkout.";
                var mailRef = $"Inv{cart?.CreatedBy?.UserId.ToString()}";
                var user = cart?.CreatedBy;
                //BackgroundJob.Enqueue(() => SendHangFireEmail(user, "Carts Reminder", msg, mailRef));
                BackgroundJob.Enqueue(() => SendHangFireEmail(user, "Checkout Reminder", msg, mailRef, cart.InvoiceTb.InvoiceId.ToString()));
                //await _emailSender.SendEmailAsync(user.Email, "Checkout Reminder", msg);
            }
        }
        
    }
}
