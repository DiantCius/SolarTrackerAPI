using Backend;
using Backend.DataAccess;
using Backend.Errors;
using Backend.Services;
using Backend.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configurationBuilder = new ConfigurationBuilder();

builder.Configuration.AddConfiguration(configurationBuilder.Build());

// Add services to the container.

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<ValidatorActionFilter>();
}).AddFluentValidation(cfg => {
    cfg.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});

ValidatorOptions.Global.LanguageManager.Enabled = false; // 

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddCors();

var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => 
    options.UseNpgsql(defaultConnectionString));


var issuer = "Issuer";
var audience = "Audience";
var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("mysecretkey12345qwerteyurfgdfghfdfdsg"));
var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

var jwtOptions = new JwtOptions()
{
    Audience = audience,
    Issuer = issuer,
    SigningCredentials = signingCredentials
};

builder.Services.AddSingleton(jwtOptions);
builder.Services.AddScoped<JwtGenerator>();
builder.Services.AddScoped<AuthHandler>();
builder.Services.AddScoped<IndicationService>();
builder.Services.AddScoped<PowerplantsHandler>();
builder.Services.AddScoped<EnergyProductionHandler>();
builder.Services.AddSingleton<HttpContextAccessor>();
builder.Services.AddScoped<CurrentUser>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateActor = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingCredentials.Key,
        ValidIssuer = issuer,
        ValidAudience = audience,
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {   new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()}
                });

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Server", Version = "v1" });
    c.CustomSchemaIds(d => d.FullName);

});



var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});


app.UseAuthorization();

app.MapControllers();

app.Run();
