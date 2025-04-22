using Auker_Minitab_Interview_Assignment.Repositories;
using Auker_Minitab_Interview_Assignment.Repositories.Impl;
using Auker_Minitab_Interview_Assignment.Services;
using Auker_Minitab_Interview_Assignment.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

builder.Services.AddHttpClient<AddressValidationService>();
builder.Services.AddScoped<IAddressValidationService, AddressValidationService>();
builder.Services.AddScoped<ICrmRepository, FakeCrmRepository>();

app.UseHttpsRedirection();

app.Run();