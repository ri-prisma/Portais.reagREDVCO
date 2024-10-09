using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ComuniqueSe.Portais.Paginas.Base;
using ComuniqueSe.Portais.Paginas.Helpers;
using ComuniqueSe.Portais.Paginas.Structs;
using ComuniqueSeWorkflow.Container;
using ComuniqueSeWorkflow.Dominio.Entidades;
using ComuniqueSeWorkflow.Dominio.IRepositorios;
using ComuniqueSeWorkflow.Infraestrutura;
using System.Configuration;
using System.Net;
using ComuniqueSeWorkflow.Dominio.Servicos;


namespace REAG
{
    public partial class ListResultados : BasePage
    {
        private CanalHelper _canalHelper;
        private CanalHelper CanalHelper
        {
            get
            {
                return _canalHelper ?? (_canalHelper = new CanalHelper());
            }
        }

        private ConfigHelper _configHelper;
        private ConfigHelper ConfigHelper
        {
            get
            {
                return _configHelper ?? (_configHelper = new ConfigHelper());
            }
        }

        private ConteudoHelper _conteudoHelper;
        private ConteudoHelper ConteudoHelper
        {
            get
            {
                return _conteudoHelper ?? (_conteudoHelper = new ConteudoHelper());
            }
        }

        private IRepositorioResultado _repositorioResultado;
        private IRepositorioCanal _repositorioCanal;

        protected Repeater RptResultados
        {
            get { return rptResultados; }
        }

        private HtmlControl linkArq_Release;
        private Literal LtrArq_Release;

        public bool PrintExtensaoArquivo = true;
        public bool TextoConteudo = false;
        private Conta conta;
        private Portal portal;

        protected virtual void Page_Load(object sender, EventArgs e)
        {



            _repositorioResultado = ContainerHelper.Resolve<IRepositorioResultado>();
            _repositorioCanal = ContainerHelper.Resolve<IRepositorioCanal>();
            conta = ConfigHelper.ObterContaDefault();
            portal = ConfigHelper.ObterPortal();

            IncluiJavaScripts();
            MontaScriptCallServer();
            CarregarResultadosTrimestrais();
        }

        protected virtual void IncluiJavaScripts()
        {
            ((BasePage)Page).AddJScript("js/MenuTopo.js");
            ((BasePage)Page).AddJScript("js/ListResultados.js");
        }

        private void CarregarResultadosTrimestrais()
        {
            var result = ObterResultadosAgrupadosPorAno();
            var anos = ConteudoHelper.ObterAnosDasEntidades(result);
            ConfigurarMenuTopo(anos);

            RptResultados.DataSource = result;
            RptResultados.DataBind();
        }

        private IEnumerable<IGrouping<int, Resultado>> ObterResultadosAgrupadosPorAno()
        {
            var resultadosAgrupadosPorAno = _repositorioResultado.ObterResultadosPorContaPortalPublicados(conta, portal, 0, 100).GroupBy(p => p.Ano);
            return resultadosAgrupadosPorAno;
        }

        private void AjustaLinksArquivos1(string periodo, string tipo, RepeaterItemEventArgs e, Arquivo arq)
        {
            linkArq_Release = (HtmlControl)e.Item.FindControl("linkArq_" + tipo + periodo);
            LtrArq_Release = (Literal)e.Item.FindControl("LtrArq_" + tipo + periodo);
            linkArq_Release.Attributes.Add("href", MontarEnderecoDeDownload(arq));
            if (arq.Idiomas.Contains(IdiomasDeArquivo.Portugues) && arq.Idiomas.Contains(IdiomasDeArquivo.Ingles))
                if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                    LtrArq_Release.Text = arq.Extensao.ToUpper() + @" *";
                else
                    LtrArq_Release.Text = arq.Extensao.ToUpper();
            else
                LtrArq_Release.Text = arq.Extensao.ToUpper();
        }

