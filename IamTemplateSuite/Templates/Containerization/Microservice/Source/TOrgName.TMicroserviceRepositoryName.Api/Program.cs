using TOrgName.TMicroserviceRepositoryName.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.ConfigureApiDocumentation()
    .ConfigureApiVersioning()
    .ConfigureOptions();
builder.Configuration.ConfigureKeyVault();

var app = builder.Build();

app.ConfigureApiDocumentation();

app.MapEndpoints();

app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();
