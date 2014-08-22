<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/MasterPages/CatalogosComplejos.master" AutoEventWireup="true" CodeBehind="CatalogoDeBoletas.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.CatalogoDeBoletas" %>

<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ MasterType VirtualPath="~/Paginas/MasterPages/CatalogosComplejos.master" %>

<%@ Register Src="../UserControls/Miembros/FiltrosMiembros.ascx" TagName="FiltrosMiembros" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/Miembros/ResultadoMiembros.ascx" TagName="ResultadoMiembros" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/BuscadorSimple.ascx" TagName="BuscadorSimple" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphStores" runat="server">

    <ext:Store ID="StoreResultados" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Email" />
                    <ext:RecordField Name="PrimerNombre" />
                    <ext:RecordField Name="SegundoNombre" />
                    <ext:RecordField Name="ApellidoPaterno" />
                    <ext:RecordField Name="ApellidoMaterno" />
                    <ext:RecordField Name="Estatus" />
                    <ext:RecordField Name="Genero" />
                    <ext:RecordField Name="InvitadoPorMiembroId" />
                    <ext:RecordField Name="Culto" />
                    <ext:RecordField Name="FechaDeCulto" />
                    <ext:RecordField Name="RazonDeVisita" />
                    <ext:RecordField Name="Municipio" />
                    <ext:RecordField Name="Colonia" />
                    <ext:RecordField Name="Direccion" />
                    <ext:RecordField Name="Nacimiento" />
                    <ext:RecordField Name="Edad" />
                    <ext:RecordField Name="EstadoCivil" />
                    <ext:RecordField Name="AsignadaACelula" />
                    <ext:RecordField Name="AsignadaAMiembro" />
                    <ext:RecordField Name="TelCasa" />
                    <ext:RecordField Name="TelMovil" />
                    <ext:RecordField Name="TelTrabajo" />
                    <ext:RecordField Name="Observaciones" />
                    <ext:RecordField Name="Categoria" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <ext:Store ID="StoreReportes" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Reporte" />
                    <ext:RecordField Name="Fecha" />
                    <ext:RecordField Name="ReportadoPorMiembro" />
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

    <ext:Store ID="StoreCultos" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Descripcion" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <ext:Store ID="StoreRazonesDeVisita" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Descripcion" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <ext:Store ID="StoreEstatus" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Key" />
                    <ext:RecordField Name="Value" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <ext:Store ID="StoreCategorias" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Descripcion" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <!-- PopUps -->

    <Z:ZModalWindow
        ID="wndAgregarReporte"
        runat="server"
        Title="Agregar Reporte"
        Width="500px"
        Height="150px">
        <Items>
            <Z:ZPanel ID="pnlAgregarReporte" runat="server" BodyBorder="false" Layout="FitLayout">
                <Items>
                    <Z:ZTextArea
                        ID="registroReporteNuevo"
                        runat="server"
                        AllowBlank="false"
                        MinLength="10" />
                </Items>
                <Buttons>
                    <ext:Button ID="btnAgregarReporte" Text="Agregar Reporte">
                        <Listeners>
                            <Click Handler="if (#{pnlAgregarReporte}.getForm().isValid()) {Ext.net.DirectMethods.AgregarReporte();}" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </Z:ZPanel>
        </Items>
    </Z:ZModalWindow>

    <Z:ZModalWindow
        ID="wndCrearMiembro"
        runat="server"
        Title="Crear Miembro de Boleta Cerrada"
        Width="400px"
        Height="175px"
        Closable="false">
        <Items>
            <Z:ZPanel ID="pnlCrearMiembro" runat="server" BodyBorder="false" Layout="FitLayout" PaddingSummary="2px 0px 10px 0px">
                <Items>
                    <ext:Label
                        Margins="20px"
                        Text="La boleta a sido cerrada correctamente, sin embargo no se ha creado/asignado ningún miembro en alguna célula, relacionado con los datos de dicha boleta. ¿Crear y asignar miembro a alguna célula?">
                    </ext:Label>
                    <ext:Hidden
                        ID="registroBoletaId"
                        runat="server">
                    </ext:Hidden>
                    <ext:Container runat="server" Layout="Form">
                        <Content>
                            <br />
                            <uc3:BuscadorSimple ID="registroCelulaDeMiembroCreado" runat="server" LabelWidth="155" FieldLabel="Asignar a Célula" TipoDeObjeto="Celula" />
                        </Content>
                    </ext:Container>
                </Items>
                <Buttons>
                    <ext:Button ID="btnCrearMiembro" Text="Crear/Asignar Miembro" Icon="Add">
                        <Listeners>
                            <Click Handler="if (#{pnlCrearMiembro}.getForm().isValid()) {Ext.net.DirectMethods.CrearMiembro();}" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="btnNoCrearMiembro" Text="Cancelar" Icon="Cancel">
                        <Listeners>
                            <Click Handler="Ext.Msg.confirm('', '¿Continuar sin crear/asignar un nuevo miembro?', function(btn) {if(btn == 'yes'){ #{wndCrearMiembro}.hide(); }}, this);" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </Z:ZPanel>
        </Items>
    </Z:ZModalWindow>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFiltros" runat="server">
    <Z:ZPanelCatalogo ID="pnlFiltros" runat="server" Height="295px">
        <Items>
            <ext:ColumnLayout ID="colsFiltros" runat="server">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colFiltros1" runat="server" Layout="Form" LabelWidth="155">
                            <Items>
                                <Z:ZNumberField
                                    ID="filtroId"
                                    runat="server"
                                    FieldLabel="Id" />
                                <Z:ZTextField
                                    ID="filtroEmail"
                                    runat="server"
                                    FieldLabel="Email" />
                                <Z:ZCompositeField ID="ZCompositeField6" runat="server" FieldLabel="Nombre (s)">
                                    <Items>
                                        <Z:ZTextField
                                            ID="filtroPrimerNombre"
                                            runat="server"
                                            FieldLabel="Primer Nombre"
                                            Flex="1" />
                                        <Z:ZTextField
                                            ID="filtroSegundoNombre"
                                            runat="server"
                                            FieldLabel="Segundo Nombre"
                                            Flex="1" />
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZCompositeField ID="ZCompositeField7" runat="server" FieldLabel="Apellidos">
                                    <Items>
                                        <Z:ZTextField
                                            ID="filtroApellidoPaterno"
                                            runat="server"
                                            FieldLabel="Apellido Paterno"
                                            Flex="1" />
                                        <Z:ZTextField
                                            ID="filtroApellidoMaterno"
                                            runat="server"
                                            FieldLabel="Apellido Materno"
                                            Flex="1" />
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZMultiCombo
                                    ID="filtroGenero"
                                    runat="server"
                                    FieldLabel="Género"
                                    StoreID="StoreGeneros" />
                                <ext:Container ID="Container5" runat="server" Layout="Form">
                                    <Content>
                                        <uc3:BuscadorSimple ID="filtroInvitadoPorMiembro" runat="server" LabelWidth="155" FieldLabel="Invitado Por" TipoDeObjeto="Miembro" />
                                    </Content>
                                </ext:Container>
                                <Z:ZCompositeField runat="server" FieldLabel="Culto">
                                    <Items>
                                        <Z:ZMultiCombo
                                            ID="filtroCulto"
                                            runat="server"
                                            FieldLabel="Culto"
                                            StoreID="StoreCultos" />
                                        <Z:ZDateField
                                            ID="filtroFechaDeCulto"
                                            runat="server"
                                            FieldLabel="Fecha de Culto" />
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZMultiCombo
                                    ID="filtroRazonDeVisita"
                                    runat="server"
                                    FieldLabel="Razón de Visita"
                                    StoreID="StoreRazonesDeVisita" />
                                <Z:ZMultiCombo
                                    ID="filtroEstatus"
                                    runat="server"
                                    FieldLabel="Estatus"
                                    ValueField="Key"
                                    DisplayField="Value"
                                    StoreID="StoreEstatus">
                                </Z:ZMultiCombo>
                                <Z:ZMultiCombo
                                    ID="filtroCategoria"
                                    runat="server"
                                    FieldLabel="Categoría"
                                    StoreID="StoreCategorias">
                                </Z:ZMultiCombo>
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>

                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colFiltros2" runat="server" Layout="Form" LabelWidth="155">
                            <Items>
                                <Z:ZComboBoxMunicipio
                                    ID="filtroMunicipio"
                                    runat="server" />
                                <Z:ZCompositeField ID="ZCompositeField8" runat="server" FieldLabel="Colonia/Direccion">
                                    <Items>
                                        <Z:ZTextField
                                            ID="filtroColonia"
                                            runat="server"
                                            FieldLabel="Colonia"
                                            Flex="1" />
                                        <Z:ZTextField
                                            ID="filtroDireccion"
                                            runat="server"
                                            FieldLabel="Direccion"
                                            Flex="1" />
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZNumberField
                                    ID="filtroEdad"
                                    runat="server"
                                    FieldLabel="Edad" />
                                <Z:ZMultiCombo
                                    ID="filtroEstadoCivil"
                                    runat="server"
                                    FieldLabel="Estado Civil"
                                    StoreID="StoreEstadosCiviles" />
                                <ext:Container ID="Container3" runat="server" Layout="Form">
                                    <Content>
                                        <uc3:BuscadorSimple ID="filtroCelulaAsignada" runat="server" LabelWidth="155" FieldLabel="Asignado a (Célula)" TipoDeObjeto="Celula" />
                                    </Content>
                                </ext:Container>
                                <ext:Container ID="Container4" runat="server" Layout="Form">
                                    <Content>
                                        <uc3:BuscadorSimple ID="filtroMiembroAsignada" runat="server" LabelWidth="155" FieldLabel="Asignado a (Miembro)" TipoDeObjeto="Miembro" />
                                    </Content>
                                </ext:Container>
                                <Z:ZNumberField
                                    ID="filtroTel"
                                    runat="server"
                                    FieldLabel="Tel" />
                                <Z:ZTextField
                                    ID="filtroObservaciones"
                                    runat="server"
                                    FieldLabel="Observaciones"
                                    Flex="1" />
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Items>
    </Z:ZPanelCatalogo>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cphEdicion" runat="server">
    <Z:ZPanelCatalogo ID="pnlEdicion" runat="server" Height="330px" ButtonAlign="Left">
        <Items>
            <ext:ColumnLayout ID="colsEdicion" runat="server">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colEdicion1" runat="server" Layout="Form" LabelWidth="155">
                            <Items>
                                <Z:ZNumberField
                                    ID="registroId"
                                    runat="server"
                                    FieldLabel="Id"
                                    ReadOnly="true"
                                    Text="-1"
                                    AllowBlank="false"
                                    Flex="1" />
                                <Z:ZCompositeField ID="ZCompositeField2" runat="server" Height="22px">
                                    <Items>
                                        <Z:ZTextField
                                            ID="registroEmail"
                                            runat="server"
                                            FieldLabel="Email"
                                            Vtype="email"
                                            AllowBlank="false"
                                            Flex="1" />
                                        <ext:Label runat="server" Text="No tiene email:" Width="90px"></ext:Label>
                                        <Z:ZCheckbox
                                            ID="registroNoTieneEmail"
                                            FieldClass="no-tiene-email-chk"
                                            Width="20px"
                                            runat="server">
                                            <Listeners>
                                                <Change Handler="determinarEmailFicticio();" />
                                                <Check Handler="determinarEmailFicticio();" />
                                            </Listeners>
                                        </Z:ZCheckbox>
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZCompositeField ID="ZCompositeField4" runat="server" FieldLabel="Nombre (s)">
                                    <Items>
                                        <Z:ZTextField
                                            ID="registroPrimerNombre"
                                            runat="server"
                                            FieldLabel="Primer Nombre"
                                            AllowBlank="false"
                                            Flex="1">
                                            <Listeners>
                                                <Change Handler="determinarEmailFicticio();" />
                                            </Listeners>
                                        </Z:ZTextField>
                                        <Z:ZTextField
                                            ID="registroSegundoNombre"
                                            runat="server"
                                            FieldLabel="Segundo Nombre"
                                            Flex="1" />
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZCompositeField ID="ZCompositeField5" runat="server" FieldLabel="Apellidos">
                                    <Items>
                                        <Z:ZTextField
                                            ID="registroApellidoPaterno"
                                            runat="server"
                                            FieldLabel="Apellido Paterno"
                                            AllowBlank="false"
                                            Flex="1">
                                            <Listeners>
                                                <Change Handler="determinarEmailFicticio();" />
                                            </Listeners>
                                        </Z:ZTextField>
                                        <Z:ZTextField
                                            ID="registroApellidoMaterno"
                                            runat="server"
                                            FieldLabel="Apellido Materno"
                                            Flex="1" />
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZComboBox
                                    ID="registroGenero"
                                    runat="server"
                                    FieldLabel="Género"
                                    StoreID="StoreGeneros"
                                    AllowBlank="false" />
                                <ext:Container ID="Container6" runat="server" Layout="Form">
                                    <Content>
                                        <uc3:BuscadorSimple ID="registroInvitadoPorMiembro" runat="server" LabelWidth="155" FieldLabel="Invitado Por" TipoDeObjeto="Miembro" />
                                    </Content>
                                </ext:Container>
                                <Z:ZCompositeField runat="server" FieldLabel="Culto">
                                    <Items>
                                        <Z:ZComboBox
                                            ID="registroCulto"
                                            runat="server"
                                            FieldLabel="Culto"
                                            StoreID="StoreCultos"
                                            AllowBlank="false" />
                                        <Z:ZDateField
                                            ID="registroFechaDeCulto"
                                            runat="server"
                                            FieldLabel="Fecha de Culto"
                                            AllowBlank="false">
                                        </Z:ZDateField>
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZComboBox
                                    ID="registroRazonDeVisita"
                                    runat="server"
                                    FieldLabel="Razón de Visita"
                                    StoreID="StoreRazonesDeVisita"
                                    AllowBlank="false" />
                                <Z:ZComboBox
                                    ID="registroEstatus"
                                    runat="server"
                                    FieldLabel="Estatus"
                                    ValueField="Key"
                                    DisplayField="Value"
                                    StoreID="StoreEstatus">
                                </Z:ZComboBox>
                                <Z:ZComboBox
                                    ID="registroCategoria"
                                    runat="server"
                                    FieldLabel="Categoría"
                                    StoreID="StoreCategorias"
                                    ReadOnly="true">
                                    <Listeners>
                                        <BeforeRender Handler="#{registroCategoria}.disable();" />
                                    </Listeners>
                                </Z:ZComboBox>
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>

                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colEdicion2" runat="server" Layout="Form" LabelWidth="155">
                            <Items>
                                <Z:ZComboBoxMunicipio
                                    ID="registroMunicipio"
                                    runat="server"
                                    AllowBlank="false" />
                                <Z:ZCompositeField ID="ZCompositeField3" runat="server" FieldLabel="Colonia/Direccion">
                                    <Items>
                                        <Z:ZTextField
                                            ID="registroColonia"
                                            runat="server"
                                            FieldLabel="Colonia"
                                            Flex="1" />
                                        <Z:ZTextField
                                            ID="registroDireccion"
                                            runat="server"
                                            FieldLabel="Direccion"
                                            Flex="1" />
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZCompositeField runat="server" FieldLabel="Cumpleaños (edad)">
                                    <Items>
                                        <Z:ZDateField
                                            ID="registroFechaDeNacimiento"
                                            runat="server"
                                            FieldLabel="Cumpleaños"
                                            AllowBlank="true">
                                            <Listeners>
                                                <Change Handler="determinarEmailFicticio();" />
                                            </Listeners>
                                        </Z:ZDateField>
                                        <Z:ZNumberField
                                            ID="registroEdad"
                                            runat="server"
                                            FieldLabel="Edad" />
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZComboBox
                                    ID="registroEstadoCivil"
                                    runat="server"
                                    FieldLabel="Estado Civil"
                                    StoreID="StoreEstadosCiviles"
                                    AllowBlank="false" />
                                <ext:Container ID="Container2" runat="server" Layout="Form">
                                    <Content>
                                        <uc3:BuscadorSimple ID="registroCelulaAsignada" runat="server" LabelWidth="155" FieldLabel="Asignado a (Célula)" TipoDeObjeto="Celula" />
                                    </Content>
                                </ext:Container>
                                <ext:Container ID="Container1" runat="server" Layout="Form">
                                    <Content>
                                        <uc3:BuscadorSimple ID="registroMiembroAsignada" runat="server" LabelWidth="155" FieldLabel="Asignado a (Miembro)" TipoDeObjeto="Miembro" />
                                    </Content>
                                </ext:Container>
                                <Z:ZNumberField
                                    ID="registroTelCasa"
                                    runat="server"
                                    FieldLabel="Tel de Casa"
                                    Validator="validarTelefonos" />
                                <Z:ZNumberField
                                    ID="registroTelMovil"
                                    runat="server"
                                    FieldLabel="Tel Movil" />
                                <Z:ZNumberField
                                    ID="registroTelTrabajo"
                                    runat="server"
                                    FieldLabel="Tel de Trabajo" />
                                <Z:ZTextField
                                    ID="registroObservaciones"
                                    runat="server"
                                    FieldLabel="Observaciones"
                                    Flex="1" />
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Items>
        <Buttons>
             <ext:Button runat="server" ID="btnCargarDatosDeBoletaAnterior" Text="Cargar Datos de Boleta Anterior...">
                <Listeners>
                    <Click Handler="cargarDatosDeBoletaAnterior();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </Z:ZPanelCatalogo>

    <hr/>

    <Z:ZGridCatalogo ID="GridDeReportes" runat="server" StoreID="StoreReportes" Layout="FitLayout" ButtonAlign="Left" Title="Reportes">
        <ColumnModel ID="ColumnModel1" runat="server">
            <Columns>
                <ext:Column Header="ID" Width="20" DataIndex="Id" />
                <ext:Column Header="Reporte" Width="275" DataIndex="Reporte" />
                <ext:Column Header="Fecha del Reporte" Width="60" DataIndex="Fecha">
                    <Renderer Fn="Ext.Renderers.IsoDateSimple" />
                </ext:Column>
                <ext:Column Header="Reportado por Miembro" Width="180" DataIndex="ReportadoPorMiembro" />
            </Columns>
        </ColumnModel>
        <Buttons>
            <ext:Button runat="server" ID="btnAgregarNuevoReporte" Icon="Add" Text="Agregar Reporte...">
                <Listeners>
                    <Click Handler="#{wndAgregarReporte}.show();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </Z:ZGridCatalogo>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphGridDeResultados" runat="server">
    <Z:ZGridCatalogo ID="GridDeResultados" runat="server" StoreID="StoreResultados" Layout="FitLayout">
        <ColumnModel ID="ColumnModel1" runat="server">
            <Columns>
                <ext:Column Header="ID" Width="40" DataIndex="Id" Hidden="true" />
                <ext:Column Header="Email" Width="100" DataIndex="Email" />
                <ext:Column Header="Primer Nombre" Width="100" DataIndex="PrimerNombre" />
                <ext:Column Header="Segundo Nombre" Width="100" DataIndex="SegundoNombre" />
                <ext:Column Header="Apellido Paterno" Width="100" DataIndex="ApellidoPaterno" />
                <ext:Column Header="Apellido Materno" Width="100" DataIndex="ApellidoMaterno" Hidden="true" />
                <ext:Column Header="Estatus" Width="100" DataIndex="Estatus" />
                <ext:Column Header="Género" Width="100" DataIndex="Genero" />
                <ext:Column Header="Invitado por Miembro (ID)" Width="40" DataIndex="InvitadoPorMiembroId" Hidden="true" />
                <ext:Column Header="Culto" Width="100" DataIndex="Culto" Hidden="true" />
                <ext:Column Header="Fecha del Culto" Width="100" DataIndex="FechaDeCulto" Hidden="true">
                    <Renderer Fn="Ext.Renderers.IsoDateSimple" />
                </ext:Column>
                <ext:Column Header="Razón De Visita del Miembro" Width="100" DataIndex="RazonDeVisita" Hidden="true" />
                <ext:Column Header="Estatus" Width="100" DataIndex="RazonDeCerrar" Hidden="true" />
                <ext:Column Header="Municipio" Width="100" DataIndex="Municipio" />
                <ext:Column Header="Colonia" Width="100" DataIndex="Colonia" />
                <ext:Column Header="Dirección" Width="100" DataIndex="Direccion" />
                <ext:Column Header="Cumpleaños" Width="100" DataIndex="Nacimiento" Hidden="true">
                    <Renderer Fn="Ext.Renderers.IsoDateSimple" />
                </ext:Column>
                <ext:Column Header="Edad" Width="100" DataIndex="Edad" />
                <ext:Column Header="Estado Civil" Width="100" DataIndex="EstadoCivil" />
                <ext:Column Header="Asignada a Célula (ID)" Width="40" DataIndex="AsignadaACelula" Hidden="true" />
                <ext:Column Header="Asignada a Miembro (ID)" Width="40" DataIndex="AsignadaAMiembro" Hidden="true" />
                <ext:Column Header="Tel (Casa)" Width="100" DataIndex="TelCasa" />
                <ext:Column Header="Tel (Cel)" Width="100" DataIndex="TelMovil" />
                <ext:Column Header="Tel (Trabajo)" Width="100" DataIndex="TelTrabajo" Hidden="true" />
                <ext:Column Header="Observaciones" Width="100" DataIndex="Observaciones" Hidden="true" />
                <ext:Column Header="Categoria" Width="100" DataIndex="Categoria" Hidden="true" />
            </Columns>
        </ColumnModel>
        <FooterBar>
            <ext:StatusBar runat="server">
                <Items>
                    <ext:DisplayField ID="registroNumeroDeBoletas" runat="server" Text=""></ext:DisplayField>
                </Items>
            </ext:StatusBar>
        </FooterBar>
    </Z:ZGridCatalogo>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooterCatalogo" runat="server">
    <script src="<%= ResolveUrl("~/Recursos/js/Paginas/Consolidación/catalogoDeBoletas.js")%>" type="text/javascript"></script>
</asp:Content>


