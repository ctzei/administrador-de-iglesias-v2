<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Buscador.ascx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.UserControls.Buscador" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>

<script src='<%= ResolveUrl("~/Recursos/js/Paginas/UserControls/buscador.js")%>'type="text/javascript"></script>

<ext:Store ID="StoreListadoDeObjetos" runat="server" IgnoreExtraFields="true">
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

<ext:Panel ID="buscadorMain" runat="server" BodyBorder="false" PaddingSummary="0px 20px 0px 0px">
    <Items>
        <Z:ZGridPanel 
            ID="gridDeListadoDeObjetos" 
            runat="server" 
            Height="150" 
            AutoWidth="true"
            StoreID="StoreListadoDeObjetos"
            ButtonAlign="Left">
            <ColumnModel ID="ColumnModel1" runat="server">
                <Columns>
                    <ext:CommandColumn Width="25">
                        <Commands>
                            <ext:GridCommand Icon="Delete" CommandName="Borrar">
                                <ToolTip Text="Borrar" />
                            </ext:GridCommand>
                        </Commands>
                    </ext:CommandColumn>
                    <ext:Column Header="ID" Width="25" DataIndex="Id" />
                    <ext:Column Header="Descripción" Width="300" DataIndex="Descripcion" />
                </Columns>
            </ColumnModel>
            <Buttons>
                <ext:Button ID="cmdMostrarBuscador" runat="server" Text="Agregar..." Icon="Add" OnClientClick="#{wndBuscador}.show();" AnchorHorizontal="100%"></ext:Button>
            </Buttons>
            <SelectionModel>
                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true" />
            </SelectionModel>
            <Listeners>
                <Command Handler="buscador.comandoDelGridDeListadoDeObjetos(command, record, #{gridDeListadoDeObjetos});" />
            </Listeners>
        </Z:ZGridPanel>
    </Items>
</ext:Panel>

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
                <ext:Button ID="cmdAceptarRegistroSeleccionado" runat="server" Text="Aceptar" Width="100px">
                    <Listeners>
                        <Click Handler="buscador.agregarObjeto(#{gridDeObjetosEncontrados}, #{gridDeListadoDeObjetos}); #{wndBuscador}.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </Z:ZGridPanel>
    </Items>
</ext:Window>