using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetcore_webapi.Models;
using aspnetcore_webapi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore_webapi.ApiControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private ApplicationDbContext dbContext;
        private UserManager<ApplicationUser> userManager;

        public ProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            ApplicationUser user = await userManager.FindByIdAsync(User.Identity.Name);

            if (user != null)
            {
                ProfileViewModel model = new ProfileViewModel
                {
                    Email = user.Email
                };

                return new OkObjectResult(model);
            }
            else
            {
                return new OkResult();
            }
        }
    }
}
