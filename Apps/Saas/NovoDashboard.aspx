<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true"
    CodeBehind="NovoDashboard.aspx.cs" Inherits="SaasAdmin.Apps.Saas.NovoDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        div.col-md-3 {
            padding: 5px;
            border: 3px solid black;
        }

        div.col-md-12 {
            padding: 5px;
            border: 3px solid black;
        }

        section.container {
            padding: 20px;
        }

        canvas#graficoDePedidos {
            max-height: 300px;
        }
    </style>
    <main>
        <section class="container">
            <div class="row">
                <div class="col-md-3">
                    <h2>Soma de todos os contratos</h2>
                </div>

                <div class="col-md-3">
                    <h2>OTL´S Ativos
                        <asp:Literal runat="server" ID="OtlsAtivos"></asp:Literal></h2>
                </div>

                <div class="col-md-3">
                    <h2>Agendamentos Efetuados</h2>
                </div>

                <div class="col-md-3">
                    <h2>Entregadores Ativos
                        <asp:Literal runat="server" ID="Entregadores"></asp:Literal></h2>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <h2>Grafico de Entregas</h2>

                    <div class="input-group">
                        <asp:DropDownList ID="ddlOtl" DataTextField="Apelido" DataValueField="Id"
                            AppendDataBoundItems="true" CssClass="col-md-5 form-control" runat="server" />
                    </div>

                    <div class="input-group">
                        <asp:DropDownList ID="ddlAno" DataValueField="Id" AppendDataBoundItems="true"
                            CssClass="col-md-5 form-control" runat="server" />
                    </div>

                    <div class="input-group" style="margin-right: 10px;">
                        <asp:Button runat="server" CssClass="btn btn-purple" ID="btnBuscar" Text="Buscar Otl"
                            OnClick="BtnOtlSelecionado_Click" />
                    </div>
                </div>
            </div>

            <asp:HiddenField ClientIDMode="Static" ID="hiddenPedidosJson" runat="server" />

            <canvas id="graficoDePedidos" width="400" height="400"></canvas>
            <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
            
        </section>
    </main>-
    <script>
        MontarGraficoDePedidos()

        function MontarGraficoDePedidos() {

            let $divGrafico = document.querySelector('#graficoDePedidos')
            let $pedidos = document.getElementById('hiddenPedidosJson')
            let titulo = 'Grafico de Pedidos'
            let tipo = 'line'

            MontarGrafico($divGrafico, $pedidos, titulo, tipo)
        }

        function MontarGrafico($divGrafico, $pedidos, titulo, tipo) {

            let pedidos = JSON.parse($pedidos.value)

            const data = {
                labels: pedidos.map(a => a.mes),
                datasets: [{
                    label: titulo,
                    backgroundColor: 'rgb(255, 99, 132)',
                    borderColor: 'rgb(255, 99, 132)',
                    data: pedidos.map(m => m.quantidadeDePedidos),
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(255, 159, 64, 0.2)',
                        'rgba(255, 205, 86, 0.2)',
                        'rgba(75, 192, 192, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(153, 102, 255, 0.2)',
                        'rgba(201, 203, 207, 0.2)'
                    ],
                    borderColor: [
                        'rgb(255, 99, 132)',
                        'rgb(255, 159, 64)',
                        'rgb(255, 205, 86)',
                        'rgb(75, 192, 192)',
                        'rgb(54, 162, 235)',
                        'rgb(153, 102, 255)',
                        'rgb(201, 203, 207)'
                    ],
                    borderWidth: 1
                }]
            };

            const config = {
                type: tipo,
                data: data,
                options: {}
            };

            const myChart = new Chart($divGrafico, config);
        }
    </script>
</asp:Content>
