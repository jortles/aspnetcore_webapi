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
    public class ResetPasswordController : ControllerBase
    {
        private UserManager<ApplicationUser> userManager;

        public ResetPasswordController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ResetPasswordViewModel model)
        {
            if (model.Password == model.ConfirmPassword)
            {
                var user = await userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return new OkResult();
                    }
                }
            }
            return new UnauthorizedResult();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
