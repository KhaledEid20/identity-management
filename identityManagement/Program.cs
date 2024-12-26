global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using identityManagement.Repositories;
global using identityManagement.Repositories.Base;
global using identityManagement.Models;
global using identityManagement.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddIdentity<User , Role>()
    .AddEntityFrameworkStores<AppdbContext>();;


builder.Services.AddDbContext<AppdbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IUnitofwork,Unitofwork>();
builder.Services.AddScoped<IAccount,AccoutServices>();
builder.Services.AddScoped<IAdmin,AdminServices>();
builder.Services.AddScoped<IClaims,ClaimsServices>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();