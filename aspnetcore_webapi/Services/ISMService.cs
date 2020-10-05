using aspnetcore_webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore_webapi.Services
{
    public interface ISMService
    {
        public Task SendTwoFactorCodeAsync(ApplicationUser user);
        public bool ValidateTwoFactorCodeAsync(ApplicationUser user, string code);
    }
}
