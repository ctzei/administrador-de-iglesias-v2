<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/MasterPages/Catalogos.master" AutoEventWireup="true" CodeBehind="CatalogoDePasos.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.CatalogoDePasos" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/Paginas/MasterPages/Catalogos.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphStores" runat="server">
        <ext:Store ID="StoreResultados" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Descripcion" />
                        <ext:RecordField Name="Categoria" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Store ID="StoreCategorias" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="Id" Mapping="PasoCategoriaId" />
                        <ext:RecordField Name="Descripcion" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFiltros" runat="server">
    <Z:ZPanelCatalogo ID="pnlFiltros" runat="server" Height="60px">
        <Items>
            <ext:ColumnLayout ID="colsFiltros" runat="server">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colFiltros1" runat="server" Layout="Form">
                            <Items>
                                <Z:ZNumberField 
                                    ID="filtroId" 
                                    runat="server"
                                    FieldLabel="Id"
                                    />
                                <Z:ZTextField 
                                    ID="filtroDescripcion" 
                                    runat="server"
                                    FieldLabel="Descripción"
                                    AnchorHorizontal="90%"
                                    />
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                    
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colFiltros2" runat="server" Layout="Form">
                            <Items>
                                <Z:ZMultiCombo
                                    ID="filtroCategoria" 
                                    runat="server"
                                    FieldLabel="Categoria"
                                    AnchorHorizontal="90%"
                                    StoreID="StoreCategorias"
                                    />
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Items>
    </Z:ZPanelCatalogo>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphEdicion" runat="server">
    <Z:ZPanelCatalogo ID="pnlEdicion" runat="server" Height="60px">
        <Items>
            <ext:ColumnLayout ID="colsEdicion" runat="server">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colEdicion1" runat="server" Layout="Form">
                            <Items>
                                <Z:ZNumberField 
                                    ID="registroId" 
                                    runat="server"
                                    FieldLabel="Id"
                                    ReadOnly="true"
                                    Text="-1"
                                    AllowBlank="false"
                                    /> 
                                <Z:ZTextField 
                                    ID="registroDescripcion" 
                                    runat="server"
                                    FieldLabel="Descripción"
                                    AnchorHorizontal="90%"
                                    AllowBlank="false"
                                    />
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                    
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colEdicion2" runat="server" Layout="Form">
                            <Items>
                                <Z:ZComboBox
                                    ID="registroCategoria" 
                                    runat="server"
                                    FieldLabel="Categoria"
                                    AnchorHorizontal="90%"
                                    StoreID="StoreCategorias"
                                    AllowBlank="false"
                                    /> 
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Items>
    </Z:ZPanelCatalogo>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphGridDeResultados" runat="server">
    <Z:ZGridCatalogo ID="GridDeResultados" runat="server" StoreID="StoreResultados">
        <ColumnModel ID="ColumnModel1" runat="server">
            <Columns>
                <ext:Column Header="ID" Width="40" DataIndex="Id" />
                <ext:Column Header="Descripción" Width="200" DataIndex="Descripcion" />
                <ext:Column Header="Categoria" Width="100" DataIndex="Categoria" />
            </Columns>
        </ColumnModel>
    </Z:ZGridCatalogo>
</asp:Content>

