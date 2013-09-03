#region Using

using System;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.IO;
using System.IO.Compression;
using System.Web.UI;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

#endregion

/// <summary>
/// Compresses the output using standard deflate.
/// </summary>
/// 
namespace ZagueEF.Core.Web.CompressionModule
{
    public sealed class ZCompressor : IHttpModule
    {

        string[] compressedExtensions = { ".aspx", ".amsx"};

        #region IHttpModule Members

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module 
        /// that implements <see cref="T:System.Web.IHttpModule"></see>.
        /// </summary>
        void IHttpModule.Dispose()
        {
            // Nothing to dispose; 
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"></see> 
        /// that provides access to the methods, properties, and events common to 
        /// all application objects within an ASP.NET application.
        /// </param>
        void IHttpModule.Init(HttpApplication context)
        {
            // For page compression
            context.PostRequestHandlerExecute += new EventHandler(context_PostRequestHandlerExecute);
        }

        #endregion

        private const string DEFLATE = "deflate";
        private const string GZIP = "gzip";

        #region Compress page

        /// <summary>
        /// Handles the BeginRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void context_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            if ((compressedExtensions.Contains(app.Context.Request.CurrentExecutionFilePathExtension) == true) && (app.Request["HTTP_X_MICROSOFTAJAX"] == null))
            {
                if (IsEncodingAccepted(DEFLATE))
                {
                    app.Response.Filter = new DeflateStream(app.Response.Filter, CompressionMode.Compress);
                    SetEncoding(DEFLATE);
                }
                else if (IsEncodingAccepted(GZIP))
                {
                    app.Response.Filter = new GZipStream(app.Response.Filter, CompressionMode.Compress);
                    SetEncoding(GZIP);
                }
            }
        }

        /// <summary>
        /// Checks the request headers to see if the specified
        /// encoding is accepted by the client.
        /// </summary>
        private static bool IsEncodingAccepted(string encoding)
        {
            HttpContext context = HttpContext.Current;
            return context.Request.Headers["Accept-encoding"] != null && context.Request.Headers["Accept-encoding"].Contains(encoding);
        }

        /// <summary>
        /// Adds the specified encoding to the response headers.
        /// </summary>
        /// <param name="encoding"></param>
        private static void SetEncoding(string encoding)
        {
            if (encoding != null)
                HttpContext.Current.Response.AppendHeader("Content-encoding", encoding);
        }

        #endregion

    }
}