<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="BorrarCelulaPermanentemente.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.BorrarCelulaPermanentemente" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

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

        <ext:Viewport ID="ViewportAsistencias" runat="server" Layout="Anchor">
            <Items>

                <ext:Panel runat="server" PaddingSummary="5px 5px 15px" BodyBorder="false" MinHeight="50px" LabelStyle="font-weight:bold;">
                    <Items>
                        <ext:Label LabelStyle="font-weight:bold;" StyleSpec="font-weight:bold;" runat="server" Text="Borrar una célula permanente significa borrar todas las asistencias y cancelaciones registradas, los miembros y la célula en si (incluyendo todos los datos de las células hijas al tratarse de una célula cabeza de red). Este procedimiento NO se puede revertir y la información será completamente eliminada."></ext:Label>
                    </Items>
                </ext:Panel>

                <ext:FormPanel ID="pnlFiltros" runat="server" AnchorHorizontal="right" Height="200px" Layout="Form" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false" LabelWidth="150">
                    <Items>
                        <Z:ZComboBox
                            FieldLabel="Célula"
                            ID="cboCelula" 
                            runat="server" 
                            StoreID="StoreCelulas"
                            Resizable="true"
                            AllowBlank="false"
                            Flex="1">
                        </Z:ZComboBox>
                    </Items>
                    <Buttons>
                        <ext:Button ID="cmdBorrarCelula" runat="server" Text="Borrar Célula" >
                            <Listeners>
                                <Click Handler="borrarCelula();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooter" runat="server">
    <script src="<%= ResolveUrl("~/Recursos/js/Paginas/Celulas/borrarCelulaPermanentemente.js")%>" type="text/javascript"></script>
</asp:Content>

