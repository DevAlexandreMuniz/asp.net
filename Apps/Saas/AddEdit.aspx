<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddEdit.aspx.cs" Inherits="SaasAdmin.Apps.Saas.AddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/cadastro.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <main>
        <section class="container">
            <article class="content">

                <div class="row">
                    <header class="page-header clearfix">
                        <h1>Clientes</h1>
                        <a href="Grid.aspx" class="btn btn-success pull-right">Ver Lista</a>
                    </header>
                </div>

                <div class="form-horizontal">

                    <div class="form-group">
                        <label class="control-label col-sm-2">Razão Social: <span style="color: red;">*</span></label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtRazaoSocial" CssClass="form-control" MaxLength="250" runat="server" placeholder="razão social" />
                            <asp:RequiredFieldValidator ID="RfvNome" ControlToValidate="txtRazaoSocial" ErrorMessage="<br/><b>Razão Social deve ser preenchida!</b>"
                                Display="Dynamic" SetFocusOnError="true" runat="server" />
                            <asp:RegularExpressionValidator
                                ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtRazaoSocial"
                                Display="Dynamic" SetFocusOnError="true" ErrorMessage="<br/><b>O campo razão social deve possuir no minímo 6 caracteres</b>"
                                ValidationExpression=".{6}.*" />
                        </div>
                        <label class="control-label col-sm-2">Nome Fantasia: <span style="color: red;">*</span></label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtNomeFantasia" CssClass="form-control" MaxLength="250" runat="server" placeholder="nome fantasia" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtNomeFantasia" ErrorMessage="<br/><b>Nome Fantasia deve ser preenchido!</b>"
                                Display="Dynamic" SetFocusOnError="true" runat="server" />
                            <asp:RegularExpressionValidator
                                ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtNomeFantasia"
                                Display="Dynamic" SetFocusOnError="true" ErrorMessage="<br/><b>O campo nome fantasia deve possuir no minímo 4 caracteres</b>"
                                ValidationExpression=".{4}.*" />

                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-2">Apelido: <span style="color: red;">*</span></label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtApelido" CssClass="form-control" MaxLength="250" runat="server" placeholder="Apelido" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtApelido" ErrorMessage="<br/><b>Apelido deve ser preenchido!</b>"
                                Display="Dynamic" SetFocusOnError="true" runat="server" />
                        </div>

                        <label class="control-label col-sm-2">Email: <span style="color: red;">*</span></label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtEmail" CssClass="form-control" MaxLength="250" runat="server" placeholder="email do estabelecimento" />

                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" ControlToValidate="txtEmail" ErrorMessage="<br/><b>Email deve ser preenchido!</b>"
                                Display="Dynamic" runat="server" />

                            <asp:RegularExpressionValidator SetFocusOnError="true" ID="validateEmail" runat="server"
                                ErrorMessage="<br/>E-mail inválido. O E-mail deve conter apenas letras numeros e pontos." ControlToValidate="txtEmail" Display="Dynamic"
                                ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" />
                        </div>

                    </div>

                    <div class="form-group">




                        <label class="control-label col-sm-2">Tema: </label>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddlTema" CssClass="form-control">
                                <asp:ListItem Text="Padrão" Value="Theme1"></asp:ListItem>
                                <asp:ListItem Text="Azul Amarelo" Value="TemaAzulAmarelo"></asp:ListItem>
                                <asp:ListItem Text="Cinza, Preto e Branco" Value="TemaCinzaPretoBranco"></asp:ListItem>
                                <asp:ListItem Text="Marrom, Verde e Amarelo" Value="TemaMarromVerdeAmarelo"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-2">Foto: </label>
                        <div class="col-sm-10">
                            <asp:FileUpload runat="server" ID="flFoto" />
                            <br />
                            <asp:CheckBox runat="server" ID="ckbExcluirFoto" Text="Excluir Foto?" Visible="False" />
                            <br />
                            <asp:Image runat="server" ID="imgFoto" Visible="False" />
                        </div>

                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-2">Código: <span style="color: red;">*</span></label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtCodigo" CssClass="form-control" MaxLength="50" runat="server" placeholder="código do estabelecimento" />
                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="RfvEmail" ControlToValidate="txtCodigo" ErrorMessage="<br/><b>Código deve ser preenchido!</b>"
                                Display="Dynamic" runat="server" />
                        </div>
                        <label class="control-label col-sm-2">CNPJ: </label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtCPF" CssClass="form-control" MaxLength="18" runat="server" placeholder="cnpj do contrato" onKeyDown="Mascara(this,Cnpj);" onKeyPress="Mascara(this,Cnpj);" onKeyUp="Mascara(this,Cnpj);" />
                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator6" ControlToValidate="txtCPF" ErrorMessage="<br/><b>CNPJ deve ser preenchido!</b>"
                                Display="Dynamic" runat="server" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="control-label col-sm-2">Telefone: <span style="color: red;">*</span></label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtTelefone" CssClass="form-control" MaxLength="18" runat="server" placeholder="telefone do estabelecimento" onKeyDown="Mascara(this,Telefone);" onKeyPress="Mascara(this,Telefone);" onKeyUp="Mascara(this,Telefone);" />
                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3" ControlToValidate="txtTelefone" ErrorMessage="<br/><b>Telefone deve ser preenchido!</b>"
                                Display="Dynamic" runat="server" />
                            <asp:RegularExpressionValidator
                                ID="RegularExpressionValidator3" SetFocusOnError="true" runat="server" ControlToValidate="txtTelefone"
                                Display="Dynamic" ErrorMessage="<br/><b>O campo telefone deve possuir no minímo 13 caracteres</b>"
                                ValidationExpression=".{13}.*" />
                        </div>
                        <label class="control-label col-sm-2"></label>
                        <div class="col-sm-4"></div>
                    </div>
                    <div class="form-group">


                        <label class="control-label col-sm-2">Valor por Entrega: <span style="color: red;">*</span></label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtValorEntrega" CssClass="form-control" MaxLength="18" runat="server" Text="0" placeholder="Valor por entrega" onKeyDown="Mascara(this,Valor);" onKeyPress="Mascara(this,Valor);" onKeyUp="Mascara(this,Valor);" />
                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator20" ControlToValidate="txtValorEntrega" ErrorMessage="<br/><b>Valor deve ser preenchido!</b>"
                                Display="Dynamic" runat="server" />
                        </div>

                        <label class="control-label col-sm-2">Valor Mínimo <span style="color: red;">*</span></label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtValorMinimo" CssClass="form-control" MaxLength="18" runat="server" Text="0" placeholder="Valor Mínimo" onKeyDown="Mascara(this,Valor);" onKeyPress="Mascara(this,Valor);" onKeyUp="Mascara(this,Valor);" />
                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator21" ControlToValidate="txtValorMinimo" ErrorMessage="<br/><b>Valor deve ser preenchido!</b>"
                                Display="Dynamic" runat="server" />
                        </div>

                    </div>

                    <div class="form-group">
                        <label class="control-label col-sm-2">Taxa Transferência: <span style="color: red;">*</span></label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtTransferencia" CssClass="form-control" MaxLength="18" runat="server" placeholder="Valor" onKeyDown="Mascara(this,Valor);" onKeyPress="Mascara(this,Valor);" onKeyUp="Mascara(this,Valor);" />
                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator15" ControlToValidate="txtTransferencia" ErrorMessage="<br/><b>Valor deve ser preenchido!</b>"
                                Display="Dynamic" runat="server" />
                        </div>
                        <label class="control-label col-sm-2">Taxa Emissão Boleto: <span style="color: red;">*</span></label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtEmissaoBoleto" CssClass="form-control" MaxLength="18" runat="server" placeholder="Valor" onKeyDown="Mascara(this,Valor);" onKeyPress="Mascara(this,Valor);" onKeyUp="Mascara(this,Valor);" />
                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator16" ControlToValidate="txtEmissaoBoleto" ErrorMessage="<br/><b>Valor deve ser preenchido!</b>"
                                Display="Dynamic" runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-2">Horário 1 Inicio: <span style="color: red;">*</span></label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="txtHorario1" CssClass="form-control" MaxLength="5" runat="server" placeholder="Valor" onKeyDown="Mascara(this,Hora);" onKeyPress="Mascara(this,Hora);" onKeyUp="Mascara(this,Hora);" />
                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator17" ControlToValidate="txtHorario1" ErrorMessage="<br/><b>Horário deve ser preenchido!</b>"
                                Display="Dynamic" runat="server" />

                        </div>
                        <label class="control-label col-sm-2">Horário 2 Inicio: <span style="color: red;">*</span></label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="txtHorario2" CssClass="form-control" MaxLength="5" runat="server" placeholder="Valor" onKeyDown="Mascara(this,Hora);" onKeyPress="Mascara(this,Hora);" onKeyUp="Mascara(this,Hora);" />
                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator18" ControlToValidate="txtHorario2" ErrorMessage="<br/><b>Horário deve ser preenchido!</b>"
                                Display="Dynamic" runat="server" />
                        </div>
                        <label class="control-label col-sm-2">Horário 3 Inicio: <span style="color: red;">*</span></label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="txtHorario3" CssClass="form-control" MaxLength="5" runat="server" placeholder="Valor" onKeyDown="Mascara(this,Hora);" onKeyPress="Mascara(this,Hora);" onKeyUp="Mascara(this,Hora);" />
                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator19" ControlToValidate="txtHorario3" ErrorMessage="<br/><b>Horário deve ser preenchido!</b>"
                                Display="Dynamic" runat="server" />

                        </div>
                    </div>
                    <Go4You:EnderecoControl runat="server" ID="objEndereco" />

                    <div class="form-group">
                        <label class="control-label col-sm-2">Código do Banco: <span style="color: red;">*</span></label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlBanco" CssClass="form-control" runat="server">
                                <asp:ListItem Value="0">Selecione o Banco</asp:ListItem>
                                <asp:ListItem Value="341">Itaú</asp:ListItem>
                                <asp:ListItem Value="237">Bradesco</asp:ListItem>
                                <asp:ListItem Value="104">Caixa Econômica</asp:ListItem>
                                <asp:ListItem Value="001">Banco do Brasil</asp:ListItem>
                                <asp:ListItem Value="033">Santander</asp:ListItem>
                                <asp:ListItem Value="070">BRB</asp:ListItem>
                                <asp:ListItem Value="077">Inter</asp:ListItem>
                                <asp:ListItem Value="756">Sicoob</asp:ListItem>
                                <asp:ListItem Value="748">Sicredi</asp:ListItem>
                                <asp:ListItem Value="041">Banrisul</asp:ListItem>
                                <asp:ListItem Value="085">Via Credi</asp:ListItem>
                                <asp:ListItem Value="655">Neon</asp:ListItem>
                                <asp:ListItem Value="260">Nubank</asp:ListItem>
                                <asp:ListItem Value="403">Cora</asp:ListItem>
                                <asp:ListItem Value="197">Stone</asp:ListItem>
                                <asp:ListItem Value="364">Gerencianet Pagamentos do Brasil</asp:ListItem>
                                <asp:ListItem Value="366">Banco C6</asp:ListItem>
                                <asp:ListItem Value="290">Pagseguro</asp:ListItem>
                                <asp:ListItem Value="422">Safra</asp:ListItem>

                            </asp:DropDownList>
                            <asp:RequiredFieldValidator Enabled="false" SetFocusOnError="true" ID="RequiredFieldValidator9" InitialValue="0" ControlToValidate="ddlBanco"
                                ErrorMessage="<br/><b>Selecione o banco!</b>"
                                Display="Dynamic" runat="server" />
                        </div>
                        <label class="control-label col-sm-2">Tipo de Conta: <span style="color: red;">*</span></label>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlTipoConta" CssClass="form-control" runat="server">
                                <asp:ListItem Value="0" Text="Selecione"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Corrente"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Poupança"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator Enabled="false" SetFocusOnError="true" ID="RequiredFieldValidator8" InitialValue="0" ControlToValidate="ddlTipoConta" ErrorMessage="<br/><b>Selecione o tipo de conta!</b>"
                                Display="Dynamic" runat="server" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="control-label col-sm-2">Agência: <span style="color: red;">*</span></label>
                        <div class="col-sm-4">
                            <asp:TextBox runat="server" ID="txtAgencia" CssClass="form-control" MaxLength="50" placeholder="agência" />
                            <asp:RequiredFieldValidator Enabled="false" SetFocusOnError="true" ID="RequiredFieldValidator10" ControlToValidate="txtAgencia" ErrorMessage="<br/><b>Informe a Agência!</b>"
                                Display="Dynamic" runat="server" />
                            <asp:Label ForeColor="Red" runat="server" ID="ltErroAgencia" Visible="False" />
                        </div>
                        <label class="control-label col-sm-2">Conta Corrente: <span style="color: red;">*</span></label>
                        <div class="col-sm-4">
                            <asp:TextBox runat="server" ID="txtCC" CssClass="form-control" MaxLength="50" placeholder="conta corrente" />
                            <asp:RequiredFieldValidator Enabled="false" SetFocusOnError="true" ID="RequiredFieldValidator11" ControlToValidate="txtCC" ErrorMessage="<br/><b>Informe a Conta corrente!</b>"
                                Display="Dynamic" runat="server" />
                            <asp:Label ForeColor="Red" runat="server" ID="ltErroCc" Visible="False" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-2">Emails Financeiro: </label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtEmailCobranca" CssClass="form-control" MaxLength="500" TextMode="MultiLine" runat="server" placeholder="Email para receber os boletos e Nf." />
                            <br />
                            <i>Separar por ";"</i>
                        </div>
                        <label class="control-label col-sm-2">Email Envio: </label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtEmailEnvio" CssClass="form-control" MaxLength="500" runat="server" placeholder="Email para envio para os clientes." />
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="control-label col-sm-2">Senha Padrão (GSR / Est): <span style="color: red;">*</span></label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtSenhaPadrao" CssClass="form-control" MaxLength="20" runat="server" placeholder="Senha Padrão" />
                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator7" ControlToValidate="txtSenhaPadrao" ErrorMessage="<br/><b>Senha Padrão deve ser preenchido!</b>"
                                Display="Dynamic" runat="server" />
                        </div>
                        <label class="control-label col-sm-2">Senha Padrão B.D.: </label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtSenhaPadraoBD" CssClass="form-control" MaxLength="20" runat="server" placeholder="Senha Padrão B.D." />
                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator12" ControlToValidate="txtSenhaPadraoBD" ErrorMessage="<br/><b>Senha Padrão deve ser preenchido!</b>"
                                Display="Dynamic" runat="server" />
                        </div>
                    </div>




                    <div class="form-group">
                        <label class="control-label col-sm-2">Website: </label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtWebsite" CssClass="form-control" MaxLength="500" runat="server" placeholder="Website." />
                        </div>

                        <label class="control-label col-sm-2">URL Novo Projeto: </label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtUrlNovoProjeto" CssClass="form-control" MaxLength="100" runat="server" />
                        </div>

                    </div>



                    <div class="form-group">
                        <label class="control-label col-sm-2"></label>
                        <div class="col-sm-4">
                            <div class="checkbox" style="margin-left: 25px;">
                                <asp:CheckBox runat="server" ID="ckbEmitirNfGsr" Text="Emitir Nf -> Gsr" />
                            </div>
                        </div>
                        <label class="control-label col-sm-2"></label>
                        <div class="col-sm-4">
                            <div class="checkbox" style="margin-left: 25px;">
                                <asp:CheckBox runat="server" ID="ckbEmitirNfParaEstabelecimento" Text="Emitir Nf -> Estabelecimento" />
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-2"></label>
                        <div class="col-sm-4">
                            <div class="checkbox" style="margin-left: 25px;">
                                <asp:CheckBox runat="server" ID="ckbCredenciamentoObrigatorio" Text="Credenciamento Obrigatório" />
                            </div>
                        </div>
                        <label class="control-label col-sm-2"></label>
                        <div class="col-sm-4">
                            <div class="checkbox" style="margin-left: 25px;">
                                <asp:CheckBox runat="server" ID="ckbFaturamentoAutomatico" Text="Faturamento Automático" />
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-2"></label>
                        <div class="col-sm-4">
                            <div class="checkbox" style="margin-left: 25px;">
                                <asp:CheckBox runat="server" ID="ckbCobrancaAutomatica" Text="Cobrança Automatica" />
                            </div>
                        </div>
                        <label class="control-label col-sm-2"></label>
                        <div class="col-sm-4">
                            <div class="checkbox" style="margin-left: 25px;">
                                <asp:CheckBox runat="server" ID="ckbCobrarSaas" Text="Cobrar SAAS" />
                            </div>
                        </div>
                    </div>


                    <div class="form-group">
                        <label class="control-label col-sm-2"></label>
                        <div class="col-sm-4">
                            <div class="checkbox" style="margin-left: 25px;">
                                <asp:CheckBox runat="server" ID="ckbExibirMei" Text="Exigir MEI GSR" />
                            </div>
                        </div>
                        <label class="control-label col-sm-2"></label>
                        <div class="col-sm-4">
                            <div class="checkbox" style="margin-left: 25px;">
                                <asp:CheckBox runat="server" ID="ckbDadosBancariosObrigatorioGsr" Text="Dados Bancarios Obrigatório para Gsr" />
                            </div>
                        </div>
                    </div>


                    <div class="form-group">

                        <label class="control-label col-sm-2"></label>
                        <div class="col-sm-4">
                            <div class="checkbox" style="margin-left: 25px;">
                                <asp:CheckBox runat="server" ID="ckbBloqueioChegueiGPS" Text="Habilitar bloqueio de GPS no Cheguei do APP" />
                            </div>
                        </div>

                        <label class="control-label col-sm-2"></label>
                        <div class="col-sm-4">
                            <div class="checkbox" style="margin-left: 25px;">
                                <asp:CheckBox runat="server" ID="ckbOtlDeAgenda" Text="OTL de Agenda iFood" />
                            </div>
                        </div>
                    </div>


                    <div class="form-group">
                        <label class="control-label col-sm-2"></label>
                        <div class="col-sm-4">
                            <div class="checkbox" style="margin-left: 25px;">
                                <asp:CheckBox runat="server" ID="ckbTransferirParaEntregadorRecebimentoBoleto" Text="Transferir Para Entregador na compensação do Boleto" />
                            </div>
                        </div>

                        <label class="control-label col-sm-2"></label>
                        <div class="col-sm-4">
                            <div class="checkbox" style="margin-left: 25px;">
                                <asp:CheckBox runat="server" ID="ckbColetaEntrega" Text="Ativar Coleta e Entrega" />
                            </div>
                        </div>

                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-2"></label>
                        <div class="col-sm-4">
                            <div class="checkbox" style="margin-left: 25px;">
                                <asp:CheckBox runat="server" ID="ckbAtivo" Text="OTL Ativo" />
                            </div>
                        </div>
                        <label class="control-label col-sm-2">Data de Saída: </label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtDataSaida" CssClass="form-control" MaxLength="10" runat="server" placeholder="dd/mm/aaaa" onKeyDown="Mascara(this,Data);" onKeyPress="Mascara(this,Data);" onKeyUp="Mascara(this,Data);" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-2">Cadastro: </label>
                        <div class="col-sm-4">
                            <div class="checkbox" style="margin-left: 25px;">
                                <asp:Literal runat="server" ID="ltDataCadastro" />
                            </div>
                        </div>
                        <label class="control-label col-sm-2">Data de Inicio de Operação:</label>
                        <div class="col-sm-4">
                            <asp:TextBox runat="server" ID="txtDataDeInicioOperacao" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="control-label col-sm-2">Forma de Cobrança: <span style="color: red;">*</span></label>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddlFormaDeCobranca" CssClass="form-control"></asp:DropDownList>
                        </div>


                        <label class="control-label col-sm-2">Data de Ativação Comercial:</label>
                        <div class="col-sm-4">
                            <asp:TextBox runat="server" ID="txtDataDeAtivacaoComercial" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>

                    </div>

                    <div class="form-group">
                        <label class="control-label col-sm-2">Usuários: </label>
                        <div class="col-sm-10">
                            <asp:Literal runat="server" ID="ltUsuarios" />
                        </div>
                    </div>


                    <div class="row">
                        <div class="background-yellow">
                            <asp:Button CssClass="btn btn-success btn-responsive" ID="BtnUpdate" OnClick="BtnUpdateClick"
                                runat="server" Text="Salvar" />&nbsp;&nbsp;
                <asp:Button ID="btnSalvarNovo" CssClass="btn btn-success btn-responsive" OnClick="BtnUpdateClick"
                    runat="server" Text="Salvar e Criar Novo" />&nbsp;&nbsp;
                <asp:Button ID="btnSalvarEditar" CssClass="btn btn-success btn-responsive" OnClick="BtnUpdateClick"
                    runat="server" Text="Salvar e Editar" />&nbsp;&nbsp;
                <asp:Button CssClass="btn btn-default btn-responsive" ID="BtnCancel" OnClick="BtnCancelClick" CausesValidation="false"
                    runat="server" Text="Cancelar" />
                        </div>
                    </div>
                </div>
            </article>
        </section>
    </main>

</asp:Content>
