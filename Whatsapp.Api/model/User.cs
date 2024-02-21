using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Whatsapp.Api.model;

public class User : IdentityUser
{
	public string Status { get; set; }

	public string ProfilePicture { get; set; }
}