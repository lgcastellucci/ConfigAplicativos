<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ConfigAplicativos._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <div class="container mt-5">

            <div class="card" id="divServidores">
                <div class="card-header">
                    Servidores
               
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label for="servidoresConfigurados">Servidores Configurados</label>
                        <select class="form-control container" id="ServidorSelecionado" name="ServidorSelecionado" onchange="fetchServidorConfiguracoes(true)">
                        </select>
                    </div>
                </div>
            </div>

            <div class="row">&nbsp;</div>

            <div class="card" id="divConfiguracoesGerais">
                <div class="card-header">
                    Configurações Gerais
               
                </div>
                <div class="card-body">
                    <div class="form-group col-md-3">
                        <label for="quantidadeOperadoras">Quantidade de Operadoras</label>
                        <input type="number" class="form-control container" id="configuracaoIni.OPERADORAS" name="configuracaoIni.OPERADORAS" required onchange="createOperadoraCards()">
                    </div>
                    <div class="form-group">
                        <label for="dirTrabalho">Diretório de Trabalho</label>
                        <input type="text" class="form-control container" id="configuracaoIni_DirTrabalho" name="configuracaoIni.DirTrabalho" required>
                    </div>
                    <div class="form-group">
                        <label for="serverCheque">Server Cheque</label>
                        <input type="url" class="form-control container" id="configuracaoIni_Server_Cheque" name="configuracaoIni.Server_Cheque">
                    </div>
                    <div class="form-group">
                        <label for="operadorasApi">Operadoras API</label>
                        <input type="url" class="form-control container" id="configuracaoIni_OPERADORAS_API" name="configuracaoIni.OPERADORAS_API">
                    </div>
                    <div class="form-group">
                        <label for="hsmServer">HSM Server</label>
                        <input type="url" class="form-control container" id="configuracaoIni_HSM_SERVER" name="configuracaoIni.HSM_SERVER">
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="servidorAutorizadorIso">Servidor Autorizador ISO</label>
                                <input type="text" class="form-control container" id="configuracaoIni.SERVIDOR_AUTORIZADOR_ISO" name="configuracaoIni.SERVIDOR_AUTORIZADOR_ISO">
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="caminhoAutorizadorIso">Caminho Autorizador ISO</label>
                                <input type="text" class="form-control container" id="configuracaoIni.CAMINHO_AUTORIZADOR_ISO" name="configuracaoIni.CAMINHO_AUTORIZADOR_ISO">
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="emailParaAlerta">Email para Alerta</label>
                        <input type="email" class="form-control container" id="configuracaoIni.EMAIL_PARA_ALERTA" name="configuracaoIni.EMAIL_PARA_ALERTA">
                    </div>
                    <div class="form-group form-check">
                        <input type="checkbox" class="form-check-input" id="configuracaoIni.Homologacao" name="configuracaoIni.Homologacao">
                        <label class="form-check-label" for="homologacao">Homologação</label>
                    </div>
                </div>
            </div>

            <div class="row">&nbsp;</div>

            <div id="operadorasContainer"></div>

        </div>

        <div class="row">&nbsp;</div>

        <div class="col-12 d-flex justify-content-end">
            <div class="form-group">
                <asp:Button class="btn btn-primary" ID="btnSalvar" runat="server" Text="Salvar Configurações" OnClick="btnSalvar_Click" />
            </div>
        </div>

    </main>

    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            //deixar a div de configurações gerais invisível
            document.getElementById('divConfiguracoesGerais').style.display = 'none';

            fetch('/api/Servidores')
                .then(response => response.json())
                .then(data => {
                    const select = document.getElementById('ServidorSelecionado');
                    data.forEach(servidor => {
                        const option = document.createElement('option');
                        option.value = servidor.EnderecoIP;
                        option.textContent = servidor.EnderecoIP;
                        select.appendChild(option);
                    });
                    fetchServidorConfiguracoes(true);
                })
                .catch(error => console.error('Erro ao carregar servidores:', error));
        });

        function fetchServidorConfiguracoes(criarCards) {
            const select = document.getElementById('ServidorSelecionado');
            const enderecoIP = select.value;

            fetch(`/api/Servidores/${enderecoIP}`)
                .then(response => response.json())
                .then(configuracaoIni => {
                    //deixar d div de configurações gerais visível
                    document.getElementById('divConfiguracoesGerais').style.display = 'block';

                    //Preencher os campos de Configurações Gerais
                    if (criarCards)
                        document.getElementById('configuracaoIni.OPERADORAS').value = configuracaoIni.OPERADORAS;

                    document.getElementById('configuracaoIni_DirTrabalho').value = configuracaoIni.DirTrabalho;
                    document.getElementById('configuracaoIni_Server_Cheque').value = configuracaoIni.Server_Cheque;
                    document.getElementById('configuracaoIni_OPERADORAS_API').value = configuracaoIni.OPERADORAS_API;
                    document.getElementById('configuracaoIni_HSM_SERVER').value = configuracaoIni.HSM_SERVER;
                    document.getElementById('configuracaoIni.SERVIDOR_AUTORIZADOR_ISO').value = configuracaoIni.SERVIDOR_AUTORIZADOR_ISO;
                    document.getElementById('configuracaoIni.CAMINHO_AUTORIZADOR_ISO').value = configuracaoIni.CAMINHO_AUTORIZADOR_ISO;
                    document.getElementById('configuracaoIni.EMAIL_PARA_ALERTA').value = configuracaoIni.EMAIL_PARA_ALERTA;
                    document.getElementById('configuracaoIni.Homologacao').checked = configuracaoIni.Homologacao;

                    if (criarCards)
                        createOperadoraCards();

                    if (configuracaoIni != null) {
                        if (configuracaoIni.ListaDeOperadoras != null) {
                            configuracaoIni.ListaDeOperadoras.forEach((operadora, index) => {
                                if (operadora != null) {
                                    if (operadora.CODIGO != null) {
                                        document.getElementById(`operadoraCodigo_${index}`).value = operadora.CODIGO;
                                        document.getElementById(`operadoraNome_${index}`).value = operadora.NOME;
                                        document.getElementById(`operadoraServidor_${index}`).value = operadora.Servidor;
                                        document.getElementById(`operadoraCaminho_${index}`).value = operadora.CAMINHO;

                                        document.getElementById(`operadoraDirFechamento_${index}`).value = operadora.DirFechamento;
                                        document.getElementById(`operadoraMovimentos_Diarios_${index}`).value = operadora.Movimentos_Diarios;
                                        document.getElementById(`operadoraLog_Movimentos_${index}`).value = operadora.Log_Movimentos;
                                        document.getElementById(`operadoraDIR_FATURAS_${index}`).value = operadora.DIR_FATURAS;
                                        document.getElementById(`operadoraDIR_CONCILIACAO_${index}`).value = operadora.DIR_CONCILIACAO;
                                        document.getElementById(`operadoraDIR_CONCILIACAO_2_${index}`).value = operadora.DIR_CONCILIACAO_2;

                                        document.getElementById(`operadoraExecuta_Fechamento_${index}`).value = operadora.Executa_Fechamento ? "Sim" : "Não";
                                        document.getElementById(`operadoraRegistra_Boletos_Online_${index}`).value = operadora.Registra_Boletos_Online ? "Sim" : "Não";
                                        document.getElementById(`operadoraUtiliza_URA_${index}`).value = operadora.Utiliza_URA ? "Sim" : "Não";
                                        document.getElementById(`operadoraAnalistas_${index}`).value = operadora.Analistas;

                                        //trocar a cor da div card-header para vermelho claro
                                        if ((operadora.Servidor == "") && (operadora.CAMINHO == "")) {
                                            document.getElementById(`divOperadora_${index}`).classList.add('bg-danger');
                                        }
                                        else if (operadora.CAMINHO == "") {
                                            document.getElementById(`divOperadora_${index}`).classList.add('bg-warning');
                                        }
                                    }
                                }
                            });
                        }
                    }
                })
                .catch(error => console.error('Erro ao carregar configurações do servidor:', error));
        }


        function createOperadoraCards() {
            const quantidadeOperadoras = document.getElementById('configuracaoIni.OPERADORAS').value;
            const container = document.getElementById('operadorasContainer');
            container.innerHTML = ''; // Limpar os cards existentes

            for (let i = 0; i < quantidadeOperadoras; i++) {
                const card = document.createElement('div');
                card.className = 'card';
                card.innerHTML = `
                <div class="card-header" id="divOperadora_${i}">
                    Operadora ${String(i + 1).padStart(2, '0')}
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="operadoraCodigo_${i}">Código</label>
                                <input type="text" class="form-control container" id="operadoraCodigo_${i}" name="Operadoras[${i}].CODIGO">
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="operadoraNome_${i}">Nome</label>
                                <input type="text" class="form-control container" id="operadoraNome_${i}" name="Operadoras[${i}].NOME">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="operadoraServidor_${i}">Servidor</label>
                                <input type="text" class="form-control container" id="operadoraServidor_${i}" name="Operadoras[${i}].Servidor">
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="operadoraCaminho_${i}">Caminho</label>
                                <input type="text" class="form-control container" id="operadoraCaminho_${i}" name="Operadoras[${i}].CAMINHO">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="operadoraDirFechamento_${i}">DirFechamento</label>
                                <input type="text" class="form-control container" id="operadoraDirFechamento_${i}" name="Operadoras[${i}].DirFechamento">
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="operadoraMovimentos_Diarios_${i}">Movimentos_Diarios</label>
                                <input type="text" class="form-control container" id="operadoraMovimentos_Diarios_${i}" name="Operadoras[${i}].Movimentos_Diarios">
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="operadoraLog_Movimentos_${i}">Log_Movimentos</label>
                                <input type="text" class="form-control container" id="operadoraLog_Movimentos_${i}" name="Operadoras[${i}].Log_Movimentos">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="operadoraDIR_FATURAS_${i}">DIR_FATURAS</label>
                                <input type="text" class="form-control container" id="operadoraDIR_FATURAS_${i}" name="Operadoras[${i}].DIR_FATURAS">
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="operadoraDIR_CONCILIACAO_${i}">DIR_CONCILIACAO</label>
                                <input type="text" class="form-control container" id="operadoraDIR_CONCILIACAO_${i}" name="Operadoras[${i}].">
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="operadoraDIR_CONCILIACAO_2_${i}">DIR_CONCILIACAO_2</label>
                                <input type="text" class="form-control container" id="operadoraDIR_CONCILIACAO_2_${i}" name="Operadoras[${i}].DIR_CONCILIACAO_2">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-3">
                            <label for="operadoraExecuta_Fechamento_${i}">Executa Fechamento</label>
                            <select class="form-control container" id="operadoraExecuta_Fechamento_${i}" name="Operadoras[${i}].Executa_Fechamento">
                                <option value="Sim">Sim</option>
                                <option value="Não">Não</option>
                            </select>
                        </div>
                        <div class="form-group col-md-3">
                            <label for="operadoraRegistra_Boletos_Online_${i}">Registra Boletos Online</label>
                            <select class="form-control container" id="operadoraRegistra_Boletos_Online_${i}" name="Operadoras[${i}].Registra_Boletos_Online">
                                <option value="Sim">Sim</option>
                                <option value="Não">Não</option>
                            </select>
                        </div>
                        <div class="form-group col-md-3">
                            <label for="operadoraUtiliza_URA_${i}">Utiliza URA</label>
                            <select class="form-control container" id="operadoraUtiliza_URA_${i}" name="Operadoras[${i}].Utiliza_URA">
                                <option value="Sim" >Sim</option>
                                <option value="Não" >Não</option>
                            </select>
                        </div>
                        <div class="form-group col-md-3">
                            <label for="operadoraAnalistas_${i}">Analistas</label>
                            <input type="number" class="form-control container" id="operadoraAnalistas_${i}" name="Operadoras[${i}].Analistas">
                        </div>
                    </div>
                </div>
            `;
                container.appendChild(card);
            }

            fetchServidorConfiguracoes(false);
        }
    </script>



</asp:Content>



