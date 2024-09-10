using System;
using System.Web.UI.WebControls;

namespace REAG.Master
{
    public partial class Master : ComuniqueSe.Portais.Paginas.Master.Master
    {
        protected override LinkButton HlkCulturaPtBr { get { return hlkCulturaPtBr; } }
        protected override LinkButton HlkCulturaEnUs { get { return hlkCulturaEnUS; } }
        protected override LinkButton HlkCulturaEs { get { return new LinkButton(); } }

        protected override HiddenField HdfSlugPT { get { return hdfSlugPT; } }

        protected override HiddenField HdfSlugEN { get { return hdfSlugEN; } }

        protected override HiddenField HdfSlugES { get { return hdfSlugES; } }

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


        }


    }
}