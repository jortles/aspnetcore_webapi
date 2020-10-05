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

namespace aspnetcore_webapi.ApiControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RegisterController : ControllerBase
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private ISMService _smService;
        private RoleManager<IdentityRole> _roleManager;

        public RegisterController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ISMService smService, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _smService = smService;
            _roleManager = roleManager;
        }

        [HttpPut]
        public async Task<ActionResult<ResponseStatusViewModel>> Put(RegisterViewModel model)
        {
            ResponseStatusViewModel responseModel = new ResponseStatusViewModel();
            responseModel.Result = true;
            if (string.IsNullOrWhiteSpace(model.FirstName))
            {
                responseModel.Result = false;
                responseModel.Messages.Add("First name cannot be blank!");
            }
            if (string.IsNullOrWhiteSpace(model.LastName))
            {
                responseModel.Result = false;
                responseModel.Messages.Add("Last name cannot be blank!");
            }
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                responseModel.Result = false;
                responseModel.Messages.Add("Email cannot be blank!");
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                responseModel.Result = false;
                responseModel.Messages.Add("Password cannot be blank!");
            }
            if (!responseModel.Result)
                return new BadRequestObjectResult(responseModel);

            ApplicationUser appUser = new ApplicationUser()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                TwoFactorEnabled = true,
                EmailConfirmed = true
            };
            IdentityResult result = await _userManager.CreateAsync(appUser, model.Password);
            if (result.Succeeded)
            {
                IdentityRole userRole = _roleManager.Roles.FirstOrDefault(r => r.Name == "User");
                if (userRole == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }
                await _userManager.AddToRoleAsync(appUser, userRole.Name);
                responseModel.Result = true;
                responseModel.Messages.Add("Thank you for registering your account.");
                return new OkObjectResult(responseModel);
            }
            else
            {
                responseModel.Result = false;
                responseModel.Messages.Add("Unable to create your user account.");
                return new BadRequestObjectResult(responseModel);
            }
        }
    }
}
