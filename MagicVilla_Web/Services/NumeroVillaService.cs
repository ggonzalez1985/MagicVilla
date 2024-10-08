﻿using MagicVilla_Utilidad;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class NumeroVillaService : BaseService, INumeroVillaService
    {
        //Extiende el BaseService, lo que significa que puede aprovechar el método SendAsync para realizar las peticiones HTTP sin tener que implementarlas de nuevo.
        //Usa BaseService para realizar las siguientes acciones:ObtenerTodos: obtiene todas las "villas".Crear: crea una nueva villa usando un DTO
        //VillaService utiliza esa funcionalidad para operaciones específicas relacionadas con las villas.
        //VillaService en la lógica de negocio específica del recurso "Villa"

        public readonly IHttpClientFactory _httpClient;
        public string _villaUrl;
        public NumeroVillaService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _villaUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }

        public Task<T> Actualizar<T>(NumeroVillaUpdateDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.PUT,
                Datos = dto,
                Url = _villaUrl + "/api/NumeroVilla/" + dto.VillaNo
            });
        }

        public Task<T> Crear<T>(NumeroVillaCreateDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = _villaUrl + "/api/NumeroVilla"
			});
        }

        public Task<T> Obtener<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _villaUrl + "/api/NumeroVilla/" + id
            });
        }

        public Task<T> ObtenerTodos<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _villaUrl + "/api/NumeroVilla"
			});
        }

        public Task<T> Remover<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.DELETE,
                Url = _villaUrl + "/api/NumeroVilla/" + id
            });
        }
    }
}
