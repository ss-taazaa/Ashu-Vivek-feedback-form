//using FeedbackForm.Data;
using FeedbackForm.Middlewares;
using FeedbackForm.Models;
using FeedbackForm.Repositories.Implementations;
using FeedbackForm.Repositories.Interfaces;
using FeedbackForm.Services.Implementations;
using FeedbackForm.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IFormService, FormService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IResponseService, ResponseService>();
builder.Services.AddScoped<IFormRepository, FormRepository>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});



builder.Services.AddControllers(options =>
{
    options.Filters.Add<RequestResponseLoggingFilter>();
});


builder.Services.AddLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