        private void AjustaLinksArquivos(List<Arquivo> arquivos, string periodo, RepeaterItemEventArgs e)
        {
            foreach (var arquivoResultado in arquivos)
            {
                switch (Thread.CurrentThread.CurrentUICulture.Name)
                {
                    case "pt-BR":
                      
                        if (arquivoResultado.Descricao.ToLower().IndexOf("demonstrações") >= 0)
                            AjustaLinksArquivos1(periodo, "Demonstracoes", e, arquivoResultado);

                        if (arquivoResultado.Descricao.ToLower().IndexOf("dfp") >= 0)
                            AjustaLinksArquivos1(periodo, "DFP", e, arquivoResultado);
              
                        if (arquivoResultado.Descricao.ToLower().IndexOf("vídeo") >= 0 || arquivoResultado.Descricao.ToLower().IndexOf("video ") >= 0)
                            AjustaLinksArquivos1(periodo, "Video", e, arquivoResultado);

                        if (arquivoResultado.Descricao.ToLower().IndexOf("apresentação") >= 0)
                            AjustaLinksArquivos1(periodo, "Apresentacao", e, arquivoResultado);
                        break;
                    case "en-US":
                       
                        if (arquivoResultado.Descricao.ToLower().IndexOf("statements ") >= 0 || arquivoResultado.Descricao.ToLower().IndexOf("financial ") >= 0)
                            AjustaLinksArquivos1(periodo, "Demonstracoes", e, arquivoResultado);

                        if (arquivoResultado.Descricao.ToLower().IndexOf("dfp") >= 0)
                            AjustaLinksArquivos1(periodo, "DFP", e, arquivoResultado);
                       
                        if (arquivoResultado.Descricao.ToLower().IndexOf("video ") >= 0)
                            AjustaLinksArquivos1(periodo, "Video", e, arquivoResultado);

                        if (arquivoResultado.Descricao.ToLower().IndexOf("presentation") >= 0)
                            AjustaLinksArquivos1(periodo, "Apresentacao", e, arquivoResultado);

                        break;
                }
            }
        }

        protected void RptResultadosItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var tituloDivulgacao = "";

            var item = (IGrouping<int, Resultado>)e.Item.DataItem;
            var criptografia = new Criptografia();

            var divResultados = (HtmlControl)e.Item.FindControl("divResultados");
            divResultados.Attributes.Add("ano", item.Key.ToString());

            var labelAnoDivulgacao = (Literal)e.Item.FindControl("LtrAnoDivulgacao");
            var linkResultado = (HtmlAnchor)e.Item.FindControl("HlkResultado");

            var primeiroTrimestre = (Literal)e.Item.FindControl("LtrPrimeiroTri");
            var subtituloPrimeiro = (HtmlControl)e.Item.FindControl("HdnPrimeiroTri");
            var linkPrimeiroTrimestre = (HtmlControl)e.Item.FindControl("linkPrimeiroTri");
            var linkVideoPrimeiroTrimestre = (HtmlControl)e.Item.FindControl("linkVideoPrimeiroTri");
            var tituloVideoPrimeiroTrimestre = (Literal)e.Item.FindControl("tituloVideoPrimeiroTri");

            var segundoTrimestre = (Literal)e.Item.FindControl("LtrSegundoTri");
            var subtituloSegundo = (HtmlControl)e.Item.FindControl("HdnSegundoTri");
            var linkSegundoTrimestre = (HtmlControl)e.Item.FindControl("linkSegundoTri");
            var linkVideoSegundoTrimestre = (HtmlControl)e.Item.FindControl("linkVideoSegundoTri");
            var tituloVideoSegundoTrimestre = (Literal)e.Item.FindControl("tituloVideoSegundoTri");

            var terceiroTrimestre = (Literal)e.Item.FindControl("LtrTerceiroTri");
            var subtituloTerceiro = (HtmlControl)e.Item.FindControl("hdnTerceiroTri");
            var linkTerceiroTrimestre = (HtmlControl)e.Item.FindControl("linkTerceiroTri");
            var linkVideoTerceiroTrimestre = (HtmlControl)e.Item.FindControl("linkVideoTerceiroTri");
            var tituloVideoTerceiroTrimestre = (Literal)e.Item.FindControl("tituloVideoTerceiroTri");

            var quartoTrimestre = (Literal)e.Item.FindControl("LtrQuartoTri");
            var subtituloQuarto = (HtmlControl)e.Item.FindControl("HdnQuartoTri");
            var linkQuartoTrimestre = (HtmlControl)e.Item.FindControl("linkQuartoTri");
            var linkVideoQuartoTrimestre = (HtmlControl)e.Item.FindControl("linkVideoQuartoTri");
            var tituloVideoQuartoTrimestre = (Literal)e.Item.FindControl("tituloVideoQuartoTri");


            var textoResultadoSelecionado = (Literal)e.Item.FindControl("LtrTextoResultado");
            var divCaixaConteCentral = (HtmlControl)e.Item.FindControl("divCaixaConteCentral");
            var divLinhasNoticias = (HtmlControl)e.Item.FindControl("divLinhasNoticias");

