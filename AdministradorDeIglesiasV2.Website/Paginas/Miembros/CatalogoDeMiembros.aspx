<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/MasterPages/Catalogos.master" AutoEventWireup="true" CodeBehind="CatalogoDeMiembros.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.CatalogoDeMiembros" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/Paginas/MasterPages/Catalogos.master" %>

<%@ Register src="../UserControls/Miembros/FiltrosMiembros.ascx" tagname="FiltrosMiembros" tagprefix="uc1" %>
<%@ Register src="../UserControls/Miembros/ResultadoMiembros.ascx" tagname="ResultadoMiembros" tagprefix="uc2" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFiltros" runat="server">
    <uc1:FiltrosMiembros ID="Filtros" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphEdicion" runat="server">

    <Z:ZPanelCatalogo ID="pnlEdicion" runat="server" Height="220px">
        <Items>
            <ext:ColumnLayout ID="colsEdicion" runat="server">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colEdicion1" runat="server" Layout="Form" LabelWidth="125">
                            <Items>
                                <Z:ZCompositeField ID="ZCompositeField1" runat="server" Height="24px">
                                    <Items>
                                        <Z:ZNumberField 
                                            ID="registroId" 
                                            runat="server"
                                            FieldLabel="Id"
                                            ReadOnly="true"
                                            Text="-1"
                                            AllowBlank="false"
                                            Flex="1"
                                            />
                                            <ext:Button ID="registroBotonFoto" runat="server" Anchor="right" Text="Foto" Icon="ImageMagnify" ToolTip="Ver/Cargar Foto..." Width="90px" >
                                                <Listeners>
                                                    <Click Handler="registroFotoClick();" />
                                                </Listeners>
                                            </ext:Button>
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZCompositeField ID="ZCompositeField2" runat="server" Height="22px">
                                    <Items>
                                         <Z:ZTextField 
                                            ID="registroEmail" 
                                            runat="server"
                                            FieldLabel="Email"
                                            Vtype="email"
                                            AllowBlank="false"
                                            Flex="1"
                                            />
                                        <ext:Label runat="server"  Text="No tiene email:" Width="90px"></ext:Label>
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
                                            Flex="1"/>
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
                                            Flex="1"/>
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZComboBox 
                                    ID="registroCelula" 
                                    runat="server"
                                    FieldLabel="Célula que Asiste"
                                    StoreID="StoreCelulas"
                                    AllowBlank="false"
                                    />
                                <Z:ZComboBox 
                                    ID="registroGenero" 
                                    runat="server"
                                    FieldLabel="Género"
                                    StoreID="StoreGeneros"
                                    AllowBlank="false"
                                    />
                                <Z:ZCheckbox 
                                    ID="registroAsisteIglesia" 
                                    runat="server" 
                                    FieldLabel="¿Asiste a la Iglesia?"
                                    Checked="true"
                                    />
                                <Z:ZTextField 
                                    ID="registroPassword" 
                                    runat="server"
                                    FieldLabel="Contraseña"
                                    InputType="Password" 
                                    MinLength="6"
                                    />
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                    
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colEdicion2" runat="server" Layout="Form">
                            <Items>
                                <Z:ZComboBoxMunicipio
                                    ID="registroMunicipio" 
                                    runat="server"
                                    AllowBlank="false"
                                    />
                                <Z:ZCompositeField ID="ZCompositeField3" runat="server" FieldLabel="Colonia/Dirección">
                                    <Items>
                                        <Z:ZTextField 
                                            ID="registroColonia" 
                                            runat="server"
                                            FieldLabel="Colonia"
                                            Flex="1"
                                            />
                                        <Z:ZTextField 
                                            ID="registroDireccion" 
                                            runat="server"
                                            FieldLabel="Dirección"
                                            Flex="1"
                                            />
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZDateField 
                                    ID="registroFechaDeNacimiento" 
                                    runat="server"
                                    FieldLabel="Cumpleaños"
									AllowBlank="false">
                                    <Listeners>
                                        <Change Handler="determinarEmailFicticio();" />
                                    </Listeners>
                                </Z:ZDateField>
                                <Z:ZComboBox 
                                    ID="registroEstadoCivil" 
                                    runat="server"
                                    FieldLabel="Estado Civil"
                                    StoreID="StoreEstadosCiviles"
                                    AllowBlank="false"
                                    />
                                <Z:ZNumberField 
                                    ID="registroTelCasa" 
                                    runat="server"
                                    FieldLabel="Tel de Casa"
                                    validator="validarTelefonos"
                                    />
                                <Z:ZNumberField 
                                    ID="registroTelMovil" 
                                    runat="server"
                                    FieldLabel="Tel Móvil"
                                    />
                                <Z:ZNumberField 
                                    ID="registroTelTrabajo" 
                                    runat="server"
                                    FieldLabel="Tel de Trabajo"
                                    />
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Items>
    </Z:ZPanelCatalogo>

    <ext:Window 
        ID="wndFoto" 
        runat="server" 
        Title="Foto del Miembro"  
        Width="160px"
        Height="240px" 
        Padding="5"
        Collapsible="false"
        Resizable="false"
        Draggable="false"
        Hidden="true"
        Modal="true">
        <Items>
            <ext:FormPanel ID="pnlFoto" runat="server" ButtonAlign="Right" Padding="5" RenderFormElement="true" FileUpload="true" HideLabels="true" BodyBorder="false" Url="FotoDeMiembro.aspx" StyleSpec="text-align:center;">
                <Items>
                    <ext:Image ID="registroFoto" runat="server" Height="160px" MaxHeight="160px" AutoHeight="false" AutoWidth="false" ImageUrl="../../Recursos/img/sin_foto.jpg"></ext:Image>
                    <ext:FileUploadField runat="server" ID="registroUploadFoto" ButtonOnly="true" ButtonText="Cargar..." Height="32px">
                        <Listeners>
                            <FileSelected Handler="mandarArchivoAServidor();" />
                        </Listeners>
                    </ext:FileUploadField>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:Window>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphGridDeResultados" runat="server">
    <uc2:ResultadoMiembros ID="Resultados" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooterCatalogo" runat="server">
    <script src='<%= ResolveUrl("~/Recursos/js/Paginas/Miembros/catalogoDeMiembros.js")%>' type="text/javascript"></script>
    <div id="extra-upload-form"></div>
</asp:Content>


