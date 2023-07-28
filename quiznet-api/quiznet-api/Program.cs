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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});


builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddScoped<IWordRepository, WordRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameRoundRepository, GameRoundRepository>();
builder.Services.AddScoped<IPlayerAnswerRepository, PlayerAnswerRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IGameService, GameService>();

var key = builder.Configuration.GetValue<string>("ApiSettings:SecretKey");

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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .WithOrigins("http://127.0.0.1:5173")
    );

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
