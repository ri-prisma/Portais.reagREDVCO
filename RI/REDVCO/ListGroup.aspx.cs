using System.Web.UI.WebControls;
using ComuniqueSe.Portais.Paginas;
using ComuniqueSe.Portais.Paginas.UserControls;
using System;
using System.Linq;
using ComuniqueSeWorkflow.Infraestrutura;

namespace REAG
{
    public partial class ListGroup : ListGroupPage
    {

        protected override void Page_Load(object sender, EventArgs e)
        {
            _itensPorPagina = 1000;
            hdCanal.Value = Request.QueryString["idCanal"].ToString();

            //_ultimoAno = true;
            //_obterCanaisUltimoAno = true;

            base.Page_Load(sender, e);
        }

        protected override MenuTopoControl MenuTopo
        {
            get { return menuTopo; }
        }
     
        protected override Panel PnlPaginacao
        {
            get { return new Panel(); }
        }
        protected override Repeater RptListaCanal
        {
            get { return rptList; }
        }

        protected override Literal LtrTituloCanal
        {
            get { return new Literal(); }
        }

        protected override Literal LtrDescricaoCanal
        {
            get { return new Literal(); }
        }

        protected override Literal LtrTituloCanalPai
        {
            get { return new Literal(); }
        }
    }
}