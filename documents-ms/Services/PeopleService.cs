using System;
using documents_ms.Domain.Errors;

namespace documents_ms.Services;

public class PeopleService
{
    private string BaseUrl { get; set; }
    private HttpClient _httpClient { get; set; }

    public PeopleService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        BaseUrl = configuration
            .GetSection("ApiSetting")
            .GetValue<string>("PeopleBaseUrl") ?? "";
    }

    public async Task<bool> ValidatePeopleId(Guid id)
    {
        var peopleId = id.ToString();
        string url = $"{BaseUrl}/People/{peopleId}";

        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/plain"));

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            return false;
        }
        catch
        {
            throw CustomError.InternalServer("Error validating people id");
        }
    }
}
