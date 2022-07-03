<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true"
    CodeBehind="Grid.aspx.cs" Inherits="SaasAdmin.Apps.Saas.Grid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/js/GridViewScript.js" type="text/javascript"></script>
    <link href="/css/filtro.css" rel="stylesheet" />
    <link href="/css/lista.css" rel="stylesheet" />
    <link href="/css/ligthbox-deletar.css?1=1" rel="stylesheet" />
    <link href="/css/skins/minimal/minimal.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="container">
            <article class="content">

                <div class="row">
                    <header class="page-header clearfix">
                        <h1>
                            Clientes
                        </h1>
                        <asp:HyperLink CssClass="btn btn-success pull-right" runat="server" ID="HyperLink1" NavigateUrl="AddEdit.aspx">Adicionar</asp:HyperLink>
                    </header>
                    <section class="background-yellow">
                        <div class="form-inline">
                            <div class="form-group">
                                <div class="input-group">
                                    <asp:DropDownList ID="ddlAtivo" AppendDataBoundItems="true" CssClass="col-md-5 form-control" runat="server" AutoPostBack="true">
                                        <asp:ListItem Text="Ativo" Value="true" Selected="True">Ativo</asp:ListItem>
                                        <asp:ListItem Text="Inativo" Value="false">Inativo</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="input-group">
                                    <asp:TextBox CssClass="col-md-5 form-control" placeholder="Pesquisar" runat="server" ID="txNome" AutoPostBack="true" MaxLength="100" />
                                    <div class="input-group-addon">
                                        <img src="/img/icon-search.png" alt="Procurar">
                                    </div>
                                </div>
                            </div>
                            <button type="button" class="btn btn-purple">Buscar</button>
                        </div>
                    </section>
                </div>
                <br />
                <br />
                <section class="pesquisa table-responsive">
                    <asp:GridView EmptyDataText="Nenhum item encontrado." AutoGenerateColumns="False" CssClass="table" ID="GridView1" runat="server" OnRowCreated="GridView1RowCreated"
                        DataKeyNames="Id" DataSourceID="EntityDataSource1" OnRowCommand="GridView1_OnRowCommand" OnRowDataBound="GridView1RowDataBound" AllowPaging="false" AllowCustomPaging="false">
                        <PagerStyle CssClass="pagination-ys" />
                        <Columns>
                            <asp:BoundField DataField="NomeFantasia" HeaderText="Nome" ReadOnly="true" SortExpression="NomeFantasia" />
                            <asp:BoundField DataField="Email" HeaderText="Email" ReadOnly="true" SortExpression="Email" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" />
                            <asp:BoundField DataField="RazaoSocial" HeaderText="Razão Social" ReadOnly="true" SortExpression="RazaoSocial" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" />
                            <asp:TemplateField HeaderText="Opções">
                                <ItemStyle HorizontalAlign="Center" CssClass="tabela-acoes" />
                                <ItemTemplate>
                                    <a class="editar" href="TransferirValor.aspx?Id=<%# Eval("Id") %>">💰    </a>
                                    <a class="editar" href="AddEdit.aspx?Id=<%# Eval("Id") %>">
                                        <img src="/img/icon-edit.png" alt="Clique para Editar" style="border: none;" /></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </section>
                <Go4You:DeleteControl runat="server" ID="deleteControl1" />
                <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Go4YouEntities"
                    DefaultContainerName="Go4YouEntities" EnableFlattening="False" EntitySetName="ContratoSaas">
                    <%--Where="it.SiteId = @SiteId"--%>
                    <WhereParameters>
                        <%--<asp:Parameter Name="SiteId" DbType="Int32" />--%>
                    </WhereParameters>
                </asp:EntityDataSource>
                <asp:QueryExtender ID="QueryExtender1" runat="server" TargetControlID="EntityDataSource1">
                    <asp:SearchExpression SearchType="Contains" DataFields="NomeFantasia, Email, Apelido">
                        <asp:ControlParameter ControlID="txNome" DbType="String" Name="NomeFantasia" PropertyName="Text" />
                        <asp:ControlParameter ControlID="txNome" DbType="String" Name="Email" PropertyName="Text" />
                        <asp:ControlParameter ControlID="txNome" DbType="String" Name="Apelido" PropertyName="Text" />
                    </asp:SearchExpression>
                    <asp:PropertyExpression>
                        <asp:ControlParameter ControlID="ddlAtivo" DbType="Boolean" Name="Ativo" />
                    </asp:PropertyExpression>

                </asp:QueryExtender>
            </article>
        </section>
    </main>

</asp:Content>
