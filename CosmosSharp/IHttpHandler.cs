/// Reference: https://github.com/GetScatter/eos-sharp

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CosmosSharp
{
    public interface IHttpHandler
    {
        Task<TResponseData> GetJsonAsync<TResponseData>(string url, Dictionary<string, string> headerKeyValues, CancellationToken cancellationToken);
        Task<TResponseData> PostJsonAsync<TResponseData, TRequestData>(string url, Dictionary<string, string> headerKeyValues, TRequestData requestBody);
    }
}