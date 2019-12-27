using Carts.Data.POCO;
using System.Threading.Tasks;

namespace Carts.Data.Repository.Interface
{
    public interface IInvoiceRepository : IRepository<InvoiceTb>
    {
        Task<InvoiceTb> CheckoutAsync(UsersTb invoiceUser, string reference);
    }
}
