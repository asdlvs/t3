using Inmeta.Moving.Services;
using Inmeta.Moving.DataAccess;
using Microsoft.EntityFrameworkCore;
using Inmeta.Moving.Services.Models;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>(
    options =>
    {
        options.UseSqlServer("name=Database:ConnectionString");
    },
    ServiceLifetime.Scoped
);

builder.Services.AddScoped<IOrdersDatabaseContext, DatabaseContext>();

builder.Services.AddScoped<OrdersService>();
builder.Services.AddScoped<IService<Order>>(s => s.GetRequiredService<OrdersService>());
builder.Services.AddScoped<ISearchingService<Order>>(s => s.GetRequiredService<OrdersService>());

builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<IService<Customer>>(s => s.GetRequiredService<CustomerService>());
builder.Services.AddScoped<ISearchingService<Customer>>(s => s.GetRequiredService<CustomerService>());

builder.Services.AddScoped<IService<Service>, ServicesService>();

builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver")
    );
});


builder.Services.AddVersionedApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureSecurity"));

if (builder.Environment.IsProduction())
{
    var keyVaultAddress = $"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/";
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultAddress),
        new DefaultAzureCredential());
}
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(
    options =>
    {
        options.WithOrigins(builder.Configuration["Origin"]).AllowCredentials();
        options.AllowAnyMethod();
        options.AllowAnyHeader();
    }
);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
