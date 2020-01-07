using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
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
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Carts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region "private"
        private IUnitOfWork _unitOfWork;
        private ILogger<dynamic> _log;
        private IUserManager _userManager;
        private IHostingEnvironment _hostingEnvironment;
        private IMapper _mapper;
        private readonly TokenManagement _tokenManagement;
        #endregion

        public AuthController(IUnitOfWork unitOfWork, ILogger<dynamic> log, IHostingEnvironment hostingEnvironment, IUserManager userManager,
            IMapper mapper, IOptions<TokenManagement> tokenManagement)
        {
            _unitOfWork = unitOfWork;
            _log = log;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
            _tokenManagement = tokenManagement.Value;
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
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> AddCustomer([FromBody] NewUserViewModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                UsersTb request = null;
                var dataModel = _mapper.Map<UsersTb>(model);

                //send request to persistent source
                request = await _userManager.Create(dataModel);

                if (request == null)
                {
                    return BadRequest("Request could not be saved.");
                }
                else
                {
                    return Ok(_mapper.Map<UserViewModel>(request));
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrWhiteSpace(request.username) || string.IsNullOrWhiteSpace(request.password))
                {
                    return BadRequest(new
                    {
                        Message = "username or password is null"
                    });
                }
                var user = await _userManager.SignIn(HttpContext, request);

                if (!await _userManager.SignIn(HttpContext, request))
                {
                    return BadRequest(new
                    {
                        Message = "Invalid Login and/or password"
                    });
                }


                string token = await IsAuthenticated(await _userManager.GetCurrentUser(HttpContext));
                if (!string.IsNullOrEmpty(token))
                {
                    return Ok(new { token });
                }

                return BadRequest("Invalid Request");
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message + ex.InnerException);
                return StatusCode(500, "An error has occured! Please try again - " + ex.Message);
            }

        }


        private async Task<string> IsAuthenticated(UsersTb request)
        {
            string token = string.Empty;
            try
            {


                if (request == null) return null;

                var claim = _userManager.GetUserClaims(request);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var jwtToken = new JwtSecurityToken(
                    _tokenManagement.Issuer,
                    _tokenManagement.Audience,
                    claim,
                    expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                    signingCredentials: credentials
                );
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            }
            catch (Exception ex)
            {

            }
            return token;
        }




    }
}
