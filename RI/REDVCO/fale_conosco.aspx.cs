using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI.WebControls;
using ComuniqueSe.Portais.Paginas;
using ComuniqueSe.Portais.Paginas.Base;
using ComuniqueSe.Portais.Paginas.Helpers;
using ComuniqueSeWorkflow.Container;
using ComuniqueSeWorkflow.Dominio.Entidades;
using ComuniqueSeWorkflow.Dominio.Entidades.CRM;
using ComuniqueSeWorkflow.Dominio.Exceptions;
using ComuniqueSeWorkflow.Dominio.IRepositorios;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web;

namespace REAG
{
    public partial class FaleConosco : BasePage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {


            if (SolicitacaoAlterarContato())
            {
                CriarSolicitacao();
                return;
            }
            MontaScriptCallServer();

            if (!IsPostBack)
            {

                var publicKey = ConfigurationManager.AppSettings["ReCaptcha_Key"];
                divRecaptcha.Attributes.Add("data-sitekey", publicKey);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
            }

        }

        protected string TemplateFaleConosco = "Nome: {0}<br>Empresa: {1}<br>Telefone: {2}<br>Mensagem: {3}";
        protected readonly bool _salvarContato = true;

        private ConfigHelper _configHelper;

        private ConfigHelper ConfigHelper
        {
            get
            {
                return _configHelper ?? (_configHelper = new ConfigHelper());
            }
        }

        protected virtual TextBox TxtEmailTrocaDadosCadastrados { get { return new TextBox(); } }

        protected virtual Button MudarDadosCadastrados
        {
            get
            {
                Button btn = new Button();
                btn.Click += btTrocarDadosCadastrados_Click;
                return btn;
            }
        }

        private void CriarSolicitacao()
        {

            var htmlAlteraDadosContato = GetLocalResourceObject("htmlAlteraDadosContato") != null ? ConfigHelper.HttpContextGetLocalResourceObjectAsString("htmlAlteraDadosContato", "~/default.aspx") : ConfigHelper.HttpContextGetLocalResourceObjectAsString("htmlAlteraDadosContato", "~/Master/Master.master");
            if (!(String.IsNullOrEmpty(htmlAlteraDadosContato)))
            {
                new AlteradorDeContatos().SolicitarAlteracaoDados(Request.Form["email"], Thread.CurrentThread.CurrentCulture.Name, true, htmlAlteraDadosContato);
            }
            else
            {
                new AlteradorDeContatos().SolicitarAlteracaoDados(Request.Form["email"], Thread.CurrentThread.CurrentCulture.Name, false, "");
            }
           
        }

        private bool SolicitacaoAlterarContato()
        {
            return !string.IsNullOrEmpty(Request.QueryString["mail"]);
        }

        protected FaleConosco() { }

        protected FaleConosco(bool salvarContato)
        {
            _salvarContato = salvarContato;
        }

        protected void btTrocarDadosCadastrados_Click(object sender, EventArgs e)
        {
            try
            {

                var htmlAlteraDadosContato = GetLocalResourceObject("htmlAlteraDadosContato") != null ? ConfigHelper.HttpContextGetLocalResourceObjectAsString("htmlAlteraDadosContato", "~/default.aspx") : ConfigHelper.HttpContextGetLocalResourceObjectAsString("htmlAlteraDadosContato", "~/Master/Master.master");
                if (!(String.IsNullOrEmpty(htmlAlteraDadosContato)))
                {
                    new AlteradorDeContatos().SolicitarAlteracaoDados(TxtEmailTrocaDadosCadastrados.Text, Thread.CurrentThread.CurrentCulture.Name, true, htmlAlteraDadosContato);
                }
                else
                {
                    new AlteradorDeContatos().SolicitarAlteracaoDados(TxtEmailTrocaDadosCadastrados.Text, Thread.CurrentThread.CurrentCulture.Name, false, "");
                }

                //new AlteradorDeContatos().SolicitarAlteracaoDados(TxtEmailTrocaDadosCadastrados.Text, Thread.CurrentThread.CurrentCulture.Name, false, "");

                string msgEnviada;

                string cultura = Thread.CurrentThread.CurrentCulture.Name;
                switch (cultura)
                {
                    case "en-US":
                        msgEnviada = GetLocalResourceObject("MsgEnviadaFaleConosco") != null ? GetLocalResourceObject("MsgEnviadaFaleConosco").ToString() : "Over the next few moments, you will receive an email with instructions on how to update your information.";
                        break;
                    case "es":
                        msgEnviada = GetLocalResourceObject("MsgEnviadaFaleConosco") != null ? GetLocalResourceObject("MsgEnviadaFaleConosco").ToString() : "En los próximos momentos, usted recibirá un correo electrónico con instrucciones sobre cómo actualizar su información.";
                        break;
                    default:
                        msgEnviada = GetLocalResourceObject("MsgEnviadaFaleConosco") != null ? GetLocalResourceObject("MsgEnviadaFaleConosco").ToString() : "Nos próximos instantes, você receberá um e-mail com instruções sobre como proceder para atualizar suas informações.";
                        break;
                }

                ClientScript.RegisterClientScriptBlock(GetType(), "msgEnvio", "alert('" + msgEnviada + "');", true);
            }
            catch (LoginException.UsuarioInvalido)
            {
                Response.Write("Usuario Inválido");
            }
        }

