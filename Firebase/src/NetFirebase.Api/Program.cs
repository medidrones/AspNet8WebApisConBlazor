using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using NetFirebase.Api.Data;
using NetFirebase.Api.Extensions;
using NetFirebase.Api.Services.Authentication;
using NetFirebase.Api.Services.Productos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("ConnectionString") ?? 
    throw new ArgumentNullException("No tiene cadena conexion");

builder.Services.AddDbContext<DatabaseContext>(options => 
{
    options.UseNpgsql(connectionString);
});

FirebaseApp.Create(new AppOptions 
{ 
    Credential = GoogleCredential.FromFile("firebase.json")
});

builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>((sp, httpClient) => 
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    httpClient.BaseAddress = new Uri(configuration["Authentication:TokenUri"]!);
});

builder.Services
    .AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
    {
        jwtOptions.Authority = builder.Configuration["Authentication:ValidIssuer"];
        jwtOptions.Audience = builder.Configuration["Authentication:Audience"];
        jwtOptions.TokenValidationParameters.ValidIssuer = builder.Configuration["Authentication:ValidIssuer"];
    });

/*builder.Services.AddDbContext<DatabaseContext>(opt =>
{
    opt.LogTo(Console.WriteLine, new[] 
    { 
        DbLoggerCategory.Database.Command.Name 
    }, 
    LogLevel.Information).EnableSensitiveDataLogging();

    opt.UseSqlite(builder.Configuration.GetConnectionString("SqliteDatabase"));
});*/

builder.Services.AddScoped<IProductoService, ProductoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.AddDataPrueba();

app.Run();
