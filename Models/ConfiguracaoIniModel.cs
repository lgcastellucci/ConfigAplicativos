using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace ConfigAplicativos.Models
{
    public class ConfiguracaoIniModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ConfiguracaoIniModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public class ConfiguracaoIni
        {
            public Global global { get; set; } = new Global();
            public List<Operadora> ListaDeOperadoras { get; set; } = new List<Operadora>();

            public class Global
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
            }
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
                public bool Executa_Processa_Filas { get; set; }
                public int Analistas { get; set; }
            }
        }
        public ConfiguracaoIni Ler(string ipDoServidor)
        {
            var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Arquivos");
            var filePath = Path.Combine(directoryPath, $"IniOperadoras_{ipDoServidor}.ini");

            if (!System.IO.File.Exists(filePath))
                return null;

            var configuration = new ConfigurationBuilder().AddIniFile(filePath).Build();

            var configuracaoIni = new ConfiguracaoIni();
            configuracaoIni.global.OPERADORAS = Convert.ToInt16(configuration["GLOBAL:OPERADORAS"].ToString());
            configuracaoIni.global.DirTrabalho = configuration["GLOBAL:DirTrabalho"];
            configuracaoIni.global.Server_Cheque = configuration["GLOBAL:Server_Cheque"];
            configuracaoIni.global.OPERADORAS_API = configuration["GLOBAL:OPERADORAS_API"];
            configuracaoIni.global.HSM_SERVER = configuration["GLOBAL:HSM_SERVER"];
            configuracaoIni.global.SERVIDOR_AUTORIZADOR_ISO = configuration["GLOBAL:SERVIDOR_AUTORIZADOR_ISO"];
            configuracaoIni.global.CAMINHO_AUTORIZADOR_ISO = configuration["GLOBAL:CAMINHO_AUTORIZADOR_ISO"];
            configuracaoIni.global.EMAIL_PARA_ALERTA = configuration["GLOBAL:EMAIL_PARA_ALERTA"];
            configuracaoIni.global.Homologacao = Convert.ToBoolean(configuration["GLOBAL:Homologacao"]);
            configuracaoIni.ListaDeOperadoras = new List<ConfiguracaoIni.Operadora>();

            for (int nK = 0; nK <= configuracaoIni.global.OPERADORAS; nK += 1)
            {
                string idOper = (nK + 1).ToString().PadLeft(2, '0');

                var operadora = new ConfiguracaoIni.Operadora();
                operadora.CODIGO = configuration[$"OPERADORA{idOper}:CODIGO"];
                operadora.NOME = configuration[$"OPERADORA{idOper}:NOME"];
                operadora.Servidor = configuration[$"OPERADORA{idOper}:Servidor"];
                operadora.CAMINHO = configuration[$"OPERADORA{idOper}:CAMINHO"];

                operadora.DirFechamento = configuration[$"OPERADORA{idOper}:DirFechamento"];
                operadora.Movimentos_Diarios = configuration[$"OPERADORA{idOper}:Movimentos_Diarios"];
                operadora.Log_Movimentos = configuration[$"OPERADORA{idOper}:Log_Movimentos"];
                operadora.DIR_FATURAS = configuration[$"OPERADORA{idOper}:DIR_FATURAS"];
                operadora.DIR_CONCILIACAO = configuration[$"OPERADORA{idOper}:DIR_CONCILIACAO"];
                operadora.DIR_CONCILIACAO_2 = configuration[$"OPERADORA{idOper}:DIR_CONCILIACAO_2"];

                if (configuration[$"OPERADORA{idOper}:Executa_Fechamento"] == "Sim")
                    operadora.Executa_Fechamento = true;
                else
                    operadora.Executa_Fechamento = false;

                if (configuration[$"OPERADORA{idOper}:Utiliza_URA"] == "Sim")
                    operadora.Utiliza_URA = true;
                else
                    operadora.Utiliza_URA = false;

                if (configuration[$"OPERADORA{idOper}:Registra_Boletos_Online"] == "Sim")
                    operadora.Registra_Boletos_Online = true;
                else
                    operadora.Registra_Boletos_Online = false;

                if (configuration[$"OPERADORA{idOper}:Executa_Processa_Filas"] == "Sim")
                    operadora.Executa_Processa_Filas = true;
                else
                    operadora.Executa_Processa_Filas = false;

                if (configuration[$"OPERADORA{idOper}:Analistas"] == "")
                    operadora.Analistas = 0;
                else
                    operadora.Analistas = Convert.ToInt16(configuration[$"OPERADORA{idOper}:Analistas"]);

                configuracaoIni.ListaDeOperadoras.Add(operadora);
            }

            return configuracaoIni;
        }
        public bool Gravar(string enderecoIP, ConfiguracaoIni configuracaoIni)
        {
            var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Arquivos");
            var filePath = Path.Combine(directoryPath, "IniOperadoras_" + enderecoIP + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".ini");

            var iniContent = new StringBuilder();
            iniContent.AppendLine("[GLOBAL]");
            iniContent.AppendLine($"OPERADORAS={configuracaoIni.global.OPERADORAS}");
            iniContent.AppendLine($"DirTrabalho={configuracaoIni.global.DirTrabalho}");
            iniContent.AppendLine($"Server_Cheque={configuracaoIni.global.Server_Cheque}");
            iniContent.AppendLine($"OPERADORAS_API={configuracaoIni.global.OPERADORAS_API}");
            iniContent.AppendLine($"HSM_SERVER={configuracaoIni.global.HSM_SERVER}");
            iniContent.AppendLine($"SERVIDOR_AUTORIZADOR_ISO={configuracaoIni.global.SERVIDOR_AUTORIZADOR_ISO}");
            iniContent.AppendLine($"CAMINHO_AUTORIZADOR_ISO={configuracaoIni.global.CAMINHO_AUTORIZADOR_ISO}");
            iniContent.AppendLine($"EMAIL_PARA_ALERTA={configuracaoIni.global.EMAIL_PARA_ALERTA}");
            iniContent.AppendLine($"Homologacao={(configuracaoIni.global.Homologacao ? "Sim" : "Nao")}");
            iniContent.AppendLine("");

            for (int i = 0; i < configuracaoIni.ListaDeOperadoras.Count; i++)
            {
                var operadora = configuracaoIni.ListaDeOperadoras[i];
                string id = (i + 1).ToString().PadLeft(2, '0');
                iniContent.AppendLine($"[OPERADORA{id}]");
                iniContent.AppendLine($"CODIGO={operadora.CODIGO}");
                iniContent.AppendLine($"NOME={operadora.NOME}");
                iniContent.AppendLine($"Servidor={operadora.Servidor}");
                iniContent.AppendLine($"CAMINHO={operadora.CAMINHO}");

                iniContent.AppendLine($"DirFechamento={operadora.DirFechamento}");
                iniContent.AppendLine($"Movimentos_Diarios={operadora.Movimentos_Diarios}");
                iniContent.AppendLine($"Log_Movimentos={operadora.Log_Movimentos}");
                iniContent.AppendLine($"DIR_FATURAS={operadora.DIR_FATURAS}");
                iniContent.AppendLine($"DIR_CONCILIACAO={operadora.DIR_CONCILIACAO}");
                iniContent.AppendLine($"DIR_CONCILIACAO_2={operadora.DIR_CONCILIACAO_2}");

                iniContent.AppendLine($"Executa_Fechamento={(operadora.Executa_Fechamento ? "Sim" : "Nao")}");
                iniContent.AppendLine($"Utiliza_URA={(operadora.Utiliza_URA ? "Sim" : "Nao")}");
                iniContent.AppendLine($"Registra_Boletos_Online={(operadora.Registra_Boletos_Online ? "Sim" : "Nao")}");
                iniContent.AppendLine($"Analistas={operadora.Analistas}");

                iniContent.AppendLine("");
            }

            File.WriteAllText(filePath, iniContent.ToString());

            //File.WriteAllText(filePath, Newtonsoft.Json.JsonConvert.SerializeObject(configuracaoIni));
            //File.WriteAllText(filePath.Replace(".ini", ".json"), Newtonsoft.Json.JsonConvert.SerializeObject(configuracaoIni, new Newtonsoft.Json.JsonSerializerSettings
            //{
            //    StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeHtml
            //}));

            return true;
        }
    }
}
