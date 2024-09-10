<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuTopo.ascx.cs" Inherits="REAG.ascx.MenuTopo" %>
<%@ Register Assembly="ComuniqueSe.WebControls, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=5b058f9e1367e870" Namespace="ComuniqueSe.WebControls.MeusDownloads" TagPrefix="cmd" %>
<%@ Register Assembly="ComuniqueSe.WebControls, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=5b058f9e1367e870" Namespace="ComuniqueSe.WebControls.ControleMeusFavoritos" tagPrefix="wcmf" %>


     
        <div class=" justify-content-center mb-3 form-contato form-inline" id="ddlAnoLink" runat="server">
            <label for="comboano" class="t-bold mr-3">
                <asp:Literal Text="<%$Resources: filtarPor%>" runat="server" ></asp:Literal>
            </label>
            <asp:DropDownList runat="server" ID="ddlAnoFiltro" onchange="javascript:filtrarAno();" CssClass="form-control">               
            </asp:DropDownList>
        </div>    


    <cmd:WebControlMeusDownloadsIcone ID="downloadLink" runat="server" visible="false"/>


   


   












 
			
					