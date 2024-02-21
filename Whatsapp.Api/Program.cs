using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Whatsapp.Api.Data;
using Whatsapp.Api.Middlewares;
using Whatsapp.Api.model;
using Whatsapp.Api.Options;
using Whatsapp.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configure controller
builder.Services.AddControllers();

// configure options
builder.Services.Configure<JwtOption>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<SmtpOption>(builder.Configuration.GetSection("Smtp"));

// configre services
builder.Services.AddSingleton<IEmailSender, SmtpMailSender>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// configure Identity
builder.Services
	.AddIdentity<User, IdentityRole>(options =>
	{
		options.SignIn.RequireConfirmedEmail = true;
	})
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();

// register authentication middleware for JWT
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
	.AddJwtBearer(options =>
	{
		JwtOption jwtOption = builder.Configuration.GetSection("Jwt").Get<JwtOption>() ?? throw new InvalidOperationException("JwtOption is missing in appsettings.json");
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidIssuer = jwtOption.Issuer,
			ValidAudience = jwtOption.Audience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.Key))
		};
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// register custom middleware that handles exceptions
app.UseMiddleware<CustomExceptionHandlerMiddleware>();

// register authentication middleware
app.UseAuthentication();
app.UseAuthorization();

// register controller
app.MapControllers();

app.Run();