            var ulAno = (HtmlControl)e.Item.FindControl("ulAno");
            var liPrimeiroTri = (HtmlControl)e.Item.FindControl("liPrimeiroTri");
            var liSegundoTri = (HtmlControl)e.Item.FindControl("liSegundoTri");
            var liTerceiroTri = (HtmlControl)e.Item.FindControl("liTerceiroTri");
            var liQuartoTri = (HtmlControl)e.Item.FindControl("liQuartoTri");

            ulAno.Attributes.Add("ano", item.Key.ToString());

            switch (Thread.CurrentThread.CurrentUICulture.Name)
            {
                case "pt-BR":
                    labelAnoDivulgacao.Text = GetLocalResourceObject("DivulgacaoResultados") != null ? GetLocalResourceObject("DivulgacaoResultados") + string.Format(" {0}", item.Key) : "Divulgação dos Resultados" + string.Format(" {0}", item.Key);
                    break;
                case "en-US":
                    if (GetLocalResourceObject("DivulgacaoResultados") != null)
                    {
                        tituloDivulgacao = string.Format("{0} ", item.Key) + GetLocalResourceObject("DivulgacaoResultados");
                    }
                    else
                    {
                        tituloDivulgacao = string.Format("{0} ", item.Key) + "Disclosure and Results";
                    }

                    labelAnoDivulgacao.Text = tituloDivulgacao;

                    break;
            }

