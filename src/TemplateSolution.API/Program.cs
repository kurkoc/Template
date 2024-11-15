using TemplateSolution.Application;
using TemplateSolution.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();
app.UseInfrastructure();

app.MapGet("/", () => "it works - CurrentAppYear");

app.Run();
