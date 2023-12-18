using EstanteMania.Email.Context;
using EstanteMania.Email.Repositories;
using GeekShopping.Email.MessageConsumer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EmailDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    x => x.MigrationsAssembly(typeof(EmailDbContext).Assembly.FullName)
));

var _builder = new DbContextOptionsBuilder<EmailDbContext>();
_builder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    x => x.MigrationsAssembly(typeof(EmailDbContext).Assembly.FullName)
);

builder.Services.AddSingleton(new EmailRepository(_builder.Options));
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddHostedService<RabbitMQPaymentConsumer>();

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
