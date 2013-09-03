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
    public class ManejadorDeLideresDeCelula
    {
        public bool GuardarLiderzagoDeCelulas(int miembroId, RegistrosHelper.RegistrosDeDatos celulasNuevasYEliminadas)
        {
            bool rtn = false;

            if (miembroId > 0 && (celulasNuevasYEliminadas.RegistrosNuevosId.Count > 0 || celulasNuevasYEliminadas.RegistrosEliminadosId.Count > 0))
            {
                Miembro miembro = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.MiembroId == miembroId select o).FirstOrDefault();
                CelulaLider celulaLider;

                if (miembro == null) { throw new ExcepcionReglaNegocio(Literales.RegistroInexistente); }

                //Agregamos las nuevas celulas (siempre y cuando no existan previamente...)
                foreach (int celulaId in celulasNuevasYEliminadas.RegistrosNuevosId)
                {
                    if (!miembro.CelulaLider.Any(o => o.CelulaId == celulaId && o.Borrado == false))
                    {
                        celulaLider = new CelulaLider();
                        celulaLider.MiembroId = miembroId;
                        celulaLider.CelulaId = celulaId;
                        miembro.CelulaLider.Add(celulaLider);
                    }
                }

                //Guardamos los cambios, antes de eliminar registros
                miembro.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());

                //Eliminamos las celulas
                foreach (int celulaId in celulasNuevasYEliminadas.RegistrosEliminadosId)
                {
                    celulaLider = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaLider where o.CelulaId == celulaId && o.MiembroId == miembroId && o.Borrado == false orderby o.CelulaLiderId descending select o).FirstOrDefault();
                    if (celulaLider != null)
                    {
                        celulaLider.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
                    }
                }
            }

            return rtn;
        }

        /// <summary>
        /// Su funcion es regresar la lista de las celulas a la que un usuario es lider
        /// </summary>
        /// <param name="usuarioId">Es el usuario de quien se quiere saber de que celulas es lider</param>
        /// <returns></returns>
        public List<RegistroBasico> ObtenerLiderazgoDeCelulas(int usuarioId)
        {
            return (
                from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaLider
                where 
                o.MiembroId == usuarioId &&
                o.Borrado == false &&
                o.Miembro.Borrado == false
                orderby o.Celula.Descripcion
                select new RegistroBasico
                {
                    Id = o.CelulaId,
                    Descripcion = o.Celula.Descripcion
                }).ToList<RegistroBasico>();
        }

        public List<Modelos.Retornos.LideresDeCelulaPorRed> ObtenerLideresDeCelulaPorRed(int celulaId)
        {
            ManejadorDeCelulas manejador = new ManejadorDeCelulas();
            List<int> celulas = manejador.ObtenerRedInferior(celulaId);
            celulas.Add(celulaId); //Agregamos la celula actual para incluir sus lideres

            return (
                from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaLider
                where 
                o.Borrado == false &&
                o.Miembro.Borrado == false &&
                celulas.Contains(o.CelulaId)
                orderby o.Celula.Descripcion
                select new Modelos.Retornos.LideresDeCelulaPorRed
                {
                    Id = o.MiembroId,
                    Nombre = o.Miembro.Primer_Nombre + " " + o.Miembro.Segundo_Nombre + " " + o.Miembro.Apellido_Paterno + " " + o.Miembro.Apellido_Materno,
                    Email = o.Miembro.Email,
                    TelMovil = o.Miembro.Tel_Movil,
                    TelCasa = o.Miembro.Tel_Casa,
                    TelTrabajo = o.Miembro.Tel_Trabajo,
                    CelulaId = o.CelulaId,
                    Celula = o.Celula.Descripcion,
                    Genero = o.Miembro.Genero.Descripcion
                }).ToList<Modelos.Retornos.LideresDeCelulaPorRed>();
        }

        public object ObtenerLideresDirectosPorMiembro(Miembro miembro)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaLider
                    where
                    o.CelulaId == miembro.CelulaId &&
                    o.Borrado == false
                    orderby o.Miembro.Primer_Nombre, o.Miembro.Segundo_Nombre, o.Miembro.Apellido_Paterno, o.Miembro.Apellido_Materno, o.Miembro.Email
                    select new
                    {
                        Id = o.MiembroId,
                        Nombre = o.Miembro.Primer_Nombre + " " + o.Miembro.Segundo_Nombre + " " + o.Miembro.Apellido_Paterno + " " + o.Miembro.Apellido_Materno,
                        Email = o.Miembro.Email,
                        TelMovil = o.Miembro.Tel_Movil,
                        TelCasa = o.Miembro.Tel_Casa,
                        TelTrabajo = o.Miembro.Tel_Trabajo
                    });
        }

        public List<RegistroBasico> ObtenerLiderazgoDirectoPorMiembro(Miembro miembro)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaLider
                    where
                    o.MiembroId == miembro.MiembroId &&
                    o.Borrado == false
                    orderby o.Celula.Descripcion
                    select new RegistroBasico
                    {
                        Id = o.CelulaId,
                        Descripcion = o.Celula.Descripcion
                    }).ToList<RegistroBasico>();
        }

        public List<RegistroBasico> ObtenerLiderazgoIndirectoPorMiembro(Miembro miembro)
        {
            ManejadorDeCelulas manejadorDeCelulas = new ManejadorDeCelulas();
            List<int> red = new List<int>();
            foreach (RegistroBasico celula in ObtenerLiderazgoDirectoPorMiembro(miembro))
            {
                red.AddRange(manejadorDeCelulas.ObtenerRedInferior(celula.Id));
            }

            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula
                    where
                    red.Contains(o.CelulaId) && o.Borrado == false
                    select new RegistroBasico
                    {
                        Id = o.CelulaId,
                        Descripcion = o.Descripcion
                    }).ToList<RegistroBasico>();
        }
    }
}
