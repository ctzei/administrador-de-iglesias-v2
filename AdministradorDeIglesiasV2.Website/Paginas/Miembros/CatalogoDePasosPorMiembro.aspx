<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/MasterPages/Catalogos.master" AutoEventWireup="true" CodeBehind="CatalogoDePasosPorMiembro.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.CatalogoDePasosPorMiembro" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/Paginas/MasterPages/Catalogos.master" %>

<%@ Register src="../UserControls/Miembros/FiltrosMiembros.ascx" tagname="FiltrosMiembros" tagprefix="uc1" %>
<%@ Register src="../UserControls/Miembros/ResultadoMiembros.ascx" tagname="ResultadoMiembros" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphStores" runat="server">
        <ext:Store ID="StoreCiclos" runat="server" IgnoreExtraFields="true" >
            <Reader>
                <ext:JsonReader IDProperty="Id">
                    <Fields>
                        <ext:RecordField Name="Id"/>
                        <ext:RecordField Name="Descripcion" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Store ID="StorePasos" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader IDProperty="Id">
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Descripcion" />
                        <ext:RecordField Name="PadreId" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Store ID="StoreCategorias" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader IDProperty="Id">
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Descripcion" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Store ID="StorePasosPorMiembro" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader IDProperty="Id">
                    <Fields>
                        <ext:RecordField Name="Id" ServerMapping="PasoId" />
                        <ext:RecordField Name="Paso" />
                        <ext:RecordField Name="CicloId" />
                        <ext:RecordField Name="Ciclo" />
                        <ext:RecordField Name="CategoriaId" />
                        <ext:RecordField Name="Categoria" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFiltros" runat="server">
   <uc1:FiltrosMiembros ID="Filtros" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphEdicion" runat="server">
    <Z:ZPanelCatalogo ID="pnlEdicion" runat="server" Height="300px">
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
                                    ID="registroEmail" 
                                    runat="server"
                                    FieldLabel="Email"
                                    AllowBlank="false"
                                    ReadOnly="true"
                                    />
                                <Z:ZCompositeField ID="ZCompositeField4" runat="server" FieldLabel="Nombre (s)">
                                    <Items>
                                        <Z:ZTextField 
                                            ID="registroPrimerNombre" 
                                            runat="server"
                                            FieldLabel="Primer Nombre"
                                            AllowBlank="false"
                                            ReadOnly="true"
                                            Flex="1"
                                            />
                                        <Z:ZTextField 
                                            ID="registroSegundoNombre" 
                                            runat="server"
                                            FieldLabel="Segundo Nombre"
                                            ReadOnly="true"
                                            Flex="1"
                                            />
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZTextField 
                                    ID="registroApellidoPaterno" 
                                    runat="server"
                                    FieldLabel="Apellido Paterno"
                                    AllowBlank="false"
                                    ReadOnly="true"
                                    />
                                <Z:ZTextField 
                                    ID="registroApellidoMaterno" 
                                    runat="server"
                                    FieldLabel="Apellido Materno"
                                    ReadOnly="true"
                                    />
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                    
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colEdicion2" runat="server" Layout="Form">
                            <Items>
                                <Z:ZGridPanel ID="registroPasos" Title="Pasos" runat="server" StoreID="StorePasosPorMiembro" Height="295">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:CommandColumn Width="25">
                                                <Commands>
                                                    <ext:GridCommand Icon="Delete" CommandName="Borrar">
                                                        <ToolTip Text="Borrar" />
                                                    </ext:GridCommand>
                                                </Commands>
                                            </ext:CommandColumn>
                                            <ext:Column Header="Categoria" Width="110" DataIndex="Categoria" />
                                            <ext:Column Header="Paso" Width="140" DataIndex="Paso" />
                                            <ext:Column Header="Ciclo" Width="75" DataIndex="Ciclo" />
                                        </Columns>
                                    </ColumnModel>
                                    <Buttons>
                                        <ext:Button ID="cmdNuevoPaso" runat="server" Text="Agregar..." Icon="Add" OnClientClick="#{wndNuevoPaso}.show();" AnchorHorizontal="100%"></ext:Button>
                                    </Buttons>
                                <Listeners>
                                    <Command Handler="comandoDelGridDePasos(command, record, #{registroPasos});" />
                                </Listeners>
                                </Z:ZGridPanel>
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Items>
    </Z:ZPanelCatalogo>

<ext:Window 
    ID="wndNuevoPaso" 
    runat="server" 
    Title="Agregar Paso..."  
    Height="155px" 
    Width="400px"
    Padding="5"
    Collapsible="false"
    Resizable="false"
    Draggable="false"
    Hidden="true"
    Modal="true"
    LabelWidth="75">
    <Items>
        <ext:FormPanel ID="pnlNuevoPaso" AnchorHorizontal="right" ButtonAlign="Center" HideBorders="true" BodyBorder="false">
            <Items>
                <Z:ZComboBox 
                    ID="registroNuevoCicloId" 
                    runat="server"
                    FieldLabel="Ciclo"
                    StoreID="StoreCiclos"
                    AllowBlank="false"
                    />
                <Z:ZComboBox 
                    ID="registroNuevoPasoId" 
                    runat="server"
                    FieldLabel="Paso"
                    StoreID="StorePasos"
                    AllowBlank="false">
                    <Listeners>
                        <Change Handler="cambiarCategoria();" />
                    </Listeners>
                </Z:ZComboBox>
                <Z:ZComboBox
                    ID="registroNuevoPasoCategoria" 
                    runat="server"
                    FieldLabel="Categoria"
                    StoreID="StoreCategorias"
                    ReadOnly="true"
                    />
            </Items>
            <Buttons>
                <ext:Button ID="cmdAgregarPaso" runat="server" Text="Aceptar" Width="100px">
                    <Listeners>
                        <Click Handler="if (agregarPaso()) {#{wndNuevoPaso}.hide();}" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:FormPanel>
    </Items>
</ext:Window>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphGridDeResultados" runat="server">
    <uc2:ResultadoMiembros ID="Resultados" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooterCatalogo" runat="server">
    <script src="../../Recursos/js/Paginas/Miembros/catalogoDePasosPorMiembro.js" type="text/javascript"></script>
    <div id="extra-upload-form"></div>
</asp:Content>




