using System;
using System.Web.UI.WebControls;
using ComuniqueSe.Portais.Paginas;

namespace REAG
{
    public partial class Show : ShowPage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            UseTituloMateria = true;
            UseTituloMateriaIgualCanal = false;
            base.Page_Load(sender, e);
        }

        protected override Literal LtrSubTituloMateria { get { return new Literal(); } }
        protected override Literal LtrTextoMateria { get { return ltrTextoMateria; } }
        protected override Literal LtrTituloMateria { get { return new Literal(); } }
        protected override Literal LtrTituloCanal { get { return ltrTituloCanal; } }
        protected override Literal LtrTituloPaiCanal { get { return new Literal(); } }
        protected override Literal LtrDescricaoCanal { get { return new Literal(); } }
    }
}
