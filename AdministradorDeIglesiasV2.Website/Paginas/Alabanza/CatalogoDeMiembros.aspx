<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="CatalogoDeMiembros.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Alabanza.CatalogoDeMiembros" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<%@ Register src="../UserControls/BuscadorSimple.ascx" tagname="BuscadorSimple" tagprefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
        <ext:Store ID="StoreMiembros" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader IDProperty="Id">
                    <Fields>
                        <ext:RecordField Name="Id"/>
                        <ext:RecordField Name="MiembroId" />
                        <ext:RecordField Name="Nombre" />
                        <ext:RecordField Name="Email" />
                        <ext:RecordField Name="Telefono" />
                        <ext:RecordField Name="Celula" />
                        <ext:RecordField Name="CelulaId" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Viewport ID="viewportMain" runat="server" Layout="Anchor">
            <Items>
                <Z:ZGridPanel ID="grdMiembros" runat="server" AnchorHorizontal="right" AnchorVertical="0" StoreID="StoreMiembros" Title="Miembros" AutoWidth="true" AgregarColumnaParaBorrar="true" ManejadorColumnaParaBorrar="borrarMiembro">
                     <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:Column Header="Nombre" Width="150" DataIndex="Nombre">
                                <Renderer Fn="Ext.Renderers.DetallesDeMiembro" />
                            </ext:Column>
                            <ext:Column Header="Email" Width="125" DataIndex="Email" />
                            <ext:Column Header="Teléfono(s)" Width="125" DataIndex="Telefono" />
                            <ext:Column Header="Célula" Width="125" DataIndex="Celula">
                                <Renderer Fn="Ext.Renderers.DetallesDeCelula" />
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <Buttons>
                        <ext:Button ID="cmdAgregarMiembro" runat="server" Text="Agregar miembro..." >
                            <Listeners>
                                <Click Handler="#{wndAgregarMiembro}.show();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <SelectionModel>
                        <ext:RowSelectionModel SingleSelect="true"></ext:RowSelectionModel>
                    </SelectionModel>
                    <FooterBar>
                        <ext:StatusBar ID="StatusBar1" runat="server">
                            <Items>
                                <ext:DisplayField ID="registroNumeroDeMiembros" runat="server" Text=""></ext:DisplayField>
                            </Items>
                        </ext:StatusBar>
                    </FooterBar>
                </Z:ZGridPanel>
            </Items>
        </ext:Viewport>

    <Z:ZModalWindow 
        ID="wndAgregarMiembro" 
        runat="server" 
        Title="Agregar Miembro"  
        Width="400px"
        Height="110px">
        <Items>
            <Z:ZFormPanel ID="pnlAgregarMiembro" runat="server" BodyBorder="false">
                <Items>
                    <ext:Container runat="server" Layout="Form" AnchorHorizontal="100%">
                        <Content>
                            <uc1:BuscadorSimple ID="registroMiembro" runat="server" LabelWidth="155" FieldLabel="Miembro" TipoDeObjeto="Miembro" />  
                        </Content>
                    </ext:Container>
                </Items>
                <Buttons>
                    <ext:Button ID="btnAgregarMiembro" Text="Agregar">
                        <Listeners>
                            <Click Handler="Ext.net.DirectMethods.AgregarMiembro();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </Z:ZFormPanel>
        </Items>
    </Z:ZModalWindow>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooter" runat="server">
    <script type="text/javascript">
        function borrarMiembro(registro) {
            Ext.Msg.confirm('', 'Desea eliminar el miembro <span class="bold">' + registro.data.Nombre + '</span> del grupo de alabanza?', function (btn) {
                if (btn == 'yes') {
                    Ext.net.DirectMethods.BorrarMiembro(registro.id);
                }
            }, this);
        }
    </script>
</asp:Content>

