using Carts.Data.Repository.Interface;

namespace Carts.Data
{
    public interface IUnitOfWork
    {
        IInvoiceRepository Invoice { get; }
        IMailLogRepository MailLog { get; }
        IOrderRepository Order { get; }
        IOrderItemRepository OrderItem { get; }
        IProductRepository Product { get; }
        IUserRepository User { get; }
    }
}
