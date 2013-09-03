<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="FaltaDeRegistroDeAsistencia.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Reportes.Celulas.FaltaDeRegistroDeAsistencia" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

        <ext:Store ID="StoreDias" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader IDProperty="Id">
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Descripcion" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Store ID="StoreHoras" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader IDProperty="Id">
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Descripcion" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Store ID="StoreTipoDeReportes" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader IDProperty="Id">
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Descripcion" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Viewport ID="ViewportAsistencias" runat="server" Layout="Anchor">
            <Items>
                <ext:FormPanel ID="pnlMain" runat="server" AnchorHorizontal="right"  Layout="Form" ButtonAlign="Center" PaddingSummary="5px 25px 0" BodyBorder="false" LabelWidth="500">
                    <Items>

                        <Z:ZCheckbox
                            FieldLabel="¿Desea inscribirse al reporte semanal de falta de asistencias registradas, para su red?"
                            ID="chkInscribirse" 
                            runat="server" 
                            Resizable="true"
                            MaxWidth="270px">
                            <Listeners>
                                <Check Handler="habilitarControles();" />
                            </Listeners>
                        </Z:ZCheckbox>


                        <Z:ZComboBox
                            FieldLabel="Dia de la semana a recibir el reporte"
                            ID="cboDiaSemana" 
                            runat="server" 
                            StoreID="StoreDias"
                            Resizable="true"
                            Disabled="true"
                            AllowBlank="false">
                        </Z:ZComboBox>

                        <Z:ZComboBox
                            FieldLabel="Hora del dia a recibir el reporte"
                            ID="cboHoraDia" 
                            runat="server" 
                            StoreID="StoreHoras"
                            Resizable="true"
                            Disabled="true"
                            AllowBlank="false">
                        </Z:ZComboBox>

                        <Z:ZComboBox
                            FieldLabel="Tipo de reporte"
                            ID="cboTipoDeReporte" 
                            runat="server" 
                            StoreID="StoreTipoDeReportes"
                            Resizable="true"
                            Disabled="true"
                            AllowBlank="false">
                        </Z:ZComboBox>
                    </Items>
                    <Buttons>
                        <ext:Button ID="cmdGuardarCambios" runat="server" Text="Guardar Cambios" >
                            <Listeners>
                                <Click Handler="guardarCambios();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="cmdMostrarReporte" runat="server" Text="Mostrar Reporte" >
                            <Listeners>
                                <Click Handler="mostrarReporte();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>

    <ext:Window 
        ID="wndReporte" 
        runat="server" 
        Title="Reporte de Falta de Asistencias"  
        Width="700px"
        Height="500px" 
        Padding="5"
        Collapsible="false"
        Resizable="false"
        Draggable="false"
        Hidden="true"
        Modal="true">
        <Items>
            <ext:FormPanel ID="pnlReporteGenerado" runat="server" HideBorders="true" AnchorHorizontal="right" Layout="Form" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false" LabelWidth="150">
                <Items>
                    <Z:ZComboBox
                        FieldLabel="Tipo de reporte"
                        ID="cboTipoDeReporteGenerado" 
                        runat="server" 
                        StoreID="StoreTipoDeReportes"
                        Resizable="true"
                        AllowBlank="false">
                        <Listeners>
                            <Change Handler="limpiarReporte();" />
                        </Listeners>
                    </Z:ZComboBox>
                    <ext:Panel ID="pnlHtml" Title="Reporte de Asistencias de Célula NO REGISTRADAS de la semana anterior inmediata" AutoScroll="true" Height="385px" runat="server"></ext:Panel>
                </Items>
                <Buttons>
                    <ext:Button ID="cmdGenerarReporte" runat="server" Text="Generar Reporte" >
                        <Listeners>
                            <Click Handler="generarReporte();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
    </ext:Window>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
    <script src="<%= ResolveUrl("~/Recursos/js/Paginas/Reportes/Celulas/faltaDeRegistroDeAsistencia.js")%>" type="text/javascript"></script>
</asp:Content>
