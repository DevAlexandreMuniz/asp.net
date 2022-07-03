<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true"
    CodeBehind="PainelDasFaturas.aspx.cs" Inherits="SaasAdmin.Apps.Saas.PainelDasFaturas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/cadastro.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="container">

            <div class="row">
                <header class="page-header clearfix">
                    <h1>Painel de Faturas<small></small></h1>
                </header>
                <section class="background-yellow">
                    <div class="form-inline">

                        <div class="form-group">
                            <select runat="server" id="selectFiltroDeFaturas" class="form-control">
                                <option value="">Todas</option>
                                <option value="paid">Pagas</option>
                                <option value="pending" selected="selected">Pendentes</option>
                                <option value="canceled">Canceladas</option>
                            </select>
                        </div>

                        <div class="input-group">
                            <asp:TextBox ID="txtDataInicio" MaxLength="10" runat="server" ClientIDMode="Static" placeholder="de dd/mm/aaaa" CssClass="col-md-5 form-control" />
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDataInicio" />
                            <asp:CompareValidator ID="CompareValidator2" ControlToValidate="txtDataInicio" ErrorMessage="<br/><b>Data Inválida!</b><br/><br/>Por favor digite uma data válida.<br/><br/>"
                                Type="Date" Operator="DataTypeCheck" Display="None" runat="server" />
                        </div>

                        <div class="input-group">
                            <asp:TextBox ID="txtDataFim" MaxLength="10" runat="server" ClientIDMode="Static" placeholder="até dd/mm/aaaa" CssClass="col-md-5 form-control" />
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDataFim" />
                            <asp:CompareValidator ID="CompareValidator1" ControlToValidate="txtDataFim" ErrorMessage="<br/><b>Data Inválida!</b><br/><br/>Por favor digite uma data válida.<br/><br/>"
                                Type="Date" Operator="DataTypeCheck" Display="None" runat="server" />
                        </div>

                        <asp:Button runat="server" CssClass="btn btn-purple ml-3" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" />
                    </div>
                </section>
            </div>

            <br />
            <div class="row text-center">

                <div class="col-xs-2">
                    <h2>
                        <asp:Literal runat="server" ID="ltTotalFaturado"></asp:Literal>
                    </h2>
                    Total Faturado
                </div>

                <div class="col-xs-2">
                    <h2>
                        <asp:Literal runat="server" ID="ltTotalAReceber"></asp:Literal>
                    </h2>
                    A Receber
                </div>

                <div class="col-xs-2">
                    <h2>
                        <asp:Literal runat="server" ID="ltTotalRecebido"></asp:Literal>
                    </h2>
                    Total Recebido
                </div>
                <div class="col-xs-2">
                    <h2>
                        <asp:Literal runat="server" ID="ltQtdFaturasAReceber"></asp:Literal>
                    </h2>
                    Quantidade de Faturas a Receber
                </div>
                 <div class="col-xs-2">
                    <h2>
                        <asp:Literal runat="server" ID="ltQtdFaturasRecebidas"></asp:Literal>
                    </h2>
                    Quantidade de Faturas Recebidas
                </div>
            </div>
            <br />

            <div class="row">
                <div class="col-md-12">
                    <table class="table">
                        <thead>
                            <th>Cliente</th>
                            <th>Descrição</th>
                            <th>Emissão</th>
                            <th>Status</th>
                            <th>Valor da Fatura</th>
                            <th>Valor Pago</th>
                            <th>Opções</th>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rptFaturas" OnItemDataBound="rptFaturas_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Literal runat="server" ID="ltNomeDoOtl"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:Literal runat="server" ID="ltDescricao"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:Literal runat="server" ID="ltEmissao"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:Literal runat="server" ID="ltStatusDoPagamento"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:Literal runat="server" ID="ltValor"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:Literal runat="server" ID="ltValorPago"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:LinkButton CssClass="btn btn-small btn-default" ID="btnCancelarFatura" OnCommand="btnCancelarFatura_Click" OnClientClick="return confirm('Deseja cancelar fatura?');" runat="server" ToolTip="Cancelar Fatura">
                                                <i class="fas fa-times"></i>
                                            </asp:LinkButton>
                                            <asp:HyperLink CssClass="btn btn-small btn-default" ID="hlVerFatura" Target="_blank" runat="server" ToolTip="Ver Fatura"> 
                                                <i class="fas fa-file-invoice-dollar"></i>
                                            </asp:HyperLink>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>

                </div>
            </div>
            <br />

        </section>
    </main>
</asp:Content>
