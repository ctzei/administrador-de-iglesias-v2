<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="InicioDeSesionSecundaria.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.InicioDeSesionSecundaria" %>

<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<%@ Register Src="../UserControls/BuscadorSimple.ascx" TagName="BuscadorSimple" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <ext:Viewport ID="Viewport" runat="server" Layout="Anchor">
        <Items>

            <ext:Panel runat="server" PaddingSummary="10px" BodyBorder="false" MinHeight="200px" LabelStyle="font-weight:bold;">
                <Items>
                    <ext:Label LabelStyle="font-weight:bold;" StyleSpec="font-weight:bold;" runat="server" Text="El inicio de sesión secundaria es el hecho de iniciar sesión con algún otro usuario (sin la necesidad de su password), siempre y cuando este sea parte de la red del usuario actual. Al iniciar una sesión secundaria se cerrará la sesión actual (y se perderan todos los cambios no guardados)."></ext:Label>
                </Items>
            </ext:Panel>

            <ext:Panel runat="server" PaddingSummary="10px" BodyBorder="false" ButtonAlign="Left">
                <Items>

                    <ext:Container runat="server" Layout="Form" AnchorHorizontal="100%">
                        <Content>
                            <uc1:BuscadorSimple ID="registroMiembro" runat="server" LabelWidth="200" FieldLabel="Miembro a iniciar sesión como" TipoDeObjeto="Miembro" />
                        </Content>
                    </ext:Container>
                </Items>
                <Buttons>
                    <ext:Button ID="cmdIniciarSesion" runat="server" Text="Iniciar Sesión">
                        <Listeners>
                            <Click Handler="iniciarSesion();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Panel>

        </Items>
    </ext:Viewport>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooter" runat="server">

    <script type="text/javascript">
        function iniciarSesion() {
            Ext.net.DirectMethods.IniciarSesionClick({
                success: function (result) {
                    if (result == true) {
                        if (window.self === window.top) { 
                            window.location.href = ResolveUrl("~/");
                        } else {
                            window.top.location.reload(true);
                        }
                    }
                }
            });
            return false;
        }
    </script>

</asp:Content>


