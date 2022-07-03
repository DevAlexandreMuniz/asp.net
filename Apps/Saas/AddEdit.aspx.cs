using Dominio;
using Dominio.Negocio;
using Dominio.Negocio.Iugu;
using Dominio.Servicos;
using SaasAdmin.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SaasAdmin.Apps.Saas
{
    public partial class AddEdit : PageBaseAddEdit
    {
        private APIShield shield;

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            shield = new APIShield();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Contexto.Instance.HasPermission("SU");

            if (ddlFormaDeCobranca.Items.Count is 0)
            {
                ddlFormaDeCobranca.Items.Add(new ListItem("Distancia por Raio", FormasDeCobranca.DistanciaPorRaio));
                ddlFormaDeCobranca.Items.Add(new ListItem("Rota Recomendada pelo GMaps", FormasDeCobranca.RotaRecomendadaDoGMaps));
            }

            if (!IsPostBack)
            {
                using (var db = new Go4YouEntities())
                {
                    if (IsEditing)
                    {
                        var contratoSaas = db.ContratoSaas.AsNoTracking().Single(p => p.Id == Id);
                        txtEmail.Text = contratoSaas.Email;
                        txtApelido.Text = contratoSaas.Apelido;
                        txtNomeFantasia.Text = contratoSaas.NomeFantasia;
                        txtRazaoSocial.Text = contratoSaas.RazaoSocial;
                        txtTelefone.Text = contratoSaas.Telefone;
                        ckbAtivo.Checked = contratoSaas.Ativo;
                        ckbCobrancaAutomatica.Checked = contratoSaas.CobrancaAutomatica;
                        ckbCredenciamentoObrigatorio.Checked = contratoSaas.CredenciamentoObrigatorio;
                        ckbEmitirNfGsr.Checked = contratoSaas.EmitirNfGsr;
                        ckbEmitirNfParaEstabelecimento.Checked = contratoSaas.EmitirNfParaEstabelecimento;
                        ckbFaturamentoAutomatico.Checked = contratoSaas.FaturamentoAutomatico;
                        ckbCobrarSaas.Checked = contratoSaas.CobrarSaas;
                        ckbDadosBancariosObrigatorioGsr.Checked = contratoSaas.DadosBancariosObrigatorioGsr;
                        ckbExibirMei.Checked = contratoSaas.MeiObrigatorioGsr;
                        ckbTransferirParaEntregadorRecebimentoBoleto.Checked = contratoSaas.TransferirParaEntregadorRecebimentoBoleto;

                        txtEmailEnvio.Text = contratoSaas.EmailEnvio;
                        txtEmailCobranca.Text = contratoSaas.EmailsFinanceiros;
                        txtCPF.Text = contratoSaas.Cnpj;
                        txtCodigo.Text = contratoSaas.Codigo;
                        txtWebsite.Text = contratoSaas.WebSite;
                        txtUrlNovoProjeto.Text = contratoSaas.urlDoNovoProjeto;
                        ckbColetaEntrega.Checked = contratoSaas.ColetaEEntregaAtivo;
                        ckbBloqueioChegueiGPS.Checked = contratoSaas.HabilitarBloqueioDeChegueiPorLocalizacao;
                        ckbOtlDeAgenda.Checked = contratoSaas.OtlDeAgenda;

                        ltUsuarios.Text = string.Empty;
                        var listaUsuarios = contratoSaas.Administradores.Where(w => !w.EstabelecimentoId.HasValue).ToList();

                        foreach (var usuario in listaUsuarios)
                        {
                            ltUsuarios.Text += $"<a target='_blank' href='https://admin.dn.app.br/login/resetPassAdmin.aspx?Id={usuario.UserId}'>{usuario.Email}</a><br/>";
                        }

                        if (ddlTema.Items.FindByValue(contratoSaas.Tema) != null)
                            ddlTema.Items.FindByValue(contratoSaas.Tema).Selected = true;

                        if (ddlFormaDeCobranca.Items.FindByValue(contratoSaas.FormaDeCobranca) != null)
                            ddlFormaDeCobranca.Items.FindByValue(contratoSaas.FormaDeCobranca).Selected = true;

                        txtValorEntrega.Text = contratoSaas.ValorPorEntrega.ToString("N2");
                        txtValorMinimo.Text = contratoSaas.ValorDeCobrancaMinimo.ToString("N2");

                        txtEmissaoBoleto.Text = contratoSaas.TaxaEmissaoBoleto.ToString("N2");
                        txtTransferencia.Text = contratoSaas.TaxaTransferenciaGsr.ToString("N2");

                        txtSenhaPadrao.Text = contratoSaas.SenhaPadrao;
                        txtSenhaPadraoBD.Text = contratoSaas.SenhaPadraBD;

                        if (contratoSaas.Endereco != null)
                            objEndereco.EnderecoObj = contratoSaas.Endereco;

                        if (contratoSaas.DadosBancario != null)
                        {
                            txtCC.Text = contratoSaas.DadosBancario.Conta;
                            txtAgencia.Text = contratoSaas.DadosBancario.Agencia;

                            if (ddlBanco.Items.FindByValue(contratoSaas.DadosBancario.CodigoBanco) != null)
                                ddlBanco.Items.FindByValue(contratoSaas.DadosBancario.CodigoBanco).Selected = true;

                            if (ddlTipoConta.Items.FindByValue(contratoSaas.DadosBancario.TipoConta.ToString()) != null)
                                ddlTipoConta.Items.FindByValue(contratoSaas.DadosBancario.TipoConta.ToString()).Selected = true;
                        }

                        ltDataCadastro.Text = contratoSaas.DataCadastro.ToString("dd/MM/yyyy");

                        if (contratoSaas.DataFim.HasValue)
                            txtDataSaida.Text = contratoSaas.DataFim.Value.ToString("dd/MM/yyyy");

                        if (contratoSaas.DataInicio.HasValue)
                            txtDataDeInicioOperacao.Text = contratoSaas.DataInicio.Value.ToString("yyyy-MM-dd");

                        if (contratoSaas.DataDeAtivacaoComercial.HasValue)
                            txtDataDeAtivacaoComercial.Text = contratoSaas.DataDeAtivacaoComercial.Value.ToString("yyyy-MM-dd");

                        if (contratoSaas.Arquivo != null)
                        {
                            ckbExcluirFoto.Visible = true;
                            imgFoto.Visible = true;
                            imgFoto.ImageUrl = "/Handlers/ExibirImagem.ashx?h=200&w=200&Id=" + contratoSaas.LogoId;
                        }

                        txtHorario1.Text = "03:00";
                        txtHorario2.Text = "11:00";
                        txtHorario3.Text = "18:00";

                        if (contratoSaas.PeriodoTrabalhoes.Any())
                        {
                            if (contratoSaas.PeriodoTrabalhoes.Any(a => a.PeriodoFixoId == (int)Periodo.Manha))
                            {
                                var p = contratoSaas.PeriodoTrabalhoes.First(a => a.PeriodoFixoId == (int)Periodo.Manha);
                                txtHorario1.Text = p.HoraInicio.ToString(@"hh\:mm");
                            }
                            if (contratoSaas.PeriodoTrabalhoes.Any(a => a.PeriodoFixoId == (int)Periodo.Tarde))
                            {
                                var p = contratoSaas.PeriodoTrabalhoes.First(a => a.PeriodoFixoId == (int)Periodo.Tarde);
                                txtHorario2.Text = p.HoraInicio.ToString(@"hh\:mm");
                            }
                            if (contratoSaas.PeriodoTrabalhoes.Any(a => a.PeriodoFixoId == (int)Periodo.Noite))
                            {
                                var p = contratoSaas.PeriodoTrabalhoes.First(a => a.PeriodoFixoId == (int)Periodo.Noite);
                                txtHorario3.Text = p.HoraInicio.ToString(@"hh\:mm");
                            }
                        }

                    }

                    else
                    {
                        txtHorario1.Text = "03:00";
                        txtHorario2.Text = "11:00";
                        txtHorario3.Text = "18:00";
                    }
                }
            }
        }

        protected async void BtnUpdateClick(object sender, EventArgs e)
        {
            var azure = new AzureStorage();

            using (var db = new Go4YouEntities())
            {
                var contratoSaas = IsEditing
                    ? db.ContratoSaas.Single(p => p.Id == Id)
                    : new ContratoSaa();

                var emailNovo = txtEmail.Text.ToLower().Trim();
                var emailAntigo = txtEmail.Text.ToLower().Trim();

                if (IsEditing)
                    emailAntigo = contratoSaas.Email;

                var alterouEmail = emailNovo != emailAntigo;

                if (alterouEmail)
                {
                    var emailJaExiste = Task.Run(() => shield.EmailJaExiste(emailNovo)).Result;

                    if (emailJaExiste)
                    {
                        AppCode.Comum.Mensagem("Erro", "Este email já foi cadastrado! Por favor utilize outro.", AppCode.Comum.TipoMensagem.Erro);
                        return;
                    }
                }

                if (!IsEditing)
                {
                    contratoSaas.Email = emailNovo;
                    contratoSaas.AppName = "";
                }

                contratoSaas.NomeFantasia = txtNomeFantasia.Text;
                contratoSaas.RazaoSocial = txtRazaoSocial.Text;
                contratoSaas.Telefone = txtTelefone.Text;
                contratoSaas.Cnpj = txtCPF.Text;
                contratoSaas.Ativo = ckbAtivo.Checked;
                contratoSaas.CredenciamentoObrigatorio = ckbCredenciamentoObrigatorio.Checked;
                contratoSaas.CobrancaAutomatica = ckbCobrancaAutomatica.Checked;
                contratoSaas.EmitirNfGsr = ckbEmitirNfGsr.Checked;
                contratoSaas.EmitirNfParaEstabelecimento = ckbEmitirNfParaEstabelecimento.Checked;
                contratoSaas.FaturamentoAutomatico = ckbFaturamentoAutomatico.Checked;
                contratoSaas.Apelido = txtApelido.Text;
                contratoSaas.WebSite = txtWebsite.Text;
                contratoSaas.urlDoNovoProjeto = txtUrlNovoProjeto.Text;                
                contratoSaas.EmailEnvio = txtEmailEnvio.Text;
                contratoSaas.EmailsFinanceiros = txtEmailCobranca.Text;
                contratoSaas.Codigo = txtCodigo.Text;
                contratoSaas.Tema = ddlTema.SelectedValue;
                contratoSaas.ValorPorEntrega = decimal.Parse(txtValorEntrega.Text);
                contratoSaas.ValorDeCobrancaMinimo = decimal.Parse(txtValorMinimo.Text);
                contratoSaas.CobrarSaas = ckbCobrarSaas.Checked;
                contratoSaas.MeiObrigatorioGsr = ckbExibirMei.Checked;
                contratoSaas.DadosBancariosObrigatorioGsr = ckbDadosBancariosObrigatorioGsr.Checked;

                contratoSaas.HabilitarBloqueioDeChegueiPorLocalizacao = ckbBloqueioChegueiGPS.Checked;
                contratoSaas.OtlDeAgenda = ckbOtlDeAgenda.Checked;


                contratoSaas.SenhaPadraBD = txtSenhaPadraoBD.Text;
                contratoSaas.SenhaPadrao = txtSenhaPadrao.Text;
                contratoSaas.UrlPadraoEmailBD = "dn.com.br";
                contratoSaas.TransferirParaEntregadorRecebimentoBoleto = ckbTransferirParaEntregadorRecebimentoBoleto.Checked;
                contratoSaas.TaxaTransferenciaGsr = decimal.Parse(txtTransferencia.Text);
                contratoSaas.TaxaEmissaoBoleto = decimal.Parse(txtEmissaoBoleto.Text);
                contratoSaas.FormaDeCobranca = ddlFormaDeCobranca.SelectedValue;

                contratoSaas.CloudMessageServerKey = ConfigurationManager.AppSettings["FirebaseServerKey"];
                contratoSaas.CloudMessageSenderId = ConfigurationManager.AppSettings["FireBaseSenderId"];

                if (string.IsNullOrEmpty(txtDataDeInicioOperacao.Text))
                    contratoSaas.DataInicio = null;
                else
                    contratoSaas.DataInicio = DateTime.Parse(txtDataDeInicioOperacao.Text);

                if (string.IsNullOrEmpty(txtDataDeAtivacaoComercial.Text))
                    contratoSaas.DataDeAtivacaoComercial = null;
                else
                    contratoSaas.DataDeAtivacaoComercial = DateTime.Parse(txtDataDeAtivacaoComercial.Text);


                contratoSaas.CriarConfiguracoesDeColetaEEntregaPadrao();

                if (ddlBanco.SelectedValue != "0")
                {
                    if (contratoSaas.DadosBancario == null)
                        contratoSaas.DadosBancario = new DadosBancario { AtualizarContaGateway = true };

                    string agencia = txtAgencia.Text;
                    string conta = txtCC.Text;
                    ltErroAgencia.Visible = false;
                    ltErroCc.Visible = false;
                    ltErroAgencia.Text = string.Empty;
                    ltErroCc.Text = string.Empty;

                    switch (ddlBanco.SelectedItem.Text)
                    {
                        case "Banco do Brasil":
                            if (agencia.Length != 6 || agencia[4] != '-')
                            {
                                ltErroAgencia.Text = $"Formato da Agência Bancária para {ddlBanco.SelectedItem.Text} é 9999-D<br/>";
                                ltErroAgencia.Visible = true;
                                txtAgencia.Focus();
                                return;
                            }
                            if (conta.Length != 10 || conta[8] != '-')
                            {
                                ltErroCc.Text = $"Formato da Conta Bancária para {ddlBanco.SelectedItem.Text} é 99999999-D<br/>";
                                ltErroCc.Visible = true;
                                txtCC.Focus();
                                return;
                            }
                            break;
                        case "Santander":
                            if (agencia.Length != 4)
                            {
                                ltErroAgencia.Text = $"Formato da Agência Bancária para {ddlBanco.SelectedItem.Text} é 9999<br/>";
                                ltErroAgencia.Visible = true;
                                txtAgencia.Focus();
                                return;
                            }
                            if (conta.Length != 10 || conta[8] != '-')
                            {
                                ltErroCc.Text = $"Formato da Conta Bancária para {ddlBanco.SelectedItem.Text} é 99999999-D<br/>";
                                ltErroCc.Visible = true;
                                txtCC.Focus();
                                return;
                            }
                            break;
                        case "Caixa Econômica":
                            if (agencia.Length != 4)
                            {
                                ltErroAgencia.Text = $"Formato da Agência Bancária para {ddlBanco.SelectedItem.Text} é 9999<br/>";
                                ltErroAgencia.Visible = true;
                                txtAgencia.Focus();
                                return;
                            }
                            if (conta.Length != 13 || conta[11] != '-')
                            {
                                ltErroCc.Text = $"Formato da Conta Bancária para {ddlBanco.SelectedItem.Text} é XXX99999999-D onde XXX é a operação<br/>";
                                ltErroCc.Visible = true;
                                txtCC.Focus();
                                return;
                            }
                            break;
                        case "Itaú":
                            if (agencia.Length != 4)
                            {
                                ltErroAgencia.Text = $"Formato da Agência Bancária para {ddlBanco.SelectedItem.Text} é 9999<br/>";
                                ltErroAgencia.Visible = true;
                                txtAgencia.Focus();
                                return;
                            }
                            if (conta.Length != 7 || conta[5] != '-')
                            {
                                ltErroCc.Text = $"Formato da Conta Bancária para {ddlBanco.SelectedItem.Text} é 99999-D<br/>";
                                ltErroCc.Visible = true;
                                txtCC.Focus();
                                return;
                            }
                            break;
                        case "Sicredi":
                            if (agencia.Length != 4)
                            {
                                ltErroAgencia.Text = $"Formato da Agência Bancária para {ddlBanco.SelectedItem.Text} é 9999<br/>";
                                ltErroAgencia.Visible = true;
                                txtAgencia.Focus();
                                return;
                            }
                            conta = int.Parse(conta.Replace(".", "").Replace("-", "")).ToString("000000");
                            if (conta.Length != 6)
                            {
                                ltErroCc.Text = $"Formato da Conta Bancária para {ddlBanco.SelectedItem.Text} é 999999<br/>";
                                ltErroCc.Visible = true;
                                txtCC.Focus();
                                return;
                            }
                            break;
                        case "Banrisul":
                        case "Sicoob":
                        case "Inter":
                        case "BRB":
                            if (agencia.Length != 4)
                            {
                                ltErroAgencia.Text = $"Formato da Agência Bancária para {ddlBanco.SelectedItem.Text} é 9999<br/>";
                                ltErroAgencia.Visible = true;
                                txtAgencia.Focus();
                                return;
                            }
                            conta = long.Parse(conta.Replace(".", "").Replace("-", "")).ToString("000000000-0");
                            if (conta.Length != 11 || conta[9] != '-')
                            {
                                ltErroCc.Text = $"Formato da Conta Bancária para {ddlBanco.SelectedItem.Text} é 000000000-D<br/>";
                                ltErroCc.Visible = true;
                                txtCC.Focus();
                                return;
                            }
                            break;
                        case "Via Credi":
                            if (agencia.Length != 4)
                            {
                                ltErroAgencia.Text = $"Formato da Agência Bancária para {ddlBanco.SelectedItem.Text} é 9999<br/>";
                                ltErroAgencia.Visible = true;
                                txtAgencia.Focus();
                                return;
                            }
                            conta = long.Parse(conta.Replace(".", "").Replace("-", "")).ToString("00000000000-0");
                            if (conta.Length != 11 || conta[9] != '-')
                            {
                                ltErroCc.Text = $"Formato da Conta Bancária para {ddlBanco.SelectedItem.Text} é 00000000000-D<br/>";
                                ltErroCc.Visible = true;
                                txtCC.Focus();
                                return;
                            }
                            break;
                        case "Safra":
                        case "Banco C6":
                        case "Pagseguro":
                            if (agencia.Length != 4)
                            {
                                ltErroAgencia.Text = $"Formato da Agência Bancária para {ddlBanco.SelectedItem.Text} é 9999<br/>";
                                ltErroAgencia.Visible = true;
                                txtAgencia.Focus();
                                return;
                            }
                            if (conta.Length != 10 || conta[8] != '-')
                            {
                                ltErroCc.Text = $"Formato da Conta Bancária para {ddlBanco.SelectedItem.Text} é 99999999-D<br/>";
                                ltErroCc.Visible = true;
                                txtCC.Focus();
                                return;
                            }
                            break;
                    }

                    contratoSaas.DadosBancario.TipoConta = int.Parse(ddlTipoConta.SelectedValue);
                    contratoSaas.DadosBancario.Agencia = txtAgencia.Text;
                    contratoSaas.DadosBancario.Conta = txtCC.Text;
                    contratoSaas.DadosBancario.CodigoBanco = ddlBanco.SelectedValue;
                    contratoSaas.DadosBancario.Cpf_Cnpj = txtCPF.Text;
                    contratoSaas.DadosBancario.Beneficiario = txtRazaoSocial.Text;
                }
                else
                    contratoSaas.DadosBancario = null;

                if (contratoSaas.Endereco == null)
                    contratoSaas.Endereco = new Endereco();

                contratoSaas.Endereco.Bairro = objEndereco.Bairro;
                contratoSaas.Endereco.Apelido = "Cadastro";
                contratoSaas.Endereco.CEP = objEndereco.CEP;
                contratoSaas.Endereco.Cidade = objEndereco.Cidade;
                contratoSaas.Endereco.Complemento = objEndereco.Complemento;
                contratoSaas.Endereco.Estado = objEndereco.Estado;
                contratoSaas.Endereco.Logradouro = objEndereco.Logradouro;
                contratoSaas.Endereco.Numero = objEndereco.Numero;
                contratoSaas.Endereco.Pais = "BR";

                var googleAPIKey = ConfigurationManager.AppSettings["GoogleAPIKey"];
                var coord = objEndereco.Coordenadas(googleAPIKey);

                contratoSaas.Endereco.Latitude = coord.Latitude.ToString().Replace(",", "."); ;
                contratoSaas.Endereco.Longitude = coord.Longitude.ToString().Replace(",", "."); ;

                int apagarFotoId = 0;
                if (ckbExcluirFoto.Checked)
                {
                    if (!string.IsNullOrEmpty(contratoSaas?.Arquivo.UrlStorage))
                        azure.Excluir(contratoSaas.Arquivo.UrlStorage);

                    if (contratoSaas.LogoId.HasValue)
                    {
                        apagarFotoId = contratoSaas.LogoId.Value;
                        contratoSaas.Arquivo = null;
                    }
                }
                else
                {
                    if (flFoto.HasFile)
                    {
                        if (contratoSaas.Arquivo == null)
                        {
                            contratoSaas.Arquivo = new Arquivo
                            {
                                Data = DateTime.Now,
                                Tipo = flFoto.PostedFile.ContentType,
                                Descricao = "",
                                Nome = flFoto.FileName,
                                UrlStorage = Guid.NewGuid().ToString()
                            };

                            azure.Upload(contratoSaas.Arquivo.UrlStorage, contratoSaas.Arquivo.Nome, flFoto.FileBytes);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(contratoSaas.Arquivo.UrlStorage))
                                azure.Excluir(contratoSaas.Arquivo.UrlStorage);

                            contratoSaas.Arquivo.Arquivo1 = null;
                            contratoSaas.Arquivo.Tipo = flFoto.PostedFile.ContentType;
                            contratoSaas.Arquivo.Nome = flFoto.FileName;
                            contratoSaas.Arquivo.Data = DateTime.Now;
                            contratoSaas.Arquivo.UrlStorage = Guid.NewGuid().ToString();

                            azure.Upload(contratoSaas.Arquivo.UrlStorage, contratoSaas.Arquivo.Nome, flFoto.FileBytes);
                        }
                    }
                }

                var inicio1 = TimeSpan.Parse(txtHorario1.Text);
                var inicio2 = TimeSpan.Parse(txtHorario2.Text);
                var inicio3 = TimeSpan.Parse(txtHorario3.Text);

                var dif1 = (inicio2 - inicio1).TotalMinutes;
                var dif2 = (inicio3 - inicio2).TotalMinutes;
                var tm00 = new TimeSpan(0, 0, 0);
                var tm00Dia = new TimeSpan(1, 0, 0, 0);
                var dif3 = (inicio1 - tm00).TotalMinutes + (tm00Dia - inicio3).TotalMinutes;

                if (contratoSaas.PeriodoTrabalhoes.Any())
                {
                    if (contratoSaas.PeriodoTrabalhoes.Any(a => a.PeriodoFixoId == (int)Periodo.Manha))
                    {
                        var p = contratoSaas.PeriodoTrabalhoes.First(a => a.PeriodoFixoId == (int)Periodo.Manha);
                        p.HoraInicio = inicio1;
                        p.DuracaoMinutos = (int)dif1;
                    }
                    else
                    {
                        contratoSaas.PeriodoTrabalhoes.Add(new PeriodoTrabalho
                        {
                            Nome = "Manhã",
                            HoraInicio = inicio1,
                            DuracaoMinutos = (int)dif1,
                            Ordem = 1,
                            PeriodoFixoId = (int)Periodo.Manha
                        });
                    }

                    if (contratoSaas.PeriodoTrabalhoes.Any(a => a.PeriodoFixoId == (int)Periodo.Tarde))
                    {
                        var p = contratoSaas.PeriodoTrabalhoes.First(a => a.PeriodoFixoId == (int)Periodo.Tarde);
                        p.HoraInicio = inicio2;
                        p.DuracaoMinutos = (int)dif2;
                    }
                    else
                    {
                        contratoSaas.PeriodoTrabalhoes.Add(new PeriodoTrabalho
                        {
                            Nome = "Tarde",
                            HoraInicio = inicio2,
                            DuracaoMinutos = (int)dif2,
                            Ordem = 2,
                            PeriodoFixoId = (int)Periodo.Tarde
                        });
                    }


                    if (contratoSaas.PeriodoTrabalhoes.Any(a => a.PeriodoFixoId == (int)Periodo.Noite))
                    {
                        var p = contratoSaas.PeriodoTrabalhoes.First(a => a.PeriodoFixoId == (int)Periodo.Noite);
                        p.HoraInicio = inicio3;
                        p.DuracaoMinutos = (int)dif3;
                    }
                    else
                    {
                        contratoSaas.PeriodoTrabalhoes.Add(new PeriodoTrabalho
                        {
                            Nome = "Noite",
                            HoraInicio = inicio3,
                            DuracaoMinutos = (int)dif3,
                            Ordem = 3,
                            PeriodoFixoId = (int)Periodo.Noite
                        });
                    }

                }
                else
                {
                    contratoSaas.PeriodoTrabalhoes.Add(new PeriodoTrabalho
                    {
                        Nome = "Manhã",
                        HoraInicio = inicio1,
                        DuracaoMinutos = (int)dif1,
                        Ordem = 1,
                        PeriodoFixoId = (int)Periodo.Manha
                    });

                    contratoSaas.PeriodoTrabalhoes.Add(new PeriodoTrabalho
                    {
                        Nome = "Tarde",
                        HoraInicio = inicio2,
                        DuracaoMinutos = (int)dif2,
                        Ordem = 2,
                        PeriodoFixoId = (int)Periodo.Tarde
                    });

                    contratoSaas.PeriodoTrabalhoes.Add(new PeriodoTrabalho
                    {
                        Nome = "Noite",
                        HoraInicio = inicio3,
                        DuracaoMinutos = (int)dif3,
                        Ordem = 3,
                        PeriodoFixoId = (int)Periodo.Noite
                    });
                }

                Dominio.Negocio.Comum.CriarDadosPadraoSaas(contratoSaas, db);

                if (!IsEditing)
                {
                    contratoSaas.DataCadastro = DateTime.Now;
                    db.ContratoSaas.Add(contratoSaas);
                }

                db.SaveChanges();


                AppCode.Comum.Mensagem("DN", "Operação efetuada com sucesso!");

                await CriarUsuarioDeBackDoor(contratoSaas, db);
                await CriarUsuarioDoOTl(contratoSaas, db);
                await AtualizarEmailDoUsuario(contratoSaas, emailNovo, db);
#if !DEBUG
                await CriarSubContaIugu(contratoSaas, db);
                await CriarClienteIugu(contratoSaas, db);
#endif
                HabilitarServicoDeColetaEEntrega(contratoSaas);


                if (apagarFotoId > 0)
                {
                    db.Arquivos.Remove(db.Arquivos.Single(x => x.Id == apagarFotoId));
                    SalvarDados(db);
                }

                string urlTo;
                switch (((Button)sender).ID)
                {
                    case "btnSalvarNovo":
                        urlTo = "AddEdit.aspx";
                        break;
                    case "btnSalvarEditar":
                        urlTo = "AddEdit.aspx?Id=" + contratoSaas.Id;
                        break;
                    default:
                        urlTo = "Grid.aspx";
                        break;
                }

                Response.Redirect(urlTo, false);
            }
        }

        private async Task CriarUsuarioDeBackDoor(ContratoSaa contratoSaas, Go4YouEntities db)
        {
            if (IsEditing)
                return;

            var emailSuperAdmin = $"{LimparTexto(contratoSaas.Apelido)}@backdoor.dn".ToLower();

            var adm = new Administradore
            {
                Email = emailSuperAdmin,
                SU = true,
                UserId = Guid.NewGuid()
            };

            contratoSaas.Administradores.Add(adm);
            db.SaveChanges();

            var senhaSuperAdmin = contratoSaas.SenhaPadraBD;

            await CriarUsuarioNoShield(emailSuperAdmin, senhaSuperAdmin, contratoSaas.Id);
        }
        public static string LimparTexto(string dirtyString)
        {
            HashSet<char> removeChars = new HashSet<char>(" ?&^$#@!()+-,:;<>’\'-_*");
            StringBuilder result = new StringBuilder(dirtyString.Length);

            foreach (char c in dirtyString)
                if (!removeChars.Contains(c)) // prevent dirty chars
                    result.Append(c);

            return Dominio.Negocio.Comum.RemoveAccents(result.ToString());
        }

        private async Task CriarUsuarioDoOTl(ContratoSaa contratoSaas, Go4YouEntities db)
        {
            if (IsEditing)
                return;

            var adm = new Administradore
            {
                Email = contratoSaas.Email,
                UserId = Guid.NewGuid()
            };

            contratoSaas.Administradores.Add(adm);
            db.SaveChanges();

            await CriarUsuarioNoShield(contratoSaas.Email, contratoSaas.SenhaPadrao, contratoSaas.Id);
        }
     
        private async Task CriarUsuarioNoShield(string email, string senha, int saasId) 
        { 
            var dto = new Dominio.Servicos.Shield.CriarUsuarioDTO
            {
                Email = email,
                Senha = senha,
                ContratoSaasId = saasId,
                Perfil = Dominio.Servicos.Shield.Perfis.OperadorLogistico
            };

            var response = await shield.CriarUsuario(dto);

            if (!response.DeuCerto)
            {
                DeathLog.LogarInformacao("Saas", "Criar Saas - " + response.Mensagem);
                AppCode.Comum.Mensagem("", $"Não foi possível criar usuário para o email {email}!");
            }
        }

        private async Task AtualizarEmailDoUsuario(ContratoSaa saas, string emailNovo, Go4YouEntities db)
        {
            var emailAntigo = saas.Email.ToLower().Trim();

            var alterouEmail = (emailNovo != emailAntigo);

            if (!alterouEmail)
                return;

            var adm = saas.Administradores.SingleOrDefault(s => s.Email == saas.Email);

            adm.Email = emailNovo;
            db.Entry(adm).State = EntityState.Modified;

            db.SaveChanges();

            var response = await shield.AlterarEmail(emailAntigo, emailNovo);

            if (!response.DeuCerto)
            {
                DeathLog.LogarInformacao("Saas", "Editar Saas - Atualizar Email: " + response.Mensagem);
                AppCode.Comum.Mensagem("", "Não ofi possível atualizar o email!");
            }
        }

        protected void BtnCancelClick(object sender, EventArgs e)
        {
            Response.Redirect("Grid.aspx");
        }

        public void HabilitarServicoDeColetaEEntrega(ContratoSaa otl)
        {
            var podeHabilitar = !string.IsNullOrEmpty(otl.IuguAccountId) && !string.IsNullOrEmpty(otl.IuguClienteId);
            
            if (podeHabilitar)
                otl.ColetaEEntregaAtivo = ckbColetaEntrega.Checked;
            else
                otl.ColetaEEntregaAtivo = false;
        }

        public async Task CriarSubContaIugu(ContratoSaa otl, Go4YouEntities db)
        {
            var apiIugu = new APIIuguMarketplace();

            if (string.IsNullOrEmpty(otl.IuguAccountId))
            {
                var tokens = await apiIugu.CriarSubconta(otl.NomeFantasia);
                if (tokens != null)
                {
                    otl.AdicionarTokensIugu(tokens);
                }
            }

            if (!otl.IuguContaVerificada && otl.DadosBancario != null)
            {
                var subcontaVerificada = await apiIugu.EnviarVerificacao(otl, ddlBanco.SelectedItem.Text, ddlTipoConta.SelectedItem.Text);
                if (subcontaVerificada != null)
                {
                    otl.VerificarContaIugu();
                }
            }

            db.SaveChanges();
        }

        public async Task CriarClienteIugu(ContratoSaa otl, Go4YouEntities db)
        {
            var apiTokenDN = "a26e39704e252b38bb602740ca066657";
            var apiIugu = new APIIuguCliente(apiTokenDN);

            if (string.IsNullOrEmpty(otl.IuguClienteId))
            {
                try
                {
                    var clienteIugu = await apiIugu.CriarCliente(otl.RazaoSocial, otl.Email);
                    otl.AdicionarClienteIuguId(clienteIugu.Id);
                }
                catch (Exception ex)
                {
                    DeathLog.LogarException("Saas CriarClienteIugu", ex, new { otlId = otl.Id });
                }
            }

            db.SaveChanges();
        }
    }
}