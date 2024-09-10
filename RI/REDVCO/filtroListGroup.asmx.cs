using ComuniqueSe.Portais.Paginas.Helpers;
using ComuniqueSe.WebControls.Helpers;
using ComuniqueSeWorkflow.Container;
using ComuniqueSeWorkflow.Dominio.Entidades;
using ComuniqueSeWorkflow.Dominio.IRepositorios;
using ComuniqueSeWorkflow.Infraestrutura;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace REAG
{
    /// <summary>
    /// Descrição resumida de filtroListGroup
    /// </summary>
    [WebService(Namespace = "http://microsoft.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que esse serviço da web seja chamado a partir do script, usando ASP.NET AJAX, remova os comentários da linha a seguir. 
     [System.Web.Script.Services.ScriptService]
    public class filtroListGroup : System.Web.Services.WebService
    {

        private static readonly HttpClient httpclient;

        static filtroListGroup()
        {
            httpclient = new HttpClient();
        }

      

        private ConteudoHelper _conteudoHelper;
        private ConteudoHelper ConteudoHelper
        {
            get
            {
                return _conteudoHelper ?? (_conteudoHelper = new ConteudoHelper());
            }
        }

        private CultureHelper _cultureHelper;
        private CultureHelper CultureHelper
        {
            get
            {
                return _cultureHelper ?? (_cultureHelper = new CultureHelper());
            }
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public  List<CanalWeb> RefreshContent(int ano, string idCanal, string linguagem)
        {

            var idCanalDescriptografado = Convert.ToInt32(new Criptografia().Descriptografar(idCanal));

            //Pegar Canais Filho
            var canaisFilhos = ObterCanaisFilhos(idCanalDescriptografado, ano);

            var ctHelper = new ConteudoHelper();
            var canalWeb = new List<CanalWeb>();

            foreach (var canal in canaisFilhos)
            {
                var materiasWeb = new List<MateriaWebLisGroup>();

                var cw = new CanalWeb();
                var materias = ObterConteudosPaginadosPorAno(canal, ano).ToList();
                foreach (var materia in materias)
                {
                    var mw = new MateriaWebLisGroup();
                    if (linguagem == "ptg")
                    {

                        CultureInfo newCulture = CultureInfo.CreateSpecificCulture("pt-BR");
                        Thread.CurrentThread.CurrentUICulture = newCulture;

                        mw.Data = materia.DataDePublicacao.ToString("dd/MM/yyyy");
                        mw.Titulo = materia.TituloPortugues;
                        mw.Link = ctHelper.GetLink(materia);
                    }
                    else
                    {
                        CultureInfo newCulture = CultureInfo.CreateSpecificCulture("en-US");
                        Thread.CurrentThread.CurrentUICulture = newCulture;

                        mw.Data = materia.DataDePublicacao.ToString("MM/dd/yyyy");
                        if (String.IsNullOrEmpty(materia.TituloIngles))
                        {
                            mw.Titulo = materia.TituloPortugues + " (Portuguese Only)";
                        }
                        else
                        {
                            mw.Titulo = materia.TituloIngles;
                        }

                        mw.Link = ctHelper.GetLink(materia);

                    }
                    materiasWeb.Add(mw);
                }

                if (linguagem == "ptg")
                {

                    CultureInfo newCulture = CultureInfo.CreateSpecificCulture("pt-BR");
                    Thread.CurrentThread.CurrentUICulture = newCulture;
                    cw.Titulo = canal.TituloPortugues;
                }
                else
                {
                    CultureInfo newCulture = CultureInfo.CreateSpecificCulture("en-US");
                    Thread.CurrentThread.CurrentUICulture = newCulture;
                    cw.Titulo = canal.TituloIngles;
                }
                cw.Materia = materiasWeb;
                canalWeb.Add(cw);
            }
            return canalWeb;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<CanalWeb> RefreshCategoria(int ano, string categoria, string idCanal, string linguagem)
        {

            var idCanalDescriptografado = Convert.ToInt32(new Criptografia().Descriptografar(idCanal));
            var idCategoriaDescriptografado = Convert.ToInt32(new Criptografia().Descriptografar(categoria));

            //Pegar Canais Filho
            var canaisFilhos = ObterCanaisFilhos(idCanalDescriptografado, ano);

            var ctHelper = new ConteudoHelper();
            var canalWeb = new List<CanalWeb>();

            foreach (var canal in canaisFilhos)
            {
                var materiasWeb = new List<MateriaWebLisGroup>();

                var cw = new CanalWeb();
                var materias = ObterConteudosPaginadosPorCategoria(canal, idCategoriaDescriptografado).ToList();
                foreach (var materia in materias)
                {
                    var mw = new MateriaWebLisGroup();
                    if (linguagem == "ptg")
                    {

                        CultureInfo newCulture = CultureInfo.CreateSpecificCulture("pt-BR");
                        Thread.CurrentThread.CurrentUICulture = newCulture;

                        mw.Data = materia.DataDePublicacao.ToString("dd/MM/yyyy");
                        mw.Titulo = materia.TituloPortugues;
                        mw.Link = ctHelper.GetLink(materia);
                    }
                    else
                    {
                        CultureInfo newCulture = CultureInfo.CreateSpecificCulture("en-US");
                        Thread.CurrentThread.CurrentUICulture = newCulture;

                        mw.Data = materia.DataDePublicacao.ToString("MM/dd/yyyy");
                        if (String.IsNullOrEmpty(materia.TituloIngles))
                        {
                            mw.Titulo = materia.TituloPortugues + " (Portuguese Only)";
                        }
                        else
                        {
                            mw.Titulo = materia.TituloIngles;
                        }

                        mw.Link = ctHelper.GetLink(materia);

                    }
                    materiasWeb.Add(mw);
                }

                if (linguagem == "ptg")
                {

                    CultureInfo newCulture = CultureInfo.CreateSpecificCulture("pt-BR");
                    Thread.CurrentThread.CurrentUICulture = newCulture;
                    cw.Titulo = canal.TituloPortugues;
                }
                else
                {
                    CultureInfo newCulture = CultureInfo.CreateSpecificCulture("en-US");
                    Thread.CurrentThread.CurrentUICulture = newCulture;
                    cw.Titulo = canal.TituloIngles;
                }
                cw.Materia = materiasWeb;
                canalWeb.Add(cw);
            }
            return canalWeb;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<CanalWeb> RefreshCategoriaAno(int ano, string categoria, string idCanal, string linguagem)
        {

            var idCanalDescriptografado = Convert.ToInt32(new Criptografia().Descriptografar(idCanal));
            var idCategoriaDescriptografado = Convert.ToInt32(new Criptografia().Descriptografar(categoria));

            //Pegar Canais Filho
            var canaisFilhos = ObterCanaisFilhos(idCanalDescriptografado, ano);

            var ctHelper = new ConteudoHelper();
            var canalWeb = new List<CanalWeb>();

            foreach (var canal in canaisFilhos)
            {
                var materiasWeb = new List<MateriaWebLisGroup>();

                var cw = new CanalWeb();
                var materias = ObterConteudosPaginadosPorCategoriaAno(canal, idCategoriaDescriptografado, ano).ToList();
                foreach (var materia in materias)
                {
                    var mw = new MateriaWebLisGroup();
                    if (linguagem == "ptg")
                    {

                        CultureInfo newCulture = CultureInfo.CreateSpecificCulture("pt-BR");
                        Thread.CurrentThread.CurrentUICulture = newCulture;

                        mw.Data = materia.DataDePublicacao.ToString("dd/MM/yyyy");
                        mw.Titulo = materia.TituloPortugues;
                        mw.Link = ctHelper.GetLink(materia);
                    }
                    else
                    {
                        CultureInfo newCulture = CultureInfo.CreateSpecificCulture("en-US");
                        Thread.CurrentThread.CurrentUICulture = newCulture;

                        mw.Data = materia.DataDePublicacao.ToString("MM/dd/yyyy");
                        if (String.IsNullOrEmpty(materia.TituloIngles))
                        {
                            mw.Titulo = materia.TituloPortugues + " (Portuguese Only)";
                        }
                        else
                        {
                            mw.Titulo = materia.TituloIngles;
                        }

                        mw.Link = ctHelper.GetLink(materia);

                    }
                    materiasWeb.Add(mw);
                }

                if (linguagem == "ptg")
                {

                    CultureInfo newCulture = CultureInfo.CreateSpecificCulture("pt-BR");
                    Thread.CurrentThread.CurrentUICulture = newCulture;
                    cw.Titulo = canal.TituloPortugues;
                }
                else
                {
                    CultureInfo newCulture = CultureInfo.CreateSpecificCulture("en-US");
                    Thread.CurrentThread.CurrentUICulture = newCulture;
                    cw.Titulo = canal.TituloIngles;
                }
                cw.Materia = materiasWeb;
                canalWeb.Add(cw);
            }
            return canalWeb;
        }
        protected static Canal getCanal(int idcanal)
        {
            IRepositorioCanal _repositorioCanal = ContainerHelper.Resolve<IRepositorioCanal>();
            return _repositorioCanal.RetornaCanal(idcanal);
        }

        protected static IList<Conteudo> ObterConteudosPaginadosPorAno(Canal canal, int ano)
        {
            IRepositorioConteudo _repositorioConteudo = ContainerHelper.Resolve<IRepositorioConteudo>();
            return _repositorioConteudo.RetornaConteudosDoCanalPorAno(canal.Id, 0, 150, ano);
        }

        protected static IList<Conteudo> ObterConteudosPaginadosPorCategoria(Canal canal, int categoria)
        {
            IRepositorioConteudo _repositorioConteudo = ContainerHelper.Resolve<IRepositorioConteudo>();
            return _repositorioConteudo.RetornaConteudosDoCanalPorCategoria(canal.Id, 0, 150, categoria);
        }

        protected static IList<Conteudo> ObterConteudosPaginadosPorCategoriaAno(Canal canal, int categoria, int ano)
        {
            IRepositorioConteudo _repositorioConteudo = ContainerHelper.Resolve<IRepositorioConteudo>();
            return _repositorioConteudo.RetornaConteudosDoCanalPorAnoCategoria(canal.Id, 0, 150, ano, categoria);
        }

        protected static IList<Canal> ObterCanaisFilhos(int idCanal, int ano)
        {

            ComuniqueSe.Portais.Paginas.Helpers.ConfigHelper _configHelperPaginas = new ComuniqueSe.Portais.Paginas.Helpers.ConfigHelper();
            IRepositorioCanal _repositorioCanal = ContainerHelper.Resolve<IRepositorioCanal>();
            return _repositorioCanal.RetornarCanaisFilho(Convert.ToInt32(_configHelperPaginas.ObterPortal().Id), idCanal, ano).ToList();
        }



    }


    public class MateriaWebLisGroup
    {
        public string Titulo { get; set; }
        public string Link { get; set; }
        public string Data { get; set; }
        public string TituloChamada { get; set; }

        // public string Idioma { get; set; }
    }

    public class CanalWeb
    {
        public string Titulo { get; set; }

        public List<MateriaWebLisGroup> Materia { get; set; }

    }
}
