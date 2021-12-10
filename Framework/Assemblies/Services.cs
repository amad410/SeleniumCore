using Framework.Handlers;
using Framework.Services;
using Framework.Services.SampleClient;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Assemblies
{
    public class Services
    {
        SampleUsersClient _sampleUsersClient;
        ICacheService cache;
        IDeserializer serializer;
        ITestLogger errorLog;
        public Services()
        {
            
        }

        public void Register()
        {
            _sampleUsersClient = new SampleUsersClient(cache, serializer, errorLog);
        }

        public SampleUsersClient SampleUsersClient
        {
            get
            {
                return _sampleUsersClient;

            }

        }
    }
}
