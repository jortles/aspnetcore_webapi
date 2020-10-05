using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetcore_webapi.MailKit;
using aspnetcore_webapi.Models;
using aspnetcore_webapi.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore_webapi.ApiControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RegisterAdminController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IMapper _mapper;
        private IMailKitSender _mailSender;

        public RegisterAdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IMailKitSender mailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _mailSender = mailSender;
        }

        [HttpPut]
        public async Task<ActionResult<ResponseStatusViewModel>> Put(RegisterViewModel model)
        {
            ResponseStatusViewModel responseModel = new ResponseStatusViewModel();
            responseModel.Result = true;
            if (string.IsNullOrWhiteSpace(model.FirstName))
            {
                responseModel.Result = false;
                responseModel.Messages.Add("First Name cannot be blank!");
            }
            if (string.IsNullOrWhiteSpace(model.LastName))
            {
                responseModel.Result = false;
                responseModel.Messages.Add("Last Name cannot be blank!");
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
            if (string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                responseModel.Result = false;
                responseModel.Messages.Add("Password cannot be blank!");
            }
            if (!responseModel.Result)
                return new BadRequestObjectResult(responseModel);


            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                TwoFactorEnabled = true,
                EmailConfirmed = true
            };



            /*
            var user = _mapper.Map<AppUser>(model);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = Url.Action(nameof(Project_2.Controllers.HomeController.ConfirmEmail), "Home", new { token, email = user.Email }, Request.Scheme);

            var message = new MessageService(new string[] { user.Email }, "Confirmation email link", confirmationLink, null);
            */

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //await _mailSender.SendEmailAsync(message);

                IdentityRole userRole = _roleManager.Roles.FirstOrDefault(r => r.Name == "Admin");
                if (userRole == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                await _userManager.AddToRoleAsync(user, userRole.Name);
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
