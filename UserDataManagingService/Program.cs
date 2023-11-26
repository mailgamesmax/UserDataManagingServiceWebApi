using Microsoft.OpenApi.Models;
using UserDataManagingService.Models.Repositories;
using UserDataManagingService;
using Microsoft.EntityFrameworkCore;
using UserDataManagingService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

    var connectionString = builder.Configuration.GetConnectionString("Database"); //duomenys sukurti appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString); //dbcontext serviso registravimas
},ServiceLifetime.Scoped);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserLoginService, UserLoginService>();
builder.Services.AddScoped<ILivingPlaceRepository, LivingPlaceRepository>();
builder.Services.AddScoped<ILivingPlaceEditService, LivingPlaceEditService>();
builder.Services.AddScoped<IAvatarRepository, AvatarRepository>();
builder.Services.AddScoped<IAvatarCRUDService, AvatarCRUDService>();
builder.Services.AddScoped<IPersonalInfoUpdateService, PersonalInfoUpdateService>();
builder.Services.AddScoped<IAdminService, AdminService>();
//+
builder.Services.AddHttpContextAccessor();





builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


builder.Services.AddCors(); //apsaugos nuemimas

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(); naudojant jwt - variantas zemiau
builder.Services.AddSwaggerGen(options =>
{
    // these options adds possibility to pass JWT through Swagger

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

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
