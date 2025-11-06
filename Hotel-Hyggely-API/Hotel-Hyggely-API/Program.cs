using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Services;
using Hotel_Hyggely_API.Middleware;
using Infrastructure.MappingProfiles;
using Infrastructure.Persistance;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Scalar.AspNetCore;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Error()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Fatal)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
// Add services to the container.
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IStaffRepo, StaffRepo>();
builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddScoped<IBookingRepo, BookingRepo>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IRoomTypeRepo, RoomTypeRepo>();
builder.Services.AddScoped<RoomTypeService>();
builder.Services.AddScoped<IRoomRepo, RoomRepo>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddAutoMapper(cf => { }, typeof(BookingProfile).Assembly);
builder.Services.AddScoped<IStaffRepo, StaffRepo>();
builder.Services.AddScoped<StaffService>();
builder.Services.AddScoped<IRoomTypeImageRepo, RoomTypeImageRepo>();
builder.Services.AddScoped<RoomTypeImageService>();
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: "allowall",
					  policy =>
					  {
                          policy.AllowAnyOrigin();
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
					  });
});

//builder.Services.AddAuthentication().AddJwtBearer();
//Console.WriteLine(builder.Services.AddAuthentication().AddJwtBearer("Bearer"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication()
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["ValidIssuer"],
        ValidAudience = jwtSettings["ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["IssuerSigningKey"]!))
    };
});

builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.Create(new Version(8, 4, 7),ServerType.MySql)));

var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1"));

    app.UseReDoc(options => options.SpecUrl("/openapi/v1.json"));

    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors("allowall"); // TODO: REMOVE CORS
app.UseAuthentication();
app.UseAuthorization();

DbInitializer.Seed(app.Services);

app.MapControllers();

app.Run();
