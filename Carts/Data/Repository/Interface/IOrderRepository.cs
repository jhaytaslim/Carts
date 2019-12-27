using Carts.Data.POCO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carts.Data.Repository.Interface
{
    public interface IOrderRepository : IRepository<OrderTb>
    {
        Task<IEnumerable<OrderTb>> GetOrderByUser(UsersTb user=null);
        Task<OrderTb> Get(string orderId);
        Task<bool> Remove(string orderReference);
        Task<IEnumerable<OrderTb>> GetCheckoutItemsByUser(UsersTb user=null);
        Task<IEnumerable<UsersTb>> GetRevistingCustomers();
        Task<IEnumerable<UsersTb>> GetDeletingCustomers();

    }
}
