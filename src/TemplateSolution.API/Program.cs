#if (EnableSwagger)
using TemplateSolution.Infrastructure.OpenApi;
#endif

using TemplateSolution.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

#if (EnableSwagger)
builder.Services.AddApiSwagger();
#endif


builder.Services.AddDatabase(builder.Configuration);

var app = builder.Build();

#if (EnableSwagger)
app.UseApiSwagger();
#endif

app.MapGet("/", () => "it works");

app.Run();
