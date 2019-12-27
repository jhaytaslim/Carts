using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Carts.Data.POCO;
using Carts.Data.Repository.Interface;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Carts.Data.Repository
{
    public class OrderRepository : Repository<OrderTb>, IOrderRepository
    {
        public OrderRepository(DbContext context, ILogger<dynamic> log) : base(context, log)
        {
        }

        private AirTableContext _appContext => (AirTableContext)_context;

        private ILogger<dynamic> logger => (ILogger<dynamic>)_log;

        public async Task<IEnumerable<UsersTb>> GetRevistingCustomers()
        {
            try
            {
                
                return await _appContext.MailLogTb
                    //.Include(o => o.CreatedBy).Include(o => o.Customer).Include(o => o.InvoiceTb).Include(o => o.OrderItemTb)
                    .Where(o =>o.CreatedFor.LastLoginDate>o.DateCreated).Select(o=>o.CreatedFor)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _log.LogDebug(ex.Message + ex.StackTrace);
                return null;
            }
        }

        public async Task<IEnumerable<UsersTb>> GetDeletingCustomers()
        {
            try
            {

                return await _appContext.OrderTb
                    //.Include(o => o.CreatedBy).Include(o => o.Customer).Include(o => o.InvoiceTb).Include(o => o.OrderItemTb)
                    .Where(o => o.IsDeleted && 
                                _appContext.MailLogTb.Where(m=>m.CreatedFor.LastLoginDate > m.DateCreated)
                                                     .Select(n=>n.OtherReference)
                                                     .ToList()
                                                     .Contains(o.OrderId.ToString())
                                ).Select(o => o.CreatedBy)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _log.LogDebug(ex.Message + ex.StackTrace);
                return null;
            }
        }

        public async Task<IEnumerable<OrderTb>> GetOrderByUser(UsersTb user=null)
        {
            try
            {
                if (user == null)
                {
                    return await _appContext.OrderTb
                    .Include(o => o.CreatedBy).Include(o => o.Customer).Include(o => o.InvoiceTb).Include(o => o.OrderItemTb)
                    .Where(o => o.InvoiceTb == null)
                    .ToListAsync();
                }
                return await _appContext.OrderTb
                    .Include(o => o.CreatedBy).Include(o => o.Customer).Include(o => o.InvoiceTb).Include(o => o.OrderItemTb)
                    .Where(o => o.CreatedBy == user && o.InvoiceTb == null)
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                _log.LogDebug(ex.Message+ex.StackTrace);
                return null;
            }
        }

        public async Task<IEnumerable<OrderTb>> GetCheckoutItemsByUser(UsersTb user=null)
        {
            try
            {
                if (user == null)
                {
                    return await _appContext.OrderTb
                    .Include(o => o.CreatedBy).Include(o => o.Customer).Include(o => o.InvoiceTb).Include(o => o.OrderItemTb)
                    .Where(o => o.InvoiceTb != null)
                    .ToListAsync();
                }
                return await _appContext.OrderTb
                    .Include(o => o.CreatedBy).Include(o => o.Customer).Include(o => o.InvoiceTb).Include(o => o.OrderItemTb)
                    .Where(o => o.CreatedBy == user && o.InvoiceTb != null)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _log.LogDebug(ex.Message + ex.StackTrace);
                return null;
            }
        }

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

        public async Task<bool> Remove(string orderReference)
        {
            try
            {
                var order = await _appContext.OrderTb
                    .FirstOrDefaultAsync(o => o.OrderId.ToString().ToUpper() == orderReference.ToUpper() && o.InvoiceTb == null);

                _appContext.OrderItemTb.RemoveRange(await _appContext.OrderItemTb.Where(i => i.Order.OrderId.ToString() == order.OrderId.ToString()).ToListAsync());

                var OrderItems =await  _appContext.OrderItemTb.Where(i => i.Order.OrderId.ToString() == order.OrderId.ToString()).ToListAsync();

                if (OrderItems.Count > 0)
                {
                    throw new InvalidOperationException("Operation could not be completed as Order items could not be deleted.");
                }
                else
                {
                    _appContext.OrderTb.Remove(await _appContext.OrderTb.FirstOrDefaultAsync(i => i.OrderId.ToString() == order.OrderId.ToString()));

                    order = await _appContext.OrderTb
                    .FirstOrDefaultAsync(o => o.OrderId.ToString().ToUpper() == orderReference.ToUpper() && o.InvoiceTb == null);

                    if (order!=null)
                    {
                        throw new InvalidOperationException("Something went wrong, order could not be deleted.");
                    }
                    return true;
                }
                   
                
            }
            catch (Exception ex)
            {
                _log.LogDebug(ex.Message + ex.StackTrace);
            }
            return false;
        }

        public async Task<bool> Delete(string orderReference)
        {
            try
            {
                var order = await GetActiveOrder(orderReference);

                _appContext.OrderItemTb.RemoveRange(await _appContext.OrderItemTb.Where(i => i.Order.OrderId.ToString() == order.OrderId.ToString()).ToListAsync());

                var OrderItems = await _appContext.OrderItemTb.Where(i => i.Order.OrderId.ToString() == order.OrderId.ToString()).ToListAsync();

                if (order==null)
                {
                    throw new InvalidOperationException("Operation could not be completed as Order could not be found.");
                }
                else
                {
                    order.IsDeleted = true;
                    order.DateDeleted = DateTime.Now;
                    _appContext.OrderTb.Update(order);

                    order = await GetActiveOrder(orderReference);

                    if (order != null)
                    {
                        throw new InvalidOperationException("Something went wrong, order could not be deleted.");
                    }
                    return true;
                }


            }
            catch (Exception ex)
            {
                _log.LogDebug(ex.Message + ex.StackTrace);
            }
            return false;
        }

        private async Task<OrderTb> GetActiveOrder(string orderReference)
        {
            return await _appContext.OrderTb
                   .FirstOrDefaultAsync(o => o.OrderId.ToString().ToUpper() == orderReference.ToUpper()
                   && o.InvoiceTb == null && !o.IsDeleted);
        }
    }
}
