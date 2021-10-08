using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VaultLibrary
{
    public class Vault
    {
        private string _url;
        private string _token;
        private static readonly HttpClient client = new HttpClient();

        public Vault(string url, string token)
        {
            _url = url;
            _token = token;

        }

        public async Task<T> GetValueAsync<T>(string storage, string key)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{_url}/{storage}/{key}");
            request.Headers.Add("X-Vault-Token", _token);
            try
            {
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string SRequest = await response.Content.ReadAsStringAsync();
                    var vRequest = JsonSerializer.Deserialize<VRequest<T>>(SRequest);
                    return vRequest.Data;
                }
                else
                {
                    string str = "";
                    if (response.Content != null)
                        str = await response.Content.ReadAsStringAsync();
                    return new VRequest<T>().Data;
                }
            }
            catch (HttpRequestException)
            {
                return new VRequest<T>().Data;
            }
        }

        public async Task SetValueAsync<T>(string storage, string key, T value)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{_url}/{storage}/{key}");
            request.Headers.Add("X-Vault-Token", _token);
            var sValue = JsonSerializer.Serialize(value);
            using (var stringContent = new StringContent(sValue, Encoding.UTF8, "application/json"))
            {
                request.Content = stringContent;

                using (var response = await client
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                    .ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }
    }
}
