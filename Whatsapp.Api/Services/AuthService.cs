using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Whatsapp.Api.AppException;
using Whatsapp.Api.Dtos;
using Whatsapp.Api.model;

namespace Whatsapp.Api.Services;

public class AuthService
{
	private readonly UserManager<User> _userManager;
	private readonly IEmailSender _emailSender;
	private readonly LinkGenerator _linkGenerator;
	public IConfiguration _configuration { get; }



	public AuthService(UserManager<User> userManager, IEmailSender emailSender, IUrlHelperFactory urlHelperFactory, LinkGenerator linkGenerator, IConfiguration configuration)
	{
		_userManager = userManager;
		_emailSender = emailSender;
		_linkGenerator = linkGenerator;
		_configuration = configuration;
	}


	public async Task SignUp(SignUpDto signUpDto)
	{
		var isEmailUnique = await _userManager.FindByEmailAsync(signUpDto.Email) == null;

		if (isEmailUnique == false)
		{
			var errors = new Dictionary<string, string>
			{
				{ "root", "Failed to register user. Please try again." }
			};
			throw new BadRequestException(errors);
		}

		var newUser = new User
		{
			UserName = signUpDto.Email,
			Email = signUpDto.Email
		};

		var userResult = await _userManager.CreateAsync(newUser, signUpDto.Password);
		if (userResult.Succeeded == false)
		{
			throw new Exception(userResult.Errors.First().Description);
		}

		var user = await _userManager.FindByEmailAsync(signUpDto.Email);

		var token = await _userManager.GenerateEmailConfirmationTokenAsync(user!);

		var appUrl = _configuration["AppUrl"];
		var confirmationLink = _linkGenerator.GetPathByAction("ConfirmEmail", "Auth", new { userId = user!.Id, token = token }, appUrl);

		await _emailSender.SendEmailAsync("ChatApp", user.Email!, "Confirm your email", string.Concat(appUrl, confirmationLink));
	}

	// confirm email
	public async Task ConfirmEmail(string userId, string token)
	{
		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
		{
			var errors = new Dictionary<string, string>
			{
				{ "root", "User not found." }
			};
			throw new BadRequestException(errors);
		}

		var result = await _userManager.ConfirmEmailAsync(user, token);
		if (result.Succeeded == false)
		{
			throw new Exception(result.Errors.First().Description);
		}
	}

}