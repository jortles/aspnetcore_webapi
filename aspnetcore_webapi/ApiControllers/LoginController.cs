using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetcore_webapi.Models;
using aspnetcore_webapi.Services;
using aspnetcore_webapi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace aspnetcore_webapi.ApiControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private ISMService _smService;
        private IConfiguration _configuration;

        public LoginController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ISMService smService, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _smService = smService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return new UnauthorizedResult();
            }

            ApplicationUser user = await _userManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.Succeeded)
                {
                    //await _signInManager.SignInAsync(user, false);

                    await _smService.SendTwoFactorCodeAsync(user);
                    return new OkResult();
                }
            }
            return new UnauthorizedResult();
        }
    }
}
