<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="ArbolDeCelulas.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.ArbolDeCelulas" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

        <ext:Store ID="StoreCelulas" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Descripcion" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Viewport ID="ViewportOrganigrama" runat="server" Layout="Anchor">
            <Items>
                <ext:FormPanel ID="pnlFiltros" runat="server" LabelWidth="60" AnchorHorizontal="right" Height="70px" Layout="Form" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false">
                    <Items>
                        <Z:ZComboBox
                            FieldLabel="Celula"
                            ID="cboCelula" 
                            runat="server" 
                            StoreID="StoreCelulas"
                            Resizable="true"
                            AnchorHorizontal="98%"
                            AllowBlank="false">
                            <Listeners>
                                <Change Handler="#{pnlOrganigrama}.getRootNode().removeChildren();" />
                            </Listeners>
                        </Z:ZComboBox>
                    </Items>
                    <Buttons>
                        <ext:Button ID="cmdCargar" runat="server" Text="Cargar" >
                            <Listeners>
                                <Click Handler="if (#{pnlFiltros}.getForm().isValid()) {refreshTree(#{pnlOrganigrama}, Ext.net.DirectMethods.CargarClick, function(){Ext.getCmp('#{pnlOrganigrama}').expandAll();}); return true;} else {return false;}" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>

                <ext:Window ID="wndDetallesDeLaCelula" runat="server" Collapsible="false" Width="640" Height="480" Hidden="true" Title="Detalles" Modal="true" Resizable="false" >
                    <AutoLoad Mode="IFrame" ShowMask="true" MaskMsg="Cargando..."></AutoLoad>
                </ext:Window>

                <Z:ZTreePanel ID="pnlOrganigrama" runat="server" AnchorHorizontal="right" AnchorVertical="-70" Title="Arbol de Celulas:" AutoWidth="true" >
                    <Root>
                        <ext:TreeNode></ext:TreeNode>
                    </Root>
                    <Listeners>
                        <Click Handler="ADMI.VerDetallesDeCelula(node.id);" />
                    </Listeners>
                </Z:ZTreePanel>
            </Items>
        </ext:Viewport>
</asp:Content>
