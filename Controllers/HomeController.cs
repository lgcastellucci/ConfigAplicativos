using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ConfigAplicativos.Models;
using static ConfigAplicativos.Models.ConfiguracaoIniModel;
namespace ConfigAplicativos.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Files()
    {
        return View();
    }

    public IActionResult SubmitForm()
    {
        var enderecoIP = Request.Form["ServidorSelecionado"].ToString();

        if (enderecoIP == "Novo Servidor")
            enderecoIP = Request.Form["ipServidor"].ToString().Replace('.', '-');

        var configuracaoIni = new ConfiguracaoIni();
        configuracaoIni.global.OPERADORAS = Convert.ToInt32(Request.Form["globalOPERADORAS"]);
        configuracaoIni.global.DirTrabalho = Request.Form["globalDirTrabalho"].ToString();
        configuracaoIni.global.Server_Cheque = Request.Form["globalServer_Cheque"].ToString();
        configuracaoIni.global.OPERADORAS_API = Request.Form["globalOPERADORAS_API"].ToString();
        configuracaoIni.global.HSM_SERVER = Request.Form["globalHSM_SERVER"].ToString();
        configuracaoIni.global.SERVIDOR_AUTORIZADOR_ISO = Request.Form["globalSERVIDOR_AUTORIZADOR_ISO"].ToString();
        configuracaoIni.global.CAMINHO_AUTORIZADOR_ISO = Request.Form["globalCAMINHO_AUTORIZADOR_ISO"].ToString();
        configuracaoIni.global.EMAIL_PARA_ALERTA = Request.Form["globalEMAIL_PARA_ALERTA"].ToString();
        configuracaoIni.global.Homologacao = Request.Form["globalHomologacao"] == "Sim";

        int qtdOperadorasAtuais = 0;
        for (int i = 0; i < configuracaoIni.global.OPERADORAS; i++)
        {
            if (Request.Form["removeOperadora[" + (i + 1).ToString().PadLeft(2, '0') + "]"] != "on")
            {
                qtdOperadorasAtuais += 1;
                string strIndex = (i+1).ToString().PadLeft(2, '0');

                var operadora = new ConfiguracaoIni.Operadora();
                operadora.CODIGO = Request.Form["operadora[" + strIndex + "]CODIGO"].ToString();
                operadora.NOME = Request.Form["operadora[" + strIndex + "]NOME"].ToString();
                operadora.Servidor = Request.Form["operadora[" + strIndex + "]CoServidorigo"].ToString();
                operadora.CAMINHO = Request.Form["operadora[" + strIndex + "]CAMINHO"].ToString();
                operadora.DirFechamento = Request.Form["operadora[" + strIndex + "]DirFechamento"].ToString();
                operadora.Movimentos_Diarios = Request.Form["operadora[" + strIndex + "]Movimentos_Diarios"].ToString();
                operadora.Log_Movimentos = Request.Form["operadora[" + strIndex + "]Log_Movimentos"].ToString();
                operadora.DIR_FATURAS = Request.Form["operadora[" + strIndex + "]DIR_FATURAS"].ToString();
                operadora.DIR_CONCILIACAO = Request.Form["operadora[" + strIndex + "]DIR_CONCILIACAO"].ToString();
                operadora.DIR_CONCILIACAO_2 = Request.Form["operadora[" + strIndex + "]DIR_CONCILIACAO_2"].ToString();
                operadora.Executa_Fechamento = Request.Form["operadora[" + strIndex + "]Executa_Fechamento"] == "Sim";
                operadora.Utiliza_URA = Request.Form["operadora[" + strIndex + "]Utiliza_URA"] == "Sim";
                operadora.Registra_Boletos_Online = Request.Form["operadora[" + strIndex + "]Registra_Boletos_Online"] == "Sim";

                if (Request.Form["operadora[" + strIndex + "]Analistas"] == "")
                    operadora.Analistas = 0;
                else
                    operadora.Analistas = Convert.ToInt16(Request.Form["operadora[" + strIndex + "]Analistas"]);

                configuracaoIni.ListaDeOperadoras.Add(operadora);
            }
        }
        configuracaoIni.global.OPERADORAS = qtdOperadorasAtuais;

        var configuracaoIniModel = new ConfiguracaoIniModel(_webHostEnvironment);
        var sucesso = configuracaoIniModel.Gravar(enderecoIP, configuracaoIni);

        return RedirectToAction("Files");

    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
