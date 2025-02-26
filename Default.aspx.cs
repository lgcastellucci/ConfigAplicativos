using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using static ConfigAplicativos.Controllers.ServidoresController;
using static ConfigAplicativos.Controllers.ServidoresController.ConfiguracaoIni;

namespace ConfigAplicativos
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string servidorSelecionado = Request.Form["ServidorSelecionado"];
            int quantidadeOperadoras = int.Parse(Request.Form["configuracaoIni.OPERADORAS"]);
            string dirTrabalho = Request.Form["configuracaoIni.DirTrabalho"];
            string serverCheque = Request.Form["configuracaoIni.Server_Cheque"];
            string operadorasApi = Request.Form["configuracaoIni.OPERADORAS_API"];
            string hsmServer = Request.Form["configuracaoIni.HSM_SERVER"];
            string servidorAutorizadorIso = Request.Form["configuracaoIni.SERVIDOR_AUTORIZADOR_ISO"];
            string caminhoAutorizadorIso = Request.Form["configuracaoIni.CAMINHO_AUTORIZADOR_ISO"];
            string emailParaAlerta = Request.Form["configuracaoIni.EMAIL_PARA_ALERTA"];
            bool homologacao = Request.Form["configuracaoIni.Homologacao"] == "Sim";

            List<Operadora> operadoras = new List<Operadora>();
            for (int i = 0; i < quantidadeOperadoras; i++)
            {
                Operadora operadora = new Operadora
                {
                    CODIGO = Request.Form[$"Operadoras[{i}].CODIGO"],
                    NOME = Request.Form[$"Operadoras[{i}].NOME"],
                    Servidor = Request.Form[$"Operadoras[{i}].Servidor"],
                    CAMINHO = Request.Form[$"Operadoras[{i}].CAMINHO"],

                    DirFechamento = Request.Form[$"Operadoras[{i}].DirFechamento"],
                    Movimentos_Diarios = Request.Form[$"Operadoras[{i}].Movimentos_Diarios"],
                    Log_Movimentos = Request.Form[$"Operadoras[{i}].Log_Movimentos"],
                    DIR_FATURAS = Request.Form[$"Operadoras[{i}].DIR_FATURAS"],
                    DIR_CONCILIACAO = Request.Form[$"Operadoras[{i}].DIR_CONCILIACAO"],
                    DIR_CONCILIACAO_2 = Request.Form[$"Operadoras[{i}].DIR_CONCILIACAO_2"],

                    Executa_Fechamento = Request.Form[$"Operadoras[{i}].Executa_Fechamento"] == "Sim",
                    Utiliza_URA = Request.Form[$"Operadoras[{i}].Utiliza_URA"] == "Sim",
                    Registra_Boletos_Online = Request.Form[$"Operadoras[{i}].Registra_Boletos_Online"] == "Sim",
                    Analistas = int.Parse(Request.Form[$"Operadoras[{i}].Analistas"])
                };
                operadoras.Add(operadora);
            }

            var configuracaoIni = new ConfiguracaoIni
            {
                OPERADORAS = quantidadeOperadoras,
                DirTrabalho = dirTrabalho,
                Server_Cheque = serverCheque,
                OPERADORAS_API = operadorasApi,
                HSM_SERVER = hsmServer,
                SERVIDOR_AUTORIZADOR_ISO = servidorAutorizadorIso,
                CAMINHO_AUTORIZADOR_ISO = caminhoAutorizadorIso,
                EMAIL_PARA_ALERTA = emailParaAlerta,
                Homologacao = homologacao,
                ListaDeOperadoras = operadoras
            };

            SalvarConfiguracoes(servidorSelecionado, configuracaoIni);

        }



        public void SalvarConfiguracoes(string enderecoIP, ConfiguracaoIni configuracaoIni)
        {
            var directoryPath = HttpContext.Current.Server.MapPath("~/Arquivos");
            var filePath = Path.Combine(directoryPath, "IniOperadoras_" + enderecoIP + "_" +DateTime.Now.ToString("yyyyMMddHHmmss") + ".ini");

            var iniContent = new StringBuilder();
            iniContent.AppendLine("[GLOBAL]");
            iniContent.AppendLine($"OPERADORAS={configuracaoIni.OPERADORAS}");
            iniContent.AppendLine($"DirTrabalho={configuracaoIni.DirTrabalho}");
            iniContent.AppendLine($"Server_Cheque={configuracaoIni.Server_Cheque}");
            iniContent.AppendLine($"OPERADORAS_API={configuracaoIni.OPERADORAS_API}");
            iniContent.AppendLine($"HSM_SERVER={configuracaoIni.HSM_SERVER}");
            iniContent.AppendLine($"SERVIDOR_AUTORIZADOR_ISO={configuracaoIni.SERVIDOR_AUTORIZADOR_ISO}");
            iniContent.AppendLine($"CAMINHO_AUTORIZADOR_ISO={configuracaoIni.CAMINHO_AUTORIZADOR_ISO}");
            iniContent.AppendLine($"EMAIL_PARA_ALERTA={configuracaoIni.EMAIL_PARA_ALERTA}");
            iniContent.AppendLine($"Homologacao={(configuracaoIni.Homologacao ? "Sim" : "Nao")}");
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
            File.WriteAllText(filePath.Replace(".ini", ".json"), Newtonsoft.Json.JsonConvert.SerializeObject(configuracaoIni, new Newtonsoft.Json.JsonSerializerSettings
            {
                StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeHtml
            }));
        }
    }


}