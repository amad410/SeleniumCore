using Framework.Enums;
using Framework.Handlers;
using Framework.Services;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Assemblies
{
    /// <summary>
    /// Base client class
    /// </summary>
    public class BaseClient : RestSharp.RestClient
    { //may have to switch to TestLogger.GetInstance()
        protected ICacheService _cache;
        protected ITestLogger _errorLogger;
        public RestClient client;
        IRestRequest request = null;
        IRestResponse response = null;
        string _token = null;
        public BaseClient(ICacheService cache,IDeserializer serializer, ITestLogger errorLogger,string baseUrl)
        {
            _cache = cache;
            _errorLogger = errorLogger;
            AddHandler("application/json", serializer);
            AddHandler("text/json", serializer);
            AddHandler("text/x-json", serializer);
            BaseUrl = new Uri(baseUrl);
            SetBackendURL(BaseUrl.ToString());
        }
        public void Authenticate(string authUrl, string username, string password)
        {
            client = new RestClient(authUrl);
            client.Authenticator = new HttpBasicAuthenticator("postman", "password");
            request = new RestRequest(Method.GET);
            response = client.Execute(request);
            
        }

        public String GetBearerToken(string tokenServiceURL, AuthenticationType type)
        {
            IRestClient client = null;
            
            string client_credentials = "";
            string client_id = "abc";
            string client_secret = "123";

            client = new RestClient(tokenServiceURL);
            request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", $"grant_type={client_credentials}&client={client_id}&client_secret={client_secret}", ParameterType.RequestBody);
            response = client.Execute(request);
            //authClient.Authenticator = new HttpBasicAuthenticator("client-app", "");

            /*switch (type)
            {
                case AuthenticationType.Basic:
                    client = new RestClient(tokenServiceURL);
                    client.Authenticator = new HttpBasicAuthenticator("postman", "password");
                    request = new RestRequest(Method.GET);
                    response = client.Execute(request);
                    break;
                case AuthenticationType.OAuth:
                    client = new RestClient(tokenServiceURL);
                    request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/x-www-form-urlencoded");
                    request.AddParameter("application/x-www-form-urlencoded", $"grant_type={client_credentials}&client={client_id}&client_secret={client_secret}", ParameterType.RequestBody);
                    response = client.Execute(request);
                    //authClient.Authenticator = new HttpBasicAuthenticator("client-app", "");
                    break;
            }*/
            dynamic respContent = JObject.Parse(response.Content);
            Token = respContent.access_token;
            return Token;

        }
        public RestClient SetBackendURL(string backendURL)
        {
            client = new RestClient(backendURL);
            return client;
        }
        public RestRequest Create(string endpoint, Method method)
        {
            RestRequest request = new RestRequest
            {
                Resource = endpoint,
                RequestFormat = DataFormat.Json,
                Method = method
            };
           
            return request;
        }
        public RestRequest Create(string endpoint, Method method, string token)
        {
            RestRequest request = new RestRequest
            {
                Resource = endpoint,
                RequestFormat = DataFormat.Json,
                Method = method
            };

            request.AddHeader("authorization", "Bearer " + token);
            return request;
        }
        public RestRequest Create(string endpoint, Method method, string username, string password)
        {
            RestRequest request = new RestRequest
            {
                Resource = endpoint,
                RequestFormat = DataFormat.Json,
                Method = method
            };

            request.AddHeader("authorization", "Bearer " /*+ token*/);
            return request;
        }
        private void TimeoutCheck(IRestRequest request, IRestResponse response)
        {
            if (response.StatusCode == 0)
            {
                LogError(BaseUrl, request, response);
            }
        }

        public override IRestResponse Execute(IRestRequest request)
        {
            var response = client.Execute(request);
            TimeoutCheck(request, response);
            return response;
        }
        
        public override IRestResponse<T> Execute<T>(IRestRequest request)
        {
            var response = client.Execute<T>(request);
            TimeoutCheck(request, response);
            return response;
        }
        public T Get<T>(IRestRequest request) where T : new()
        {
            var response = Execute<T>(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
            {
                LogError(BaseUrl, request, response);
                return default(T);
            }
        }
        public T GetFromCache<T>(IRestRequest request, string cacheKey) where T : class, new()
        {
            var item = _cache.Get<T>(cacheKey);
            if (item == null)
            {
                var response = Execute<T>(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _cache.Set(cacheKey, response.Data);
                    item = response.Data;
                }
                else
                {
                    LogError(BaseUrl, request, response);
                    return default(T);
                }
            }
            return item;
        }

        private void LogError(Uri BaseUrl,IRestRequest request,IRestResponse response)
        {
            //Get the values of the parameters passed to the API
            string parameters = string.Join(", ", request.Parameters.Select(x => x.Name.ToString() + "=" + ((x.Value == null) ? "NULL" : x.Value)).ToArray());

            //Set up the information message with the URL, 
            //the status code, and the parameters.
            string info = "Request to " + BaseUrl.AbsoluteUri
                          + request.Resource + " failed with status code "
                          + response.StatusCode + ", parameters: "
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

            //Log the exception and info message
            _errorLogger.Error(info + "/r/n" + ex.StackTrace);
            //_errorLogger.Error(ex, info);
        }

        public String Token
        {
            get
            {
                return _token;
            }
            set
            {
                _token = value;
            }
        }

    }

}
