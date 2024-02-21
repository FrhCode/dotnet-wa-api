
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Whatsapp.Api.AppException;
using Whatsapp.Api.Dtos;
using Whatsapp.Api.Extensions;
using Whatsapp.Api.model;
using Whatsapp.Api.Options;
using Whatsapp.Api.Services;

namespace Whatsapp.Api.Controllers;

[Route("api/auth")]
public class AuthController : ControllerBase
{
	private readonly UserManager<User> _userManager;
	private readonly IEmailSender _emailSender;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly SignInManager<User> _signInManager;
	private readonly JwtOption _jwtOption;

	private readonly AuthService _authService;

	public AuthController(UserManager<User> userManager, IEmailSender emailSender, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, IOptions<JwtOption> jwtOption, AuthService authService)
	{
		_userManager = userManager;
		_emailSender = emailSender;
		_roleManager = roleManager;
		_signInManager = signInManager;
		_jwtOption = jwtOption.Value;
		_authService = authService;
	}

	[HttpPost("sign-up")]
	public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
	{
		if (ModelState.IsValid == false)
		{
			throw new BadRequestException(ModelState.ToDictionary());
		}
		await _authService.SignUp(signUpDto);

		return Ok();
	}

	// confirm email
	[HttpGet("confirm-email")]
	public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
	{
		if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
		{
			var errors = new Dictionary<string, string>{
				{ "root", "Invalid user id or token" }
			};

			throw new BadRequestException(errors);
		}
		await _authService.ConfirmEmail(userId, token);

		return Ok();
	}

}