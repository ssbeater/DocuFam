using Microsoft.Extensions.Options;
using documents_ms.Services;
using documents_ms.Data.Firestore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<FirestoreSettings>(builder.Configuration.GetSection("FirestoreSettings"));
builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<FirestoreSettings>>().Value;
    return new FirestoreService(settings).FirestoreDb;
});

builder.Services.AddScoped<PeopleService>();
builder.Services.AddScoped<ComisaryCaseService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
