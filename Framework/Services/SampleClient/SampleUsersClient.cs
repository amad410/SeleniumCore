using Framework.Assemblies;
using Framework.Enums;
using Framework.Handlers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Framework.Services.SampleClient
{
    //may have to switch to TestLogger.GetInstance()
    public class SampleUsersClient : BaseClient
    {
        protected ITestLogger _errorLogger;
        public SampleUsersClient(ICacheService cache,IDeserializer serializer, ITestLogger errorLogger) : base(cache, serializer, errorLogger, "https://reqres.in/") {
            _errorLogger = errorLogger;
        }

        public IRestResponse GetListOfUser()
        {
            IRestResponse response = null;
            try
            {
                GetBearerToken("https://postman-echo.com/basic-auth/", AuthenticationType.Basic);
   
               // RestRequest request = new RestRequest("api/users?page=2", Method.GET);
                RestRequest request = Create("api/users?page=2", Method.GET);
                response = Execute(request);
            }
            catch(Exception ex)
            {
                _errorLogger.Error($"Problem getting a list of users with method {Method.GET}" + "\r\n" + ex.StackTrace);
                throw new Exception(ex.Message);
            }
            
            return response;
        }
        public IRestResponse CreateUser()
        {
            IRestResponse response = null;
            try
            {
                RestRequest request = Create("api/users", Method.POST);
                string jsonToSend = File.ReadAllText(System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).
                    Parent.Parent.FullName + @"\..\Framework\Services\SampleClient\User.json");
                request.AddParameter("application/json", jsonToSend, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;
                response = Execute(request);
            }
            catch (Exception ex)
            {
                _errorLogger.Error($"Problem creating a user with method {Method.POST}" + "\r\n" + ex.StackTrace);
                throw new Exception(ex.Message);
            }
            
            return response;
        }
    }
}
