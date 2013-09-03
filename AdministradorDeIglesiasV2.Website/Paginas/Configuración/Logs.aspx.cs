using System;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtensionMethods;
using Ext.Net;
using ZagueEF.Core;
using ZagueEF.Core.Web;
using ZagueEF.Core.Web.Interfaces;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Constantes;
using AdministradorDeIglesiasV2.Core.Modelos;
using AdministradorDeIglesiasV2.Core.Manejadores;

namespace AdministradorDeIglesiasV2.Website.Paginas.Configuracion
{
    public partial class Logs : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (string file in Directory.GetFiles(Server.MapPath(ConfigurationManager.AppSettings["DirDeVisorDeLogs"])))
            {
                cboLogs.Items.Add(Path.GetFileName(file));
            }

            StringBuilder sbAppSettings = new StringBuilder(255);
            sbAppSettings.AppendLine();
            foreach (string key in ConfigurationManager.AppSettings)
            {
                sbAppSettings.AppendLine(string.Format("{0} = {1}", key, ConfigurationManager.AppSettings[key]));
            }
            txtPropiedades.Text = sbAppSettings.ToString();
        }

        [DirectMethod(ShowMask = true)]
        public void CargarLog(string logFileName)
        {
            if (!string.IsNullOrEmpty(logFileName))
            {
                string path = Server.MapPath(string.Format("~/logs/{0}", logFileName));
                if (File.Exists(path))
                {
                    using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader streamReader = new StreamReader(fileStream))
                        {
                            txtLog.Text = streamReader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    txtLog.Text = "Log inexistente!!!";
                }
            }
        }

    }
}