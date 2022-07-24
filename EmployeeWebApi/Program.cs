using Microsoft.EntityFrameworkCore;
using EmployeeWebApi.DbContexts;
using EmployeeWebApi.Services;
using EmployeeWebApi.Models;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
})
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters()
    .AddFluentValidation(options =>
    {
        options.RegisterValidatorsFromAssemblyContaining<EmployeeUpdateDto>();
    }); 


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EmployeeContext>(
    dbContextOptions => dbContextOptions.UseSqlite(
        builder.Configuration["ConnectionStrings:EmployeeDBConnectionString"]));


builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
