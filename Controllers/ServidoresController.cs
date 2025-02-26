using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;
using Microsoft.Extensions.Configuration;

namespace ConfigAplicativos.Controllers
{
    public class ServidoresController : ApiController
    {
        public IEnumerable<Servidor> Get()
        {
            var servidores = new List<Servidor>();
            servidores.Add(new Servidor { EnderecoIP = "Novo Servidor" });

            var directoryPath = HttpContext.Current.Server.MapPath("~/Arquivos");
            var filePattern = "IniOperadoras_*.ini";

            var files = Directory.GetFiles(directoryPath, filePattern);

            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    string nomeArquivo = Path.GetFileName(file);
                    string enderecoDoServidor = nomeArquivo.Replace("IniOperadoras_", "").Replace(".ini", "");

                    servidores.Add(new Servidor { EnderecoIP = enderecoDoServidor });
                }
            }
            return servidores;
        }

        [HttpGet]
        [Route("api/Servidores/{enderecoIP}")]
        public ConfiguracaoIni GetServidorConfiguracoes(string enderecoIP)
        {
            if (enderecoIP == "Novo Servidor")
            {
                var configuracaoIniNovo = new ConfiguracaoIni();
                configuracaoIniNovo.OPERADORAS = 1;
                return configuracaoIniNovo;
            }


            var directoryPath = HttpContext.Current.Server.MapPath("~/Arquivos");
            var filePath = Path.Combine(directoryPath, $"IniOperadoras_{enderecoIP}.ini");

            if (!File.Exists(filePath))
                return null;

            var configuration = new ConfigurationBuilder().AddIniFile(filePath).Build();

            var configuracaoIni = new ConfiguracaoIni();
            configuracaoIni.OPERADORAS = Convert.ToInt16(configuration["GLOBAL:OPERADORAS"].ToString());
            configuracaoIni.DirTrabalho = configuration["GLOBAL:DirTrabalho"];
            configuracaoIni.Server_Cheque = configuration["GLOBAL:Server_Cheque"];
            configuracaoIni.OPERADORAS_API = configuration["GLOBAL:OPERADORAS_API"];
            configuracaoIni.HSM_SERVER = configuration["GLOBAL:HSM_SERVER"];
            configuracaoIni.SERVIDOR_AUTORIZADOR_ISO = configuration["GLOBAL:SERVIDOR_AUTORIZADOR_ISO"];
            configuracaoIni.CAMINHO_AUTORIZADOR_ISO = configuration["GLOBAL:CAMINHO_AUTORIZADOR_ISO"];
            configuracaoIni.EMAIL_PARA_ALERTA = configuration["GLOBAL:EMAIL_PARA_ALERTA"];
            configuracaoIni.Homologacao = Convert.ToBoolean(configuration["GLOBAL:Homologacao"]);
            configuracaoIni.ListaDeOperadoras = new List<ConfiguracaoIni.Operadora>();

            for (int nK = 0; nK <= configuracaoIni.OPERADORAS; nK += 1)
            {
                string id = (nK + 1).ToString().PadLeft(2, '0');

                var operadora = new ConfiguracaoIni.Operadora();
                operadora.CODIGO = configuration[$"OPERADORA{id}:CODIGO"];
                operadora.NOME = configuration[$"OPERADORA{id}:NOME"];
                operadora.Servidor = configuration[$"OPERADORA{id}:Servidor"];
                operadora.CAMINHO = configuration[$"OPERADORA{id}:CAMINHO"];

                operadora.DirFechamento = configuration[$"OPERADORA{id}:DirFechamento"];
                operadora.Movimentos_Diarios = configuration[$"OPERADORA{id}:Movimentos_Diarios"];
                operadora.Log_Movimentos = configuration[$"OPERADORA{id}:Log_Movimentos"];
                operadora.DIR_FATURAS = configuration[$"OPERADORA{id}:DIR_FATURAS"];
                operadora.DIR_CONCILIACAO = configuration[$"OPERADORA{id}:DIR_CONCILIACAO"];
                operadora.DIR_CONCILIACAO_2 = configuration[$"OPERADORA{id}:DIR_CONCILIACAO_2"];

                if (configuration[$"OPERADORA{id}:Executa_Fechamento"] == "Sim")
                    operadora.Executa_Fechamento = true;
                else
                    operadora.Executa_Fechamento = false;

                if (configuration[$"OPERADORA{id}:Utiliza_URA"] == "Sim")
                    operadora.Utiliza_URA = true;
                else
                    operadora.Utiliza_URA = false;

                if (configuration[$"OPERADORA{id}:Registra_Boletos_Online"] == "Sim")
                    operadora.Registra_Boletos_Online = true;
                else
                    operadora.Registra_Boletos_Online = false;

                operadora.Analistas = Convert.ToInt16(configuration[$"OPERADORA{id}:Analistas"]);

                configuracaoIni.ListaDeOperadoras.Add(operadora);
            }

            return configuracaoIni;
        }

        public class Servidor
        {
            public string NomeArquivoIni { get; set; }
            public string EnderecoIP { get; set; }
        }

        public class ConfiguracaoIni
        {
            public int OPERADORAS { get; set; }
            public string DirTrabalho { get; set; }
            public string Server_Cheque { get; set; }
            public string OPERADORAS_API { get; set; }
            public string HSM_SERVER { get; set; }
            public string SERVIDOR_AUTORIZADOR_ISO { get; set; }
            public string CAMINHO_AUTORIZADOR_ISO { get; set; }
            public string EMAIL_PARA_ALERTA { get; set; }
            public bool Homologacao { get; set; }

            // Nova propriedade para armazenar a lista de operadoras
            public List<Operadora> ListaDeOperadoras { get; set; } = new List<Operadora>();

            public class Operadora
            {
                public string CODIGO { get; set; }
                public string NOME { get; set; }
                public string Servidor { get; set; }
                public string CAMINHO { get; set; }

                public string DirFechamento { get; set; }
                public string Movimentos_Diarios { get; set; }
                public string Log_Movimentos { get; set; }
                public string DIR_FATURAS { get; set; }
                public string DIR_CONCILIACAO { get; set; }
                public string DIR_CONCILIACAO_2 { get; set; }

                public bool Executa_Fechamento { get; set; }
                public bool Utiliza_URA { get; set; }
                public bool Registra_Boletos_Online { get; set; }
                public int Analistas { get; set; }
            }
        }
    }
}
