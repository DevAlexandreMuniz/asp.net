<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true"
    CodeBehind="ContasAReceber.aspx.cs" Inherits="SaasAdmin.Apps.Saas.Faturas.ContasAReceber" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/cadastro.css" rel="stylesheet" />
    <link href="/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <script src="/js/bootstrap-multiselect.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $('#ddlContratoSaas').multiselect({
                includeSelectAllOption: true,
                nonSelectedText: 'Contratos Saas',
                enableFiltering: false,
                allSelectedText: 'Todos Contrato Saas',
                selectAllText: 'Selecionar Todos',
                selectAllValue: '0',
                require: true
            });
        });

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="container">

            <div class="row">
                <header class="page-header clearfix">
                    <h1>Contas a Receber<small></small></h1>
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

                        <div class="form-group">
                            <div class="input-group">
                                <asp:ListBox SelectionMode="Multiple" ID="ddlContratoSaas" ClientIDMode="Static" AppendDataBoundItems="true" DataValueField="Id" CssClass="col-md-5 form-control"
                                    DataTextField="Apelido" runat="server"></asp:ListBox>
                            </div>
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

                        <asp:Button runat="server" CssClass="btn btn-purple" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" />
                    </div>
                </section>
            </div>

            <div class="panel">
                <div class="panel-body">
                    <h3><asp:Literal runat="server" ID="ltOTL"></asp:Literal></h3>
                </div>
            </div>

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
