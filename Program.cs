using WebApi;

var builder = WebApplication.CreateBuilder(args);

var startup = new StartUp(builder.Configuration);

startup.ConfigureServices(builder.Services);


var app = builder.Build();

startup.Confugure(app, app.Environment);

app.Run();
