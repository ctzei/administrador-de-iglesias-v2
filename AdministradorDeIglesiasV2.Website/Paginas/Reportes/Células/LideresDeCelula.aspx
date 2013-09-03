<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="LideresDeCelula.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Reportes.Celulas.LideresDeCelula" %>
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

        <ext:Store ID="StoreLideres" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader IDProperty="Id">
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Nombre" />
                        <ext:RecordField Name="Email" />
                        <ext:RecordField Name="TelMovil" />
                        <ext:RecordField Name="TelCasa" />
                        <ext:RecordField Name="TelTrabajo" />
                        <ext:RecordField Name="CelulaId" />
                        <ext:RecordField Name="Celula" />
                        <ext:RecordField Name="Genero" />
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
                        <ext:Button ID="cmdObtenerLideresDeCelula" runat="server" Text="Obtener Líderes de Célula" >
                            <Listeners>
                                <Click Handler="obtenerLideresDeCelula();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>

                <Z:ZGridPanel ID="gridLideres" runat="server" Title="Líderes de Célula" StoreID="StoreLideres" AnchorHorizontal="right"  AnchorVertical="-70" AutoWidth="true" >
                    <ColumnModel>
                        <Columns>
                            <ext:Column Header="Id" Width="25" DataIndex="Id" />
                            <ext:Column Header="Nombre" Width="150" DataIndex="Nombre" />
                            <ext:Column Header="Email" Width="100" DataIndex="Email" />
                            <ext:Column Header="Tel. Móvil" Width="60" DataIndex="TelMovil" />
                            <ext:Column Header="Tel. Casa" Width="60" DataIndex="TelCasa" />
                            <ext:Column Header="Tel. Trabajo" Width="60" DataIndex="TelTrabajo" />
                            <ext:Column Header="Célula Id" Width="60" DataIndex="CelulaId" />
                            <ext:Column Header="Célula" Width="60" DataIndex="Celula" />
                            <ext:Column Header="Género" Width="60" DataIndex="Genero" />
                        </Columns>
                    </ColumnModel>
                </Z:ZGridPanel>

            </Items>
        </ext:Viewport>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooter" runat="server">
    <script src="<%= ResolveUrl("~/Recursos/js/Paginas/Reportes/Celulas/lideresDeCelula.js")%>" type="text/javascript"></script>
</asp:Content>

