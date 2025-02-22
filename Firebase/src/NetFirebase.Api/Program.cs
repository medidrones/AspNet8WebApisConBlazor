using AutoMapper;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NetFirebase.Api;
using NetFirebase.Api.Authentication;
using NetFirebase.Api.Data;
using NetFirebase.Api.Extensions;
using NetFirebase.Api.Mappings;
using NetFirebase.Api.Pagination;
using NetFirebase.Api.Services.Authentication;
using NetFirebase.Api.Services.Permisos;
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

builder.Services.AddSignalR();
builder.Services.AddHostedService<ServerNotifier>();

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

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationHandler, PermisoAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermisoAuthorizationPolicyProvider>();
builder.Services.AddScoped<IPermisoService, PermisoService>();

/*builder.Services.AddDbContext<DatabaseContext>(opt =>
{
    opt.LogTo(Console.WriteLine, new[] 
    { 
        DbLoggerCategory.Database.Command.Name 
    }, 
    LogLevel.Information).EnableSensitiveDataLogging();

    opt.UseSqlite(builder.Configuration.GetConnectionString("SqliteDatabase"));
});*/

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IPagedList, PagedList>();

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.AddDataPrueba();

app.MapHub<NotificationHub>("notifications");

app.Run();
