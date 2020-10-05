using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using aspnetcore_webapi.Controllers;
using aspnetcore_webapi.Models;
using aspnetcore_webapi.Services;
using aspnetcore_webapi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace aspnetcore_webapi.ApiControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TwoFactorController : ControllerBase
    {
        private IConfiguration _configuration;
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private ISMService _smService;
        private RoleManager<IdentityRole> _roleManager;

        public TwoFactorController(IConfiguration configuration, ISMService smService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _smService = smService;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<ActionResult<LoginViewModel>> Post([FromBody] TwoFactorViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password) || string.IsNullOrWhiteSpace(model.TwoFactorValue))
            {
                return new UnauthorizedResult();
            }

            ApplicationUser user = await _userManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.Succeeded)
                {

                    if (_smService.ValidateTwoFactorCodeAsync(user, model.TwoFactorValue))
                    {

                        IList<string> roles = await _userManager.GetRolesAsync(user);
                        string role = "";
                        if (roles.Contains("Admin"))
                            role = "Admin";
                        else if (roles.Contains("User"))
                            role = "User";



                        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtKey"]));
                        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                            new Claim(ClaimTypes.Name, user.Id.ToString()),
                            new Claim(ClaimTypes.Role, role)
                            }),
                            Expires = DateTime.UtcNow.AddDays(7),
                            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
                        };
                        SecurityToken securityToken = handler.CreateToken(tokenDescriptor);
                        LoginResponseViewModel responseModel = new LoginResponseViewModel
                        {
                            Token = handler.WriteToken(securityToken)
                        };
                        return new OkObjectResult(responseModel);
                    }
                }
            }
            return new UnauthorizedResult();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController), "Index");
        }
    }

    public class TwoFactorViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string TwoFactorValue { get; set; }
    }

    public class LoginResponseViewModel
    {
        public string Token { get; set; }
    }
}
