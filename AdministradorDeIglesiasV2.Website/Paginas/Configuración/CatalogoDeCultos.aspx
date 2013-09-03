<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/MasterPages/Catalogos.master" AutoEventWireup="true" CodeBehind="CatalogoDeCultos.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.CatalogoDeCultos" %>
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
                    <ext:RecordField Name="DiaSemanaDesc" />
                    <ext:RecordField Name="HoraDiaDesc" />
                </Fields>
            </ext:JsonReader>
        </Reader>            
    </ext:Store>

    <ext:Store ID="StoreDiasDeLaSemana" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Descripcion" />
                </Fields>
            </ext:JsonReader>
        </Reader>            
    </ext:Store>

    <ext:Store ID="StoreHorasDelDia" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Id" />
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
                                <Z:ZCompositeField ID="ZCompositeField1" runat="server" FieldLabel="Día/Hora">
                                    <Items>
                                        <Z:ZMultiCombo
                                            ID="filtroDiaDeLaSemana" 
                                            runat="server"
                                            FieldLabel="Día de la Semana"
                                            StoreID="StoreDiasDeLaSemana"
                                            Flex="1"
                                            />
                                        <Z:ZMultiCombo
                                            ID="filtroHoraDelDia" 
                                            runat="server"
                                            FieldLabel="Hora del Día"
                                            StoreID="StoreHorasDelDia"
                                            Flex="1"
                                            />
                                    </Items>
                                </Z:ZCompositeField>
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                    
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colFiltros2" runat="server" Layout="Form">
                            <Items>
                                <Z:ZTextField 
                                    ID="filtroDescripcion" 
                                    runat="server"
                                    FieldLabel="Descripción"
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
                                <Z:ZCompositeField ID="ZCompositeField2" runat="server" FieldLabel="Día/Hora">
                                    <Items>
                                        <Z:ZComboBox
                                            ID="registroDiaDeLaSemana" 
                                            runat="server"
                                            FieldLabel="Día de la Semana"
                                            StoreID="StoreDiasDeLaSemana"
                                            AllowBlank="false"
                                            Flex="1"
                                            />
                                        <Z:ZComboBox
                                            ID="registroHoraDelDia" 
                                            runat="server"
                                            FieldLabel="Hora del Día"
                                            StoreID="StoreHorasDelDia"
                                            AllowBlank="false"
                                            Flex="1"
                                            />
                                    </Items>
                                </Z:ZCompositeField>
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                    
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colEdicion2" runat="server" Layout="Form">
                            <Items>
                                <Z:ZTextField 
                                    ID="registroDescripcion" 
                                    runat="server"
                                    FieldLabel="Descripción"
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
                <ext:Column Header="Día" Width="100" DataIndex="DiaSemanaDesc" />
                <ext:Column Header="Hora" Width="100" DataIndex="HoraDiaDesc" />
            </Columns>
        </ColumnModel>
    </Z:ZGridCatalogo>
</asp:Content>

