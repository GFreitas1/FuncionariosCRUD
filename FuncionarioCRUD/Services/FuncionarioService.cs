using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FuncionarioCRUD.Models;

namespace FuncionarioCRUD.Services
{
    public class FuncionarioService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl = "https://funcionarioapi-aacbgjh8a8gje0dv.brazilsouth-01.azurewebsites.net/api/funcionarios";

        public FuncionarioService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Funcionario>> GetFuncionariosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_baseApiUrl);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<List<Funcionario>>(jsonResponse, options);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro de requisição HTTP ao buscar funcionários: {ex.Message}");
                return new List<Funcionario>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro de desserialização JSON ao buscar funcionários: {ex.Message}");
                return new List<Funcionario>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao buscar funcionários: {ex.Message}");
                return new List<Funcionario>();
            }
        }

        public async Task<Funcionario> AddFuncionarioAsync(Funcionario funcionario)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(funcionario);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_baseApiUrl, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<Funcionario>(jsonResponse, options);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro de requisição HTTP ao adicionar funcionário: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro de desserialização JSON ao adicionar funcionário: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao adicionar funcionário: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateFuncionarioAsync(Funcionario funcionario)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(funcionario);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_baseApiUrl}/{funcionario.Id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro de requisição HTTP ao atualizar funcionário: {ex.Message}");
                return false;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro de desserialização JSON ao atualizar funcionário: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao atualizar funcionário: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteFuncionarioAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseApiUrl}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro de requisição HTTP ao deletar funcionário: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao deletar funcionário: {ex.Message}");
                return false;
            }
        }
    }
}