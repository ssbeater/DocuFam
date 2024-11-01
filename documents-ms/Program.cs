using Google.Api.Gax;
using Google.Cloud.Firestore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

var firestoreSettings = builder.Configuration.GetSection("FirestoreSettings");
var useEmulator = firestoreSettings.GetValue<bool>("UseEmulator");
var emulatorHost = firestoreSettings.GetValue<string>("EmulatorHost");
var credentialPath = firestoreSettings.GetValue<string>("CredentialPath");
var projectId = firestoreSettings.GetValue<string>("ProjectId");
var databaseId = firestoreSettings.GetValue<string>("DatabaseId");

Console.WriteLine(projectId);

// Firestore service
FirestoreDb firestoreDb;

if (useEmulator)
{
    Environment.SetEnvironmentVariable("FIRESTORE_EMULATOR_HOST", emulatorHost);
    firestoreDb = new FirestoreDbBuilder
    {
        ProjectId = projectId,
        EmulatorDetection = EmulatorDetection.EmulatorOrProduction
    }.Build();
}
else
{
    firestoreDb = new FirestoreDbBuilder
    {
        ProjectId = projectId,
        CredentialsPath = credentialPath,
        DatabaseId = databaseId
    }.Build();
}

builder.Services.AddSingleton(firestoreDb);

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
