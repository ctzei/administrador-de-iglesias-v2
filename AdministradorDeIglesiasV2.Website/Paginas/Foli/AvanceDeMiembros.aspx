<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="AvanceDeMiembros.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Foli.AvanceDeMiembros" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
        <ext:Store ID="StoreGrupos" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Descripcion" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Store ID="StoreAvances" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader IDProperty="MiembroId">
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="MiembroId" />
                        <ext:RecordField Name="FoliMiembroId" />
                        <ext:RecordField Name="PrimerNombre" />
                        <ext:RecordField Name="SegundoNombre" />
                        <ext:RecordField Name="ApellidoPaterno" />
                        <ext:RecordField Name="ApellidoMaterno" />
                        <ext:RecordField Name="Asistencia" Type="Boolean" />
                        <ext:RecordField Name="Tarea" Type="Boolean" />
                        <ext:RecordField Name="CalificacionTarea" />
                        <ext:RecordField Name="CalificacionExamen" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Viewport ID="ViewportAsistencias" runat="server" Layout="Anchor">
            <Items>
                <ext:FormPanel ID="pnlFiltros" runat="server" AnchorHorizontal="right" Height="80px" Layout="Form" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false" LabelWidth="150">
                    <Items>

                        <Z:ZCompositeField ID="CompositeField1" runat="server" FieldLabel="Grupo/Fecha de la Clase" AnchorHorizontal="98%">
                            <Items>
                                <Z:ZComboBox
                                    FieldLabel="Grupo"
                                    ID="cboGrupo" 
                                    runat="server" 
                                    StoreID="StoreGrupos"
                                    Resizable="true"
                                    AllowBlank="false"
                                    Flex="1">
                                    <Listeners>
                                        <Change Handler="limpiarPantalla();" />
                                    </Listeners>
                                </Z:ZComboBox>
                                <Z:ZDateField  
                                    FieldLabel="Fecha de la Clase" 
                                    ID="dtpFecha" 
                                    runat="server"
                                    AllowBlank="false"
                                    Flex="1">
                                    <Listeners>
                                        <Change Handler="limpiarPantalla();" />
                                    </Listeners>
                                </Z:ZDateField >
                            </Items>
                        </Z:ZCompositeField>

                    </Items>
                    <Buttons>
                        <ext:Button ID="cmdMostrarAvance" runat="server" Text="Mostrar" >
                            <Listeners>
                                <Click Handler="mostrarAvance();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="cmdGuardarAvance" runat="server" Text="Guardar" >
                            <Listeners>
                                <Click Handler="guardarAvance();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
                <Z:ZGridPanel ID="grdAlumnos" runat="server" AnchorHorizontal="right" AnchorVertical="-80" StoreID="StoreAvances" Title="Alumnos" AutoWidth="true">
                     <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:Column Header="Primer Nombre" Width="115" DataIndex="PrimerNombre" />
                            <ext:Column Header="Segundo Nombre" Width="80" DataIndex="SegundoNombre" />
                            <ext:Column Header="Apellido Paterno" Width="115" DataIndex="ApellidoPaterno" />
                            <ext:Column Header="Apellido Materno" Width="80" DataIndex="ApellidoMaterno" />
                            <ext:CheckColumn Header="Asistencia" Width="50" DataIndex="Asistencia" Editable="true">
                                <Editor>
                                    <ext:Checkbox runat="server" AllowBlank="false">
                                    </ext:Checkbox>
                                </Editor>
                            </ext:CheckColumn>
                            <ext:CheckColumn Header="Tarea" Width="50" DataIndex="Tarea" Editable="true">
                                <Editor>
                                    <ext:Checkbox ID="Checkbox1" runat="server" AllowBlank="false">
                                    </ext:Checkbox>
                                </Editor>
                            </ext:CheckColumn>
                            <ext:NumberColumn Header="Cal. Tarea" Width="85" DataIndex="CalificacionTarea" Editable="true">
                                <Editor>
                                    <Z:ZNumberField runat="server" MaxLength="5" AllowBlank="true" AllowNegative="false" MinValue="0" MaxValue="100"/>
                                </Editor>
                            </ext:NumberColumn>
                            <ext:NumberColumn Header="Cal. Examen" Width="85" DataIndex="CalificacionExamen" Editable="true">
                                <Editor>
                                    <Z:ZNumberField ID="ZTextField1" runat="server" MaxLength="5" AllowBlank="true" AllowNegative="false" MinValue="0" MaxValue="100" />
                                </Editor>
                            </ext:NumberColumn>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel SingleSelect="true"></ext:RowSelectionModel>
                    </SelectionModel>
                    <FooterBar>
                        <ext:StatusBar ID="StatusBar1" runat="server">
                            <Items>
                                <ext:DisplayField ID="registroNumeroDeAsistencias" runat="server" Text=""></ext:DisplayField>
                                <ext:DisplayField ID="registroNumeroDeAlumnos" runat="server" Text=""></ext:DisplayField>
                            </Items>
                        </ext:StatusBar>
                    </FooterBar>
                    <Listeners>
                        <AfterEdit Fn="registroModificado" />
                        <KeyDown Fn="registroPresionado" />
                    </Listeners>
                </Z:ZGridPanel>
            </Items>
        </ext:Viewport>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooter" runat="server">
    <script src='<%= ResolveUrl("~/Recursos/js/Paginas/Foli/avanceDeMiembros.js")%>' type="text/javascript"></script>
</asp:Content>

