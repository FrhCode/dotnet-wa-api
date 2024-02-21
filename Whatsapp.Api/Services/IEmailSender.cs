using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Whatsapp.Api.Services;

public interface IEmailSender
{
	Task SendEmailAsync(string fromAddress, string toAddress, string subject, string body);
}