            foreach (Resultado resultado in item)
            {
                if (!resultado.TipoResultado.Equals(TipoResultado.Mensal))
                {

                    if (divCaixaConteCentral != null && divLinhasNoticias != null)
                    {
                        divCaixaConteCentral.Visible = true;
                        divLinhasNoticias.Visible = true;
                    }

                    switch (resultado.Periodo)
                    {
                        case 1:
                            PreencherLi(primeiroTrimestre, liPrimeiroTri, linkPrimeiroTrimestre, resultado);

                            textoResultadoSelecionado.Text = ObterSubtituloResultado(resultado);
                            subtituloPrimeiro.Attributes.Add("value", ObterSubtituloResultado(resultado));
                            linkResultado.InnerText = ObterSubtituloResultado(resultado);
                            linkResultado.HRef = string.Format("ShowResultado.aspx?IdResultado={0}", criptografia.Criptografar(resultado.Id));
                            {
                                var arquivos = ObterArquivosPorCultura(resultado.Arquivos.OrderBy(p => p.Posicao).ToList());
                                AjustaLinksArquivos(arquivos, "1T", e);
                            }

                            if (ConfigHelper.ObterCultura(Session) == Culturas.PtBr)
                            {
                                if (!String.IsNullOrEmpty(resultado.Link_L1))
                                {
                                    linkVideoPrimeiroTrimestre.Visible = true;
                                    linkVideoPrimeiroTrimestre.Attributes.Add("href", resultado.Link_L1);
                                    tituloVideoPrimeiroTrimestre.Text = resultado.Nome_Link_L1;
                                }
                            }
                            else if (ConfigHelper.ObterCultura(Session) == Culturas.EnUs)
                            {
                                if (!String.IsNullOrEmpty(resultado.Link_L2))
                                {
                                    linkVideoPrimeiroTrimestre.Visible = true;
                                    linkVideoPrimeiroTrimestre.Attributes.Add("href", resultado.Link_L2);
                                    tituloVideoPrimeiroTrimestre.Text = resultado.Nome_Link_L2;
                                }
                            }
                            else if (ConfigHelper.ObterCultura(Session) == Culturas.Es)
                            {
                                if (!String.IsNullOrEmpty(resultado.Link_L3))
                                {
                                    linkVideoPrimeiroTrimestre.Visible = true;
                                    linkVideoPrimeiroTrimestre.Attributes.Add("href", resultado.Link_L3);
                                    tituloVideoPrimeiroTrimestre.Text = resultado.Nome_Link_L3;
                                }
                            }



                            break;
                        case 2:
                            PreencherLi(segundoTrimestre, liSegundoTri, linkSegundoTrimestre, resultado);

                            segundoTrimestre.Text = ObterTituloPorCultura(resultado);
                            subtituloSegundo.Attributes.Add("value", ObterSubtituloResultado(resultado));
                            {
                                var arquivos = ObterArquivosPorCultura(resultado.Arquivos.OrderBy(p => p.Posicao).ToList());
                                AjustaLinksArquivos(arquivos, "2T", e);
                            }

                            if (ConfigHelper.ObterCultura(Session) == Culturas.PtBr)
                            {
                                if (!String.IsNullOrEmpty(resultado.Link_L1))
                                {
                                    linkVideoSegundoTrimestre.Visible = true;
                                    linkVideoSegundoTrimestre.Attributes.Add("href", resultado.Link_L1);
                                    tituloVideoSegundoTrimestre.Text = resultado.Nome_Link_L1;
                                }
                            }
                            else if (ConfigHelper.ObterCultura(Session) == Culturas.EnUs)
                            {
                                if (!String.IsNullOrEmpty(resultado.Link_L2))
                                {
                                    linkVideoSegundoTrimestre.Visible = true;
                                    linkVideoSegundoTrimestre.Attributes.Add("href", resultado.Link_L2);
                                    tituloVideoSegundoTrimestre.Text = resultado.Nome_Link_L2;
                                }
                            }
                            else if (ConfigHelper.ObterCultura(Session) == Culturas.Es)
                            {
                                if (!String.IsNullOrEmpty(resultado.Link_L3))
                                {
                                    linkVideoSegundoTrimestre.Visible = true;
                                    linkVideoSegundoTrimestre.Attributes.Add("href", resultado.Link_L3);
                                    tituloVideoSegundoTrimestre.Text = resultado.Nome_Link_L3;
                                }
                            }



                            break;
                        case 3:
                            PreencherLi(terceiroTrimestre, liTerceiroTri, linkTerceiroTrimestre, resultado);

                            terceiroTrimestre.Text = ObterTituloPorCultura(resultado);
                            subtituloTerceiro.Attributes.Add("value", ObterSubtituloResultado(resultado));
                            {
                                var arquivos = ObterArquivosPorCultura(resultado.Arquivos.OrderBy(p => p.Posicao).ToList());
                                AjustaLinksArquivos(arquivos, "3T", e);

                            }

                            if (ConfigHelper.ObterCultura(Session) == Culturas.PtBr)
                            {
                                if (!String.IsNullOrEmpty(resultado.Link_L1))
                                {
                                    linkVideoTerceiroTrimestre.Visible = true;
                                    linkVideoTerceiroTrimestre.Attributes.Add("href", resultado.Link_L1);
                                    tituloVideoTerceiroTrimestre.Text = resultado.Nome_Link_L1;
                                }
                            }
                            else if (ConfigHelper.ObterCultura(Session) == Culturas.EnUs)
                            {
                                if (!String.IsNullOrEmpty(resultado.Link_L2))
                                {
                                    linkVideoTerceiroTrimestre.Visible = true;
                                    linkVideoTerceiroTrimestre.Attributes.Add("href", resultado.Link_L2);
                                    tituloVideoTerceiroTrimestre.Text = resultado.Nome_Link_L2;
                                }
                            }
                            else if (ConfigHelper.ObterCultura(Session) == Culturas.Es)
                            {
                                if (!String.IsNullOrEmpty(resultado.Link_L3))
                                {
                                    linkVideoTerceiroTrimestre.Visible = true;
                                    linkVideoTerceiroTrimestre.Attributes.Add("href", resultado.Link_L3);
                                    tituloVideoTerceiroTrimestre.Text = resultado.Nome_Link_L3;
                                }
                            }




                            break;
                        case 4:
                            PreencherLi(quartoTrimestre, liQuartoTri, linkQuartoTrimestre, resultado);

                            quartoTrimestre.Text = ObterTituloPorCultura(resultado);
                            subtituloQuarto.Attributes.Add("value", ObterSubtituloResultado(resultado));
                            {
                                var arquivos = ObterArquivosPorCultura(resultado.Arquivos.OrderBy(p => p.Posicao).ToList());
                                AjustaLinksArquivos(arquivos, "4T", e);

                            }

                            if (ConfigHelper.ObterCultura(Session) == Culturas.PtBr)
                            {
                                if (!String.IsNullOrEmpty(resultado.Link_L1))
                                {
                                    linkVideoQuartoTrimestre.Visible = true;
                                    linkVideoQuartoTrimestre.Attributes.Add("href", resultado.Link_L1);
                                    tituloVideoQuartoTrimestre.Text = resultado.Nome_Link_L1;
                                }
                            }
                            else if (ConfigHelper.ObterCultura(Session) == Culturas.EnUs)
                            {
                                if (!String.IsNullOrEmpty(resultado.Link_L2))
                                {
                                    linkVideoQuartoTrimestre.Visible = true;
                                    linkVideoQuartoTrimestre.Attributes.Add("href", resultado.Link_L2);
                                    tituloVideoQuartoTrimestre.Text = resultado.Nome_Link_L2;
                                }
                            }
                            else if (ConfigHelper.ObterCultura(Session) == Culturas.Es)
                            {
                                if (!String.IsNullOrEmpty(resultado.Link_L3))
                                {
                                    linkVideoQuartoTrimestre.Visible = true;
                                    linkVideoQuartoTrimestre.Attributes.Add("href", resultado.Link_L3);
                                    tituloVideoQuartoTrimestre.Text = resultado.Nome_Link_L3;
                                }
                            }

                            break;
                    }
                }
            }
        }

