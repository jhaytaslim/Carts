using Carts.Data.POCO;
using Carts.Data.Repository;
using Carts.Data.Repository.Interface;
using Microsoft.Extensions.Logging;

namespace Carts.Data
{
    public class UnitOfWork : IUnitOfWork
    {

        public UnitOfWork(AirTableContext context, ILogger<dynamic> log)
        {
            _context = context;
            _log = log;
        }

        #region "field"
        readonly AirTableContext _context;
        readonly ILogger<dynamic> _log;

        private IInvoiceRepository _InvoiceRepository;
        private IUserRepository _IUserRepository;
        private IOrderRepository _IOrderRepository;
        private IOrderItemRepository _IOrderItemRepository;
        private IProductRepository _IProductRepository;
        private IMailLogRepository _IMailLogRepository;


        #endregion

        #region "public"
        public IInvoiceRepository Invoice
        {
            get
            {
                if (_InvoiceRepository == null)
                    _InvoiceRepository = new InvoiceRepository(_context, _log);

                return _InvoiceRepository;
            }
        }

        public IUserRepository User
        {
            get
            {
                if (_IUserRepository == null)
                    _IUserRepository = new UserRepository(_context, _log);

                return _IUserRepository;
            }
        }

        public IOrderRepository Order
        {
            get
            {
                if (_IOrderRepository == null)
                    _IOrderRepository = new OrderRepository(_context, _log);

                return _IOrderRepository;
            }
        }

        public IOrderItemRepository OrderItem
        {
            get
            {
                if (_IOrderItemRepository == null)
                    _IOrderItemRepository = new OrderItemRepository(_context, _log);

                return _IOrderItemRepository;
            }
        }

        public IProductRepository Product
        {
            get
            {
                if (_IProductRepository == null)
                    _IProductRepository = new ProductRepository(_context, _log);

                return _IProductRepository;
            }
        }

        public IMailLogRepository MailLog
        {
            get
            {
                if (_IMailLogRepository == null)
                    _IMailLogRepository = new MailLogRepository(_context, _log);

                return _IMailLogRepository;
            }
        }
        #endregion
    }
}
