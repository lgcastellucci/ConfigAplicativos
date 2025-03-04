using ConfigAplicativos.Models;
using Microsoft.AspNetCore.Mvc;
using static ConfigAplicativos.Models.ConfiguracaoIniModel;

namespace ConfigAplicativos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServidoresController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ServidoresController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public List<Servidor> Get()
        {
            var servidores = new List<Servidor>();
            servidores.Add(new Servidor { EnderecoIP = "Novo Servidor" });

            var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Arquivos");
            var filePattern = "IniOperadoras_*.ini";

            var files = Directory.GetFiles(directoryPath, filePattern);

            foreach (var file in files)
            {
                if (System.IO.File.Exists(file))
                {
                    string nomeArquivo = Path.GetFileName(file);
                    string enderecoDoServidor = nomeArquivo.Replace("IniOperadoras_", "").Replace(".ini", "");

                    servidores.Add(new Servidor { NomeArquivoIni = nomeArquivo, EnderecoIP = enderecoDoServidor });
                }
            }
            return servidores;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public ConfiguracaoIni Get(string id)
        {
            if (id == "Novo Servidor")
            {
                var configuracaoIniNovo = new ConfiguracaoIni();
                configuracaoIniNovo.global.OPERADORAS = 1;               
                return configuracaoIniNovo;
            }

            var configuracaoIniModel = new ConfiguracaoIniModel(_webHostEnvironment);
            return configuracaoIniModel.Ler(id);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public class Servidor
        {
            public string NomeArquivoIni { get; set; }
            public string EnderecoIP { get; set; }
        }

 


    }
}
