<%@ Page Language="C#" MasterPageFile="~/Master/Internal.Master" AutoEventWireup="true" CodeBehind="ListaBusca.aspx.cs" Inherits="REAG.ListaBusca" culture="auto" uiculture="auto"%>
<%@ Register Assembly="ComuniqueSe.WebControls, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=5b058f9e1367e870" NameSpace="ComuniqueSe.WebControls.Paginacao" TagPrefix="cc3" %>
<%@ Register Src="~/ascx/MenuTopo.ascx" TagName="MenuTopo" TagPrefix="uc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="ContentHead">
    <script type="text/javascript">
        $(document).ready(function () {
            $('a[id*=linkListaBusca]').each(function () {
                var link = $(this).attr('href');
                $(this).parent().parent().find('a#linkVejaMais').attr('href', link);
            });

            if ($('.resultadosBusca').text().trim() === '') {
                if ($(".hidLinguagem").val() == "ptg") {
                    $('.resultadosBusca').html('<li class="list-group-item"><p>Não foram encontrados resultados para essa pesquisa.</p ></li> ');
                } else {
                    $('.resultadosBusca').html('<li class="list-group-item"><p>No results were found for this search.</p ></li> ');
                }
            }
        });
    </script>						                
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolderTituloCanalPai">
    <asp:Literal runat="server" Text="<%$resources:ltrResultadoBusca.Text %>"></asp:Literal>
  
</asp:Content> 

<asp:Content ContentPlaceHolderID="ContentPlaceHolderConteudo" runat="server">

    <h2>
        <asp:Literal runat="server" Text="<%$resources:ltrResultados.Text %>"></asp:Literal>
    </h2>

    <ul class="list-inline">
		<li class="list-inline-item">
            <p class="btn btn-primary">
                <asp:Literal runat="server" ID="ltrItemDeBusca"></asp:Literal>                
		    </p>
		</li>
	</ul>
    

    <ul class="list-group resultadosBusca">
        <asp:Repeater ID="rptListaData" OnItemDataBound="rptListaData_OnItemDataBound" runat="server">
            <ItemTemplate>                
                <li class="list-group-item">
                    <span class="h3">
                        <asp:Literal runat="server" ID="ltrTituloCanalPai"></asp:Literal>
                    </span>
                    <span id="dataListBusca" runat="server" visible="false"></span>
                    <p>
                        <a  id="linkListaBusca" runat="server"  /> 
                    </p>                         
                    <a id="linkVejaMais" href="#" class="btn btn-outline-azul">
                        <asp:Literal runat="server"  Text="<%$resources:LeiaMais %>" ></asp:Literal>
                    </a>
                 
                    <asp:Literal ID="ltrSubtitulo" runat="server" Visible="false" />                        
                </li>                    
            </ItemTemplate>
        </asp:Repeater>
    </ul>

     <cc3:WebControlPaginacao ID="webControlPaginacao" runat="server"
        HabilitaBtnPrimeiro="false" HabilitaBtnAnterior="true" HabilitaBtnNumeracao="true" HabilitaBtnProximo="True" HabilitaBtnUltimo="false" 
        HabilitaPagAtualETotal="false" pagAtualETotalComLink="false" PaginacaoConteudo="false" PaginacaoComentario="false"
        HtmlBtnPrimeiro=""
        HtmlBtnAnterior="@textoBtnAnterior"
        HtmlBtnNumeracao="@htmlNumeracao"
        HtmlBtnProximo="@textoBtnProximo"
        HtmlBtnUltimo=""
        HtmlPagAtualETotal=""       

        CssBtnPrimeiro=""
        CssBtnAnterior="pegaA btn btn-lg"
        CssBtnNumeracao="pegaA btn btn-lg btn-outline-azul"
        CssBtnProximo="pegaA btn btn-lg"
        CssBtnUltimo=""
        CssPagAtualETotal="active"
        CssNavList="pagination justify-content-center paglist"
                
        textoBtnPrimeiro=""
        textoBtnAnterior="<"
        textoBtnProximo=">"
        textoBtnUltimo=""
        textoPagAtualETotal="">
    </cc3:WebControlPaginacao>

</asp:Content>