using System;
using System.Web.UI.WebControls;
using ComuniqueSe.Portais.Paginas;

namespace REAG
{
    public partial class ListaBusca : ListBuscaPage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            LtrTituloCanalPai = true;
            BuscaNoConteudo = true;
            base.Page_Load(sender, e);
        }

        protected override Literal LtrItemDeBusca
        {
            get { return ltrItemDeBusca; }
        }
        protected override Literal LtrResultados
        {
            get { return new Literal(); }
        }
        protected override Literal LtrTextoTit1
        {
            get { return new Literal(); }
        }
        protected override Literal LtrTotalMaterias
        {
            get { return new Literal(); }
        }
        protected override Panel PnlPaginacao
        {
            get { return new Panel(); }
        }
        protected override Repeater RptListaData
        {
            get { return rptListaData; }
        }
    }
}