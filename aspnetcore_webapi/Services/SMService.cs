using aspnetcore_webapi.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore_webapi.Services
{
    public class SMService : ISMService
    {
        private ApplicationDbContext _dbContext;
        private IEmailService _emailService;
        private UserManager<ApplicationUser> _userManager;
        private static Random _random = new Random();

        public SMService(ApplicationDbContext dbContext, IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task SendTwoFactorCodeAsync(ApplicationUser user)
        {
            int code = _random.Next(0, 999999);
            user.TwoFactorCode = code.ToString("000000");
            user.TwoFactorCodeDateTime = DateTime.Now;
            await _userManager.UpdateAsync(user);
            _emailService.EmailTwoFactorCode(user);
        }

        public bool ValidateTwoFactorCodeAsync(ApplicationUser user, string code)
        {
            if (user.TwoFactorEnabled && user.TwoFactorCodeDateTime != null && !string.IsNullOrEmpty(user.TwoFactorCode))
            {
                TimeSpan codeTimeSpan = DateTime.Now - user.TwoFactorCodeDateTime;
                if (codeTimeSpan <= TimeSpan.FromMinutes(5))
                {
                    if (code == user.TwoFactorCode)
                    {
                        user.TwoFactorCode = "";
                        _userManager.UpdateAsync(user);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
