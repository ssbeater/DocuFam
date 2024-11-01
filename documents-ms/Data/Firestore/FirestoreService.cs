using Google.Api.Gax;
using Google.Cloud.Firestore;

namespace documents_ms.Data.Firestore;

public class FirestoreService
{
    public FirestoreDb FirestoreDb { get; }

    public FirestoreService(FirestoreSettings settings)
    {
        if (settings.UseEmulator)
        {
            Environment.SetEnvironmentVariable("FIRESTORE_EMULATOR_HOST", settings.EmulatorHost);
            FirestoreDb = new FirestoreDbBuilder
            {
                ProjectId = settings.ProjectId,
                EmulatorDetection = EmulatorDetection.EmulatorOrProduction
            }.Build();
        }
        else
        {
            FirestoreDb = new FirestoreDbBuilder
            {
                ProjectId = settings.ProjectId,
                CredentialsPath = settings.CredentialPath,
                DatabaseId = settings.DatabaseId
            }.Build();
        }
    }
}