        public bool ValidarCaptcha()
        {
            //caso a div não esteja visivel, podemos retornar como valido
            if (!divRecaptcha.Visible)
            {
                return true;
            }

            string Response = Request["g-recaptcha-response"];//Getting Response String Append to Post Method
            bool Valid = false;
            //Request to Google Server
            string chavePrivada = ConfigurationManager.AppSettings["ReCaptcha_Secret"];
            
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
           ("https://www.google.com/recaptcha/api/siteverify?secret=" + chavePrivada + "&response=" + Response);
            try
            {
                //Google recaptcha Response
                using (WebResponse wResponse = req.GetResponse())
                {

                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();
                        MyObject data = js.Deserialize<MyObject>(jsonResponse);// Deserialize Json

                        Valid = Convert.ToBoolean(data.success);
                    }
                }

                return Valid;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        protected void btEnviar_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            if (!ValidarCaptcha())
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = GetLocalResourceObject("MsgCaptcha").ToString();
                return;
            }

            var servEmail = new ComuniqueSeWorkflow.Infraestrutura.ServicoDeEnvioDeEmail();
            var msgEmail = (Session["MyCulture"].ToString() == "pt-BR") ? "Email Inválido" : "Invalid Email";

            const string regexEmail = @"\S+@\S+.\S{2,3}";
            var oRegexEmail = new Regex(regexEmail);

            var assuntoFormulario = ConfigurationManager.AppSettings["AssuntoFaleRI"];

            string emailDestinatario =  ConfigurationManager.AppSettings["EmailFaleRI"];

            var emailRemetente = ConfigurationManager.AppSettings["EmailRemetente"];

            var templateFaleConosco = GetLocalResourceObject("TemplateFaleConosco").ToString();


            

            if (!oRegexEmail.IsMatch(txtEmail.Text)) {
                ClientScript.RegisterClientScriptBlock(GetType(), "msgValidaEmail", string.Format("alert('{0}');", msgEmail), true);
            }               

