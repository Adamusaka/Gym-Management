using backend.Services;
using backend.Layers.BLL;
using backend.Layers.DAL;
using backend.Layers.VL;
using backend.Interfaces.Services;
using backend.Interfaces.Layers.BLL;
using backend.Interfaces.Layers.DAL;
using backend.Interfaces.Layers.VL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy  =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


//BLL Binds
builder.Services.AddTransient<IAuthenticationLogic, AuthenticationLogic>();
builder.Services.AddTransient<IGymVisitLogic, GymVisitLogic>();
builder.Services.AddTransient<IUserLogic, UserLogic>();
builder.Services.AddTransient<ISubscriptionLogic, SubscriptionLogic>();

//VL Binds
builder.Services.AddTransient<IAuthenticationValidation, AuthenticationVaildation>();
builder.Services.AddTransient<IGymVisitValidation, GymVisitValidation>();
builder.Services.AddTransient<IUserValidation, UserValidation>();
builder.Services.AddTransient<ISubscriptionValidation, SubscriptionValidation>();

//DAL Binds
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IGymVisitRepository, GymVisitRepository>();
builder.Services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();


//Services Binds
builder.Services.AddTransient<IDatabaseService, DatabaseService>();

var app = builder.Build();

app.UseCors();

app.UseHttpsRedirection();

app.MapControllers();

app.UseRouting();

app.UseAuthorization();

app.Run();