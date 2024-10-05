﻿using MagicVilla_API.Modelos;
using MagicVilla_Utilidad;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory _httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            _httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("MagicAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);

                if (apiRequest.Datos != null) //cuando es diferente a null -> es un POST/PUT (envian datos)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Datos), Encoding.UTF8, "application/json");
                }

                switch (apiRequest.APITipo)
                {
                    case DS.APITipo.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case DS.APITipo.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case DS.APITipo.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null; //almacena la respuesta de la API
                apiResponse = await client.SendAsync(message); //le pasas el 'message' con toda la info
                var apiContent = await apiResponse.Content.ReadAsStringAsync(); //aca se recibe la respuesta
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent); //deserializas el contenido - convierte a un objeto
                return APIResponse; //return del APIResponse

            }
            catch (Exception ex)
            {

                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsExitoso = false,
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
        }
    }
}
