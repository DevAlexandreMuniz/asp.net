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

namespace SaasAdmin.Apps.Saas.Faturas
{
    public partial class ContasAReceber : PageBase
    {
        private Go4YouEntities db;
        private APIIuguFatura apiIuguFatura;
        const string tokeniuguDN = "a26e39704e252b38bb602740ca066657";
        private List<FaturaIuguDTO> faturasIuguDTO;
        private List<ContratoSaa> OtlsSelecionados = new List<ContratoSaa>();

        protected override void OnPreLoad(EventArgs e)
        {
            this.db = new Go4YouEntities();
            this.apiIuguFatura = new APIIuguFatura(tokeniuguDN);
            base.OnPreLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Contexto.Instance.HasPermission("SU");

            if (!IsPostBack)
            {
                CarregarOtlDoSelect();
                CarregarDatasDosFiltros();
            }
        }

        protected async void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var dataInicio = Convert.ToDateTime(txtDataInicio.Text);
                var dataFim = Convert.ToDateTime(txtDataFim.Text);

                BuscarOtlsSelecionados();

                if (!OtlsSelecionados.Any())
                {
                    Notificar("Selecione um Contrato Saas!");
                    return;
                }

                await CarregarFaturas(dataInicio, dataFim);
            }
            catch (Exception)
            {
                Notificar("Ocorreu um erro ao listar as Faturas geradas!");
                return;
            }
        }

        private async Task CarregarFaturas(DateTime dataInicio, DateTime dataFim)
        {
            var filtro = selectFiltroDeFaturas.Value;

            faturasIuguDTO = new List<FaturaIuguDTO>();

            foreach (var otl in OtlsSelecionados)
            {
                if (string.IsNullOrEmpty(otl.IuguClienteId))
                {
                    continue;
                }

                var faturas = await apiIuguFatura.BuscarFaturasPorClienteIdPorPeriodo(otl.IuguClienteId, filtro, dataInicio, dataFim);
                if (faturas.Any())
                {
                    if (string.IsNullOrEmpty(filtro))
                        faturas = faturas.Where(x => x.Ativa).ToList();

                    faturasIuguDTO.AddRange(faturas);
                }
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

            ltNomeDoOtl.Text = OtlsSelecionados.FirstOrDefault(x => x.Email == fatura.Email).Apelido ?? fatura.Email;
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
                Notificar("Fatura paga, não pode ser mais cancelada!");
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

        private void Notificar(string mensagem)
        {
            AppCode.Comum.Mensagem("Saas DN", mensagem);
        }

        private void CarregarOtlDoSelect()
        {
            ddlContratoSaas.AppendDataBoundItems = true;

            ddlContratoSaas.Items.Add(new ListItem("Otl", "0"));

            ddlContratoSaas.DataSource = db.ContratoSaas
                .AsNoTracking()
                .Where(a => a.DataFim == null)
                .OrderBy(a => a.Apelido)
                .ToList();

            ddlContratoSaas.DataBind();
        }

        private void CarregarDatasDosFiltros()
        {
            txtDataInicio.Text = DateTime.Now.PrimeiroDiaDoMes().ToString("dd/MM/yyyy");
            txtDataFim.Text = DateTime.Now.UltimoDiaDoMes().ToString("dd/MM/yyyy");
        }

        private void BuscarOtlsSelecionados()
        {
            var OTlsSelecionadosIds = new List<int>();

            foreach (ListItem item in ddlContratoSaas.Items)
            {
                int id = int.Parse(item.Value);

                if (item.Selected && id > 0)
                {
                    OTlsSelecionadosIds.Add(id);
                }
            }

            var otls = db.ContratoSaas
                .Where(x => OTlsSelecionadosIds.Contains(x.Id))
                .ToList();

            OtlsSelecionados.AddRange(otls);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            this.db.Dispose();
        }
    }
}