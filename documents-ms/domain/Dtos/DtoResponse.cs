using System;

namespace documents_ms.Domain.Dtos;

public class DtoResponse<T>
{
    public Exception? error { get; set; }
    public T? Value { get; set; }

    public DtoResponse(Exception? error, T? value){
        this.error = error;
        this.Value = value;
    }
}
