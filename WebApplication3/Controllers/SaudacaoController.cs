using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication3.Controllers
{
    [Route("api/saudacao")]
    [ApiController]
    public class SaudacaoController : ControllerBase
    {
        [HttpGet("bom-dia")]
        public string BomDia()
        {
            return "Bom dia";
        }

        [HttpGet("boa-tarde")]
        public Saudacao BoaTarde()
        {
            return new Saudacao
            {
                Data = DateTime.Now,
                Mensagem = "Boa Tarde"
            };
        }
    }

    public class Saudacao
    {
        public DateTime Data { get; set; }
        public string Mensagem { get; set; }
    }
}
