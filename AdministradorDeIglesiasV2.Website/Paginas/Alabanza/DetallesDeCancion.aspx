<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="DetallesDeCancion.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Alabanza.DetallesDeCancion" %>

<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <ext:Viewport ID="Viewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:FormPanel ID="pnlDatosGenerales" runat="server" Title="Datos Generales" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false" LabelWidth="200" Region="Center">
                <Items>

                    <ext:DisplayField
                        ID="registroId"
                        runat="server"
                        FieldLabel="Id" />

                    <Z:ZCompositeField runat="server" FieldLabel="Título/Artista/Disco">
                        <Items>
                            <Z:ZTextField
                                ID="registroTitulo"
                                runat="server"
                                FieldLabel="Título"
                                ReadOnly="true"
                                Flex="1" />
                            <Z:ZTextField
                                ID="registroArtista"
                                runat="server"
                                FieldLabel="Artista"
                                ReadOnly="true"
                                Flex="1" />
                            <Z:ZTextField
                                ID="registroDisco"
                                runat="server"
                                FieldLabel="Disco"
                                ReadOnly="true"
                                Flex="1" />
                        </Items>
                    </Z:ZCompositeField>

                    <Z:ZCompositeField runat="server" FieldLabel="Título/Artista/Disco (Alternativos)">
                        <Items>
                            <Z:ZTextField
                                ID="registroTituloAlternativo"
                                runat="server"
                                FieldLabel="Título"
                                ReadOnly="true"
                                Flex="1" />
                            <Z:ZTextField
                                ID="registroArtistaAlternativo"
                                runat="server"
                                FieldLabel="Artista"
                                ReadOnly="true"
                                Flex="1" />
                            <Z:ZTextField
                                ID="registroDiscoAlternativo"
                                runat="server"
                                FieldLabel="Disco"
                                ReadOnly="true"
                                Flex="1" />
                        </Items>
                    </Z:ZCompositeField>

                    <Z:ZHyperLink
                        ID="registroLiga"
                        runat="server"
                        Target="_blank"
                        FieldLabel="Liga" />

                    <Z:ZHyperLink
                        ID="registroLigaAlternativa"
                        runat="server"
                        Target="_blank"
                        FieldLabel="Liga (Alternativa)" />

                    <ext:DisplayField
                        ID="registroTono"
                        runat="server"
                        FieldLabel="Tono" />

                    <ext:Container runat="server" Layout="Form" LabelWidth="200" Wrap="off">
                        <Items>
                            <ext:TextArea
                                ID="registroLetra"
                                runat="server"
                                FieldLabel="Letra"
                                ReadOnly="true"
                                AnchorHorizontal="97%"
                                Height="300px">
                            </ext:TextArea>
                        </Items>
                    </ext:Container>

                </Items>
            </ext:FormPanel>
        </Items>
    </ext:Viewport>

</asp:Content>
