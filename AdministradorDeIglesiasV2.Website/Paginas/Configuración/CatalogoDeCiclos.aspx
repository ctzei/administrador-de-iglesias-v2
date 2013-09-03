<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/MasterPages/Catalogos.master" AutoEventWireup="true" CodeBehind="CatalogoDeCiclos.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.CatalogoDeCiclos" %>
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
                        <ext:RecordField Name="FechaInicio" />
                        <ext:RecordField Name="FechaMax" />
                        <ext:RecordField Name="FechaFin" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFiltros" runat="server">
    <Z:ZPanelCatalogo ID="pnlFiltros" runat="server" Height="60px" LabelWidth="130">
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
                                <Z:ZCompositeField runat="server" FieldLabel="Fecha de Inicio/Fin">
                                    <Items>
                                        <Z:ZDateField  
                                            FieldLabel="Fecha de Inicio" 
                                            ID="filtroFechaInicio" 
                                            runat="server"
                                            Flex="1"
                                            />  
                                        <Z:ZDateField  
                                            FieldLabel="Fecha de Fin" 
                                            ID="filtroFechaFin" 
                                            runat="server"
                                            Flex="1"
                                            /> 
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZDateField  
                                    FieldLabel="Fecha Maxima" 
                                    ID="filtroFechaMax" 
                                    runat="server"
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
    <Z:ZPanelCatalogo ID="pnlEdicion" runat="server" Height="60px" LabelWidth="130">
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
                                    AllowBlank="false"
                                    />
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                    
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colEdicion2" runat="server" Layout="Form">
                            <Items>
                                <Z:ZCompositeField ID="ZCompositeField1" runat="server" FieldLabel="Fecha de Inicio/Fin">
                                    <Items>
                                     <Z:ZDateField  
                                        FieldLabel="Fecha de Inicio" 
                                        ID="registroFechaInicio" 
                                        runat="server"
                                        AllowBlank="false"
                                        Flex="1"
                                        />
                                    <Z:ZDateField  
                                        FieldLabel="Fecha de Fin" 
                                        ID="registroFechaFin" 
                                        runat="server"
                                        AllowBlank="false"
                                        Flex="1"
                                        />
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZDateField  
                                    FieldLabel="Fecha Maxima" 
                                    ID="registroFechaMax" 
                                    runat="server"
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
                <ext:Column Header="Fecha de Inicio" Width="100" DataIndex="FechaInicio" />
                <ext:Column Header="Fecha Maxima" Width="100" DataIndex="FechaMax" />
                <ext:Column Header="Fecha Fin" Width="100" DataIndex="FechaFin" />
            </Columns>
        </ColumnModel>
    </Z:ZGridCatalogo>
</asp:Content>

