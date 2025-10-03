using Application.Abstraction;
using Application.Interfaces;
using Application.Services;
using Application.Services.Implementations;

using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<GymDbContext>(options => 
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  //  .AddJwtBearer(options =>
    //{
      //  options.TokenValidationParameters = new TokenValidationParameters
        //{
          //  ValidateIssuer = true,
            //ValidateAudience = true,
            //ValidateLifetime = true,
            //ValidateIssuerSigningKey = true,
            //ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //ValidAudience = builder.Configuration["Jwt:Audience"],
            //IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        //};
    //});

//builder.Services.AddAuthorization(options =>
//{
  //  options.AddPolicy(nameof(TypeRole.Socio), policy => policy.RequireRole(nameof(TypeRole.Socio)));
    //options.AddPolicy(nameof(TypeRole.Administrador), policy => policy.RequireRole(nameof(TypeRole.Administrador)));
    //options.AddPolicy(nameof(TypeRole.SuperAdministrador), policy => policy.RequireRole(nameof(TypeRole.SuperAdministrador)));
//});

builder.Services.AddScoped<IGymClassService, GymClassService>();
builder.Services.AddScoped<IGymClassRepository, GymClassRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
