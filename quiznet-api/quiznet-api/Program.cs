using Microsoft.EntityFrameworkCore;
using quiznet_api.Data;
using quiznet_api.Repository;
using quiznet_api.Repository.IRepository;
using AutoMapper;
using quiznet_api;
using quiznet_api.Services.IServices;
using quiznet_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using dotenv.net;
using quiznet_api.Handlers.IHandlers;
using quiznet_api.Handlers;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();



//  Database connection configuration

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
});

builder.Services.AddAutoMapper(typeof(MappingConfig));

// Repositories

builder.Services.AddScoped<IWordRepository, WordRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameRoundRepository, GameRoundRepository>();
builder.Services.AddScoped<IPlayerAnswerRepository, PlayerAnswerRepository>();
builder.Services.AddScoped<IFriendshipRepository, FriendshipRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IBlockedPlayerRepository, BlockedPlayerRepository>();

// Services

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IMessageService, MessageService>();  

// Handlers

builder.Services.AddScoped<IJwtHandler, JwtHandler>();

// Authentication Configuration

var key = Environment.GetEnvironmentVariable("SECRET_KEY");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Cors configuration

app.UseCors(x => x.AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .WithOrigins("http://127.0.0.1:5173")
    );

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
