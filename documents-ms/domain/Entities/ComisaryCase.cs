using documents_ms.Utils;

namespace documents_ms.Domain.Entities;

public class ComisaryCase
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string PatientId { get; set; }
    public string OffenderId { get; set; }
    public List<string> ProcessesList { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    public bool OpenCase { get; set; }

    public ComisaryCase(
        string id,
        string title,
        string patientId,
        string offenderId,
        DateTime dateCreated,
        DateTime dateUpdated,
        bool? openCase,
        List<string>? processesList)
    {
        Id = id;
        Title = title;
        PatientId = patientId;
        OffenderId = offenderId;
        ProcessesList = processesList ?? new List<string>();
        DateCreated = dateCreated;
        DateUpdated = dateUpdated;
        OpenCase = openCase ?? true;
    }

    public static ComisaryCase FromObject(object obj)
    {
        var propertyDict = JsonUtils.FromObjToPropertyDict(obj);

        string id = JsonUtils.GetRequiredProperty<string>(propertyDict, "Id");
        string title = JsonUtils.GetRequiredProperty<string>(propertyDict, "Title");
        string patientId = JsonUtils.GetRequiredProperty<string>(propertyDict, "PatientId");
        string offenderId = JsonUtils.GetRequiredProperty<string>(propertyDict, "OffenderId");

        DateTime dateCreated = JsonUtils.GetOptionalProperty<DateTime>(propertyDict, "DateCreated") ?? new DateTime();
        DateTime dateUpdated = JsonUtils.GetOptionalProperty<DateTime>(propertyDict, "DateUpdated") ?? new DateTime();
        bool openCase = JsonUtils.GetOptionalProperty<bool>(propertyDict, "OpenCase") ?? true;

        List<string> processesList = JsonUtils.GetOptionalReferenceProperty<List<string>>(propertyDict, "ProcessesList") ?? new List<string>();


        return new ComisaryCase(
            id,
            title,
            patientId,
            offenderId,
            dateCreated,
            dateUpdated,
            openCase,
            processesList
        );
    }

}
