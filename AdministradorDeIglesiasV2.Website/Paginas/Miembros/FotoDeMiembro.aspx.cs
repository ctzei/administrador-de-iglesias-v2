using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZagueEF.Core;
using ZagueEF.Core.Web;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Modelos;
using ExtensionMethods;

namespace AdministradorDeIglesiasV2.Website.Paginas.Miembros
{
    public partial class FotoDeMiembro : PaginaBase
    {
        static readonly int fotoTamanoMaxKBytes = 30;

        protected void Page_Load(object sender, EventArgs e)
        {
            string sMiembroId = Request["miembroId"];
            Response.Clear();
            
            if (string.IsNullOrEmpty(sMiembroId))
            {
                throw new ExcepcionAplicacion("Esta pantalla necesita tener el miembroId como parametro de entrada.");
            }
            else
            {
                int iMiembroId;
                if (int.TryParse(sMiembroId, out iMiembroId))
                {
                    byte[] foto;

                    //Se esta CARGANDO la foto
                    if (Request.Files.Count > 0)
                    {
                        //Primero validamos si el formato de imagen es valido
                        string[] tiposDeImagenesSoportadas = new string[]{"jpg", "jpeg"};
                        if (tiposDeImagenesSoportadas.Contains(System.IO.Path.GetExtension(Request.Files[0].FileName).TrimStart('.')))
                        {
                            Response.ContentType = "text/html";

                            if (Request.Files[0].ContentLength <= (fotoTamanoMaxKBytes * 1024))
                            {
                                HttpPostedFile file = Request.Files[0];
                                foto = ObtenerBytesDesdeStream(file.InputStream);

                                if (foto != null)
                                {
                                    MiembroFoto mFoto;
                                    mFoto = (from m in SesionActual.Instance.getContexto<IglesiaEntities>().MiembroFoto where m.MiembroId == iMiembroId select m).SingleOrDefault();

                                    //Si NO existe una foto previa se creara un nuevo registro, de lo contrario se actualizara el existente
                                    if (mFoto == null)
                                    {
                                        mFoto = new MiembroFoto();
                                    }

                                    mFoto.MiembroId = iMiembroId;
                                    mFoto.Foto = foto;
                                    mFoto.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());

                                    Response.Write(new { success = true }.ToJson());
                                }
                                else
                                {
                                    throw new ExcepcionAplicacion(string.Format("No se pudo obtener el arreglo de bytes desde el InputStream del request. [MiembroId = {0}]", iMiembroId));
                                }
                            }
                            else
                            {
                                Response.Write(new { sucess = false, error = string.Format("El tamaño de la imágen excede el límite permitido de <b>{0}KB</b>. Cambios no guardados.", fotoTamanoMaxKBytes) }.ToJson());
                            }
                        }
                        else
                        {
                            Response.Write(new {sucess = false, error = string.Format("El formato de la imágen cargada no es válido. Solo los siguientes formatos son válidos: <b>{0}</b>. Cambios no guardados.", string.Join(", ", tiposDeImagenesSoportadas))}.ToJson());
                        }
                    }

                    //Se esta SIRVIENDO la foto
                    else
                    {
                        Response.ContentType = "image/jpeg";
                        foto = (from m in SesionActual.Instance.getContexto<IglesiaEntities>().MiembroFoto where m.MiembroId == iMiembroId select m.Foto).SingleOrDefault();
                        
                        if (foto != null)
                        {
                            Response.BinaryWrite(foto);
                        }
                        else
                        {
                            Response.BinaryWrite(ObtenerBytesDesdeArchivo(Server.MapPath("~/Recursos/img/sin_foto.jpg")));
                        }
                    }

                    Response.End();
                }
                else
                {
                    throw new ExcepcionAplicacion("El parametro de entrada miembroId necesita ser un numero entero.");
                }
            }

        }

        public byte[] ObtenerBytesDesdeArchivo(string fullFilePath)
        {
            byte[] bytes;

            //Este metodo esta limitado hasta 2^32 bytes (4.2 GB)
            using (System.IO.FileStream fs = System.IO.File.OpenRead(fullFilePath)){
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
            }

            return bytes;
        }

        public byte[] ObtenerBytesDesdeStream(System.IO.Stream stream)
        {
            byte[] bytes;

            //Este metodo esta limitado hasta 2^32 bytes (4.2 GB)
            using (stream)
            {
                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, Convert.ToInt32(stream.Length));
            }

            return bytes;
        }
    }
}