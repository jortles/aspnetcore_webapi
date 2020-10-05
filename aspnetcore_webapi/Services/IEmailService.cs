using aspnetcore_webapi.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore_webapi.Services
{
    public interface IEmailService : IEmailSender
    {
        public void EmailTwoFactorCode(ApplicationUser user);
    }
}
