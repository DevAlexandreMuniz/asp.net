using SaasAdmin.AppCode;
using Dominio;
using Dominio.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SaasAdmin.Apps.Saas {
    public partial class TransferirValor : PageBaseAddEdit {


        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e) {
            Contexto.Instance.HasPermission("SU");
            if (!IsPostBack) {

            }
        }

        /// <summary>
        /// Handles the Click event of the BtnUpdate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnUpdateClick(object sender, EventArgs e) {
            using (var db = new Go4YouEntities()) {
                var obj = db.ContratoSaas.AsNoTracking().SingleOrDefault(p => p.Id == Id);

                var uuidGo4You = "76e229a7d7a20b2739884f594a7059d6";

                int valor = int.Parse(txtValor.Text.NumbersOnly());
                var sacar = ckbTransferido.Checked;
                var valorDouble = double.Parse(txtValor.Text);
                var valorFloat = float.Parse(txtValor.Text);

                Dominio.Negocio.Iugu.Geral.TransferirValores(uuidGo4You, obj.IuguAccountId, valor);

                if (sacar) {
                    Dominio.Negocio.Iugu.Geral.SacarGrana(valorFloat, obj.IuguAccountId, obj.IuguApiToken);
                }

                AppCode.Comum.Mensagem("Atenção", "Transferencia realizada com sucesso!");

                Response.Redirect("Grid.aspx");
            }
        }

        /// <summary>
        /// Handles the Click event of the BtnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnCancelClick(object sender, EventArgs e) {
            Response.Redirect("Grid.aspx");
        }
    }
}
