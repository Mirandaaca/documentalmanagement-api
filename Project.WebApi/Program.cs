var builder = WebApplication.CreateBuilder(args);

Project.WebApi.Startup startup = new Project.WebApi.Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);

app.Run();
