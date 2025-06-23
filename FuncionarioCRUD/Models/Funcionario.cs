using System.Text.Json.Serialization;

namespace FuncionarioCRUD.Models
{
    public class Funcionario
    {
        [JsonPropertyName("id_funcionario")]
        public int Id { get; set; }

        [JsonPropertyName("nome_Funcionario")]
        public string Nome { get; set; }

        [JsonPropertyName("senha_funcionario")]
        public string Senha { get; set; }
    }
}