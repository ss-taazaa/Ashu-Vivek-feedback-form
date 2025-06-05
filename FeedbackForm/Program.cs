using FeedbackForm.Data;
using FeedbackForm.Repositories.Implementations;
using FeedbackForm.Repositories.Interfaces;
using FeedbackForm.Services.Implementations;
using FeedbackForm.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IFormService, FormService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IResponseService, ResponseService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
=======
// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IFormService, FormService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IResponseService, ResponseService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
>>>>>>> f76708e4a2fe714f50c934da197d8353f2a3a727
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

<<<<<<< HEAD
=======
// Configure the HTTP request pipeline
>>>>>>> f76708e4a2fe714f50c934da197d8353f2a3a727
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
<<<<<<< HEAD
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
=======
app.UseAuthorization();
app.UseCors("AllowAll");

app.MapControllers();

>>>>>>> f76708e4a2fe714f50c934da197d8353f2a3a727
app.Run();
