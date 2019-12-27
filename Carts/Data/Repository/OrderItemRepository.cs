using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Carts.Data.POCO;
using Carts.Data.Repository.Interface;

namespace Carts.Data.Repository
{
    public class OrderItemRepository : Repository<OrderItemTb>, IOrderItemRepository
    {
        public OrderItemRepository(DbContext context, ILogger<dynamic> log) : base(context, log)
        {
        }

        private AirTableContext _appContext => (AirTableContext)_context;

        private ILogger<dynamic> logger => (ILogger<dynamic>)_log;


    }
}
