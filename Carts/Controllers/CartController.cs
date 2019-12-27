using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Carts.Data;
using Carts.Data.POCO;
using Carts.Helpers;
using Carts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Carts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        #region "private"
        private IUnitOfWork _unitOfWork;
        private ILogger<dynamic> _log;
        private IUserManager _userManager;
        private IHostingEnvironment _hostingEnvironment;
        private IMapper _mapper;
        #endregion

        public CartController(IUnitOfWork unitOfWork, ILogger<dynamic> log, IHostingEnvironment hostingEnvironment, IUserManager userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _log = log;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
        }

        // GET api/values
        [Authorize]
        [HttpGet, Route("GetRevisitingCustomers")]
        public async Task<ActionResult> GetRevisitingCustomers()
        {
            try
            {
                var users = await _unitOfWork.Order.GetRevistingCustomers();
                var resolvedUsers = _mapper.Map<List<UsersTb>, List<UserViewModel>>(users.ToList());
                return Ok(resolvedUsers);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet, Route("GetDeletingCustomers")]
        public async Task<ActionResult> GetDeletingCustomers()
        {
            try
            {
                var users=await _unitOfWork.Order.GetDeletingCustomers();
                var resolvedUsers = _mapper.Map<List<UsersTb>, List<UserViewModel>>(users.ToList());
                return Ok(resolvedUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                return Ok(await _unitOfWork.Order.GetOrderByUser(await _userManager.GetCurrentUser(HttpContext)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            try
            {
                return Ok(await _unitOfWork.Order.Get(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/values
        [HttpPost, Route("Post")]
        public async Task<ActionResult> Post([FromBody] OrderViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                OrderTb request = null;
                var dataModel = _mapper.Map<OrderTb>(model);
                dataModel.OrderReference = string.Empty.GetOrderRef();
                //send request to persistent source
                request = await _unitOfWork.Order.AddAsync(dataModel);
                if (request == null)
                {
                    return BadRequest("Request could not be saved.");
                }
                else
                {
                    return Ok(_mapper.Map<OrderViewModel>(request));   
                }

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost, Route("AddToCart")]
        public async Task<ActionResult> AddToCart([FromBody] OrderItemViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                OrderItemTb request = null;
                var referencedOrder = _unitOfWork.Order.GetSingleOrDefault(o => o.OrderReference.ToUpper() == model.OrderReference.ToUpper());

                if (referencedOrder == null)
                {
                    return BadRequest("Order not Found");
                }

                var dataModel = _mapper.Map<OrderItemTb>(model);
                dataModel.Order = referencedOrder;
                //send request to persistent source
                request = await _unitOfWork.OrderItem.AddAsync(dataModel);
                if (request == null)
                {
                    return BadRequest("Request could not be saved.");
                }
                else
                {
                    return Ok(_mapper.Map<OrderViewModel>(request));
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost, Route("Checkout")]
        public async Task<ActionResult> Checkout(string OrderReference)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                OrderItemTb request = null;
                var referencedOrder = _unitOfWork.Order.GetSingleOrDefault(o => o.OrderReference.ToUpper() == OrderReference.ToUpper());

                if (referencedOrder == null)
                {
                    return BadRequest("Order not Found");
                }

                //send request to persistent source
                var requestInvoice = await _unitOfWork.Invoice.CheckoutAsync(await _userManager.GetCurrentUser(HttpContext), OrderReference);

                if (requestInvoice == null)
                {
                    return BadRequest("Request could not be saved.");
                }
                else
                {
                    return Ok(_mapper.Map<InvoiceViewModel>(requestInvoice));
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        //[HttpGet]
        [HttpGet, Route("GetCheckoutItems")]
        public async Task<ActionResult> GetCheckoutItems()
        {
            try
            {
                return Ok(await _unitOfWork.Order.GetCheckoutItemsByUser(await _userManager.GetCurrentUser(HttpContext)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/values/5
        [HttpPost, Route("Edit")]
        public async Task<ActionResult> Edit([FromBody] OrderViewModel model)
        {
            try
            {
                var user = await _userManager.GetCurrentUser(HttpContext);
                var orderSearch = await _unitOfWork.Order.Get(model.OrderReference);
                if (orderSearch == null)
                {
                    return BadRequest("Order could not be found");
                }
                else if (orderSearch != null && orderSearch.CreatedBy != user)
                {
                    return BadRequest("Order can not be edited.");
                }

                _unitOfWork.Order.Update(_mapper.Map<OrderTb>(model));
                var updatedOrder = await _unitOfWork.Order.Get(model.OrderReference);

                if (updatedOrder != null)
                {
                    return Ok(updatedOrder);
                }
                else
                {

                    return BadRequest("Order could not be updated");
                }


            }
            catch (Exception ex)
            {
                _log.LogDebug(ex.Message + ex.StackTrace);
                return StatusCode(500, "An Error Occured");
            }
        }

        // DELETE api/values/5
        [HttpDelete("{OrderRef}")]
        //[HttpDelete("{OrderRef}"), Route("Edit")]
        public async Task<ActionResult> Delete(string OrderRef)
        {
            try
            {
                var orderSearch = await _unitOfWork.Order.Get(OrderRef);
                if (orderSearch == null)
                {
                    return BadRequest("Order could not be found");
                }

                var IsDeleted = await _unitOfWork.Order.Remove(OrderRef);

                if (IsDeleted)
                {
                    return Ok("Successfully Deleted");
                }
                else
                {
                    return BadRequest("Order could not be deleted");
                }

            }
            catch (Exception ex)
            {
                _log.LogDebug(ex.Message + ex.StackTrace);
                return StatusCode(500, "An Error Occured");
            }
        }
    }
}
