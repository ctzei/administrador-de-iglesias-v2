<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="ConfirmacionDeInscipciones.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.ConfirmacionDeInscripciones" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
        <ext:Store ID="StoreEventos" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Descripcion" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Store ID="StoreFichas" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader IDProperty="Folio">
                    <Fields>
                        <ext:RecordField Name="Folio" />
                        <ext:RecordField Name="Cerrada" />
                        <ext:RecordField Name="RefNum" />
                        <ext:RecordField Name="RefAlfa" />
                        <ext:RecordField Name="Cantidad" />
                        <ext:RecordField Name="Fecha" Type="Date" />
                        <ext:RecordField Name="Email" />
                        <ext:RecordField Name="PrimerNombre" />
                        <ext:RecordField Name="SegundoNombre" />
                        <ext:RecordField Name="ApellidoPaterno" />
                        <ext:RecordField Name="ApellidoMaterno" />
                        <ext:RecordField Name="Tel" />
                        <ext:RecordField Name="Estado" />
                        <ext:RecordField Name="Municipio" />
                        <ext:RecordField Name="Genero" />
                        <ext:RecordField Name="TipoDeRegistrante" />
                        <ext:RecordField Name="InfoExtraDeRegistrate" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Viewport ID="ViewportMain" runat="server" Layout="Anchor">
            <Items>
                <ext:FormPanel ID="pnlFiltros" runat="server" AnchorHorizontal="right" Height="100px" Layout="Form" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false" LabelWidth="190">
                    <Items>
                        <ext:ColumnLayout ID="colsFiltros" runat="server">
                                <Columns>
                                    <ext:LayoutColumn ColumnWidth="0.5">
                                        <ext:Container ID="colFiltros1" runat="server" Layout="Form">
                                            <Items>
                                                <Z:ZComboBox
                                                    FieldLabel="Evento"
                                                    ID="cboEvento" 
                                                    runat="server" 
                                                    StoreID="StoreEventos"
                                                    Resizable="true"
                                                    AllowBlank="false">
                                                    <Listeners>
                                                        <Change Handler="limpiarGridDeFichas();" />
                                                    </Listeners>
                                                </Z:ZComboBox>
                                            </Items>
                                            <Items>
                                                <Z:ZCheckbox
                                                    FieldLabel="¿Mostrar fichas cerradas?"
                                                    ID="chkFichasCerradas" 
                                                    runat="server" 
                                                    Resizable="true">
                                                    <Listeners>
                                                        <Change Handler="limpiarGridDeFichas();" />
                                                    </Listeners>
                                                </Z:ZCheckbox>
                                            </Items>
                                        </ext:Container>
                                    </ext:LayoutColumn>
                    
                                    <ext:LayoutColumn ColumnWidth="0.5">
                                        <ext:Container ID="colFiltros2" runat="server" Layout="Form">
                                            <Items>
                                                <Z:ZCompositeField ID="ZCompositeField1" runat="server" FieldLabel="Fechas del Deposito (Inicio/Fin)" AnchorHorizontal="98%">
                                                    <Items>
                                                        <Z:ZDateField  
                                                            FieldLabel="Fecha Inicial" 
                                                            ID="dtpFechaInicial" 
                                                            runat="server"
                                                            AllowBlank="false"
                                                            Flex="1">
                                                            <Listeners>
                                                                <Change Handler="limpiarGridDeFichas();" />
                                                            </Listeners>
                                                        </Z:ZDateField >
                                                        <Z:ZDateField  
                                                            FieldLabel="Fecha Final" 
                                                            ID="dtpFechaFinal" 
                                                            runat="server"
                                                            AllowBlank="false"
                                                            Flex="1">
                                                            <Listeners>
                                                                <Change Handler="limpiarGridDeFichas();" />
                                                            </Listeners>
                                                        </Z:ZDateField >
                                                    </Items>
                                                </Z:ZCompositeField>
                                            </Items>
                                        </ext:Container>
                                    </ext:LayoutColumn>
                                </Columns>
                            </ext:ColumnLayout>
                    </Items>
                    <Buttons>
                        <ext:Button ID="cmdMostrarFichas" runat="server" Text="Mostrar Fichas" >
                            <Listeners>
                                <Click Handler="mostrarFichas();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="cmdGuardarCambios" runat="server" Text="Guardar Cambios" >
                            <Listeners>
                                <Click Handler="guardarCambios();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
                <Z:ZGridPanel ID="grdFichas" runat="server" AnchorHorizontal="right" AnchorVertical="-100" StoreID="StoreFichas" Title="Fichas de Inscripcion" AutoWidth="true">
                     <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:Column Header="Folio" Width="40" DataIndex="Folio" />
                            <ext:CheckColumn Header="Cerrada" Width="60" DataIndex="Cerrada" Editable="true">
                                <Editor>
                                    <ext:Checkbox runat="server" AllowBlank="false" />
                                </Editor>
                            </ext:CheckColumn>
                            <ext:Column Header="Ref. Numerica" Width="100" DataIndex="RefNum" />
                            <ext:Column Header="Ref. Alfanumerica" Width="100" DataIndex="RefAlfa" />
                            <ext:Column Header="# Personas" Width="75" DataIndex="Cantidad" />
                            <ext:DateColumn Header="Fecha" Width="65" DataIndex="Fecha" />
                            <ext:Column Header="Email" Width="175" DataIndex="Email" />
                            <ext:Column Header="Primer Nombre" Width="100" DataIndex="PrimerNombre" />
                            <ext:Column Header="Segundo Nombre" Width="100" DataIndex="SegundoNombre" />
                            <ext:Column Header="Apellido Paterno" Width="100" DataIndex="ApellidoPaterno" />
                            <ext:Column Header="Tel" Width="100" DataIndex="Tel" />
                            <ext:Column Header="Estado" Width="100" DataIndex="Estado" />
                        </Columns>
                    </ColumnModel>
                </Z:ZGridPanel>
            </Items>
        </ext:Viewport>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooter" runat="server">
    <script src="../../Recursos/js/Paginas/Eventos/confirmacionDeInscripciones.js" type="text/javascript"></script>
</asp:Content>

