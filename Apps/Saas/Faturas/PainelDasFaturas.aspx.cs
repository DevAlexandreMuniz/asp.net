using Dominio;
using Dominio.Negocio;
using Dominio.Negocio.Iugu;
using Dominio.Negocio.Iugu.DTOs.Fatura;
using SaasAdmin.AppCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SaasAdmin.Apps.Saas
{
    public partial class PainelDasFaturas : PageBase
    {
        private Go4YouEntities db;
        private APIIuguFatura apiIuguFatura;
        const string tokeniuguDN = "a26e39704e252b38bb602740ca066657";
        private List<FaturaIuguDTO> faturasIuguDTO;
        private List<ContratoSaa> otls = new List<ContratoSaa>();

        protected override void OnPreLoad(EventArgs e)
        {
            this.db = new Go4YouEntities();
            this.apiIuguFatura = new APIIuguFatura(tokeniuguDN);
            base.OnPreLoad(e);
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            Contexto.Instance.HasPermission("SU");

            if (!IsPostBack)
            {
                CarregarDatasDosFiltros();
                await BuscarFaturas();
            }
        }

        protected async void btnBuscar_Click(object sender, EventArgs e)
        {
            await BuscarFaturas();
        }

        private async Task BuscarFaturas()
        {
            try
            {
                var dataInicio = Convert.ToDateTime(txtDataInicio.Text);
                var dataFim = Convert.ToDateTime(txtDataFim.Text);

                BuscarOtls();

                await CarregarFaturas(dataInicio, dataFim);

                CarregarValoresTotaisDoPainel();
            }
            catch (Exception ex)
            {
                Notificar("Ocorreu um erro ao listar as Faturas geradas!");
                return;
            }
        }

        private async Task CarregarFaturas(DateTime dataInicio, DateTime dataFim)
        {
            var otlClienteIds = otls
                .Where(x => !string.IsNullOrEmpty(x.IuguClienteId))
                .Select(x => x.IuguClienteId).ToList();

            var filtro = selectFiltroDeFaturas.Value;

            faturasIuguDTO = new List<FaturaIuguDTO>();

            if (string.IsNullOrEmpty(filtro))
            {
                var faturasPendentes = await apiIuguFatura.BuscarFaturasPorStatusPorPeriodo(StatusFaturaIugu.Pendente, dataInicio, dataFim);
                faturasIuguDTO.AddRange(faturasPendentes.Where(x => otlClienteIds.Contains(x.ClienteId)).ToList());

                var faturasPagas = await apiIuguFatura.BuscarFaturasPorStatusPorPeriodo(StatusFaturaIugu.Paga, dataInicio, dataFim);
                faturasIuguDTO.AddRange(faturasPagas.Where(x => otlClienteIds.Contains(x.ClienteId)).ToList());
            }
            else
            {
                var faturas = await apiIuguFatura.BuscarFaturasPorStatusPorPeriodo(filtro, dataInicio, dataFim);
                faturasIuguDTO.AddRange(faturas.Where(x => otlClienteIds.Contains(x.ClienteId)).ToList());
            }

            rptFaturas.DataSource = faturasIuguDTO
                    .OrderBy(x => x.Email)
                    .ThenBy(x => x.DataDeCriacao)
                    .ToList();

            rptFaturas.DataBind();
        }

        protected void rptFaturas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var fatura = (FaturaIuguDTO)e.Item.DataItem;

            var ltNomeDoOtl = (Literal)e.Item.FindControl("ltNomeDoOtl");
            var ltDescricao = (Literal)e.Item.FindControl("ltDescricao");
            var ltEmissao = (Literal)e.Item.FindControl("ltEmissao");
            var ltStatus = (Literal)e.Item.FindControl("ltStatusDoPagamento");
            var ltValor = (Literal)e.Item.FindControl("ltValor");
            var ltValorPago = (Literal)e.Item.FindControl("ltValorPago");
            var btnCancelarFatura = (LinkButton)e.Item.FindControl("btnCancelarFatura");
            var hlVerFatura = (HyperLink)e.Item.FindControl("hlVerFatura");

            ltNomeDoOtl.Text = otls.FirstOrDefault(x => x.Email == fatura.Email).Apelido ?? fatura.Email;
            ltDescricao.Text = fatura.DescricaoDaFatura;
            ltEmissao.Text = fatura.DataDeCriacao.ToString();
            ltStatus.Text = fatura.StatusDePagamento;
            ltValor.Text = fatura.TotalDaFatura;
            ltValorPago.Text = fatura.ValorPago;
            btnCancelarFatura.CommandArgument = fatura.Pago ? "" : fatura.Id;
            hlVerFatura.NavigateUrl = fatura.UrlPdf;
        }

        protected async void btnCancelarFatura_Click(Object sender, CommandEventArgs e)
        {
            var faturaId = e.CommandArgument.ToString();

            if (string.IsNullOrEmpty(faturaId))
            {
                Notificar("Erro ao cancelar fatura. Fatura paga, não pode ser mais cancelada!");
                return;
            }

            var fatura = db.Faturas.FirstOrDefault(x => x.IuguFaturaId.Contains(faturaId));

            if (fatura.Cancelada)
            {
                Notificar("Fatura cancelada!");
                return;
            }

            var faturacancelada = await apiIuguFatura.CancelarFatura(faturaId);
            if (!faturacancelada.DeuCerto)
            {
                Notificar("Erro ao cancelar a fatura na iugu! Tente novamente");
                return;
            }

            fatura.Cancelar();

            db.SaveChanges();

            Notificar("Fatura cancelada com sucesso!");
        }

        private void CarregarValoresTotaisDoPainel()
        {
            decimal totalFaturado = (decimal)faturasIuguDTO.Sum(x => x.ValorTotal) / 100;
            decimal totalAReceber = (decimal)faturasIuguDTO.Where(x => x.Pendente).Sum(x => x.ValorTotal) / 100;
            decimal totalRecebido = faturasIuguDTO.Sum(x => x.ValorPagoDecimal);
            int qtdDeFaturasRecebidas = faturasIuguDTO.Count(x => x.Pago);
            int qtdDeFaturasAReceber = faturasIuguDTO.Count(x => x.Pendente);

            ltTotalFaturado.Text = totalFaturado.ToString("c");
            ltTotalAReceber.Text = totalAReceber.ToString("c");
            ltTotalRecebido.Text = totalRecebido.ToString("c");
            ltQtdFaturasAReceber.Text = qtdDeFaturasAReceber.ToString();
            ltQtdFaturasRecebidas.Text = qtdDeFaturasRecebidas.ToString();
        }

        private void BuscarOtls()
        {
            var otls = db.ContratoSaas
                .Where(x => x.DataFim == null)
                .ToList();

            this.otls.AddRange(otls);
        }

        private void CarregarDatasDosFiltros()
        {
            txtDataInicio.Text = DateTime.Now.PrimeiroDiaDoMes().ToString("dd/MM/yyyy");
            txtDataFim.Text = DateTime.Now.UltimoDiaDoMes().ToString("dd/MM/yyyy");
        }

        private void Notificar(string mensagem)
        {
            AppCode.Comum.Mensagem("Saas DN", mensagem);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            this.db.Dispose();
        }
    }
}