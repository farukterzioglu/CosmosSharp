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
    public interface IRestClientFactory
    {
        RestClient Create(string baseUrl);
    }
    public class RestClientFactory : IRestClientFactory
    {
        RestClient IRestClientFactory.Create(string baseUrl)
        {
            return new RestClient(baseUrl);
        }
    }

    public interface IRestRequestFactory
    {
        RestRequest Create(Method method);
        RestRequest Create(string url, Method method);
    }
    public class RestRequestFactory : IRestRequestFactory
    {
        RestRequest IRestRequestFactory.Create(string url, Method method)
        {
            return new RestRequest(url, method);
        }

        RestRequest IRestRequestFactory.Create(Method method)
        {
            return new RestRequest(method);
        }
    }

    public interface IHttpHandler
    {
        Task<TResponseData> GetJsonAsync<TResponseData>(string url, CancellationToken cancellationToken);
        Task<TResponseData> PostJsonAsync<TResponseData, TRequestData>(string url, TRequestData requestBody);
        Task<IRestResponse> ExecuteGet(string url, CancellationToken cancellationToken);
    }

    public class HttpHandler : IHttpHandler
    {
        private IRestClientFactory restClientFactory; 
        private IRestRequestFactory restRequestFactory;

        public HttpHandler()
        {
            this.restClientFactory = new RestClientFactory();
            this.restRequestFactory = new RestRequestFactory();
        }

        public HttpHandler(IRestClientFactory restClientFactory, IRestRequestFactory restRequestFactory)
        {
            this.restClientFactory = restClientFactory;
            this.restRequestFactory = restRequestFactory;
        }

        Task<IRestResponse> IHttpHandler.ExecuteGet(string url, CancellationToken cancellationToken)
        {
            var restClient = restClientFactory.Create(url);
            var request = restRequestFactory.Create(Method.GET);

            return restClient.ExecuteGetTaskAsync(request, cancellationToken);
        }

        async Task<TResponseData> IHttpHandler.GetJsonAsync<TResponseData>(string url, CancellationToken cancellationToken)
        {
            var restClient = restClientFactory.Create(url);
            var request = restRequestFactory.Create(Method.GET);

            var response = await restClient.ExecuteGetTaskAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return default;
            }
            if (response.StatusCode != HttpStatusCode.OK || response.ErrorException != null)
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
