using System;
using Dominio;
using Flurl.Http;
using System.Linq;
using SaasAdmin.AppCode;
using System.Threading.Tasks;
using Flurl;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Globalization;
using SaasAdmin.Services;

namespace SaasAdmin.Apps.Saas
{
    public partial class Painel : PageBase
    {
        private Go4YouEntities db;

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            db = new Go4YouEntities();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            CarregarIndicadores();
            CarregarGraficoDePedidosAnual();
        }

        private void CarregarIndicadores()
        {
            var contagemFoodeColeta = new ContarEntregadoresFoodDeliveryeColeta(db).Executar();
            var contagemAgenda = new ContarEntregadoresDaAgenda().Executar();

            qtdeTotalEntregadoresAtivos.Text = (contagemFoodeColeta.qtdeDeEntregadoresAtivos + contagemAgenda.qtdeDeEntregadoresAtivos).ToString();

            ltOtlsAtivos.Text = db.ContratoSaas.Count(a => a.Ativo).ToString();

            ltQtdeEstabelecimentosAtivos.Text = db.Estabelecimentos.Count(c => c.Ativo && c.ContratoSaa.Ativo).ToString();
        }

        private void CarregarGraficoDePedidosAnual()
        {
            List<DateTime> lDias = new List<DateTime> { };
            var primeiroPedido = db.Saidas.OrderBy(a => a.DataCriacao).First();

            var dtIni = new DateTime(primeiroPedido.Data.Year, 1, 1);

            while (dtIni < DateTime.Now)
            {
                lDias.Add(dtIni);
                dtIni = dtIni.AddMonths(1);
            }

            Hashtable lPedidos = new Hashtable();
            Hashtable lPedidosOff = new Hashtable();

            foreach (var hj in lDias.OrderBy(a => a))
            {
                var dataIni = new DateTime(hj.Year, hj.Month, hj.Day, 3, 0, 1);
                var dtF = hj.AddMonths(1);
                var dataFim = new DateTime(dtF.Year, dtF.Month, dtF.Day, 3, 0, 0);

                var pedidosHoje = db.Pedidoes.AsNoTracking().Where(x =>
                    x.Data > dataIni && x.Data <= dataFim &&
                    !x.Saida.Estabelecimento.Treinamento &&
                    x.Saida.StatusId == (int)StatusPedido.Finalizado).ToList();

                lPedidos.Add(hj, pedidosHoje);

                var pedidosOffHoje = db.PedidosOfflines.AsNoTracking().Where(x =>
                    x.Data > dataIni && x.Data <= dataFim &&
                    !x.Estabelecimento.Treinamento &&
                    x.PedidoStatusOfflines.OrderByDescending(z => z.Data).FirstOrDefault().StatusId == (int)StatusPedido.Finalizado).ToList();

                lPedidosOff.Add(hj, pedidosOffHoje);
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("$(function () {");
            sb.AppendLine("$('#graficoPedidosAnual').highcharts({");
            //sb.AppendLine("chart: { type: 'area' },");
            sb.AppendLine("title: { text: 'Gráfico de Pedidos Anual' },");
            sb.AppendLine("subtitle: { text: '' },");
            sb.AppendLine("xAxis: { categories: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'] },");
            sb.AppendLine("yAxis: {");
            sb.AppendLine("title: { text: 'Quantidade' }, plotLines: [{ value: 0, width: 1, color: '#808080' }]");
            sb.AppendLine("},");
            sb.AppendLine("tooltip: {");
            sb.AppendLine("valueSuffix: ' entregas'");
            sb.AppendLine("},");
            sb.AppendLine("legend: {");
            sb.AppendLine("layout: 'vertical',");
            sb.AppendLine("align: 'right',");
            sb.AppendLine("verticalAlign: 'middle',");
            sb.AppendLine("borderWidth: 0");
            sb.AppendLine("},");
            List<int> lAnos = lDias.Select(a => a.Year).Distinct().ToList();
            sb.AppendLine("series: [");
            int index = 0;
            foreach (var ano in lAnos.OrderByDescending(a => a))
            {
                var dtIniAno = new DateTime(ano, 1, 1, 3, 0, 1);
                var dtFimAno = new DateTime(ano + 1, 1, 1, 3, 0, 0);
                int qtdOnAno = db.Pedidoes.AsNoTracking().Count(x => !x.Saida.Estabelecimento.Treinamento && x.Data > dtIniAno && x.Data <= dtFimAno && x.Saida.StatusId == (int)StatusPedido.Finalizado);
                int qtdOffAno = db.PedidosOfflines.AsNoTracking().Count(x => !x.Estabelecimento.Treinamento && x.Data > dtIniAno && x.Data <= dtFimAno && x.PedidoStatusOfflines.OrderByDescending(z => z.Data).FirstOrDefault().StatusId == (int)StatusPedido.Finalizado);
                sb.AppendLine("{");
                sb.AppendLine($"name: '{ano} - {(qtdOffAno + qtdOnAno).ToString("N0", new CultureInfo("pt-BR"))}',");
                sb.AppendLine($"legendIndex:{index},");
                sb.AppendLine($"index:{index},");
                index++;
                sb.AppendLine("data: [");
                foreach (var dt in lDias.Where(a => a.Year == ano).OrderBy(a => a))
                {
                    //var dt = (DateTime)pedido;
                    var pedidos = ((List<Pedido>)lPedidos[dt]).ToList();
                    var pedidos2 = ((List<Dominio.PedidosOffline>)lPedidosOff[dt]).ToList();
                    sb.AppendLine(pedidos.Count() + pedidos2.Count() + ",");
                }
                sb.AppendLine("]},");
            }
            sb.AppendLine("]");
            sb.AppendLine("});");
            sb.AppendLine("});");

            Page.ClientScript.RegisterStartupScript(GetType(), "grafico", sb.ToString(), true);

        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            db.Dispose();
        }


    }
}