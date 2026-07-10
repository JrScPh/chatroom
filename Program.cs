using ChatroomAPI.Data;
using ChatroomAPI.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder => policyBuilder
            .WithOrigins("http://localhost:5173") // client origin (scheme + host + port)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()); // required for credentialed requests / some SignalR scenarios
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Make sure CORS middleware runs before endpoints
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Ensure the hub endpoint uses the same CORS policy
app.MapHub<ChatHub>("/hubs/chat").RequireCors("AllowSpecificOrigin");

app.Run();
