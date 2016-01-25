using PetAdopt.DTO;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Net;

namespace PetAdopt.Logic
{
    public class Client
    {
        string ApiBaseUrl = "http://data.coa.gov.tw/Service/OpenData/AnimalOpenData.aspx";
        private RestClient _restClient;

        public Client()
        {
            _restClient = new RestClient(ApiBaseUrl);
        }

        public List<AnimalInfo> GetAnimalInfo()
        {
            var request = new RestRequest(Method.GET);

            return Execute<AnimalInfo>(request);
        }

        private List<T> Execute<T>(IRestRequest request) where T : new()
        {
            IRestResponse<T> response;
            response = _restClient.Execute<T>(request);


            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception();

            RestSharp.Deserializers.JsonDeserializer deserial = new JsonDeserializer();
            var peopleList = deserial.Deserialize<List<T>>(response);

            return peopleList;
        }
    }
}