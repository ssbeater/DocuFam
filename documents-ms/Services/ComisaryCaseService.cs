using Google.Cloud.Firestore;

using documents_ms.Domain.Entities;
using documents_ms.Domain.Errors;
using documents_ms.Domain.Dtos;
using documents_ms.Data.Firestore;

namespace documents_ms.Services;

public class ComisaryCaseService
{
    private readonly FirestoreDb _firestore;

    public ComisaryCaseService(FirestoreDb firestore)
    {
        _firestore = firestore;
    }

    public async Task<IEnumerable<ComisaryCase>> GetCasesAsync()
    {
        try
        {
            var snapshot = await _firestore.Collection("ComisaryCases").GetSnapshotAsync();
            var cases = snapshot.Documents.Select(doc =>
            {
                var fsDoc = doc.ConvertTo<FSComisaryCase>();
                return ComisaryCase.FromObject(fsDoc);
            }).ToList();

            return cases;
        }
        catch (CustomError ex)
        {
            throw new CustomError(ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            throw CustomError.InternalServer(ex.Message);
        }
    }

    public async Task<ComisaryCase> GetCaseById(string id)
    {
        try
        {
            var docRef = _firestore.Collection("ComisaryCases").WhereEqualTo("Id", id);
            var querySnapshot = await docRef.GetSnapshotAsync();

            if (querySnapshot.Documents.Count == 0)
                throw CustomError.NotFound($"Case with id {id} not found");

            var fsDoc = querySnapshot.Documents[0].ConvertTo<FSComisaryCase>();
            return ComisaryCase.FromObject(fsDoc);
        }
        catch (CustomError ex)
        {
            throw new CustomError(ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            throw CustomError.InternalServer(ex.Message);
        }
    }

    public async Task<ComisaryCase> CreateCase(CreateComisaryCaseDto dto)
    {
        try
        {
            var prevDoc = _firestore
                .Collection("ComisaryCases")
                .WhereEqualTo("Id", dto.Id.ToString());

            var querySnapshot = await prevDoc.GetSnapshotAsync();
            if (querySnapshot.Documents.Count > 0)
                throw CustomError.BadRequest($"Case {dto.Id} already exists");

            var newCase = dto.ToComisaryCase();
            var docRef = await _firestore
                .Collection("ComisaryCases")
                .AddAsync(new FSComisaryCase()
                {
                    Id = newCase.Id,
                    Title = newCase.Title,
                    PatientId = newCase.PatientId,
                    OffenderId = newCase.OffenderId,
                    DateCreated = newCase.DateCreated.ToUniversalTime(),
                    DateUpdated = newCase.DateUpdated.ToUniversalTime(),
                    OpenCase = newCase.OpenCase
                });
            newCase.Id = docRef.Id;

            return newCase;
        }
        catch (CustomError ex)
        {
            throw new CustomError(ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            throw CustomError.InternalServer(ex.Message);
        }
    }
}
