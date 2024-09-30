<%@ Page Language="C#" MasterPageFile="~/Master/Internal.master" AutoEventWireup="true" CodeBehind="fale_conosco.aspx.cs" Inherits="REAG.FaleConosco" %>
<%@ Register Assembly="ComuniqueSe.WebControls, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=5b058f9e1367e870" Namespace="ComuniqueSe.WebControls.ControleMateria" TagPrefix="wcd" %>
<%@ Register Src="~/ascx/MenuTopo.ascx" TagName="MenuTopo" TagPrefix="uc1" %>


<asp:content ContentPlaceHolderID="ContentPlaceHolderBarraBotoes" runat="server">    
    <uc1:MenuTopo ID="menuTopo" runat="server"
        PermiteCompartilhar="True" />
</asp:content>

<asp:Content runat="server" ContentPlaceHolderID="ContentHead"> 
    <script type="text/javascript" src="js/fale_ri.js"></script>  
    <script type="text/javascript" src="js/meiomask.js"></script>
    <script src='https://www.google.com/recaptcha/api.js'></script>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolderConteudo" runat="server">  

  <div id="" class="justify-content-center form-contato form-inline">
  <div class="tabs-main">
    <ul class="nav nav-tabs" id="myTab" role="tablist">
      <li class="nav-item">
        <a
          class="nav-link"
          id="home-tab"
          href="./central-de-resultados"
          >Central de Resultados</a
        >
      </li>

      <li class="nav-item">
        <a
          class="nav-link"
          id="profile-tab"            
          href="./Documentos"
          >Documentos CVM</a>
      </li>

      <li class="nav-item">
        <a
         class="nav-link active"
         id="contact-tab"            
         href="#"
         data-toggle="tab"
         role="tab"
         aria-controls="contact-tab"
         aria-selected="true"
          >Fale com RI</a
        >
      </li>
    </ul>
  </div>
   </div>

       <div
              class="tab-pane"
              id="contact"
              role="tabpanel"
              aria-labelledby="contact-tab"
            >
              <div class="forms-main" id="formFaleComRi">
                <div class="div-input">
                   <asp:TextBox ID="txtNomeContato" runat="server" placeholder="<%$Resources: Nome %>"  />
	            <div class="validacaocampos">
					<asp:RequiredFieldValidator runat="server" ControlToValidate="txtNomeContato" Display="Dynamic" SetFocusOnError="true" ErrorMessage="<%$ resources:Obrigatorio %>" ValidationGroup="FormFaleConosco" ForeColor="Red" />
	            </div>
                </div>

                <div class="div-input">
                  <asp:TextBox ID="txtEmpresa" runat="server" placeholder="<%$Resources: Empresa %>"   />
                </div>

                <div class="div-input">
                    <asp:TextBox ID="txtEmail" runat="server" placeholder="<%$Resources: Email %>"  />
		<div class="validacaocampos">
						<asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" Display="Dynamic" SetFocusOnError="true" ErrorMessage="<%$ resources:Obrigatorio %>" ValidationGroup="FormFaleConosco" ForeColor="Red" />                                    
		</div>
		<div class="validacaocampos">
						<asp:RegularExpressionValidator runat="server" ErrorMessage="<%$ resources:Error_Email %>" ControlToValidate="txtEmail" Display="Dynamic" ValidationGroup="FormFaleConosco"  ForeColor="Red"
							SetFocusOnError="true" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
						</asp:RegularExpressionValidator>
		</div>
                </div>

                <div class="div-input">
                   <asp:TextBox ID="txtTelefone" runat="server" placeholder="<%$Resources: Telefone %>"   />
                </div>

                <div class="div-input">
                          <asp:TextBox ID="txtMensagem" runat="server" TextMode="MultiLine" Rows="6" placeholder="<%$Resources: Mensagem %>"   />	
	        <div class="validacaocampos">
					        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMensagem" Display="Dynamic" SetFocusOnError="true" ErrorMessage="<%$ resources:Obrigatorio %>" ValidationGroup="FormFaleConosco" ForeColor="Red" />                                    
	        </div>
                </div>

                  <div class="div-input">

                         <div id="divRecaptcha" class="g-recaptcha" runat="server" ></div>
						        <div class="validacaocampos">
							        <asp:Label runat="server" ID="lblMessage" />  
						  </div>
                        </div>
                  

                <div class="div-input">
                  <asp:LinkButton runat="server" ID="enviarRI" Onclick="btEnviar_Click" onclientclick=" javascript:return ValidaCaptcha();" CssClass="submit-input" ValidationGroup="FormFaleConosco" Text="<%$resources:Enviar %>" />
                </div>

                  <em>
                     <asp:Literal ID="ltrMsg" runat="server" Text="<%$resources:MsgEnviada %>" Visible="false" />
                 </em>
              </div>
            </div>
</asp:Content>