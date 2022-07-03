<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" CodeBehind="TransferirValor.aspx.cs" Inherits="SaasAdmin.Apps.Saas.TransferirValor" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/cadastro.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="container">
            <article class="content">

                <div class="row">
                    <header class="page-header clearfix">
                        <h1>Transferir Valor <small></small></h1>
                        <a href="Grid.aspx" class="btn btn-green pull-right">Ver Lista</a>
                    </header>
                </div>

                <div class="form-horizontal">

                    <div class="form-group">
                        <label class="control-label col-sm-2">Valor: <span style="color: red;">*</span></label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtValor" CssClass="form-control" MaxLength="20" runat="server" placeholder="valor do pagamento" onKeyDown="Mascara(this,Valor);" onKeyPress="Mascara(this,Valor);" onKeyUp="Mascara(this,Valor);" />
                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="RfvEmail" ControlToValidate="txtValor" ErrorMessage="<br/><b>Valor é obrigatório!</b>"
                                Display="Dynamic" runat="server" />
                        </div>
                        <label class="control-label col-sm-2">Enviar Conta Fisica? </label>
                        <div class="col-sm-4">
                            <asp:CheckBox runat="server" ID="ckbTransferido" Text="Enviar?" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="background-yellow">
                            <asp:Button CssClass="btn btn-success btn-responsive" ID="BtnUpdate" OnClick="BtnUpdateClick"
                                runat="server" Text="Salvar" />&nbsp;&nbsp;
                <asp:Button CssClass="btn btn-default btn-responsive" ID="BtnCancel" OnClick="BtnCancelClick" CausesValidation="false"
                    runat="server" Text="Cancelar" />
                        </div>
                    </div>
                </div>
            </article>
        </section>
    </main>

</asp:Content>
