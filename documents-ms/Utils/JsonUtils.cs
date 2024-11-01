using System;
using Newtonsoft.Json;

namespace documents_ms.Utils;

public class JsonUtils
{
    public static Dictionary<string, object> FromObjToPropertyDict(object obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));
        var json = JsonConvert.SerializeObject(obj);
        return JsonConvert.DeserializeObject<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();
    }
    public static T GetRequiredProperty<T>(Dictionary<string, object> propertyDict, string propertyName)
    {
        if (!propertyDict.TryGetValue(propertyName, out var value))
            throw new ArgumentException($"{propertyName} is required and must be of type {typeof(T).Name}");

        // Try to handle collections
        if (value is T typedValue)
        {
            return typedValue;
        }
        else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
        {
            var elementType = typeof(T).GetGenericArguments()[0];
            var list = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(value), typeof(T));

            if (list != null && list is T result)
                return result;
        }

        throw new ArgumentException($"{propertyName} must be of type {typeof(T).Name}");
    }

    public static T? GetOptionalProperty<T>(Dictionary<string, object> propertyDict, string propertyName) where T : struct
    {
        if (propertyDict.TryGetValue(propertyName, out var value))
        {
            if (value is T typedValue)
                return typedValue;
            else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                var elementType = typeof(T).GetGenericArguments()[0];
                var list = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(value), typeof(T));

                if (list != null && list is T result)
                    return result;
            }
        }

        return null;
    }

    public static T? GetOptionalReferenceProperty<T>(Dictionary<string, object> propertyDict, string propertyName) where T : class
    {
        if (propertyDict.TryGetValue(propertyName, out var value))
        {
            if (value is T typedValue)
                return typedValue;
            else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                var elementType = typeof(T).GetGenericArguments()[0];
                var list = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(value), typeof(T));

                if (list != null && list is T result)
                    return result;
            }
        }

        return null;
    }
}
