using Newtonsoft.Json;
using System.Text;

public class ApiRequestHelper
{
    private readonly HttpClient _httpClient;

    public ApiRequestHelper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        var response = await _httpClient.GetStringAsync(url);

        return JsonConvert.DeserializeObject<T>(response); 
    }

    public async Task<HttpResponseMessage> PostAsync<T>(T requestBody, string url)
    {
        var jsonContent = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);

        return response;
    }

    public async Task<HttpResponseMessage> PutAsync<T>(T requestBody, string url)
    {
        var jsonContent = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync(url, content);

        return response;
    }

    public async Task<HttpResponseMessage> DeleteAsync(string url)
    {
        var response = await _httpClient.DeleteAsync(url);
        
        return response;
    }
}
