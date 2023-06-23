using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Web;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureService(builder.Services);

var app = builder.Build();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
startup.Configure(app, builder.Environment, provider);
