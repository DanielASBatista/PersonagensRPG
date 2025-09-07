using rpgApi.Models;
using rpgApi.Models.Enuns;
using Microsoft.AspNetCore.Mvc;

namespace rpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class PersonagensController : ControllerBase
    {
        private static List<Personagem> personagens = new List<Personagem>()
        {
            new Personagem() { Id = 1, Nome = "Frodo", PontosVida=100, Forca=17, Defesa=23, Inteligencia=33, Classe=ClasseEnum.Cavaleiro},
            new Personagem() { Id = 2, Nome = "Sam", PontosVida=100, Forca=15, Defesa=25, Inteligencia=30, Classe=ClasseEnum.Cavaleiro},
            new Personagem() { Id = 3, Nome = "Galadriel", PontosVida=100, Forca=18, Defesa=21, Inteligencia=35, Classe=ClasseEnum.Clerigo },
            new Personagem() { Id = 4, Nome = "Gandalf", PontosVida=100, Forca=18, Defesa=18, Inteligencia=37, Classe=ClasseEnum.Mago },
            new Personagem() { Id = 5, Nome = "Hobbit", PontosVida=100, Forca=20, Defesa=17, Inteligencia=31, Classe=ClasseEnum.Cavaleiro },
            new Personagem() { Id = 6, Nome = "Celeborn", PontosVida=100, Forca=21, Defesa=13, Inteligencia=34, Classe=ClasseEnum.Clerigo },
            new Personagem() { Id = 7, Nome = "Radagast", PontosVida=100, Forca=25, Defesa=11, Inteligencia=35, Classe=ClasseEnum.Mago }
        };

        //Método para seleção de um personagem pelo nome. 
        [HttpGet("nome/{nome}")]
        public IActionResult GetSingle(string nome)
        {
            Personagem pnome = personagens.FirstOrDefault(p => p.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
            if (pnome == null)
            {
                return NotFound($"Personagem com o nome '{nome}' não foi encontrado.");
            }
            return Ok(pnome);
        }

        //Método que cria uma lista com personagens classificados como Mago ou Clérigo, ordenados por pontos de vida.
        [HttpGet("GetClerigoMago")]
        public IActionResult GetClerigoMago()
        {
            List<Personagem> listaBusca = personagens
                .Where(p => p.Classe == ClasseEnum.Clerigo || p.Classe == ClasseEnum.Mago)
                .OrderBy(p => p.PontosVida)
                .ToList();

            return Ok(listaBusca);
        }

        //Método que retorna a quantidade de personagens e a soma da inteligência de todos os personagens.
        [HttpGet("GetEstatisticas")]    
        public IActionResult GetEstatisticas()
        {
            int qtd = personagens.Count;
            int somaInteligencia = personagens.Sum(p => p.Inteligencia);
            return Ok(new
            { QuantidadePersonagens = qtd, SomaInteligencia = somaInteligencia });
        }

        //Método que adiciona um novo personagem à lista, com as seguintes validações:
        //Defesa não pode ser menor que 10.     
        //Inteligência não pode ser maior que 30.
        //Se alguma dessas validações falhar, retornar BadRequest com uma mensagem apropriada.  
        //Se as validações passarem, adicionar o personagem à lista e retornar a lista atualizada.  
        [HttpPost("PostValidacao")]
        public IActionResult PostValidacao(Personagem novoPersonagem)
        {
        if (novoPersonagem.Defesa < 10)
        {
            return BadRequest("Requisição inválida: a Defesa não pode ser menor que 10. Sugestão: defina um valor de Defesa igual ou maior que 10.");
        }

        if (novoPersonagem.Inteligencia > 30)
        {
            return BadRequest("Requisição inválida: a Inteligência não pode ser maior que 30. Sugestão: defina um valor de Inteligência igual ou menor que 30.");
        }

    personagens.Add(novoPersonagem);
    return Ok(personagens);
}

        //Método que adiciona um novo personagem à lista, com a seguinte validação:
        //Se a classe do personagem for Mago, a inteligência não pode ser menor que 35.
        //Se essa validação falhar, retornar BadRequest com uma mensagem apropriada.    
        //Se a validação passar, adicionar o personagem à lista e retornar a lista atualizada.
        [HttpPost("PostValidacaoMago")]
        public IActionResult PostValidacaoMago(Personagem novoPersonagem)
        {
            if (novoPersonagem.Classe == ClasseEnum.Mago && novoPersonagem.Inteligencia < 35)
            {
                return BadRequest("Um Mago não pode ter Inteligência menor que 35.");
            }

            personagens.Add(novoPersonagem);
            return Ok(personagens);
        }

        //Método que retorna uma lista de personagens filtrada pela classe recebida como parâmetro na URL.  
        //A lista deve ser ordenada por força em ordem decrescente. 
        [HttpGet("GetByClasse/{idClasse}")]
        public IActionResult GetByClasse(int idClasse)
        {
            List<Personagem> listaBusca = personagens
                .Where(p => (int)p.Classe == idClasse)
                .ToList();

            return Ok(listaBusca);
        }
    }
}