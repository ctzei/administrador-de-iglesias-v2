<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FiltrosMiembros.ascx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.UserControls.FiltrosMiembros" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>

<ext:Store ID="StoreResultados" runat="server" IgnoreExtraFields="true">
    <Reader>
        <ext:JsonReader IDProperty="Id">
            <Fields>
                <ext:RecordField Name="GUID" />
                <ext:RecordField Name="Id" />
                <ext:RecordField Name="Email" />
                <ext:RecordField Name="PrimerNombre" />
                <ext:RecordField Name="SegundoNombre" />
                <ext:RecordField Name="ApellidoPaterno" />
                <ext:RecordField Name="ApellidoMaterno" />
                <ext:RecordField Name="Celula" />
                <ext:RecordField Name="EstadoCivil" />
                <ext:RecordField Name="Municipio" />
                <ext:RecordField Name="Colonia" />
                <ext:RecordField Name="TelCasa" />
                <ext:RecordField Name="TelMovil" />
                <ext:RecordField Name="TelTrabajo" />
                <ext:RecordField Name="Nacimiento" />
                <ext:RecordField Name="AsisteIglesia" Type="Boolean" />
                <ext:RecordField Name="Genero" />
                <ext:RecordField Name="RowColor" />
            </Fields>
        </ext:JsonReader>
    </Reader>            
</ext:Store>

<ext:Store ID="StoreCelulas" runat="server" IgnoreExtraFields="true">
    <Reader>
        <ext:JsonReader>
            <Fields>
                <ext:RecordField Name="Id" Mapping="CelulaId" />
                <ext:RecordField Name="Descripcion" />
            </Fields>
        </ext:JsonReader>
    </Reader>            
</ext:Store>

<ext:Store ID="StoreEstadosCiviles" runat="server" IgnoreExtraFields="true">
    <Reader>
        <ext:JsonReader>
            <Fields>
                <ext:RecordField Name="Id" />
                <ext:RecordField Name="Descripcion" />
            </Fields>
        </ext:JsonReader>
    </Reader>            
</ext:Store>

<ext:Store ID="StoreGeneros" runat="server" IgnoreExtraFields="true">
    <Reader>
        <ext:JsonReader>
            <Fields>
                <ext:RecordField Name="Id" />
                <ext:RecordField Name="Descripcion" />
            </Fields>
        </ext:JsonReader>
    </Reader>            
</ext:Store>

<Z:ZPanelCatalogo ID="pnlFiltros" runat="server" Height="190px">
    <Items>
        <ext:ColumnLayout ID="colsFiltros" runat="server">
            <Columns>
                <ext:LayoutColumn ColumnWidth="0.5">
                    <ext:Container ID="colFiltros1" runat="server" Layout="Form" LabelWidth="125">
                        <Items>
                            <Z:ZNumberField 
                                ID="filtroId" 
                                runat="server"
                                FieldLabel="Id"
                                />
                            <Z:ZTextField 
                                ID="filtroEmail" 
                                runat="server"
                                FieldLabel="Email"
                                />
                            <Z:ZCompositeField ID="ZCompositeField4" runat="server" FieldLabel="Nombre (s)">
                                <Items>
                                    <Z:ZTextField 
                                        ID="filtroPrimerNombre" 
                                        runat="server"
                                        FieldLabel="Primer Nombre"
                                        Flex="1"
                                        />
                                    <Z:ZTextField 
                                        ID="filtroSegundoNombre" 
                                        runat="server"
                                        FieldLabel="Segundo Nombre"
                                        Flex="1"
                                        />
                                </Items>
                            </Z:ZCompositeField>
                            <Z:ZCompositeField ID="ZCompositeField1" runat="server" FieldLabel="Apellidos">
                                <Items>
                                    <Z:ZTextField 
                                        ID="filtroApellidoPaterno" 
                                        runat="server"
                                        FieldLabel="Apellido Paterno"
                                        Flex="1"
                                        />
                                    <Z:ZTextField 
                                        ID="filtroApellidoMaterno" 
                                        runat="server"
                                        FieldLabel="Apellido Materno"
                                        Flex="1"
                                        />
                                </Items>
                            </Z:ZCompositeField>
                            <Z:ZMultiCombo 
                                ID="filtroCelula" 
                                runat="server" 
                                FieldLabel="Célula que Asiste"
                                StoreID="StoreCelulas"
                                />
                            <Z:ZMultiCombo 
                                ID="filtroGenero" 
                                runat="server" 
                                FieldLabel="Género"
                                StoreID="StoreGeneros"
                                />

                            <ext:CheckboxGroup FieldLabel="Mostrar solo" runat="server" Width="170">
                                <Items>
                                    <Z:ZCheckbox 
                                        ID="filtroBorrado" 
                                        runat="server"
                                        BoxLabel="Borrados"
                                        Checked="false"
                                        />
                                    <Z:ZCheckbox 
                                        ID="filtroSinCelula" 
                                        runat="server" 
                                        BoxLabel="Sin Célula"
                                        Checked="false"
                                        />
                                </Items>
                            </ext:CheckboxGroup>

                            
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
                            <Z:ZCompositeField ID="ZCompositeField2" runat="server" FieldLabel="Colonia/Dirección">
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
                                        FieldLabel="Dirección"
                                        Flex="1"
                                        />
                                </Items>
                            </Z:ZCompositeField>
                            <Z:ZDateField 
                                ID="filtroFechaDeNacimiento" 
                                runat="server"
                                FieldLabel="Cumpleaños"
                                />
                            <Z:ZMultiCombo 
                                ID="filtroEstadoCivil" 
                                runat="server"
                                FieldLabel="Estado Civil"
                                StoreID="StoreEstadosCiviles"
                                />
                            <Z:ZNumberField 
                                ID="filtroTel" 
                                runat="server"
                                FieldLabel="Tel"
                                />
                        </Items>
                    </ext:Container>
                </ext:LayoutColumn>
            </Columns>
        </ext:ColumnLayout>
    </Items>
</Z:ZPanelCatalogo>
