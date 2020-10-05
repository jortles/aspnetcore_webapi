using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore_webapi.MailKit
{
    public interface IMailKitSender
    {
        void SendEmail(MessageService message);
        Task SendEmailAsync(MessageService message);
    }
}
