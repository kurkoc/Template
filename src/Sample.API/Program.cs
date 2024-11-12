var builder = WebApplication.CreateBuilder(args);

#if (EnableSwagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endif

var app = builder.Build();

#if (EnableSwagger)
app.UseSwagger();
app.UseSwaggerUI();
#endif

app.MapGet("/", () => "it works");

app.Run();
