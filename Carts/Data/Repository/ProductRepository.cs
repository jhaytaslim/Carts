using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Carts.Data.POCO;
using Carts.Data.Repository.Interface;
using System.Threading.Tasks;
using System;

namespace Carts.Data.Repository
{
    public class ProductRepository : Repository<ProductTb>, IProductRepository
    {
        public ProductRepository(DbContext context, ILogger<dynamic> log) : base(context, log)
        {
        }

        private AirTableContext _appContext => (AirTableContext)_context;

        private ILogger<dynamic> logger => (ILogger<dynamic>)_log;

        public async Task<OrderTb> Get(string orderId)
        {
            try
            {
                return await _appContext.OrderTb
                    .Include(o => o.CreatedBy).Include(o => o.Customer).Include(o => o.InvoiceTb).Include(o => o.Customer)
                    .FirstOrDefaultAsync(o => o.OrderId.ToString().ToUpper() == orderId.ToUpper() && o.InvoiceTb == null);
            }
            catch (Exception ex)
            {
                _log.LogDebug(ex.Message + ex.StackTrace);
                return null;
            }
        }

    }
}
