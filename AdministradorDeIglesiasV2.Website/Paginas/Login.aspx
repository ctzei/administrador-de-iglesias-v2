<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Login" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <ext:Viewport ID="LoginViewport" runat="server" Layout="Center">
        <Items>
            <ext:FormPanel ID="pnlLogin" runat="server" Width="350px" AutoHeight="true" Title="Inicio de Sesión" Layout="Form" PaddingSummary="10px 20px" ButtonAlign="Right">
                <Defaults>
                    <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
                </Defaults>
                <Items>
                    <Z:ZTextField 
                        ID="txtUsername" 
                        runat="server" 
                        FieldLabel="Email" 
                        AnchorHorizontal="100%"
                        Vtype="email"
                        />
                    <Z:ZTextField 
                        ID="txtPassword" 
                        runat="server" 
                        InputType="Password" 
                        FieldLabel="Contraseña" 
                        AnchorHorizontal="100%"
                        MinLength="6"
                        />
                    <Z:ZCheckbox
                        ID="chkRecordarme"
                        runat="server"
                        FieldLabel="Recordarme"
                        AnchorHorizontal="100%"
                        Checked="true"
                        />
                </Items>
                <Buttons>
                    <ext:LinkButton ID="cmdVersionMobile" runat="server" Text="Versión Móvil..." />
                    <ext:LinkButton ID="cmdAbrirVentanaDeCambiarContrasena" runat="server" Text="Cambiar Contraseña...">
                        <Listeners>
                            <Click Handler="if (#{pnlLogin}.getForm().isValid()) {#{wndCambiarContrasena}.show();}" />   
                        </Listeners>
                    </ext:LinkButton>              
                    <ext:Button ID="cmdAceptar" runat="server" Text="Aceptar">
                        <Listeners>
                            <Click Handler="if (#{pnlLogin}.getForm().isValid()) {Ext.net.DirectMethods.LoginClick();}" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
    </ext:Viewport>

    <ext:Viewport ID="LogoViewport" runat="server" Layout="Center">
        <Items>
            <ext:Container ID="LogoContainer" runat="server" AutoHeight="true" Height="250">
                <Content>
                    <div id="divLogo" class="logo"></div>
                </Content>
            </ext:Container>
        </Items>
    </ext:Viewport>

    <ext:Window 
        ID="wndCambiarContrasena" 
        runat="server" 
        Title="Cambiar Contraseña"  
        Height="125px" 
        Width="300px"
        Padding="5"
        Collapsible="false"
        Resizable="false"
        Draggable="false"
        Hidden="true"
        Modal="true">
        <Content>
             <ext:FormPanel ID="pnlCambiarContrasena" runat="server" Width="275px" AutoHeight="true" Layout="Form" PaddingSummary="10px 20px" MonitorPoll="500" MonitorValid="true" ButtonAlign="Center" LabelWidth="120">
                <Defaults>
                    <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
                </Defaults>
                <Items>
                    <Z:ZTextField 
                        ID="txtNewPassword" 
                        runat="server" 
                        FieldLabel="Nueva Contraseña" 
                        AnchorHorizontal="100%"
                        InputType="Password" 
                        MinLength="6"
                        />
                </Items>
                <Buttons>
                    <ext:Button ID="cmdCambiarContrasena" runat="server" Text="Aceptar" Causesvalidation="true">
                        <Listeners>
                            <Click Handler="if (#{pnlCambiarContrasena}.getForm().isValid()) {Ext.net.DirectMethods.CambiarContrasenaClick(); return true;} else {return false;}" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Content>
    </ext:Window>

</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
    <script src="<%= ResolveUrl("~/Recursos/js/Paginas/login.js")%>" type="text/javascript"></script>
</asp:Content>