        private List<Arquivo> ObterArquivosPorCultura(IList<Arquivo> arquivos)
        {
            var arquivosPorCultura = new List<Arquivo>();

            if (arquivos != null)
            {
                IdiomasDeArquivo idiomaCorrente = 0;
                switch (ConfigHelper.ObterCultura(Session))
                {
                    case Culturas.EnUs:
                        idiomaCorrente = IdiomasDeArquivo.Ingles;
                        break;
                    default:
                        idiomaCorrente = IdiomasDeArquivo.Portugues;
                        break;
                }
                arquivos.ToList().ForEach(item => { if (item.Idiomas.Contains(idiomaCorrente)) arquivosPorCultura.Add(item); });
            }
            return arquivosPorCultura;
        }

        private object ObterArquivosPorCultura(IList<Arquivo> arquivos, Resultado resultado)
        {
            var arquivosPorCultura = new List<Arquivo>();

            if (arquivos != null)
            {
                IdiomasDeArquivo idiomaCorrente = 0;
                switch (ConfigHelper.ObterCultura(Session))
                {
                    case Culturas.EnUs:
                        idiomaCorrente = IdiomasDeArquivo.Ingles;
                        break;
                    case Culturas.Es:
                        idiomaCorrente = IdiomasDeArquivo.Espanhol;
                        break;
                    default:
                        idiomaCorrente = IdiomasDeArquivo.Portugues;
                        break;
                }
                arquivos.ToList().ForEach(item => { if (item.Idiomas.Contains(idiomaCorrente)) arquivosPorCultura.Add(item); });
            }
            if (resultado != null)
            {
                if (ConfigHelper.ObterCultura(Session) == Culturas.PtBr)
                {
                    if (!String.IsNullOrEmpty(resultado.Link_L1))
                    {
                        Arquivo videoResultado = new Arquivo();
                        videoResultado.Descricao = "test";

                    }
                }
            }

            return arquivosPorCultura;
        }

        private string MontarEnderecoDeDownload(Arquivo arquivo)
        {
            if (arquivo != null)
            {
                return string.Format("Download.aspx?Arquivo={0}",
                                     (new Criptografia()).Criptografar(arquivo.Id));
            }
            return string.Empty;
        }

        private void PreencherLi(Literal literalTituloTrimestre, HtmlControl liTrimestre, HtmlControl linkTrimestral, Resultado resultado)
        {
            var criptografia = new Criptografia();

            literalTituloTrimestre.Text = ObterTituloPorCultura(resultado);
            liTrimestre.Attributes.Add("resultado", criptografia.Criptografar(resultado.Id));
            linkTrimestral.Attributes.Add("href", string.Format("ShowResultado.aspx?IdResultado={0}", criptografia.Criptografar(resultado.Id)));

            liTrimestre.Visible = true;
        }

        private string ObterTituloPorCultura(Resultado resultado)
        {
            if (Session["MyCulture"] != null)
            {
                switch (Session["MyCulture"].ToString())
                {
                    case Culturas.EnUs:
                        return resultado.TituloIngles;
                    default:
                        return resultado.TituloPortugues;
                }
            }
            return string.Empty;
        }

        private string ObterSubtituloResultado(Resultado resultado)
        {
            if (Session["MyCulture"] != null)
            {
                var minhaCultura = Session["MyCulture"].ToString();
                switch (minhaCultura)
                {
                    case Culturas.EnUs:
                        return resultado.SubtituloIngles;
                    default:
                        return resultado.SubtituloPortugues;
                }
            }
            return string.Empty;
        }

        protected void ConfigurarMenuTopo(IList<int> anos)
        {
            menuTopo.AnosDeFiltro = anos;
        }

    }
}