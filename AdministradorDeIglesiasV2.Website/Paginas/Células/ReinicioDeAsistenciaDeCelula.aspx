<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="ReinicioDeAsistenciaDeCelula.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.ReinicioDeAsistenciaDeCelula" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
        <ext:Store ID="StoreCelulas" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Descripcion" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Viewport ID="ViewportAsistencias" runat="server" Layout="Anchor">
            <Items>

                <ext:Panel runat="server" PaddingSummary="5px 5px 15px" BodyBorder="false" MinHeight="50px" LabelStyle="font-weight:bold;">
                    <Items>
                        <ext:Label LabelStyle="font-weight:bold;" StyleSpec="font-weight:bold;" runat="server" Text="Reiniciar la asistencia de una célula significa borrar todas las asistencias y cancelaciones registradas y modificar la fecha de creación de la célula. El reinicio de la asistencia de una célula se hace para poder registrar asistencias de células que fueron creadas con mucho tiempo de anterioridad a en verdad haber empezado. Este procedimiento NO se puede revertir y la información será completamente eliminada."></ext:Label>
                    </Items>
                </ext:Panel>

                <ext:FormPanel ID="pnlFiltros" runat="server" AnchorHorizontal="right" Height="200px" Layout="Form" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false" LabelWidth="150">
                    <Items>
                    
                        <Z:ZCompositeField ID="CompositeField1" runat="server" FieldLabel="Célula/Fecha de inicio de la Célula" AnchorHorizontal="98%">
                            <Items>
                                <Z:ZComboBox
                                    FieldLabel="Celula"
                                    ID="cboCelula" 
                                    runat="server" 
                                    StoreID="StoreCelulas"
                                    Resizable="true"
                                    AllowBlank="false"
                                    Flex="1">
                                </Z:ZComboBox>
                                <Z:ZDateField  
                                    FieldLabel="Fecha de inicio de la Célula" 
                                    ID="dtpFecha" 
                                    runat="server"
                                    AllowBlank="false"
                                    Flex="1">
                                </Z:ZDateField >
                            </Items>
                        </Z:ZCompositeField>

                    </Items>
                    <Buttons>
                        <ext:Button ID="cmdReiniciarCelula" runat="server" Text="Reiniciar Célula" >
                            <Listeners>
                                <Click Handler="reiniciarCelula();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooter" runat="server">
    <script src="<%= ResolveUrl("~/Recursos/js/Paginas/Celulas/reinicioDeAsistenciaDeCelula.js")%>" type="text/javascript"></script>
</asp:Content>

