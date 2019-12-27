using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Carts.Data.POCO;
using Carts.Data.Repository.Interface;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Carts.Data.Repository
{
    public class InvoiceRepository : Repository<InvoiceTb>, IInvoiceRepository
    {
        public InvoiceRepository(DbContext context, ILogger<dynamic> log) : base(context, log)
        {
        }

        private AirTableContext _appContext => (AirTableContext)_context;

        private ILogger<dynamic> logger => (ILogger<dynamic>)_log;

        public async Task<InvoiceTb> CheckoutAsync(UsersTb invoiceUser,string reference)
        {
            try
            {
                var referencedOrder = await _appContext.OrderTb
                                                        .Include(o=>o.CreatedBy)
                                                        .Include(o => o.OrderItemTb)
                                                        .FirstOrDefaultAsync(o => o.OrderReference.ToUpper() == reference.ToUpper() );
                if (referencedOrder == null)
                {
                    throw new InvalidOperationException("Order not Found.");
                }
                else if (referencedOrder != null && referencedOrder.CreatedBy == invoiceUser)
                {
                    throw new InvalidOperationException("Only the owner(creator) of the order can execute this action.");
                }

                var createdInvoice = await base.AddAsync(new InvoiceTb
                {
                    CreatedBy = referencedOrder.CreatedBy,
                    Order = referencedOrder,
                    TotalOrderCost =Convert.ToDecimal( referencedOrder.OrderItemTb.Sum(i=>i.PriceSold)),
                    DateCreated=DateTime.Now,
                });

                if (createdInvoice != null)
                {
                    return createdInvoice;
                }
            }
            catch(Exception ex)
            {

                _log.LogInformation(ex.Message+ex.InnerException);
            }
            return null;
        }

    }
}
