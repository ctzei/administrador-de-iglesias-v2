<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/MasterPages/Catalogos.master" AutoEventWireup="true" CodeBehind="CatalogoDeCelulas.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.CatalogoDeCelulas" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/Paginas/MasterPages/Catalogos.master" %>

<asp:Content ID="Content0" ContentPlaceHolderID="cphHeadCatalogos" runat="server">
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no"/> 
    <script src="http://maps.google.com/maps/api/js?sensor=false" type="text/javascript"></script> 
    <script src="../../Recursos/js/extjs.gmaps.helpers.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphStores" runat="server">
        <ext:Store ID="StoreResultados" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader IDProperty="Id">
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Descripcion" />
                        <ext:RecordField Name="DiaSemanaDesc" />
                        <ext:RecordField Name="HoraDiaDesc" />
                        <ext:RecordField Name="Categoria" />
                        <ext:RecordField Name="Municipio" />
                        <ext:RecordField Name="Colonia" />
                        <ext:RecordField Name="Direccion" />
                        <ext:RecordField Name="Coordenadas" />
                        <ext:RecordField Name="RowColor" />
                    </Fields>
                </ext:JsonReader>
            </Reader>           
        </ext:Store>

        <ext:Store ID="StoreDiasDeLaSemana" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="Id"/>
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFiltros" runat="server">
    <Z:ZPanelCatalogo ID="pnlFiltros" runat="server" Height="115px">
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
                                <Z:ZComboBoxMunicipio
                                    ID="filtroMunicipio" 
                                    runat="server"
                                    />
                                <Z:ZCompositeField ID="ZCompositeField2" runat="server" FieldLabel="Colonia/Direccion">
                                    <Items>
                                         <Z:ZTextField 
                                            ID="filtroColonia" 
                                            runat="server"
                                            FieldLabel="Colonia"
                                            Flex="1"
                                            />
                                        <Z:ZTextField 
                                            ID="filtroDireccion" 
                                            runat="server"
                                            FieldLabel="Direccion"
                                            Flex="1"
                                            />  
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZMultiCombo
                                    ID="filtroCategoria"
                                    runat="server"
                                    FieldLabel="Categoria"
                                    StoreID="StoreCelulaCategorias"
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
    <Z:ZPanelCatalogo ID="pnlEdicion" runat="server" Height="115px">
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
                                <Z:ZCompositeField ID="ZCompositeField5" runat="server" FieldLabel="Día/Hora">
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
                                <Z:ZComboBoxMunicipio
                                    ID="registroMunicipio" 
                                    runat="server"
                                    AllowBlank="false">
                                    <Listeners>
                                        <Change Handler="limpiarCoordenadas();" />
                                    </Listeners>
                                </Z:ZComboBoxMunicipio>
                                <Z:ZCompositeField ID="ZCompositeField3" runat="server" FieldLabel="Colonia/Direccion">
                                    <Items>
                                        <Z:ZTextField 
                                            ID="registroColonia" 
                                            runat="server"
                                            FieldLabel="Colonia"
                                            AllowBlank="false"
                                            Flex="1">
                                            <Listeners>
                                                <Change Handler="limpiarCoordenadas();" />
                                            </Listeners>
                                        </Z:ZTextField>
                                        <Z:ZTextField 
                                            ID="registroDireccion" 
                                            runat="server"
                                            FieldLabel="Direccion"
                                            AllowBlank="false"
                                            Flex="1">
                                            <Listeners>
                                                <Change Handler="limpiarCoordenadas();" />
                                            </Listeners>
                                        </Z:ZTextField>
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZComboBox
                                    ID="registroCategoria" 
                                    runat="server"
                                    FieldLabel="Categoria"
                                    StoreID="StoreCelulaCategorias"
                                    AllowBlank="false"
                                    />
                                <Z:ZCompositeField ID="ZCompositeField4" runat="server">
                                    <Items>
                                        <Z:ZTextField 
                                            ID="registroCoordenadas" 
                                            runat="server"
                                            FieldLabel="Coordenadas"
                                            AllowBlank="false"
                                            ReadOnly="true"
                                            EmptyText="Sin coordenadas"
                                            FieldClass="small-text"
                                            Flex="1"
                                            />
                                        <ext:Button ID="cmdBuscarCoordenadas" runat="server" Icon="Map" Width="30px" ToolTip="Ver/Buscar coordenadas...">
                                            <Listeners>
                                                <Click Handler="buscarCordenadasClick();" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </Z:ZCompositeField>
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Items>
    </Z:ZPanelCatalogo>

    <ext:Window 
        ID="wndMapa" 
        runat="server" 
        Title="Ubicacion de la Celula"  
        Height="480px" 
        Width="640px"
        Padding="5"
        Collapsible="false"
        Resizable="false"
        Draggable="false"
        Hidden="true"
        Modal="true">
        <Content>
            <div id="mapa"></div>
        </Content>
    </ext:Window>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphGridDeResultados" runat="server">
    <Z:ZGridCatalogo ID="GridDeResultados" runat="server" StoreID="StoreResultados" ColorearFilas="True">
        <ColumnModel ID="ColumnModel1" runat="server">
            <Columns>
                <ext:Column Header="ID" Width="40" DataIndex="Id" />
                <ext:Column Header="Descripción" Width="200" DataIndex="Descripcion" />
                <ext:Column Header="Día" Width="55" DataIndex="DiaSemanaDesc" />
                <ext:Column Header="Hora" Width="45" DataIndex="HoraDiaDesc" />
                <ext:Column Header="Categoria" Width="100" DataIndex="Categoria" />
                <ext:Column Header="Municipio" Width="100" DataIndex="Municipio" />
                <ext:Column Header="Colonia" Width="100" DataIndex="Colonia" />
                <ext:Column Header="Direccion" Width="100" DataIndex="Direccion" />
                <ext:Column Header="Coordenadas" Width="100" DataIndex="Coordenadas" />
            </Columns>
        </ColumnModel>
    </Z:ZGridCatalogo>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooterCatalogo" runat="server">
    <script src="../../Recursos/js/Paginas/Celulas/catalogoDeCelulas.js" type="text/javascript"></script>
</asp:Content>

