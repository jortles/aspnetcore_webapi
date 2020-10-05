using aspnetcore_webapi.ApiControllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore_webapi.Extensions
{
    public static class UrlHelperExtension
    {
        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(ResetPasswordController),
                controller: "ResetPassword",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}
