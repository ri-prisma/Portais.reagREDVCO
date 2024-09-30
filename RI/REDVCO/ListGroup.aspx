<%@ Page Language="C#" MasterPageFile="~/Master/Internal.master" AutoEventWireup="true" CodeBehind="ListGroup.aspx.cs" Inherits="REAG.ListGroup" %>

<%@Register Src="~/ascx/MenuTopo.ascx" TagName="MenuTopo" TagPrefix="uct" %>



<asp:content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderBarraBotoes" runat="server">    

</asp:content>



<asp:content ContentPlaceHolderID="ContentPlaceHolderTituloCanal" runat="server">    
   
    <asp:Literal Text="<%$Resources: titulo %>" runat="server" ></asp:Literal>
</asp:content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolderConteudo">
    <center><div class="loader"></div> </center>
     <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True">
    </asp:ScriptManager>


    <div class=" txt-show list-show " id="totalContent" style="display:none;">

  <div id="" class="justify-content-center form-contato form-inline">
   <div class="tabs-main">
     <ul class="nav nav-tabs" id="myTab" role="tablist">
       <li class="nav-item">
         <a
           class="nav-link"
           id="home-tab"
           href="/central-de-resultados"
           >Central de Resultados</a
         >
       </li>

       <li class="nav-item">
         <a
           class="nav-link active"
           id="profile-tab"            
           href="#"
          data-toggle="tab"
          role="tab"
          aria-controls="profile-tab"
          aria-selected="true"
           >Documentos CVM</a>
       </li>

       <li class="nav-item">
         <a
           class="nav-link"
           id="contact-tab"                 
           href="./fale-com-o-ri"             
           >Fale com RI</a
         >
       </li>
     </ul>
   </div>

     <uct:MenuTopo ID="menuTopo" runat="server"
     PermiteDllAno="True"/>
        
 </div>

        <div id="accordion">
            <asp:Repeater ID="rptList" OnItemDataBound="rptList_OnItemDataBound" runat="server">
                <ItemTemplate>             
                    <div class="card">
                        <div class="card-header" id="heading-">
                            <h4 class="mb-0">
                                <a href="#" id="tituloList" class="btn collapsed  btn-secondary idLink" runat="server" data-toggle="collapse" data-target="#collapse-" aria-expanded="false" aria-controls="collapse-">
                                    <asp:Literal ID="ltrTituloCanalMaterias" runat="server"></asp:Literal>
                                </a>
                            </h4>
                        </div>
                         <div id="collapse-" class="collapse" aria-labelledby="heading-" data-parent="#accordion">
                            <div class="card-body">
                                <ul class="list-group">
                                    <asp:Repeater ID="rptSubList" runat="server" OnItemDataBound="rptSubList_ItemDataBound">
                                        <ItemTemplate>
                                          <li class="list-group-item d-flex justify-content-between align-items-center">
                                              <div>
                                                    <span class="float-left">
                                                        <asp:Literal ID="ltrData" runat="server"></asp:Literal>
                                                    </span>
                                                    <a id="linkListaTituloChamada" runat="server"></a>
                                                  </div>
                                                    <span id="spTextoMateria" runat="server" class="textoListNormal" Visible="False"></span>
                                                    <asp:Literal ID="ltrTextoChamada" runat="server" Visible="False"/>
                                              </li>
                                    
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>  
        </div>
    </div>  


    

    
    <input type="hidden" id="hdnList" value="1" /> 
     <asp:HiddenField id="hdCanal" runat="server"></asp:HiddenField>

</asp:Content>
