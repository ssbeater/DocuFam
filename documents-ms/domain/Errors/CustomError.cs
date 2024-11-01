namespace documents_ms.Domain.Errors;

public class CustomError : Exception
{
    public int StatusCode { get; }

    public CustomError(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    // Métodos estáticos para crear instancias de errores específicos
    public static CustomError BadRequest(string message)
    {
        return new CustomError(400, message);
    }

    public static CustomError Unauthorized(string message)
    {
        return new CustomError(401, message);
    }

    public static CustomError Forbidden(string message)
    {
        return new CustomError(403, message);
    }

    public static CustomError NotFound(string message)
    {
        return new CustomError(404, message);
    }

    public static CustomError InternalServer(string message = "Internal server error")
    {
        return new CustomError(500, message);
    }
}
