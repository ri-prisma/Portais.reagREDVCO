<%@ Page Language="C#" MasterPageFile="~/Master/Internal.master" AutoEventWireup="true" CodeBehind="ListResultados.aspx.cs" Inherits="REAG.ListResultados" %>
<%@ Register Assembly="ComuniqueSe.WebControls, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=5b058f9e1367e870" Namespace="ComuniqueSe.WebControls.ControleMateria" TagPrefix="wcd" %>
<%@ Register Src="~/ascx/MenuTopo.ascx" TagName="MenuTopo" TagPrefix="uct" %>   


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderConteudo" runat="server">

	 <div class="container">
        <div class="txt-show mg-top-tst">
          <div id="" class="justify-content-center form-contato form-inline">
            <div class="tabs-main">
              <ul class="nav nav-tabs" id="myTab" role="tablist">
                <li class="nav-item">
                  <a
                    class="nav-link active"
                    id="home-tab"
                    data-toggle="tab"
                    href="#home"
                    role="tab"
                    aria-controls="home"
                    aria-selected="true"
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
                    class="nav-link"
                    id="contact-tab"                 
                    href="./fale-com-o-ri"             
                    >Fale com RI</a
                  >
                </li>
              </ul>
            </div>

                 <uct:MenuTopo ID="menuTopo" runat="server"
					 PermiteDllAno="True" />
          </div>

          <div class="tab-content" id="myTabContent">
            <div
              class="tab-pane fade show active table-responsive"
              id="home"
              role="tabpanel"
              aria-labelledby="home-tab"
            >
        <asp:Repeater runat="server" ID="rptResultados" OnItemDataBound="RptResultadosItemDataBound">
		<ItemTemplate>
			<div  id="divResultados" runat="server" style="display:none">
				<span class="subTitList">
					<asp:Literal runat="server" ID="LtrAnoDivulgacao" Visible="False" />
				</span>
				<table  class="table table-central table-hover table-borderless" runat="server" id="ulAno">
					<thead>
						<tr class="trThead">
							<th class="recebeAno"></th>
							<th id="liPrimeiroTri" runat="server">
								<asp:Literal runat="server" ID="LtrPrimeiroTri" />
								<input type="hidden" runat='server' id="HdnPrimeiroTri" />
							</th>
							<th id="liSegundoTri" runat="server">
								<asp:Literal runat="server" ID="LtrSegundoTri" />
								<input type="hidden" runat='server' id="HdnSegundoTri" />
							</th>
							<th  id="liTerceiroTri" runat="server">
								<asp:Literal runat="server" ID="LtrTerceiroTri" />
								<input type="hidden" runat='server' id="HdnTerceiroTri" />
							</th>
							<th id="liQuartoTri" runat="server">
								<asp:Literal runat="server" ID="LtrQuartoTri" />
								<input type="hidden" runat='server' id="HdnQuartoTri" />
							</th>									
						</tr>
					</thead>
					<tbody>
						<tr runat="server" Visible="false">
							<td>
								<a runat="server" id="linkPrimeiroTri" href="javascript:void(0);">
								</a>
							</td>
								<td>
								<a runat="server" id="linkSegundoTri" href="javascript:void(0)">
								</a>
							</td>
							<td>
								<a runat="server" id="linkTerceiroTri" href="javascript:void(0)">
								</a>
							</td> 
							<td>
								<a runat="server" id="linkQuartoTri" href="javascript:void(0)">
								</a>
							</td>
						</tr>
						
						<tr>
							<td class="tituloCentral">
								Demonstrações Financeiras
							</td>

							<td >
								<a class="pdf" runat="server" id="linkArq_Demonstracoes1T" href="#" target="_blank">
									<asp:Literal ID="LtrArq_Demonstracoes1T" runat="server" />
								</a>
							</td>

							<td >
								<a class="pdf"  runat="server" id="linkArq_Demonstracoes2T" href="#" target="_blank">
									<asp:Literal ID="LtrArq_Demonstracoes2T" runat="server" />
								</a>
							</td>

							<td>
								<a class="pdf" runat="server" id="linkArq_Demonstracoes3T" href="#" target="_blank">
									<asp:Literal ID="LtrArq_Demonstracoes3T" runat="server" />
								</a>
							</td>

							<td>
								<a class="pdf" runat="server" id="linkArq_Demonstracoes4T" href="#" target="_blank">
									<asp:Literal ID="LtrArq_Demonstracoes4T" runat="server" />
								</a>
							</td>   
							
									<td runat="server" Visible="false">
									<a class="Release" runat="server" ID="HlkResultado" Visible="false">
											<asp:Literal runat="server" ID="LtrTextoResultado" />
									</a>
									</td>
						</tr>
						<tr>
							<td class="tituloCentral">
								DFP REDVCO SA
							</td>
							<td>
								<a class="pdf" runat="server" id="linkArq_DFP1T" href="#" target="_blank">
									<asp:Literal ID="LtrArq_DFP1T" runat="server" />
								</a>
							</td>
							<td >
								<a class="pdf"  runat="server" id="linkArq_DFP2T" href="#" target="_blank">
									<asp:Literal ID="LtrArq_DFP2T" runat="server" />
								</a>
							</td>
							<td >
								<a class="pdf"  runat="server" id="linkArq_DFP3T" href="#" target="_blank">
									<asp:Literal ID="LtrArq_DFP3T" runat="server" />
								</a>
							</td>
							<td >
								<a class="pdf"  runat="server" id="linkArq_DFP4T" href="#" target="_blank">
									<asp:Literal ID="LtrArq_DFP4T" runat="server" />
								</a>
							</td>
						</tr>
						
						<tr runat="server" visible="false">
							<td class="tituloCentral">
								<asp:Localize meta:resourcekey="Video" runat="server" />
							</td>
							<td>
								<a class="videoArquivo" runat="server" id="linkArq_Video1T" href="#" target="_blank">
									<asp:Literal ID="LtrArq_Video1T" runat="server" />
								</a>
								<a  class="video link" runat="server" id="linkVideoPrimeiroTri" href="javascript:" target="_blank">
									<asp:Literal ID="tituloVideoPrimeiroTri" runat="server" Visible="false" />
								</a>
							</td>
							<td >
								<a class="videoArquivo"  runat="server" id="linkArq_Video2T" href="#" target="_blank">
									<asp:Literal ID="LtrArq_Video2T" runat="server" />
								</a>
								<a class="video link"  runat="server" id="linkVideoSegundoTri" href="javascript:" target="_blank">
                                    <asp:Literal ID="tituloVideoSegundoTri" runat="server" Visible="false" />
                                </a>
							</td>
							<td >
								<a class="videoArquivo"  runat="server" id="linkArq_Video3T" href="#" target="_blank">
									<asp:Literal ID="LtrArq_Video3T" runat="server" />
								</a>
								<a class="video link"  runat="server" id="linkVideoTerceiroTri" href="javascript:" target="_blank">
                                    <asp:Literal ID="tituloVideoTerceiroTri" runat="server" Visible="false" />
                                </a>
							</td>
							<td >
								<a class="videoArquivo"  runat="server" id="linkArq_Video4T" href="#" target="_blank">
									<asp:Literal ID="LtrArq_Video4T" runat="server" />
								</a>
								<a class="video link"  runat="server" id="linkVideoQuartoTri" href="javascript:" target="_blank">
                                    <asp:Literal ID="tituloVideoQuartoTri" runat="server" Visible="false" />
                                </a>
							</td>
						</tr>
						
					</tbody>
				</table>
			</div>
			
		</ItemTemplate>
	</asp:Repeater>

            </div>
          </div>
        </div>
      </div>

</asp:Content>
