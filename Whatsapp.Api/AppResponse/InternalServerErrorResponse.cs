using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Whatsapp.Api.AppResponse;

public class InternalServerErrorResponse : BaseResponse
{
	// traceId 
	public string TraceId { get; }
	// details
	public string Details { get; set; }

	public InternalServerErrorResponse(string traceId, string details) : base(500, "Internal Server Error")
	{
		TraceId = traceId;
		Details = details;
	}
}