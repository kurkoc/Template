#if (EnableSwagger)
using Sample.Infrastructure.OpenApi;
#endif

var builder = WebApplication.CreateBuilder(args);

#if (EnableSwagger)
builder.Services.AddApiSwagger();
#endif

var app = builder.Build();

#if (EnableSwagger)
app.UseApiSwagger();
#endif

app.MapGet("/", () => "it works");

app.Run();
