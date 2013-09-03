using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Modelos;
using ZagueEF.Core;

namespace AdministradorDeIglesiasV2.Core.Manejadores
{
    public class ManejadorDeServidores
    {
        #region Manejo de Capitanes e Integrantes

        public RegistroBasico ObtenerCapitan(int usuarioId)
        {
            ServidorCapitan entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().ServidorCapitan where o.MiembroId == usuarioId select o).FirstOrDefault();
            if (entidad != null)
            {
                RegistroBasico rtn = new RegistroBasico(); ;
                rtn.Id = entidad.Id;
                rtn.Descripcion = entidad.Miembro.NombreCompleto;
                return rtn;
            }
            else
            {
                return new RegistroBasico();
            }
        }

        public List<RegistroBasico> ObtenerIntegrantesPorCapitan(int capitanUsuarioId)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().ServidorIntegrante where 
                        o.ServidorCapitan.MiembroId == capitanUsuarioId &&
                        o.Miembro.Borrado == false &&
                        o.ServidorCapitan.Borrado == false &&
                        o.Borrado == false 
                    select new RegistroBasico
                    { 
                        Id = o.MiembroId,
                        Descripcion = o.Miembro.Primer_Nombre + " " + o.Miembro.Segundo_Nombre + " " + o.Miembro.Apellido_Paterno + " " + o.Miembro.Apellido_Materno + " (" + o.Miembro.Email + ")"
                    }).ToList<RegistroBasico>();
        }

        /// <summary>
        /// Agrega y elimina integrantes a un grupo de servidores. El grupo de servidores depende un capitan.
        /// </summary>
        /// <param name="capitanUsuarioId">Id del miembro "capitan" del grupo de servidores</param>
        /// <param name="integrantesUsuariosNuevosId">Listado de id's de los miembros "integrantes" a agregar al grupo de servidores</param>
        /// <param name="integrantesUsuariosEliminadosId">Listado de id's de los miembros "integrantes" a eliminar del grupo de servidores</param>
        /// <returns>True = Es un nuevo grupo; False = Es un grupo existente y solo fue una actualizacion</returns>
        public bool GuardarIntegrantesPorCapitan(int capitanUsuarioId, RegistrosHelper.RegistrosDeDatos integrantesNuevosYEliminados)
        {
            bool rtn = false;

            if (capitanUsuarioId > 0 && (integrantesNuevosYEliminados.RegistrosNuevosId.Count > 0 || integrantesNuevosYEliminados.RegistrosEliminadosId.Count > 0))
            {
                ServidorCapitan capitan = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().ServidorCapitan where o.Miembro.MiembroId == capitanUsuarioId select o).FirstOrDefault();
                ServidorIntegrante integrante;

                if (capitan == null)
                {
                    capitan = new ServidorCapitan();
                    capitan.MiembroId = capitanUsuarioId;
                    rtn = true;
                }

                //Agregamos los nuevos integrantes (siempre y cuando no existan previamente...)
                foreach (int integranteUsuarioId in integrantesNuevosYEliminados.RegistrosNuevosId)
                {
                    if (!capitan.ServidorIntegrante.Any(o => o.MiembroId == integranteUsuarioId && o.Borrado == false))
                    {
                        integrante = new ServidorIntegrante();
                        integrante.CapitanId = capitanUsuarioId;
                        integrante.MiembroId = integranteUsuarioId;
                        capitan.ServidorIntegrante.Add(integrante);
                    }
                }

                //Guardamos los cambios, antes de eliminar registros
                capitan.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());

                //Eliminamos los integrantes
                foreach (int integranteUsuarioId in integrantesNuevosYEliminados.RegistrosEliminadosId)
                {
                    integrante = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().ServidorIntegrante where o.MiembroId == integranteUsuarioId && o.ServidorCapitan.MiembroId == capitanUsuarioId && o.Borrado == false select o).FirstOrDefault();
                    if (integrante != null)
                    {
                        integrante.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
                    }
                }
            }

            return rtn; ;
        }

        public void BorrarCapitan(int capitanUsuarioId)
        {
            if (capitanUsuarioId > 0)
            {
                ServidorCapitan capitan = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().ServidorCapitan where o.Miembro.MiembroId == capitanUsuarioId select o).FirstOrDefault();
                if (capitan != null)
                {
                    capitan.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
                }
            }
        }

        #endregion

        #region Manejo de Coordinadores

        #endregion

        #region Manejo de AsistenciasPorSemana de Integrantes

        #endregion
    }
}
