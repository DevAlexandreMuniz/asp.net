using Dominio;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SaasAdmin.AppCode;
using System.Web.UI.WebControls.Expressions;

namespace SaasAdmin.Apps.Saas
{
    public partial class Grid : PageBase, ICallbackEventHandler {
        private string _callBackStatus;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e) {
            GridExtra.RegisterDeleteCallBackReference(this);
            GridExtra.Secao = "SU";
            if (!IsPostBack) {

                Contexto.Instance.HasPermission(GridExtra.Secao);

                //EntityDataSource1.WhereParameters["SiteId"].DefaultValue = Convert.ToString(Contexto.Instance.SiteId);
            }
        }

        /// <summary>
        /// Handles the RowCreated event of the GridView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void GridView1RowCreated(object sender, GridViewRowEventArgs e) {
            GridExtra.GridViewRowCreated(sender, e, 3);
        }

        /// <summary>
        /// Handles the RowDataBound event of the GridView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void GridView1RowDataBound(object sender, GridViewRowEventArgs e) {
            GridExtra.GridViewRowDataBound(sender, e, 1);


        }

        /// <summary>
        /// Returns the results of a callback event that targets a control.
        /// </summary>
        /// <returns>The result of the callback.</returns>
        public string GetCallbackResult() {
            return _callBackStatus;
        }

        /// <summary>
        /// Processes a callback event that targets a control.
        /// </summary>
        /// <param name="eventArgument">A string that represents an event argument to pass to the event handler.</param>
        public void RaiseCallbackEvent(string eventArgument) {
            _callBackStatus = "failed";

            if (!String.IsNullOrEmpty(eventArgument)) {
                string[] eventArgumentArray = eventArgument.Split(",".ToCharArray(), StringSplitOptions.None);
                int id0 = Convert.ToInt32(eventArgumentArray[0]);
                using (var db = new Go4YouEntities()) {

                    var itemToDelete = db.ContratoSaas.Single(a => a.Id == id0);

                    db.ContratoSaas.Remove(itemToDelete);

                    try {
                        SalvarDados(db);

                        _callBackStatus = "success";
                    }
                    catch (Exception ex) {
                        _callBackStatus = ex.ToString();
                    }
                }
            }
        }



        /// <summary>
        /// Handles the OnRowCommand event of the GridView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void GridView1_OnRowCommand(object sender, GridViewCommandEventArgs e) {

            //if (e.CommandName == "ReiniciarSenha") {
            //    string email = e.CommandArgument.ToString();
            //    MembershipUser oUser = Membership.GetUser(email);

            //    if (oUser != null) {
            //        if (oUser.IsLockedOut)
            //            oUser.UnlockUser();

            //        string oldpswd = oUser.ResetPassword(Contexto.Instance.SaasNome);
            //        string newpass = "go4you2015";

            //        oUser.ChangePassword(oldpswd, newpass);
            //        string msg = "Sua senha foi reiniciada com sucesso! <br/>Nova senha: " + newpass;

            //        EMail.SendEmail(Contexto.Instance.ContratoSaas.LogoId.Value, "DN", Contexto.Instance.ContratoSaas.EmailEnvio, oUser.UserName, oUser.Email, "Senha", msg);

            //        ClientScript.RegisterStartupScript(GetType(), Guid.NewGuid().ToString(),
            //            "alerta('Senha do Estabelecimento foi reiniciada com sucesso !',15000);", true);
            //    }
            //    else {
            //        MembershipCreateStatus status;
            //        string senha = "go4you2015";
            //        MembershipUser newUser = Membership.CreateUser(email, senha, email, "Software", "Go4You", true, out status);

            //        if (newUser != null) {
            //            using (var db = new Go4YouEntities()) {
            //                var admin = db.Administradores.Single(a => a.Email.ToLower() == email);

            //                if (newUser.ProviderUserKey != null)
            //                    SaasAdmin.UserId = (Guid)newUser.ProviderUserKey;

            //                SalvarDados(db);

            //                if (!Roles.IsUserInRole(email, "Estabelecimento"))
            //                    Roles.AddUserToRole(email, "Estabelecimento");

            //                string msg = "Sua senha foi reiniciada com sucesso! <br/>Nova senha: " + senha;

            //                EMail.SendEmail(Contexto.Instance.ContratoSaas.LogoId.Value, "DN", Contexto.Instance.ContratoSaas.EmailEnvio, email, email, "Senha", msg);

            //                ClientScript.RegisterStartupScript(GetType(), Guid.NewGuid().ToString(),
            //                    "alerta('Senha do Estabelecimento foi reiniciada com sucesso !',15000);", true);
            //            }
            //        }
            //    }
            //}
        }

    }
}