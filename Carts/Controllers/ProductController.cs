using System;
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
    public class ProductController : ControllerBase
    {
        #region "private"
        private IUnitOfWork _unitOfWork;
        private ILogger<dynamic> _log;
        private IUserManager _userManager;
        private IHostingEnvironment _hostingEnvironment;
        private IMapper _mapper;
        #endregion

        public ProductController(IUnitOfWork unitOfWork, ILogger<dynamic> log, IHostingEnvironment hostingEnvironment, IUserManager userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _log = log;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                return Ok(_unitOfWork.Product.GetAll());
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
                return Ok(_unitOfWork.Product.Get(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/values
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                ProductTb request = null;
                var dataModel = _mapper.Map<ProductTb>(model);
                dataModel.ProductCode = string.Empty.GetOrderRef();
                //send request to persistent source
                request = await _unitOfWork.Product.AddAsync(dataModel);
                if (request == null)
                {
                    return BadRequest("Request could not be saved.");
                }
                else
                {
                    return Ok(_mapper.Map<ProductViewModel>(request));
                }

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        
    }
}
