using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Whatsapp.Api.AppResponse;

public class BadRequestResponse : BaseResponse
{
	public IDictionary<string, string> Errors { get; }

	public BadRequestResponse(string message, IDictionary<string, string> errors) : base(400, message)
	{
		Errors = errors;
	}
}
