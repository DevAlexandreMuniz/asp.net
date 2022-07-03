<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true"
    CodeBehind="Painel.aspx.cs" Inherits="SaasAdmin.Apps.Saas.Painel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="container">

            <div class="row mt-2">

                <div class="col-sm-2">

                    <h1>
                        <i class="fas fa-motorcycle"></i>
                        <asp:Literal runat="server" ID="qtdeTotalEntregadoresAtivos"></asp:Literal>
                    </h1>
                    Entregadores Ativos

                    <h1 class="mt-4">
                        <i class="fas fa-store-alt"></i>
                        <asp:Literal runat="server" ID="ltQtdeEstabelecimentosAtivos"></asp:Literal>

                    </h1>
                    Estabelecimentos Ativos

                    <br />

                    <h1 class="mt-4">
                        <i class="fas fa-users"></i>
                        <asp:Literal runat="server" ID="ltOtlsAtivos"></asp:Literal>
                    </h1>

                    Operadores Logísticos Ativos

                </div>


                <div class="col-sm-10">

                    <div id="graficoPedidosAnual" class="grafico" style='height: 90vh; width: 100%;'></div>

                </div>
            </div>

        </section>

    </main>

</asp:Content>
