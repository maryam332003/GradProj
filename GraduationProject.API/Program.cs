using GraduationProject.Repository.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GraduationProject.Core.Entities;
using GraduationProject.Services;
using GraduationProject.Core.ServiceInterfaces;
using GraduationProject.Core.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddIdentity<AppUser, IdentityRole>()
                              .AddEntityFrameworkStores<AppIdentityDbContext>()
                              .AddDefaultTokenProviders();
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});

builder.Services.AddSingleton<EmailConfiguration>();

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ITokenService,TokenService>();
var Configuration = builder.Configuration;
var EmailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddTransient<IEmailService,EmailService>();

builder.Services.Configure<IdentityOptions>(
    opts => opts.SignIn.RequireConfirmedEmail = true
    );

builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(10));


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
