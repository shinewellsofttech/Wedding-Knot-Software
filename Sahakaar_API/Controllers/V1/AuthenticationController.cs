using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net.Http;
using Sahakaar_API.Authentication;
using Sahakaar_API.Models;

namespace Sahakaar_API.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager,IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._configuration = configuration;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try { 
                var userExist = await userManager.FindByEmailAsync(model.Email);
                if (userExist != null)
                    return StatusCode(StatusCodes.Status208AlreadyReported, new Response { Success=false,Status = StatusCodes.Status208AlreadyReported, Message = "User already exist." });
                ApplicationUser user = new ApplicationUser()
                {
                    Email = model.Email,
                    UserName = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if(!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status501NotImplemented, new Response { Success=false, Status = StatusCodes.Status501NotImplemented, Message = "User creation failed." });
                }
                return Ok(new Response { Success=true, Status = StatusCodes.Status200OK, Message = "User created successfully." });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    };
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                    
                    await userManager.RemoveAuthenticationTokenAsync(user, "Login", "LoginToken");
                    var newLoginToken = await userManager.GenerateUserTokenAsync(user, "Login", "LoginToken");
                    await userManager.SetAuthenticationTokenAsync(user, "Login", "LoginToken", newLoginToken);

                    return Ok(new Response
                    {
                        Time = DateTime.Now.ToShortTimeString(),
                        Success = true,
                        Status = StatusCodes.Status200OK,
                        Message = "Login successfully.",
                        Data = new
                        {
                            token = newLoginToken,
                            User = user.UserName,
                            UserId = user.Id
                        }
                    });
                }
                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Success = false, Status = StatusCodes.Status401Unauthorized, Message = "Unauthorized Access." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("MemberLogin")]
        public async Task<IActionResult> MemberLogin([FromBody] VendorLoginModel model)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    };
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    await userManager.RemoveAuthenticationTokenAsync(user, "Login", "LoginToken");
                    var newLoginToken = await userManager.GenerateUserTokenAsync(user, "Login", "LoginToken");
                    await userManager.SetAuthenticationTokenAsync(user, "Login", "LoginToken", newLoginToken);

                    return Ok(new Response
                    {
                        Time = DateTime.Now.ToShortTimeString(),
                        Success = true,
                        Status = StatusCodes.Status200OK,
                        Message = "Login successfully.",
                        Data = new
                        {
                            token = newLoginToken,
                            User = user.UserName,
                            UserId = user.Id
                        }
                    });
                }
                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Success = false, Status = StatusCodes.Status401Unauthorized, Message = "Unauthorized Access." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Status = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }
    }
}