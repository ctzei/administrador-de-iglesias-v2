<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="BuscadorDeCelulas.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.BuscadorDeCelulas" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no"/> 
    <script src="http://maps.google.com/maps/api/js?sensor=false" type="text/javascript"></script> 
    <script src="../../Recursos/js/extjs.gmaps.helpers.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

        <ext:Store ID="StoreCelulaCategorias" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Descripcion" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Viewport ID="Viewport" runat="server" Layout="Anchor">
            <Items>

                <Z:ZPanelCatalogo ID="pnlFiltros" runat="server" AutoWidth="true" AnchorHorizontal="right" Height="105px" Layout="Form" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false" LabelWidth="125">
                    <Items>
                        <ext:ColumnLayout ID="colsFiltros" runat="server">
                            <Columns>
                                <ext:LayoutColumn ColumnWidth="0.5">
                                    <ext:Container ID="colFiltros1" runat="server" Layout="Form">
                                        <Items>
                                            <Z:ZComboBoxMunicipio
                                                ID="filtroMunicipio" 
                                                runat="server" 
                                                AllowBlank="false">
                                            </Z:ZComboBoxMunicipio>
                                            <Z:ZCompositeField ID="CompositeField1" runat="server" FieldLabel="Colonia/Direccion">
                                                <Items>
                                                    <Z:ZTextField 
                                                        ID="filtroColonia" 
                                                        runat="server"
                                                        FieldLabel="Colonia"
                                                        AllowBlank="false"
                                                        Flex="1">
                                                    </Z:ZTextField>
                                                    <Z:ZTextField 
                                                        ID="filtroDireccion" 
                                                        runat="server"
                                                        FieldLabel="Direccion"
                                                        AllowBlank="false"
                                                        Flex="1">
                                                    </Z:ZTextField>
                                                </Items>
                                            </Z:ZCompositeField>
                                        </Items>
                                    </ext:Container>
                                </ext:LayoutColumn>
                    
                                <ext:LayoutColumn ColumnWidth="0.5">
                                    <ext:Container ID="colFiltros2" runat="server" Layout="Form">
                                        <Items>
                                            <Z:ZMultiCombo
                                                ID="filtroCategoria"
                                                FieldLabel="Categoria"
                                                runat="server" 
                                                StoreID="StoreCelulaCategorias"
                                                AllowBlank="true">
                                            </Z:ZMultiCombo>
                                            <Z:ZSpinnerField
                                                ID="filtroKilometros" 
                                                MaxValue="100"
                                                MinValue="1"
                                                Text="5"
                                                FieldLabel="Kms. a la redonda"
                                                AllowBlank="false"
                                                runat="server">
                                            </Z:ZSpinnerField>
                                        </Items>
                                    </ext:Container>
                                </ext:LayoutColumn>
                            </Columns>
                        </ext:ColumnLayout>
                    </Items>
                     <Buttons>
                        <ext:Button ID="cmdBuscar" runat="server" Text="Buscar..." >
                            <Listeners>
                                <Click Handler="buscarCelulasClick();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </Z:ZPanelCatalogo>
                <ext:Panel ID="grdCelulas" runat="server" AnchorHorizontal="right" AnchorVertical="-105" Title="Celulas Cercanas" AutoWidth="true" />
            </Items>
        </ext:Viewport>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
    <script src="../../Recursos/js/Paginas/Celulas/buscadorDeCelulas.js" type="text/javascript"></script>
</asp:Content>