            else
            {
                 
                if (emailDestinatario != null && emailRemetente != null)
                {

                   
                    servEmail.EnviarNovoEmail(emailDestinatario, emailRemetente, new MailAddress(txtEmail.Text), assuntoFormulario
                                            , string.Format(templateFaleConosco
                                                                , txtNomeContato.Text
                                                                , txtTelefone.Text
                                                                , txtMensagem.Text
                                                                , txtEmail.Text
                                                                , txtEmpresa.Text
                                                            ));
                  
                   
                 
                    ClientScript.RegisterClientScriptBlock(GetType(), "msgEnvio", string.Format("alert('{0}');", GetLocalResourceObject("MsgEnviada")), true);
                    SalvarContato();
                   
                    LimparCampos();
                }
                
                else
                {
                    const string msgEmailNulo = "Email Vazio";
                    ClientScript.RegisterClientScriptBlock(GetType(), "msgEmailNulo", string.Format("alert('{0}');", msgEmailNulo), true);
                }
            }

           
        }

        private bool ContatoExiste()
        {
            var conta = new ConfigHelper().ObterContaDefault();
            var contato = ContainerHelper.Resolve<IRepositorioContato>().ObterContatoPorEmail(conta, txtEmail.Text.Trim());

            return contato != null;
        }

        private void MostrarAlertContatoExistente(string email)
        {
            string msgEmailExistente;
            string msgEnviada;

            string cultura = Thread.CurrentThread.CurrentCulture.Name;
            switch (cultura)
            {
                case "en-US":
                    //Alteração feita para o pedido da Cielo de uma mensagem personalizada.
                    msgEmailExistente = GetLocalResourceObject("MsgEmailExistente") != null ? GetLocalResourceObject("MsgEmailExistente").ToString() : "There is already a contact with this e-mail address registered in our system, you want to change your data?";
                    msgEnviada = GetLocalResourceObject("MsgEnviadaFaleConosco") != null ? GetLocalResourceObject("MsgEnviadaFaleConosco").ToString() : "Over the next few moments, you will receive an email with instructions on how to update your information.";
                    break;

                case "es":
                    msgEmailExistente = GetLocalResourceObject("MsgEmailExistente") != null ? GetLocalResourceObject("MsgEmailExistente").ToString() : "Ya existe un contacto con esta dirección de email registrada en nuestro sistema, que desea cambiar sus datos?";
                    msgEnviada = GetLocalResourceObject("MsgEnviadaFaleConosco") != null ? GetLocalResourceObject("MsgEnviadaFaleConosco").ToString() : "En los próximos momentos, usted recibirá un correo electrónico con instrucciones sobre cómo actualizar su información.";
                    break;

                default:
                    //Alteração feita para o pedido da Cielo de uma mensagem personalizada.
                    msgEmailExistente = GetLocalResourceObject("MsgEmailExistente") != null ? GetLocalResourceObject("MsgEmailExistente").ToString() : "Mensagem enviada com sucesso! Verificamos que já existe um contato com este e-mail cadastrado em nosso sistema, deseja alterar seus dados?";
                    msgEnviada = GetLocalResourceObject("MsgEnviadaFaleConosco") != null ? GetLocalResourceObject("MsgEnviadaFaleConosco").ToString() : "Nos próximos instantes, você receberá um e-mail com instruções sobre como proceder para atualizar suas informações.";
                    break;
            }

            ClientScript.RegisterClientScriptBlock(GetType(), "msgEnvio",
                "if (confirm('" + msgEmailExistente + "')) { $.post('fale_conosco.aspx?alterarCadastro=1&mail=" + email + "'); alert('" + msgEnviada + "') }", true);
        }

        private void LimparCampos()
        {
            txtNomeContato.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            txtMensagem.Text = string.Empty;
            txtEmail.Text = string.Empty;
            lblMessage.Text = string.Empty;
            txtEmpresa.Text = string.Empty;

        }

        private void SalvarContato()
        {
            var repositorioContato = ContainerHelper.Resolve<IRepositorioContato>();
            var repositorioEmail = ContainerHelper.Resolve<IRepositorioEnderecoEletronico>();
            var repositorioEmpresa = ContainerHelper.Resolve<IRepositorioEmpresa>();
            var repositorioAtividade = ContainerHelper.Resolve<IRepositorioAtividade>();

            var contato = repositorioContato.ObterContatoPorEmail(ConfigHelper.ObterContaDefault(), txtEmail.Text.Trim());
            var email = repositorioEmail.Retorna(txtEmail.Text.Trim());

            if (email == null)
            {
                email = new EnderecoEletronico { Endereco = txtEmail.Text.Trim() };
                repositorioEmail.Salva(email);
            }

            if (contato == null)
            {

                var empresa = repositorioEmpresa.RetornaPorNomeExato(ConfigHelper.ObterContaDefault(), txtEmpresa.Text.Trim());
                if (empresa == null)
                {
                    if (!string.IsNullOrEmpty(txtEmpresa.Text.Trim()))
                    {
                        empresa = new Empresa { Nome = txtEmpresa.Text.Trim() };
                    }
                }

                contato = new Contato();

                contato.Origem = Origem.Site;
                contato.Conta = ConfigHelper.ObterContaDefault();
                contato.Empresa = empresa;
                contato.Nome = txtNomeContato.Text.Trim();

               
                var emailContato = new Email<Contato>
                {
                    Owner = contato,
                    EnderecoEletronico = email,
                    Principal = true,
                    Tipo = EmailTipo.Outro
                };

                contato.Emails = new List<Email<Contato>> { emailContato };

                contato.Atividades = new List<Atividade>();
            }

            var telefoneContato = new Telefone<Contato>();

            telefoneContato.Owner = contato;
            telefoneContato.NumeroTelefone = txtTelefone.Text.Trim();
            telefoneContato.Principal = true;

            contato.TelefonesDeContato.Add(telefoneContato);
            var atividade = new Atividade
            {
                Conta = ConfigHelper.ObterContaDefault(),
                Descricao = txtMensagem.Text,
                Assunto = ConfigurationManager.AppSettings["AssuntoFaleRI"],
                TipoAtividade =
                    repositorioAtividade.RetornaTipoAtividade(
                        (int)TipoAtividadeTraducao.faleConosco),
                Publico = true,
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddMinutes(1)
            };

            contato.Atividades.Add(atividade);

            repositorioContato.Salva(contato);
        }

    }

    public class MyObject
    {
        public string success { get; set; }
    }
}