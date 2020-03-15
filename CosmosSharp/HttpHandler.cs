using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CosmosSharp
{
    public class HttpHandler : IHttpHandler
    {
        async Task<TResponseData> IHttpHandler.GetJsonAsync<TResponseData>(string url, CancellationToken cancellationToken)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            var response = await client.ExecuteGetTaskAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return default;
            }
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw GetApiError(new Uri(url), request, response);
            }

            var result = JsonConvert.DeserializeObject<TResponseData>(response.Content);
            return result;
        }

        Task<TResponseData> IHttpHandler.PostJsonAsync<TResponseData, TRequestData>(string url, TRequestData requestBody)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            var body= JsonConvert.SerializeObject(requestBody);
            request.AddJsonBody(body);
            var response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw GetApiError(new Uri(url), request, response);
            }

            if(response.Content == null) throw new Exception($"Response is null. Status code: {response.StatusCode}");
            
            var result = JsonConvert.DeserializeObject<TResponseData>(response.Content);
            return Task.FromResult(result);
        }

        private Exception GetApiError(Uri BaseUrl, IRestRequest request, IRestResponse response)
        {
            //Get the values of the parameters passed to the API
            string parameters = string.Join(", ", request.Parameters.Select(x => x.Name.ToString() + "=" + ((x.Value == null) ? "NULL" : x.Value)).ToArray());

            //Set up the information message with the URL, the status code, and the parameters.
            string info = "Request to " + BaseUrl.AbsoluteUri + request.Resource + " failed with status code " + response.StatusCode + ", parameters: "
            + parameters + ", and content: " + response.Content;

            //Acquire the actual exception
            Exception ex;
            if (response != null && response.ErrorException != null)
            {
                ex = response.ErrorException;
            }
            else
            {
                ex = new Exception(info);
                info = string.Empty;
            }

            return ex;
        }
    }
}
