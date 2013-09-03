<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="UltimasAsistencias.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Reportes.Celulas.UltimasAsistencias" %>

<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
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

    <ext:Store ID="StoreAsistencias" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Descripcion" />
                    <ext:RecordField Name="Fecha" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <ext:Viewport ID="ViewportMain" runat="server" Layout="Anchor">
        <Items>
            <ext:FormPanel ID="pnlFiltros" runat="server" AnchorHorizontal="right" Height="70px" Layout="Form" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false" LabelWidth="150">
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
                    <ext:Button ID="cmdObtenerUltimasAsistenciasPorCelula" runat="server" Text="Obtener Últimas Asistencias por Célula">
                        <Listeners>
                            <Click Handler="ObtenerUltimasAsistenciasPorCelula();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>

            <Z:ZGridPanel ID="gridAsistencias" runat="server" Title="Últimas Asistencias por Célula" StoreID="StoreAsistencias" AnchorHorizontal="right" AnchorVertical="-70" AutoWidth="true">
                <ColumnModel>
                    <Columns>
                        <ext:Column Header="Id" Width="25" />
                        <ext:Column Header="Descripcion" Width="300" />
                        <ext:Column Header="Fecha" Width="50">
                            <Renderer Fn="Ext.Renderers.IsoDateSimple" />
                        </ext:Column>
                    </Columns>
                </ColumnModel>
            </Z:ZGridPanel>

        </Items>
    </ext:Viewport>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooter" runat="server">
    <script src="<%= ResolveUrl("~/Recursos/js/Paginas/Reportes/Celulas/ultimasAsistencias.js")%>" type="text/javascript"></script>
</asp:Content>

