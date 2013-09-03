<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuscadorSimple.ascx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.UserControls.BuscadorSimple" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>

<script src='<%= ResolveUrl("~/Recursos/js/Paginas/UserControls/buscadorSimple.js")%>'type="text/javascript"></script>

<ext:Store ID="StoreObjetoSeleccionado" runat="server" IgnoreExtraFields="true">
    <Reader>
        <ext:JsonReader IDProperty="Id">
            <Fields>
                <ext:RecordField Name="Id"/>
                <ext:RecordField Name="Descripcion" />
            </Fields>
        </ext:JsonReader>
    </Reader>
</ext:Store>

<ext:Store ID="StoreObjetosEncontrados" runat="server" IgnoreExtraFields="true">
    <Reader>
        <ext:JsonReader IDProperty="Id">
            <Fields>
                <ext:RecordField Name="Id" />
                <ext:RecordField Name="Descripcion" />
                <ext:RecordField Name="RowColor" />
            </Fields>
        </ext:JsonReader>
    </Reader>           
</ext:Store>

<Z:ZCompositeField ID="BuscadorSimpleContenedor" runat="server" Height="24px">
    <Items>
        <Z:ZComboBox 
            ID="objetoSeleccionado" 
            runat="server"
            FieldLabel="MiTitulo"
            Text="-1"
            AllowBlank="false"
            ReadOnly="true"
            Flex="1"
            StoreID="StoreObjetoSeleccionado"
            MsgTarget="Title"
            Width="180"
            >
        </Z:ZComboBox>
        <ext:Button ID="buscarObjeto" runat="server" Anchor="right" Icon="Magnifier" ToolTip="Buscar..." >
            <Listeners>
                <Click Handler="buscadorSimple.abrirVentanaDeBusqueda(#{objetoSeleccionado}, #{wndBuscador});" />
            </Listeners>
        </ext:Button>
        <ext:Button ID="verObjeto" runat="server" Anchor="right" Icon="Eye" ToolTip="Ver Detalles..." >
            <Listeners>
                <Click Handler="buscadorSimple.verObjeto(#{objetoSeleccionado});" />
            </Listeners>
        </ext:Button>
    </Items>
</Z:ZCompositeField>

<ext:Window 
    ID="wndBuscador" 
    runat="server" 
    Title="Buscar..."  
    Height="380px" 
    Width="540px"
    Padding="5"
    Collapsible="false"
    Resizable="false"
    Draggable="false"
    Hidden="true"
    Modal="true"
    LabelWidth="75">
    <Items>
        <Z:ZTextField 
            ID="registroConceptoABuscar" 
            runat="server"
            FieldLabel="Buscar"
            AllowBlank="true"
            Width="450">
            <Listeners>
                <AfterRender Handler="processOnEnter('#{registroConceptoABuscar}', '#{cmdBuscarConcepto}');" />
            </Listeners>
        </Z:ZTextField>
        <Z:ZGridPanel ID="gridDeObjetosEncontrados" runat="server" StoreID="StoreObjetosEncontrados" ColorearFilas="True" Height="310px" Footer="true">
            <ColumnModel ID="ColumnModel2" runat="server">
                <Columns>
                    <ext:Column Header="ID" Width="40" DataIndex="Id" />
                    <ext:Column Header="Descripción" Width="200" DataIndex="Descripcion" />
                </Columns>
            </ColumnModel>
            <Listeners>
                <RowDblClick Handler="Ext.getCmp('#{cmdAceptarRegistroSeleccionado}').fireEvent('click');" />
            </Listeners>
            <BottomBar>
                <ext:StatusBar runat="server">
                    <Items>
                        <ext:DisplayField ID="registroNumeroDeResultados" runat="server" Text=""></ext:DisplayField>
                    </Items>
                </ext:StatusBar>
            </BottomBar>
            <SelectionModel>
                <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" SingleSelect="true" />
            </SelectionModel>
            <Buttons>
                <ext:Button ID="cmdBuscarConcepto" runat="server" Icon="Magnifier"  Text="Buscar" Width="100px">
                    <DirectEvents>
                        <Click OnEvent="cmdBuscarConcepto_Click">
                            <EventMask ShowMask="true" />
                        </Click>
                    </DirectEvents>
                </ext:Button>
                 <ext:Button ID="cmdVerDetalles" runat="server" Text="Ver Detalle..." Width="100px">
                    <Listeners>
                         <Click Handler="buscadorSimple.seleccionarObjeto(#{gridDeObjetosEncontrados}, #{objetoSeleccionado}); buscadorSimple.verObjeto(#{objetoSeleccionado}); buscadorSimple.borrarObjeto(#{objetoSeleccionado});" />
                    </Listeners>
                </ext:Button>
                <ext:Button ID="cmdAceptarRegistroSeleccionado" runat="server" Text="Aceptar" Width="100px">
                    <Listeners>
                        <Click Handler="buscadorSimple.seleccionarObjeto(#{gridDeObjetosEncontrados}, #{objetoSeleccionado}); #{wndBuscador}.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </Z:ZGridPanel>
    </Items>
</ext:Window>