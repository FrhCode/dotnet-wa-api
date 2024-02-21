using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Whatsapp.Api.AppResponse;

public class BaseResponse
{
	public int StatusCode { get; }
	public string Message { get; }

	public BaseResponse(int statusCode, string message)
	{
		StatusCode = statusCode;
		Message = message;
	}
}