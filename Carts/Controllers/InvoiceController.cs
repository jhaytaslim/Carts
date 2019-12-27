using System.Collections.Generic;
using AutoMapper;
using Carts.Data;
using Carts.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Carts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        #region "private"
        private IUnitOfWork _unitOfWork;
        private ILogger<dynamic> _log;
        private IUserManager _userManager;
        private IHostingEnvironment _hostingEnvironment;
        private IMapper _mapper;
        #endregion

        public InvoiceController(IUnitOfWork unitOfWork, ILogger<dynamic> log,IHostingEnvironment hostingEnvironment, IUserManager userManager,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _log = log;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
