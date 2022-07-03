using Dominio;
using Dominio.Negocio.Iugu;
using Dominio.Negocio.Iugu.DTOs.Fatura;
using Dominio.Servicos;
using SaasAdmin.AppCode;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SaasAdmin.Apps.Saas
{
    public partial class FaturamentoMensal : PageBase
    {
        private Go4YouEntities db;
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            db = new Go4YouEntities();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GridExtra.Secao = "SU";
            Contexto.Instance.HasPermission(GridExtra.Secao);
            var hoje = DateTime.Now;
            var diasDoMes = DateTime.DaysInMonth(hoje.Year, hoje.Month);

            if (!IsPostBack)
            {
                txtDataInicio.Text = new DateTime(hoje.Year, hoje.Month, 1).ToString("yyyy-MM-dd");
                txtDataFinal.Text = new DateTime(hoje.Year, hoje.Month, diasDoMes).ToString("yyyy-MM-dd");
            }

            CarregarGrid();
        }

        protected void BtnUpdateClick(object sender, EventArgs e)
        {
            CarregarGrid();
        }

        private void CarregarGrid()
        {
            var filtroDeTexto = txtPesquisar.Text;

            var queryOtls = db.ContratoSaas.Where(x => x.Ativo).AsQueryable();
            
            if (!string.IsNullOrEmpty(filtroDeTexto))
                queryOtls = queryOtls.Where(w => w.Apelido.Contains(filtroDeTexto));

            if (statusDoOtl.Value == "Ativo")
                queryOtls = queryOtls.Where(x => x.Ativo && x.DataDeAtivacaoComercial.HasValue);

            if (statusDoOtl.Value == "Onboarding")
                queryOtls = queryOtls.Where(x => x.Ativo && x.DataInicio.HasValue && !x.DataDeAtivacaoComercial.HasValue);


            var lista = queryOtls
                .OrderBy(a => a.NomeFantasia)
                .Select(s => new SaasDTO
                {
                    Id = s.Id,
                    NomeFantasia = s.NomeFantasia,
                    ValorDaEntrega = s.ValorPorEntrega,
                    ValorMinimo = s.ValorDeCobrancaMinimo,
                    DataDeAtivacaoComercial = s.DataDeAtivacaoComercial,
                    DataDeInicioDeOperacao = s.DataInicio
                })
                .ToList();

            var inicio = Convert.ToDateTime(txtDataInicio.Text);
            var fim = Convert.ToDateTime(txtDataFinal.Text);

            lista.ForEach(otl => CalcularValorDeCobranca(otl, inicio, fim));

            qtdeColetas.Text = lista.Sum(a => a.ContagemDasEntregas.QuantidadeColetas).ToString();
            qtdeEntregasOnline.Text = lista.Sum(a => a.ContagemDasEntregas.QuantidadePedidosOnline).ToString();
            qtdeEntregasOffline.Text = lista.Sum(a => a.ContagemDasEntregas.QuantidadePedidosOffline).ToString();
            qtdeTotalDeEntregas.Text = lista.Sum(a => a.ContagemDasEntregas.Total).ToString();

            ltClientesAtivos.Text = lista.Count.ToString();

            var totalDeCobrança = lista.Sum(a => a.TotalDeCobranca);
            var totalGap = lista.Where(x => x.DataDeInicioDeOperacao.HasValue && !x.DataDeAtivacaoComercial.HasValue).Sum(a => a.ValorMinimo);
            var totalPrevisto = totalDeCobrança + totalGap;

            ltFaturamentoPrevisto.Text = totalPrevisto.ToString("C");
            ltGap.Text = totalGap.ToString("C");
            ltFaturamentoTotal.Text = totalDeCobrança.ToString("C");

            gridSaas.DataSource = lista;
            gridSaas.DataBind();
        }

        private void CalcularValorDeCobranca(SaasDTO saasDto, DateTime inicio, DateTime fim)
        {
            var otlAtivadoComercialmenteNoPeriodoDoRelatório = saasDto.DataDeAtivacaoComercial.HasValue && saasDto.DataDeAtivacaoComercial < fim;

            saasDto.ContagemDasEntregas = new ContarPedidosFinalizados(db).Executar(inicio, fim, saasDto.Id);

            if (!otlAtivadoComercialmenteNoPeriodoDoRelatório)
                return;

            var otlAtivadoComercialmenteNoMeioDoMesDoRelatorio = inicio < saasDto.DataDeAtivacaoComercial;

            if (otlAtivadoComercialmenteNoMeioDoMesDoRelatorio)
                inicio = saasDto.DataDeAtivacaoComercial.Value;

            saasDto.TotalDeCobranca = (saasDto.ContagemDasEntregas.Total * saasDto.ValorDaEntrega);

            if (saasDto.ValorMinimo > saasDto.TotalDeCobranca)
                saasDto.TotalDeCobranca = saasDto.ValorMinimo;
        }

        protected async void gridSaas_OnRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "GerarFatura")
            {
                var contratoSaasId = Convert.ToInt32(e.CommandArgument);

                await GerarFaturaParaOtl(contratoSaasId);
            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            db.Dispose();
        }

        private async Task GerarFaturaParaOtl(int otlId)
        {
            var hoje = DateTime.Now.AddMonths(-1);
            var diasDoMes = DateTime.DaysInMonth(hoje.Year, hoje.Month);

            var dataInicio = new DateTime(hoje.Year, hoje.Month, 1).Date;
            var dataFim = new DateTime(hoje.Year, hoje.Month, diasDoMes).Date;

            var periodoJaFaturado = db.Faturas
                .Any(x => x.ContratoSaasId == otlId &&
                          x.EstabelecimentoId == null &&
                          !x.Cancelada &&
                          x.PeriodoDeInicio == dataInicio);

            if (periodoJaFaturado)
            {
                Notificar("Período já faturado!");
                return;
            }

            var otl = db.ContratoSaas
                .FirstOrDefault(x => x.Id == otlId);

            var otlDto = new SaasDTO
            {
                Id = otl.Id,
                NomeFantasia = otl.NomeFantasia,
                ValorDaEntrega = otl.ValorPorEntrega,
                ValorMinimo = otl.ValorDeCobrancaMinimo,
                DataDeAtivacaoComercial = otl.DataDeAtivacaoComercial,
                DataDeInicioDeOperacao = otl.DataInicio,
                DataDeSaida = otl.DataFim
            };

            CalcularValorDeCobranca(otlDto, dataInicio, dataFim);

            var valorMinimoParaGerarFatura = otlDto.TotalDeCobranca < 5;
            if (valorMinimoParaGerarFatura)
            {
                Notificar("Valor não pode ser menor que R$ 5,00");
                return;
            }

            var descricaDaCobranca = $"Período de cobrança - {dataInicio.ToString("MMMM")} {dataInicio.ToString("MM")}/{dataInicio.ToString("yyyy")}";
            var faturaIugu = await CriarFaturaIugu(otl, descricaDaCobranca, otlDto.TotalDeCobranca);

            if (faturaIugu.DeuCerto)
            {
                var fatura = new Fatura(otl.Id, dataInicio, dataFim, otlDto.TotalDeCobranca, faturaIugu.Id);

                db.Faturas.Add(fatura);

                db.SaveChanges();
                Notificar($"Fatura para o {otl.Apelido} gerada com sucesso!");
            }
            else
            {
                Notificar("Não foi possível gerar a fatura, verifique o cadastro do OTL!");
            }
        }

        private async Task<FaturaIuguDTO> CriarFaturaIugu(ContratoSaa otl, string descricaoDaCobranca, decimal valorParaCobrar)
        {
            var apiTokenDN = "a26e39704e252b38bb602740ca066657";
            var servicoFaturaiugu = new APIIuguFatura(apiTokenDN);
            var diasParaVencimento = 5;

            var novaFatura = new CriarFaturaDTO();

            novaFatura.AdicionarPagadorParaBoleto(
                otl.RazaoSocial,
                otl.Cnpj.Replace(".", "").Replace("-", ""),
                otl.Email,
                otl.Endereco.Logradouro,
                otl.Endereco.Numero,
                otl.Endereco.Bairro,
                otl.Endereco.Cidade,
                otl.Endereco.Estado,
                otl.Endereco.CEP.Replace(".", "").Replace("-", ""));

            novaFatura.customer_id = otl.IuguClienteId;
            novaFatura.AdicionarDiasParaOVencimento(diasParaVencimento, somenteEmDiaUtil: false);
            novaFatura.AdicionarCobranca(descricaoDaCobranca, valorParaCobrar);
            novaFatura.AdicionarCustoDeEmissaoDoBoleto(otl.TaxaEmissaoBoleto);

            return await servicoFaturaiugu.CriarFatura(novaFatura);
        }

        private void Notificar(string mensagem)
        {
            Comum.Mensagem("Saas DN", mensagem);
        }
    }

    public class SaasDTO
    {
        public int Id { get; set; }
        public string NomeFantasia { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
        public decimal ValorDaEntrega { get; set; }
        public decimal ValorMinimo { get; set; }
        public decimal TotalDeCobranca { get; set; }
        public string RazaoSocial { get; set; }
        public ContarPedidosFinalizados.TotaisDTO ContagemDasEntregas { get; set; } = new ContarPedidosFinalizados.TotaisDTO();
        public DateTime? DataDeAtivacaoComercial { get; set; }
        public DateTime? DataDeInicioDeOperacao { get; set; }
        public DateTime? DataDeSaida { get; set; }
    }
}