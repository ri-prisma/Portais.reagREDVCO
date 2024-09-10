<%@ Page Language="C#" MasterPageFile="~/Master/Internal.master" AutoEventWireup="true" CodeBehind="Show.aspx.cs" Inherits="REAG.Show" %>
<%@ Register Assembly="ComuniqueSe.WebControls, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=5b058f9e1367e870" NameSpace="ComuniqueSe.WebControls.ControleEmail" TagPrefix="cc2" %>
<%@ Register Src="~/ascx/MenuTopo.ascx" TagName="MenuTopo" TagPrefix="uc1" %>



<asp:content ContentPlaceHolderID="ContentPlaceHolderTituloCanalPai" runat="server">    
         <asp:Literal ID="ltrTituloCanal" runat="server"></asp:Literal>
</asp:content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolderConteudo" runat="server"> 

    <asp:Literal runat="server" ID="ltrTextoMateria"></asp:Literal>

    <input type="hidden" id="hdnShow" value="1" />

</asp:Content>







