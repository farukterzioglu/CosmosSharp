/// Reference: https://github.com/GetScatter/eos-sharp
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CosmosSharp
{
    public interface IHttpHandler
    {
        Task<TResponseData> GetJsonAsync<TResponseData>(string url, CancellationToken cancellationToken);
        Task<TResponseData> PostJsonAsync<TResponseData, TRequestData>(string url, TRequestData requestBody);
    }
}