using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Web;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found!");

var startup = new Startup(builder.Configuration, connectionString);
startup.ConfigureService(builder.Services);

var app = builder.Build();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
startup.Configure(app, builder.Environment, provider);
