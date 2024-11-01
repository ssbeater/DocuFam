using System;
using documents_ms.Domain.Entities;

namespace documents_ms.Domain.Dtos;

public class CreateComisaryCaseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Guid PatientId { get; set; }
    public Guid OffenderId { get; set; }

    public CreateComisaryCaseDto(Guid id, string title, Guid patientId, Guid offenderId)
    {
        Id = id;
        Title = title;
        PatientId = patientId;
        OffenderId = offenderId;
    }

    public static DtoResponse<CreateComisaryCaseDto> Create(Guid id, string title, Guid patientId, Guid offenderId)
    {
        if (id == Guid.Empty)
            return new DtoResponse<CreateComisaryCaseDto>(new Exception("Id is required"), null);

        if (String.IsNullOrEmpty(title))
            return new DtoResponse<CreateComisaryCaseDto>(new Exception("Title is required"), null);

        if (patientId == Guid.Empty)
            return new DtoResponse<CreateComisaryCaseDto>(new Exception("patientId is required"), null);

        if (offenderId == Guid.Empty)
            return new DtoResponse<CreateComisaryCaseDto>(new Exception("offenderId is required"), null);

        var dto = new CreateComisaryCaseDto(id, title, patientId, offenderId);
        return new DtoResponse<CreateComisaryCaseDto>(null, dto);
    }

    public ComisaryCase ToComisaryCase()
    {
        return new ComisaryCase(
            Id.ToString(),
            Title,
            PatientId.ToString(),
            OffenderId.ToString(),
            DateTime.Now,
            DateTime.Now,
            true,
            new List<string>()
        );
    }
}
