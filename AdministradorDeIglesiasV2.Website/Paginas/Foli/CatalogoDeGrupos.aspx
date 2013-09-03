<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/MasterPages/Catalogos.master" AutoEventWireup="true" CodeBehind="CatalogoDeGrupos.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Foli.CatalogoDeGrupos" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/Paginas/MasterPages/Catalogos.master" %>

<%@ Register src="../UserControls/Buscador.ascx" tagname="Buscador" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphStores" runat="server">
        <ext:Store ID="StoreResultados" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Descripcion" />
                        <ext:RecordField Name="FechaInicioMod1" />
                        <ext:RecordField Name="FechaInicioMod2" />
                        <ext:RecordField Name="FechaInicioMod3" />
                        <ext:RecordField Name="FechaInicioMod4" />
                        <ext:RecordField Name="FechaFin" />
                        <ext:RecordField Name="DiaSemana" />
                        <ext:RecordField Name="HoraDia" />
                        <ext:RecordField Name="Ciclo" />
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

        <ext:Store ID="StoreCiclos" runat="server" IgnoreExtraFields="true">
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
    <Z:ZPanelCatalogo ID="pnlFiltros" runat="server" Height="80px">
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
                                <Z:ZCompositeField ID="ZCompositeField2" runat="server" FieldLabel="Día/Hora">
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
                                <Z:ZCompositeField runat="server" FieldLabel="Fechas de Inicio">
                                    <Items>
                                        <Z:ZDateField  
                                            FieldLabel="Fecha de Inicio (Módulo 1)" 
                                            ID="filtroFechaInicioModulo1" 
                                            runat="server"
                                            Flex="1"
                                            />  
                                        <Z:ZDateField  
                                            FieldLabel="Fecha de Inicio (Módulo 2)" 
                                            ID="filtroFechaInicioModulo2" 
                                            runat="server"
                                            Flex="1"
                                            /> 
                                        <Z:ZDateField  
                                            FieldLabel="Fecha de Inicio (Módulo 3)" 
                                            ID="filtroFechaInicioModulo3" 
                                            runat="server"
                                            Flex="1"
                                            /> 
                                        <Z:ZDateField  
                                            FieldLabel="Fecha de Inicio (Módulo 4)" 
                                            ID="filtroFechaInicioModulo4" 
                                            runat="server"
                                            Flex="1"
                                            /> 
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZDateField  
                                    FieldLabel="Fecha de Fin" 
                                    ID="filtroFechaFin" 
                                    runat="server"
                                    />
                                <Z:ZMultiCombo
                                    ID="filtroCiclo" 
                                    runat="server"
                                    FieldLabel="Ciclo"
                                    StoreID="StoreCiclos"
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
    <Z:ZPanelCatalogo ID="pnlEdicion" runat="server" Height="420px">
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
                                    MaxLength="25"
                                    />
                                <Z:ZCompositeField ID="ZCompositeField3" runat="server" FieldLabel="Día/Hora">
                                    <Items>
                                        <Z:ZComboBox
                                            ID="registroDiaDeLaSemana" 
                                            runat="server"
                                            FieldLabel="Día de la Semana"
                                            StoreID="StoreDiasDeLaSemana"
                                            Flex="1"
                                            AllowBlank="false"
                                            />
                                        <Z:ZComboBox
                                            ID="registroHoraDelDia" 
                                            runat="server"
                                            FieldLabel="Hora del Día"
                                            StoreID="StoreHorasDelDia"
                                            Flex="1"
                                            AllowBlank="false"
                                            /> 
                                    </Items>
                                </Z:ZCompositeField>
                                <ext:Container ID="Container2" runat="server" Layout="Form">
                                    <Content>
                                        <br />
                                        <uc3:Buscador ID="BuscadorMaestros" runat="server" TipoDeBusqueda="Miembro" Titulo="Maestros" Altura="330" />  
                                    </Content>
                                </ext:Container>
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                    
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colEdicion2" runat="server" Layout="Form">
                            <Items>
                                <Z:ZCompositeField ID="ZCompositeField1" runat="server" FieldLabel="Fechas de Inicio">
                                    <Items>
                                        <Z:ZDateField  
                                            FieldLabel="Fecha de Inicio (Módulo 1)" 
                                            ID="registroFechaInicioModulo1" 
                                            runat="server"
                                            Flex="1"
                                            AllowBlank="false"
                                            />  
                                        <Z:ZDateField  
                                            FieldLabel="Fecha de Inicio (Módulo 2)" 
                                            ID="registroFechaInicioModulo2" 
                                            runat="server"
                                            Flex="1"
                                            AllowBlank="false"
                                            /> 
                                        <Z:ZDateField  
                                            FieldLabel="Fecha de Inicio (Módulo 3)" 
                                            ID="registroFechaInicioModulo3" 
                                            runat="server"
                                            Flex="1"
                                            AllowBlank="false"
                                            /> 
                                        <Z:ZDateField  
                                            FieldLabel="Fecha de Inicio (Módulo 4)" 
                                            ID="registroFechaInicioModulo4" 
                                            runat="server"
                                            Flex="1"
                                            AllowBlank="false"
                                            /> 
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZDateField  
                                    FieldLabel="Fecha de Fin" 
                                    ID="registroFechaFin" 
                                    runat="server"
                                    AllowBlank="false"
                                    />
                                <Z:ZComboBox
                                    ID="registroCiclo" 
                                    runat="server"
                                    FieldLabel="Ciclo"
                                    StoreID="StoreCiclos"
                                    AllowBlank="false"
                                    />
                                <ext:Container ID="Container1" runat="server" Layout="Form">
                                    <Content>
                                        <br />
                                        <uc3:Buscador ID="BuscadorAlumnos" runat="server" TipoDeBusqueda="Miembro" Titulo="Alumnos" Altura="330" />  
                                    </Content>
                                </ext:Container>
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
                <ext:Column Header="Fecha (Mod 1)" Width="100" DataIndex="FechaInicioMod1" />
                <ext:Column Header="Fecha (Mod 2)" Width="100" DataIndex="FechaInicioMod2" />
                <ext:Column Header="Fecha (Mod 3)" Width="100" DataIndex="FechaInicioMod3" />
                <ext:Column Header="Fecha (Mod 4)" Width="100" DataIndex="FechaInicioMod4" />
                <ext:Column Header="Fecha Fin" Width="100" DataIndex="FechaFin" />
                <ext:Column Header="Día" Width="100" DataIndex="DiaSemana" />
                <ext:Column Header="Hora" Width="100" DataIndex="HoraDia" />
                <ext:Column Header="Ciclo" Width="100" DataIndex="Ciclo" />
            </Columns>
        </ColumnModel>
    </Z:ZGridCatalogo>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooterCatalogo" runat="server">
    <style type="text/css">
        body {overflow-y:scroll!important;}
    </style>
</asp:Content>


