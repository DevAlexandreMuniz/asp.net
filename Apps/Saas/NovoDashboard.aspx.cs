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
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace SaasAdmin.Apps.Saas
{
    public partial class NovoDashboard : PageBase
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

            OtlsAtivos.Text = db.ContratoSaas
                .Count(a => a.Ativo)
                .ToString();

            Entregadores.Text = db.Motoqueiros
                .Count(a => a.Ativo && a.ContratoSaa.Ativo)
                .ToString();

            CarregarListaDeOTLS();
            CarregarListaDeAnos();

        }

        private void CarregarListaDeOTLS()
        {
            using (var db = new Go4YouEntities())
            {
                ddlOtl.Items.Clear();
                ddlOtl.AppendDataBoundItems = true;
                ddlOtl.Items.Add(new ListItem("Selecione o Otl", "0"));
                

                var otl = db.ContratoSaas.Where(a => a.Ativo).OrderBy(a => a.Apelido).ToList();

                ddlOtl.DataSource = otl;
                ddlOtl.DataBind();
            }
        }

        private void CarregarListaDeAnos()
        {
            using (var db = new Go4YouEntities())
            {
                ddlAno.Items.Clear();
                ddlAno.AppendDataBoundItems = true;
                ddlAno.Items.Add(new ListItem("Selecione o Ano", "0"));
                ddlAno.Items.Add(new ListItem("2015", "2015"));
                ddlAno.Items.Add(new ListItem("2016", "2016"));
                ddlAno.Items.Add(new ListItem("2017", "2017"));
                ddlAno.Items.Add(new ListItem("2018", "2018"));
                ddlAno.Items.Add(new ListItem("2019", "2019"));
                ddlAno.Items.Add(new ListItem("2020", "2020"));
                ddlAno.Items.Add(new ListItem("2021", "2021"));
                ddlAno.Items.Add(new ListItem("2022", "2022"));
            }
        }

        protected void BtnOtlSelecionado_Click(object sender, EventArgs e)
        {
            var otl = int.Parse(ddlOtl.SelectedValue);
            var ano = int.Parse(ddlAno.SelectedValue);

            

            var listaPedidosPorMes = db.Pedidoes
                .Where(w =>
                    w.DataCriacao.Year == ano && w.Saida.ContratoSaasId == otl)
                .Select(s => new {
                    s.Id,
                    s.DataCriacao
                })
                .ToList();

            var objetosDoGrafico = listaPedidosPorMes
                .OrderBy(a => a.DataCriacao.Month )
                .GroupBy(g => g.DataCriacao.ToString("MMM"))
                .Select(s => new TodosOsPedidos
                {
                    mes = s.Key,
                    quantidadeDePedidos = s.Count()
                })
                .ToList();
            
            string jsonListaDePedidos = JsonConvert.SerializeObject(objetosDoGrafico);
            hiddenPedidosJson.Value = jsonListaDePedidos;
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            db.Dispose();
        }

    }
    
}