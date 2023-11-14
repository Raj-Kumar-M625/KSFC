using EDCS_TG.API.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using EDCS_TG.API;
using EDCS_TG.API.Data;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.Data.Repository;
using EDCS_TG.API.Services.Interfaces;
using EDCS_TG.API.Services.Implementation;
using EDCS_TG.API.Mapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SRS.API.Services.Implementations;
using static System.Net.WebRequestMethods;
using System.Security.Policy;
using EDCS_TG.API.Helpers;
using System.Text;
using System.Xml;
using System.Reflection;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<KarmaniDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("KarmaniConnection") ??
throw new InvalidOperationException("Connection string 'KarmaniConnection' not found.")));

builder.Services.AddIdentity<User,Role>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<KarmaniDbContext>().AddDefaultTokenProviders();
// Add services to the container.


//Log4Net Configuration

XmlDocument log4netConfig = new XmlDocument();
log4netConfig.Load(System.IO.File.OpenRead("log4net.config"));

var repo = log4net.LogManager.CreateRepository(
    Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

//Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:ValidAudience"],
        ValidIssuer = builder.Configuration["JwtSettings:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
    };
});
//cache control
builder.Services.AddControllersWithViews().AddMvcOptions(options => options.Filters.Add(
    new ResponseCacheAttribute
    {
        NoStore = true,
        
    }));

//needed to store rate limit counters and ip rules
builder.Services.AddMemoryCache();

//load general configuration from appsettings.json
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));

//inject counter and rules stores
builder.Services.AddInMemoryRateLimiting();

// configuration (resolvers, counter key builders)
builder.Services.AddSingleton<IRateLimitConfiguration,RateLimitConfiguration>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Karmani",
        Version = "v1",
        Description = "Karmani OpenAPI Documentation",
        TermsOfService = new Uri("https://epicmindsit.com"),
        Contact = new OpenApiContact
        {
            Name = "EpicmindsIT",
            Email = "abhilashba@epicmindsit.com",
            Url = new Uri("https://epicmindsit.com")
        },
        License = new OpenApiLicense
        {
            Name = "LICX",
            Url = new Uri("https://example.com/license")
        }
    });

    //    // To Enable authorization using Swagger (JWT)
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description =
        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
});
swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IPersonalDetailsServices,PersonalDetailsService>();
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<IEmploymentService, EmploymentService>();
builder.Services.AddScoped<IHousingService, HousingService>();
builder.Services.AddScoped<IHealthService, HealthService>();
builder.Services.AddScoped<ISocialSecurityService, SocialSecurityService>();
builder.Services.AddScoped<IAdditionalInformationService, AdditionalInformationService>();
builder.Services.AddScoped<IQuestionService, Questionservice>();
builder.Services.AddScoped<IBasicSurveyDetailService, BasicSurveyDetailService>();
builder.Services.AddScoped<ISurveyService, SurveyService>();
builder.Services.AddScoped<IOfficeService, OfficeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserAssignmentService, UserAssignmentService>();
builder.Services.AddScoped<IDBTService, DBTService>();
builder.Services.AddScoped<IUserAssignmentService, UserAssignmentService>();
builder.Services.AddScoped<ISurveyImagesService, SurveyImagesService>();

builder.Services.AddScoped<IDownloadService, DownloadService>();
builder.Services.AddScoped<IDownloadSurveyService, DownloadSurveyService>();


// Add SmsService
builder.Services.AddScoped(typeof(SmsService));

builder.Services.AddScoped<IKutumbaService, KutumbaService>();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);




//http://localhost:3000



//Application: Karmani_DEV_API

//1.Server - > 13 - 234 - 57 - 114
//URL - > http://13.234.57.114:9013/

//Application: UAT_Karmani_React

//https : https://karmani.epicmindsitapi.in
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});
var app = builder.Build();
//app.UseCors(MyAllowSpecificOrigins);
app.UseIpRateLimiting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
//app.UseHttpsRedirection();

app.UseCookiePolicy(
new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always,
    MinimumSameSitePolicy = SameSiteMode.None
});

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();
