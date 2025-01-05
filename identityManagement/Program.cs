global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using identityManagement.Repositories;
global using identityManagement.Repositories.Base;
global using identityManagement.Models;
global using identityManagement.Data;
global using identityManagement.DTOs;
using Microsoft.EntityFrameworkCore;
using identityManagement.Extention;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://localhost:7037");

builder.Services.AddControllers();
builder.Services.AddIdentity<User , IdentityRole>(o=>{
    o.Password.RequireDigit = true;
    o.Password.RequireLowercase = false; 
    o.Password.RequireUppercase = false; 
    o.Password.RequireNonAlphanumeric = false; 
    o.Password.RequiredLength = 10; 
    o.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<AppdbContext>();;


builder.Services.AddDbContext<AppdbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.configureJwt(builder.Configuration); // The Jwt Configuration

builder.Services.AddScoped<IUnitofwork,Unitofwork>();
builder.Services.AddScoped<IAuthentication,AuthenticationService>();
builder.Services.AddScoped<IAdmin,AdminServices>();
builder.Services.AddScoped<IClaims,ClaimsServices>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();