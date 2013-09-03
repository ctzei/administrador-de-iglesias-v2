<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/MasterPages/CatalogosComplejos.master" AutoEventWireup="true" CodeBehind="CatalogoDeEventos.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Alabanza.CatalogoDeEventos" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/Paginas/MasterPages/Catalogos.master" %>

<%@ Register src="../UserControls/BuscadorSimple.ascx" tagname="BuscadorSimple" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphStores" runat="server">

    <ext:Store ID="StoreResultados" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Descripcion" />
                    <ext:RecordField Name="Fecha" />
                    <ext:RecordField Name="HoraInicio" />
                    <ext:RecordField Name="HoraFin" />
                </Fields>
            </ext:JsonReader>
        </Reader>            
    </ext:Store>

    <ext:Store ID="StoreHorasDelDia" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Descripcion" />
                </Fields>
            </ext:JsonReader>
        </Reader>            
    </ext:Store>

    <ext:Store ID="StoreInstrumentos" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Descripcion" />
                </Fields>
            </ext:JsonReader>
        </Reader>            
    </ext:Store>

    <ext:Store ID="StoreCanciones" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="AlabanzaCancionId" />
                    <ext:RecordField Name="Titulo" />
                    <ext:RecordField Name="Artista" />
                    <ext:RecordField Name="Disco" />
                    <ext:RecordField Name="Tono" />
                </Fields>
            </ext:JsonReader>
        </Reader>            
    </ext:Store>

    <ext:Store ID="StoreCancionesEncontradas" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Titulo" />
                    <ext:RecordField Name="Artista" />
                    <ext:RecordField Name="Disco" />
                    <ext:RecordField Name="Tono" />
                </Fields>
            </ext:JsonReader>
        </Reader>            
    </ext:Store>

    <ext:Store ID="StoreMiembros" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="AlabanzaMiembroId" />
                    <ext:RecordField Name="MiembroId" />
                    <ext:RecordField Name="Nombre" />
                    <ext:RecordField Name="InstrumentoId" />
                    <ext:RecordField Name="Instrumento" />
                    <ext:RecordField Name="Asistencia" Type="Boolean" />
                    <ext:RecordField Name="Retraso" Type="Boolean" />
                </Fields>
            </ext:JsonReader>
        </Reader>            
    </ext:Store>

    <ext:Store ID="StoreEnsayos" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Fecha" />
                    <ext:RecordField Name="HoraInicio" />
                    <ext:RecordField Name="HoraFin" />
                </Fields>
            </ext:JsonReader>
        </Reader>            
    </ext:Store>

    <ext:Store ID="StoreEnsayoMiembros" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="AlabanzaMiembroId" />
                    <ext:RecordField Name="MiembroId" />
                    <ext:RecordField Name="Nombre" />
                    <ext:RecordField Name="Instrumento" />
                    <ext:RecordField Name="Asistencia" Type="Boolean" />
                    <ext:RecordField Name="Retraso" Type="Boolean" />
                </Fields>
            </ext:JsonReader>
        </Reader>            
    </ext:Store>

    <!-- PopUps -->

     <Z:ZModalWindow 
        ID="wndAgregarMiembro" 
        runat="server" 
        Title="Agregar Miembro"  
        Width="400px"
        Height="130px">
        <Items>
            <Z:ZFormPanel ID="pnlAgregarMiembro" runat="server" BodyBorder="false">
                <Items>
                    <ext:Container runat="server" Layout="Form" AnchorHorizontal="100%">
                        <Content>
                            <uc1:BuscadorSimple ID="registroMiembro" runat="server" LabelWidth="155" FieldLabel="Miembro" TipoDeObjeto="AlabanzaMiembro" />  
                        </Content>
                    </ext:Container>
                    <Z:ZComboBox
                        ID="registroInstrumento" 
                        runat="server"
                        FieldLabel="Instrumento"
                        StoreID="StoreInstrumentos"
                        AnchorHorizontal="100%"
                        AllowBlank="false"
                        />
                </Items>
                <Buttons>
                    <ext:Button ID="btnAgregarMiembro" Text="Agregar Miembro">
                        <Listeners>
                            <Click Handler="if (#{pnlAgregarMiembro}.getForm().isValid()) {Ext.net.DirectMethods.AgregarMiembro();}" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </Z:ZFormPanel>
        </Items>
    </Z:ZModalWindow>

    <Z:ZModalWindow 
        ID="wndAgregarCancion" 
        runat="server" 
        Title="Agregar Canción"  
        Width="550px"
        Height="355px">
        <Items>
            <Z:ZFormPanel ID="pnlAgregarCancion" runat="server">
                <Items>
                    <Z:ZTextField 
                        ID="filtroCancionTitulo" 
                        runat="server"
                        FieldLabel="Título"
                        AnchorHorizontal="100%"
                        />
                    <Z:ZTextField 
                        ID="filtroCancionArtista" 
                        runat="server"
                        FieldLabel="Artista"
                        AnchorHorizontal="100%"
                        />
                    <Z:ZTextField 
                        ID="filtroCancionDisco" 
                        runat="server"
                        FieldLabel="Disco"
                        AnchorHorizontal="100%"
                        />
                    <Z:ZTextField 
                        ID="filtroCancionTono" 
                        runat="server"
                        FieldLabel="Tono"
                        AnchorHorizontal="100%"
                        />
                    <Z:ZTextField 
                        ID="filtroCancionLetra" 
                        runat="server"
                        FieldLabel="Letra"
                        AnchorHorizontal="100%"
                        />
                    
                    <Z:ZGridPanel ID="registroCancionesEncontradas" Title="Canciones Encontradas" runat="server" StoreID="StoreCancionesEncontradas" Height="150" AnchorHorizontal="100%">
                        <ColumnModel>
                            <Columns>
                                <ext:Column Header="ID" Width="20" DataIndex="Id" />
                                <ext:Column Header="Título" Width="150" DataIndex="Titulo" />
                                <ext:Column Header="Artista" Width="150" DataIndex="Artista" />
                                <ext:Column Header="Disco" Width="75" DataIndex="Disco" />
                                <ext:Column Header="Tono" Width="50" DataIndex="Tono" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                             <ext:RowSelectionModel SingleSelect="true">
                             </ext:RowSelectionModel>
                        </SelectionModel>
                    </Z:ZGridPanel>
                </Items>
                <Buttons>
                    <ext:Button ID="btnCancionBuscar" Icon="Magnifier" Text="Buscar Cancion">
                        <Listeners>
                            <Click Handler="Ext.net.DirectMethods.BuscarCancion();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="btnCancionAgregar" Text="Agregar Canción Seleccionada">
                        <Listeners>
                            <Click Handler="if (#{registroCancionesEncontradas}.getSelectionModel().hasSelection()) {Ext.net.DirectMethods.AgregarCancion();}" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </Z:ZFormPanel>
        </Items>
    </Z:ZModalWindow>

    <Z:ZModalWindow 
        ID="wndAgregarEnsayo" 
        runat="server" 
        Title="Agregar/Modificar Ensayo"  
        Width="600px"
        Height="450px">
        <Items>
            <Z:ZFormPanel ID="pnlAgregarEnsayo" runat="server">
                <Items>
                    <Z:ZNumberField
                        ID="registroEnsayoId"
                        runat="server"
                        Hidden="true">
                    </Z:ZNumberField>
                    <Z:ZCompositeField runat="server" FieldLabel="Fecha/Horas" AnchorHorizontal="100%">
                        <Items>
                            <Z:ZDateField 
                                ID="registroEnsayoFecha" 
                                runat="server"
                                FieldLabel="Fecha del Ensayo"
                                Flex="1"
                                AllowBlank="false"
                                />
                            <Z:ZComboBox
                                ID="registroEnsayoHoraInicio" 
                                runat="server"
                                FieldLabel="Hora del Día (inicio)"
                                StoreID="StoreHorasDelDia"
                                Flex="1"
                                AllowBlank="false"
                                />
                             <Z:ZComboBox
                                ID="registroEnsayoHoraFin" 
                                runat="server"
                                FieldLabel="Hora del Día (fin)"
                                StoreID="StoreHorasDelDia"
                                Flex="1"
                                AllowBlank="false"
                                />
                        </Items>
                    </Z:ZCompositeField>
                    <Z:ZGridPanel ID="registroEnsayoMiembros" Title="Miembros para el Ensayo" runat="server" StoreID="StoreEnsayoMiembros" Height="350" AnchorHorizontal="100%" AgregarColumnaParaBorrar="true">
                        <ColumnModel>
                            <Columns>
                                <ext:Column Header="Nombre" Width="250" DataIndex="Nombre" />
                                <ext:Column Header="Instrumento" Width="50" DataIndex="Instrumento" />
                                <ext:CheckColumn Header="Asistencia" Width="30" DataIndex="Asistencia" Editable="true">
                                    <Editor>
                                        <ext:Checkbox runat="server" AllowBlank="false" />
                                    </Editor>
                                </ext:CheckColumn>
                                    <ext:CheckColumn Header="Retraso" Width="30" DataIndex="Retraso" Editable="true">
                                    <Editor>
                                        <ext:Checkbox runat="server" AllowBlank="false" />
                                    </Editor>
                                </ext:CheckColumn>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                             <ext:RowSelectionModel SingleSelect="true">
                             </ext:RowSelectionModel>
                        </SelectionModel>
                    </Z:ZGridPanel>
                </Items>
                <Buttons>
                    <ext:Button ID="btnEnsayoAgregar" Text="Agregar/Modificar Ensayo">
                    <Listeners>
                        <Click Handler="if (#{pnlAgregarEnsayo}.getForm().isValid()) {Ext.net.DirectMethods.GuardarEnsayo(getModifiedRows(#{registroEnsayoMiembros}));}" />
                    </Listeners>
                    </ext:Button>
                </Buttons>
            </Z:ZFormPanel>
        </Items>
    </Z:ZModalWindow>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="cphFiltros" runat="server">
   <Z:ZPanelCatalogo ID="pnlFiltros" runat="server" Height="50px" LabelWidth="130">
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

                                <Z:ZCompositeField runat="server" FieldLabel="Fecha/Horas">
                                    <Items>
                                        <Z:ZDateField 
                                            ID="filtroFecha" 
                                            runat="server"
                                            FieldLabel="Fecha del Evento"
                                            Flex="1"
                                            />
                                        <Z:ZMultiCombo
                                            ID="filtroHoraDelDiaInicio" 
                                            runat="server"
                                            FieldLabel="Hora del Día (inicio)"
                                            StoreID="StoreHorasDelDia"
                                            Flex="1"
                                            />
                                        <Z:ZMultiCombo
                                            ID="filtroHoraDelDiaFin" 
                                            runat="server"
                                            FieldLabel="Hora del Día (fin)"
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
                                <Z:ZTextField 
                                    ID="filtroDescripcion" 
                                    runat="server"
                                    FieldLabel="Descripción"
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
    <Z:ZPanelCatalogo ID="pnlEdicion" runat="server" Height="600" LabelWidth="130" AutoWidth="true">
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
                                    MinLength="5"
                                    AllowBlank="false"
                                    />

                                <Z:ZCompositeField ID="ZCompositeField1" runat="server" FieldLabel="Fecha/Horas">
                                    <Items>
                                        <Z:ZDateField 
                                            ID="registroFecha" 
                                            runat="server"
                                            FieldLabel="Fecha del Evento"
                                            Flex="1"
                                            AllowBlank="false"
                                            />
                                        <Z:ZComboBox
                                            ID="registroHoraDelDiaInicio" 
                                            runat="server"
                                            FieldLabel="Hora del Día (inicio)"
                                            StoreID="StoreHorasDelDia"
                                            Flex="1"
                                            AllowBlank="false"
                                            />
                                        <Z:ZComboBox
                                            ID="registroHoraDelDiaFin" 
                                            runat="server"
                                            FieldLabel="Hora del Día (fin)"
                                            StoreID="StoreHorasDelDia"
                                            Flex="1"
                                            AllowBlank="false"
                                            />
                                    </Items>
                                </Z:ZCompositeField>

                                 <Z:ZGridPanel ID="registroMiembros" Title="Miembros para el Evento" runat="server" StoreID="StoreMiembros" Height="570" AgregarColumnaParaBorrar="true">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column Header="Nombre" Width="200" DataIndex="Nombre">
                                                <Renderer Fn="Ext.Renderers.DetallesDeMiembro" />
                                            </ext:Column>
                                            <ext:Column Header="Instrumento" Width="75" DataIndex="Instrumento" />
                                            <ext:CheckColumn Header="Asistencia" Width="30" DataIndex="Asistencia" Editable="true">
                                                <Editor>
                                                    <ext:Checkbox runat="server" AllowBlank="false" />
                                                </Editor>
                                            </ext:CheckColumn>
                                              <ext:CheckColumn Header="Retraso" Width="30" DataIndex="Retraso" Editable="true">
                                                <Editor>
                                                    <ext:Checkbox runat="server" AllowBlank="false" />
                                                </Editor>
                                            </ext:CheckColumn>
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                         <ext:RowSelectionModel SingleSelect="true">
                                         </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <Buttons>
                                        <ext:Button runat="server" ID="btnAgregarNuevoMiembro" Icon="Add" Text="Agregar...">
                                            <Listeners>
                                                <Click Handler="#{wndAgregarMiembro}.show();" />
                                            </Listeners>
                                        </ext:Button>
                                    </Buttons>
                                </Z:ZGridPanel>

                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                    
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colEdicion2" runat="server" Layout="Form">
                            <Items>

                                 <Z:ZGridPanel ID="registroCanciones" Title="Canciones para el Evento" runat="server" StoreID="StoreCanciones" Height="430" AnchorHorizontal="99%" AgregarColumnaParaBorrar="true">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column Header="Título" Width="150" DataIndex="Titulo">
                                                <Renderer Fn="Ext.Renderers.DetallesDeCancion" />
                                            </ext:Column>
                                            <ext:Column Header="Artista" Width="150" DataIndex="Artista" />
                                            <ext:Column Header="Disco" Width="100" DataIndex="Disco" />
                                            <ext:Column Header="Tono" Width="25" DataIndex="Tono" />
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                         <ext:RowSelectionModel SingleSelect="true">
                                         </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <Buttons>
                                        <ext:Button runat="server" ID="btnAgregarNuevaCancion" Icon="Add" Text="Agregar...">
                                            <Listeners>
                                                <Click Handler="#{wndAgregarCancion}.show();" />
                                            </Listeners>
                                        </ext:Button>
                                    </Buttons>
                                </Z:ZGridPanel>

                                <Z:ZGridPanel ID="registroEnsayos" Title="Ensayos para el Evento" runat="server" StoreID="StoreEnsayos" Height="260" AnchorHorizontal="99%" AgregarColumnaParaBorrar="true" AgregarColumnaParaEditar="true" ManejadorColumnaParaEditar="(function(registro){Ext.net.DirectMethods.CargarEnsayo(registro.id);})">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column Header="Fecha" Width="50" DataIndex="Fecha">
                                                <Renderer Fn="Ext.Renderers.IsoDateSimple" />
                                            </ext:Column>
                                            <ext:Column Header="Día" Width="75" DataIndex="Fecha">
                                                <Renderer Fn="Ext.Renderers.IsoDateDayOfWeekOnly" />
                                            </ext:Column>
                                            <ext:Column Header="Hora (inicio)" Width="50" DataIndex="HoraInicio" />
                                            <ext:Column Header="Hora (fin)" Width="50" DataIndex="HoraFin" />
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel SingleSelect="true">
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <Buttons>
                                        <ext:Button runat="server" ID="btnAgregarNuevoEnsayo" Icon="Add" Text="Agregar...">
                                            <Listeners>
                                                <Click Handler="Ext.net.DirectMethods.CargarEnsayo(-1);" />
                                            </Listeners>
                                        </ext:Button>
                                    </Buttons>
                                </Z:ZGridPanel>

                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Items>
    </Z:ZPanelCatalogo>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphGridDeResultados" runat="server">
  <Z:ZGridCatalogo ID="GridDeResultados" runat="server" StoreID="StoreResultados" Layout="FitLayout">
        <ColumnModel ID="ColumnModel1" runat="server">
            <Columns>
                <ext:Column Header="ID" Width="20" DataIndex="Id" />
                <ext:Column Header="Descripción" Width="225" DataIndex="Descripcion" />
                <ext:Column Header="Fecha" Width="50" DataIndex="Fecha">
                    <Renderer Fn="Ext.Renderers.IsoDateSimple" />
                </ext:Column>
                <ext:Column Header="Día" Width="75" DataIndex="Fecha">
                    <Renderer Fn="Ext.Renderers.IsoDateDayOfWeekOnly" />
                </ext:Column>
                <ext:Column Header="Hora (inicio)" Width="50" DataIndex="HoraInicio" />
                <ext:Column Header="Hora (fin)" Width="50" DataIndex="HoraFin" />
            </Columns>
        </ColumnModel>
    </Z:ZGridCatalogo>
</asp:Content>
