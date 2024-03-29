﻿using Contracts.DomainEntitites;
using Contracts.RepositoryContracts;
using InfrastructureData.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InfrastructureData.RepositoryImplementations
{
    public class RecetasRepository : IRecetasRepository
    {
        private readonly string _url;
        private readonly ILogger<RecetasRepository> _logger;

        public RecetasRepository(ILogger<RecetasRepository> logger, IConfiguration cofiguration)
        {
            _url = cofiguration.GetSection("ApiCalls:Recetas").Value;
            _logger = logger;
        }
        public Recetas GetRecipeByName(string recipeName)
        {
            List<RecetasDTO> recipesFromJson = GetDataFromJson();
            RecetasDTO recipeSelecte = recipesFromJson.FirstOrDefault(product => product.Nombre == recipeName);

            if (recipeSelecte == null)
            {
                return null;
            }
            Recetas recipe = new Recetas
            {
                Nombre = recipeName,
                Ingredientes = recipeSelecte.Ingredientes,
                MinutosCocinando = recipeSelecte.TiempoPreparacion
            };
            return recipe;

        }

        private List<RecetasDTO> GetDataFromJson()
        {
            using HttpClient client = new();
            HttpRequestMessage webRequest = new HttpRequestMessage(HttpMethod.Get, _url);

            var response = client.Send(webRequest);
            StreamReader reader = new StreamReader(response.Content.ReadAsStream());
            string content = reader.ReadToEnd();

            return JsonSerializer.Deserialize<List<RecetasDTO>>(content);
        }

    }
}
