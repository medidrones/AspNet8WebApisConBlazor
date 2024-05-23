using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NetFirebase.Api.Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

FirebaseApp.Create(new AppOptions { 
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

app.Run();
