using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetcore_webapi.MailKit;
using aspnetcore_webapi.Models;
using aspnetcore_webapi.Services;
using aspnetcore_webapi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore_webapi.ApiControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private ISMService _smService;
        private IMailKitSender _emailSender;
        private RoleManager<IdentityRole> _roleManager;

        public ForgotPasswordController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ISMService smService, IMailKitSender emailSender, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _smService = smService;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ForgotPasswordViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                return new UnauthorizedResult();
            }

            else
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink = Url.Action("ResetPassword", "Home",
                        new { UserId = user.Id, Token = token }, Request.Scheme);

                    var message = new MessageService(new string[] { user.Email }, "Reset password link", passwordResetLink, null);

                    await _emailSender.SendEmailAsync(message);

                    return new OkResult();

                }

                return new UnauthorizedResult();
            }
        }
    }
}
