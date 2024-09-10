using System;
using ComuniqueSe.Portais.Paginas.Helpers;
using ComuniqueSe.Portais.Paginas.Helpers.Interfaces;
using ComuniqueSe.Portais.Paginas.Master;


namespace REAG.Master
{
    public partial class Internal : InternalMasterPage
    {
        private readonly IConteudoHelper _conteudoHelper = new ConteudoHelper();
        private readonly ICanalHelper _canalHelper = new CanalHelper();

        protected void Page_Load(object sender, EventArgs e)
        {

            string img;
            if (CanalParametro > 0)
            {
                img = _canalHelper.GetImagemCanal(CanalParametro);
                if (!string.IsNullOrEmpty(img))
                    idEscpaanimacao.Style.Add("background-image", "url(img/" + img + ")");
                else
                    idEscpaanimacao.Style.Add("background-image", "url(img/bg-breadcrumb.jpg)");
            }
            else if (ConteudoParametro > 0)
            {
                img = _conteudoHelper.GetImagemCanal(ConteudoParametro);
                if (!string.IsNullOrEmpty(img))
                    idEscpaanimacao.Style.Add("background-image", "url(img/" + img + ")");
                else
                    idEscpaanimacao.Style.Add("background-image", "url(img/bg-breadcrumb.jpg)");
            }
            else
            {
                idEscpaanimacao.Style.Add("background-image", "url(img/bg-breadcrumb.jpg)");
            }

        }

        protected long CanalParametro
        {
            get
            {
                var idCanal = Request.QueryString["idCanal"];
                return string.IsNullOrEmpty(idCanal) ? 0 : ConfigHelper.Descriptografar(idCanal);
            }
        }

        protected long ConteudoParametro
        {
            get
            {
                var idConteudo = Request.QueryString["idConteudo"];
                idConteudo = idConteudo ?? Request.QueryString["idMateria"];
                idConteudo = idConteudo ?? Request.QueryString["IdResultado"];
                idConteudo = idConteudo ?? Request.QueryString["Arquivo"];

                return string.IsNullOrEmpty(idConteudo) ? 0 : ConfigHelper.Descriptografar(idConteudo);
            }
        }

        protected long TeleconferenciaParametro
        {
            get
            {
                var idConteudo = Request.QueryString["IdTeleconferencia"];

                return string.IsNullOrEmpty(idConteudo) ? 0 : ConfigHelper.Descriptografar(idConteudo);
            }
        }
    }
}
