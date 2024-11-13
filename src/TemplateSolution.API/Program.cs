#if (EnableSwagger)
using TemplateSolution.Infrastructure.OpenApi;
#endif
#if (EnableJwt)
using TemplateSolution.Infrastructure.Authentication;
#endif

using TemplateSolution.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

#if (EnableSwagger)
builder.Services.AddApiSwagger();
#endif

#if (EnableJwt)
builder.Services.AddJwtToken(builder.Configuration);
#endif

builder.Services.AddDatabase(builder.Configuration);

var app = builder.Build();
#if (EnableJwt)
app.UseJwt();
#endif

#if (EnableSwagger)
app.UseApiSwagger();
#endif

app.MapGet("/", () => "it works - CurrentAppYear");

#if (EnableJwt)
app.MapGet("/secure", () => "it works - secure").RequireAuthorization();
#endif

app.Run();
