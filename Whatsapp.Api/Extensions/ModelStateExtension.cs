using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Whatsapp.Api.Extensions;

public static class ModelStateExtension
{
	public static Dictionary<string, string> ToDictionary(this ModelStateDictionary modelState)
	{
		return modelState.ToDictionary(
			kvp => kvp.Key.Substring(0, 1).ToLower() + kvp.Key.Substring(1),
			kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).First()
		);
	}

}
