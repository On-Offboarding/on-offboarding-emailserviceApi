using OnOffboarding_EmailApi.Configuration;
using OnOffboarding_EmailApi.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();



// KONFIGURATION

builder.Services.Configure<AzureEmailSettings>(builder.Configuration.GetSection(AzureEmailSettings.SectionName));

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection(EmailSettings.SectionName));

// Add services to the container.
builder.Services.AddScoped<IEmailService, EmailService>();



//builder.Services.AddMemoryCache();

var app = builder.Build();
app.MapOpenApi();
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
