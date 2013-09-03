<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="RegistroDeAsistencia.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.RegistroDeAsistencia" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
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

        <ext:Store ID="StoreAsistencias" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader IDProperty="MiembroId">
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="MiembroId" />
                        <ext:RecordField Name="Nombre" />
                        <ext:RecordField Name="Asistencia" Type="Boolean" />
                        <ext:RecordField Name="Peticiones"/>
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Viewport ID="ViewportAsistencias" runat="server" Layout="Anchor">
            <Items>
                <ext:FormPanel ID="pnlFiltros" runat="server" AnchorHorizontal="right" Height="80px" Layout="Form" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false" LabelWidth="150">
                    <Items>

                        <Z:ZCompositeField ID="CompositeField1" runat="server" FieldLabel="Célula/Fecha de la Célula" AnchorHorizontal="98%">
                            <Items>
                                <Z:ZComboBox
                                    FieldLabel="Celula"
                                    ID="cboCelula" 
                                    runat="server" 
                                    StoreID="StoreCelulas"
                                    Resizable="true"
                                    AllowBlank="false"
                                    Flex="1"
                                    Height="30px">
                                    <Listeners>
                                        <Change Handler="limpiarPantalla();" />
                                    </Listeners>
                                </Z:ZComboBox>
                                <Z:ZDateField  
                                    FieldLabel="Fecha de la Celula" 
                                    ID="dtpFecha" 
                                    runat="server"
                                    AllowBlank="false"
                                    Flex="1"
                                    Height="30px">
                                    <Listeners>
                                        <Change Handler="limpiarPantalla();" />
                                    </Listeners>
                                </Z:ZDateField >
                            </Items>
                        </Z:ZCompositeField>

                    </Items>
                    <Buttons>
                        <ext:Button ID="cmdMostrarAsistencia" runat="server" Text="Mostrar Asistencia" >
                            <Listeners>
                                <Click Handler="mostrarAsistencia();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="cmdMostrarUltimaAsistencia" runat="server" Text="Mostrar última Asistencia registrada" >
                            <Listeners>
                                <Click Handler="mostrarUltimaAsistencia();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="cmdGuardarAsistencia" runat="server" Text="Guardar Asistencia" Icon="Accept" >
                            <Listeners>
                                <Click Handler="guardarAsistencia();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="cmdCancelarAsistencia" runat="server" Text="Cancelar Asistencia (No hubo célula)" Icon="Delete" >
                            <Listeners>
                                <Click Handler="mostrarRazonDeCancelacion();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
                <Z:ZGridPanel ID="grdAsistencias" runat="server" AnchorHorizontal="right" AnchorVertical="-80" StoreID="StoreAsistencias" Title="Asistencias" AutoWidth="true">
                     <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                             <ext:CheckColumn Header="Asistencia" Width="45" DataIndex="Asistencia" Editable="true">
                                <Editor>
                                    <ext:Checkbox runat="server" AllowBlank="false">
                                    </ext:Checkbox>
                                </Editor>
                            </ext:CheckColumn>
                            <ext:Column Header="Nombre" Width="500" DataIndex="Nombre">
                                <Renderer Fn="Ext.Renderers.DetallesDeMiembro" />
                            </ext:Column>
                            <ext:Column Header="Peticiones" Width="150" DataIndex="Peticiones" Editable="true">
                                <Editor>
                                    <Z:ZTextField runat="server" MaxLength="25" AllowBlank="true" />
                                </Editor>
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel SingleSelect="true"></ext:RowSelectionModel>
                    </SelectionModel>
                    <FooterBar>
                        <ext:Toolbar ID="StatusBar1" runat="server">
                            <Items>
                                <ext:Label Text="Número de Invitados:" Cls="bold"></ext:Label>
                                <Z:ZNumberField ID="registroNumeroDeInvitados" runat="server" Width="50" MinValue="0" Number="0" StyleSpec="background-image: none; background-color : #BDC7D6;"></Z:ZNumberField>
                                <ext:Label Text="  -  "></ext:Label>
                                <ext:DisplayField ID="registroNumeroDeAsistencias" runat="server" Text="" Cls="bold"></ext:DisplayField>
                                <ext:DisplayField ID="registroNumeroDeMiembros" runat="server" Text="" Cls="bold"></ext:DisplayField>
                            </Items>
                        </ext:Toolbar>
                    </FooterBar>
                    <Listeners>
                        <AfterEdit Fn="registroModificado" />
                        <KeyDown Fn="registroPresionado" />
                    </Listeners>
                </Z:ZGridPanel>
            </Items>
        </ext:Viewport>

        <ext:Window 
            ID="wndRazonCancelacion" 
            runat="server" 
            Title="Razon de la Cancelación:"  
            Height="185px" 
            Width="350px"
            Padding="5"
            Collapsible="false"
            Resizable="false"
            Draggable="false"
            Hidden="true"
            Modal="true">
            <Items>
                <ext:FormPanel ID="pnlRazonCancelacion" runat="server" HideBorders="true" AnchorHorizontal="right" Height="150px" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false" HideLabels="true">
                    <Items>
                        <Z:ZTextArea 
                            ID="txtRazonCancelacion" 
                            runat="server" 
                            AllowBlank="false" 
                            AnchorHorizontal="right"
                            Height="100px"
                            Width="300px"
                            />
                    </Items>
                    <Buttons>
                        <ext:Button ID="cmdAceptar" runat="server" Text="Aceptar" >
                            <Listeners>
                                <Click Handler="confirmarCancelacionDeAsistencia();" />
                            </Listeners>
                        </ext:Button>    
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooter" runat="server">
    <script src="../../Recursos/js/Paginas/Celulas/registroDeAsistencia.js" type="text/javascript"></script>
</asp:Content>

