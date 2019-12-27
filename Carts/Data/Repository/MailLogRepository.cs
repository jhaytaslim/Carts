using Carts.Data.POCO;
using Carts.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Carts.Data.Repository
{
    public class MailLogRepository : Repository<MailLogTb>, IMailLogRepository
    {
        public MailLogRepository(DbContext context, ILogger<dynamic> log) : base(context, log)
        {
        }

        private AirTableContext _appContext => (AirTableContext)_context;

        private ILogger<dynamic> logger => (ILogger<dynamic>)_log;
    }
}
