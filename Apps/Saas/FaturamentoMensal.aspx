<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" Async="true" CodeBehind="FaturamentoMensal.aspx.cs" Inherits="SaasAdmin.Apps.Saas.FaturamentoMensal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/js/GridViewScript.js" type="text/javascript"></script>
    <link href="/css/filtro.css" rel="stylesheet" />
    <link href="/css/ligthbox-deletar.css?1=1" rel="stylesheet" />
    <link href="/css/skins/minimal/minimal.css" rel="stylesheet" />
    <link href="../../css/tabelaTop.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="container">
            <article class="content">

                <div class="row">
                    <header class="page-header clearfix">
                        <h1>
                            <i class="fas fa-coins"></i>
                            Dashboard
                        </h1>
                    </header>
                    <section class="background-yellow">
                        <div class="form-inline">
                            <div class="form-group">
                            </div>

                            <div class="form-group">
                                <select runat="server" id="statusDoOtl" class="form-control">
                                    <option value="" selected="selected">Todos</option>
                                    <option value="Onboarding">Onboarding</option>
                                    <option value="Ativo">Ativos</option>
                                </select>
                            </div>

                            <div class="form-group">
                                <div class="input-group">
                                    <asp:TextBox ID="txtDataInicio" CssClass="col-md-5 form-control" runat="server" AutoPostBack="false" TextMode="Date" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="input-group">
                                    <asp:TextBox ID="txtDataFinal" CssClass="col-md-5 form-control" runat="server" AutoPostBack="false" TextMode="Date" />
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="input-group">
                                    <asp:TextBox CssClass="col-md-5 form-control" placeholder="Pesquisar" runat="server" ID="txtPesquisar" AutoPostBack="true" MaxLength="100" />
                                    <div class="input-group-addon">
                                        <img src="/img/icon-search.png" alt="Procurar">
                                    </div>
                                </div>
                            </div>
                            <button runat="server" onclick="BtnUpdateClick" class="btn btn-purple">Buscar</button>
                        </div>
                    </section>
                </div>
                <br />

                <div class="row text-center">

                    <div class="col-xs-1">
                        <h2>
                            <i class="fas fa-users"></i>
                            <asp:Literal runat="server" ID="ltClientesAtivos"></asp:Literal>
                        </h2>
                        Clientes
                    </div>

                    <div class="col-xs-1">
                        <h2>
                            <i class="fas fa-wifi"></i>
                            <asp:Literal runat="server" ID="qtdeEntregasOnline"></asp:Literal>
                        </h2>
                        Entregas Online
                    </div>

                    <div class="col-xs-1">
                        <h2>
                            <i class="fas fa-unlink"></i>
                            <asp:Literal runat="server" ID="qtdeEntregasOffline"></asp:Literal>
                        </h2>
                        Entregas Offline
                    </div>

                    <div class="col-xs-1">
                        <h2>
                            <i class="fas fa-box-open"></i>
                            <asp:Literal runat="server" ID="qtdeColetas"></asp:Literal>
                        </h2>
                        Coletas
                    </div>

                    <div class="col-xs-1">
                        <h2>
                            <i class="fas fa-boxes"></i>
                            <asp:Literal runat="server" ID="qtdeTotalDeEntregas"></asp:Literal>
                        </h2>
                        Total de Entregas
                    </div>
                    <div class="col-xs-2">
                        <h2>
                            <asp:Literal runat="server" ID="ltFaturamentoTotal"></asp:Literal>
                        </h2>
                        Faturamento Total
                    </div>
                    <div class="col-xs-2">
                        <h2>
                            <asp:Literal runat="server" ID="ltGap"></asp:Literal>
                        </h2>
                        GAP De Faturamento
                    </div>
                    <div class="col-xs-2">
                        <h2>
                            <asp:Literal runat="server" ID="ltFaturamentoPrevisto"></asp:Literal>
                        </h2>
                        Faturamento Previsto
                    </div>
                </div>

                <br />

                <asp:GridView EmptyDataText="Nenhum item encontrado." AutoGenerateColumns="False" CssClass="table" ID="gridSaas" runat="server"
                    DataKeyNames="Id" AllowPaging="false" OnRowCommand="gridSaas_OnRowCommand" AllowCustomPaging="false">
                    <PagerStyle CssClass="pagination-ys" />
                    <Columns>
                        <asp:BoundField DataField="NomeFantasia" HeaderText="OTL" />
                        <asp:BoundField DataField="DataDeInicioDeOperacao" HeaderText="Início do Onboarding" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="DataDeAtivacaoComercial" HeaderText="Ativação" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="ContagemDasEntregas.Total" HeaderText="Entregas" />
                        <asp:BoundField DataField="ValorDaEntrega" HeaderText="Valor da Entrega" DataFormatString="{0:C}" />
                        <asp:BoundField DataField="ValorMinimo" HeaderText="Valor Mínimo" DataFormatString="{0:C}" />
                        <asp:BoundField DataField="TotalDeCobranca" HeaderText="Total" DataFormatString="{0:C}" />
                        <asp:TemplateField HeaderText="Gerar Fatura">
                            <ItemTemplate>
                                <asp:LinkButton CssClass="btn" ID="btnGerarFatura" CommandArgument='<%# Eval("Id") %>' CommandName="GerarFatura" OnClientClick="return confirm('Confirmar valor e gerar a fatura?');" runat="server" ToolTip="Gerar Fatura">
                                    <i class="fas fa-file-invoice-dollar"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <br />
                <br />
                <br />
                <br />

            </article>
        </section>
    </main>

</asp:Content>
