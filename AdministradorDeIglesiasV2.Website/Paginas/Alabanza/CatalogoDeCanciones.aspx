<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/MasterPages/Catalogos.master" AutoEventWireup="true" CodeBehind="CatalogoDeCanciones.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Alabanza.CatalogoDeCanciones" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/Paginas/MasterPages/Catalogos.master" %>

<%@ Register src="../UserControls/Miembros/FiltrosMiembros.ascx" tagname="FiltrosMiembros" tagprefix="uc1" %>
<%@ Register src="../UserControls/Miembros/ResultadoMiembros.ascx" tagname="ResultadoMiembros" tagprefix="uc2" %>
<%@ Register src="../UserControls/BuscadorSimple.ascx" tagname="BuscadorSimple" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphStores" runat="server">
        <ext:Store ID="StoreResultados" runat="server" IgnoreExtraFields="true" >
            <Reader>
                <ext:JsonReader IDProperty="Id">
                    <Fields>
                        <ext:RecordField Name="Id"/>
                        <ext:RecordField Name="Titulo" />
                        <ext:RecordField Name="Artista" />
                        <ext:RecordField Name="Disco" />
                        <ext:RecordField Name="TituloAlternativo" />
                        <ext:RecordField Name="ArtistaAlternativo" />
                        <ext:RecordField Name="DiscoAlternativo" />
                        <ext:RecordField Name="Liga" />
                        <ext:RecordField Name="LigaAlternativa" />
                        <ext:RecordField Name="Tono" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFiltros" runat="server">
    <Z:ZPanelCatalogo ID="pnlFiltros" runat="server" Height="200px" LabelWidth="130">
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

                                <Z:ZCompositeField runat="server" FieldLabel="Título/Artista/Disco">
                                    <Items>
                                            <Z:ZTextField 
                                            ID="filtroTitulo" 
                                            runat="server"
                                            FieldLabel="Título"
                                            Flex="1"
                                            />
                                        <Z:ZTextField 
                                            ID="filtroArtista" 
                                            runat="server"
                                            FieldLabel="Artista"
                                            Flex="1"
                                            />
                                        <Z:ZTextField 
                                            ID="filtroDisco" 
                                            runat="server"
                                            FieldLabel="Disco"
                                            Flex="1"
                                            />
                                    </Items>
                                </Z:ZCompositeField>

                                <Z:ZCompositeField runat="server" FieldLabel="Título/Artista/Disco (Alternativos)">
                                    <Items>
                                            <Z:ZTextField 
                                            ID="filtroTituloAlternativo" 
                                            runat="server"
                                            FieldLabel="Título"
                                            Flex="1"
                                            />
                                        <Z:ZTextField 
                                            ID="filtroArtistaAlternativo" 
                                            runat="server"
                                            FieldLabel="Artista"
                                            Flex="1"
                                            />
                                        <Z:ZTextField 
                                            ID="filtroDiscoAlternativo" 
                                            runat="server"
                                            FieldLabel="Disco"
                                            Flex="1"
                                            />
                                    </Items>
                                </Z:ZCompositeField>

                                <Z:ZTextField 
                                    ID="filtroLiga" 
                                    runat="server"
                                    FieldLabel="Liga"
                                    />

                                <Z:ZTextField 
                                    ID="filtroLigaAlternativa" 
                                    runat="server"
                                    FieldLabel="Liga (Alternativa)"
                                    />

                                <Z:ZTextField 
                                    ID="filtroTono" 
                                    runat="server"
                                    FieldLabel="Tono"
                                    />
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                    
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colFiltros2" runat="server" Layout="Form" LabelWidth="40" Wrap="off">
                            <Items>
                                <ext:TextArea
                                    ID="filtroLetra" 
                                    runat="server"
                                    FieldLabel="Letra"
                                    AnchorHorizontal="95%"
                                    Height="190px"
                                    >
                                </ext:TextArea>
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Items>
    </Z:ZPanelCatalogo>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphEdicion" runat="server">
    <Z:ZPanelCatalogo ID="pnlEdicion" runat="server" Height="200px" LabelWidth="130">
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

                                <Z:ZCompositeField runat="server" FieldLabel="Título/Artista/Disco">
                                    <Items>
                                        <Z:ZTextField 
                                            ID="registroTitulo" 
                                            runat="server"
                                            FieldLabel="Título"
                                            Flex="1"
                                            AllowBlank="false"
                                            />
                                        <Z:ZTextField 
                                            ID="registroArtista" 
                                            runat="server"
                                            FieldLabel="Artista"
                                            Flex="1"
                                            AllowBlank="false"
                                            />
                                        <Z:ZTextField 
                                            ID="registroDisco" 
                                            runat="server"
                                            FieldLabel="Disco"
                                            Flex="1"
                                            />
                                    </Items>
                                </Z:ZCompositeField>

                                <Z:ZCompositeField runat="server" FieldLabel="Título/Artista/Disco (Alternativos)">
                                    <Items>
                                            <Z:ZTextField 
                                            ID="registroTituloAlternativo" 
                                            runat="server"
                                            FieldLabel="Título"
                                            Flex="1"
                                            />
                                        <Z:ZTextField 
                                            ID="registroArtistaAlternativo" 
                                            runat="server"
                                            FieldLabel="Artista"
                                            Flex="1"
                                            />
                                        <Z:ZTextField 
                                            ID="registroDiscoAlternativo" 
                                            runat="server"
                                            FieldLabel="Disco"
                                            Flex="1"
                                            />
                                    </Items>
                                </Z:ZCompositeField>

                                <Z:ZTextField 
                                    ID="registroLiga" 
                                    runat="server"
                                    FieldLabel="Liga"
                                    />

                                <Z:ZTextField 
                                    ID="registroLigaAlternativa" 
                                    runat="server"
                                    FieldLabel="Liga (Alternativa)"
                                    />

                                <Z:ZTextField 
                                    ID="registroTono" 
                                    runat="server"
                                    FieldLabel="Tono"
                                    />
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                    
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colEdicion2" runat="server" Layout="Form" LabelWidth="40" Wrap="off">
                            <Items>
                                <ext:TextArea
                                    ID="registroLetra" 
                                    runat="server"
                                    FieldLabel="Letra"
                                    AnchorHorizontal="95%"
                                    Height="190px"
                                    AllowBlank="false"
                                    >
                                </ext:TextArea>
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Items>
    </Z:ZPanelCatalogo>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphGridDeResultados" runat="server">
      <Z:ZGridCatalogo ID="GridDeResultados" runat="server" StoreID="StoreResultados">
        <ColumnModel ID="ColumnModel1" runat="server">
            <Columns>
                <ext:Column Header="ID" Width="40" DataIndex="Id" />
                <ext:Column Header="Título" Width="100" DataIndex="Titulo" />
                <ext:Column Header="Artista" Width="100" DataIndex="Artista" />
                <ext:Column Header="Disco" Width="100" DataIndex="Disco" />
                <ext:Column Header="Título (Alternativo)" Width="100" DataIndex="TituloAlternativo" />
                <ext:Column Header="Artista (Alternativo)" Width="100" DataIndex="ArtistaAlternativo" />
                <ext:Column Header="Disco (Alternativo)" Width="100" DataIndex="DiscoAlternativo" />
                <ext:Column Header="Liga" Width="250" DataIndex="Liga">
                    <Renderer Fn="Ext.Renderers.Url" />
                </ext:Column>
                <ext:Column Header="Liga (Alternativa)" Width="250" DataIndex="LigaAlternativa">
                    <Renderer Fn="Ext.Renderers.Url" />
                </ext:Column>
                <ext:Column Header="Tono" Width="50" DataIndex="Tono" />
            </Columns>
        </ColumnModel>
    </Z:ZGridCatalogo>
</asp:Content>