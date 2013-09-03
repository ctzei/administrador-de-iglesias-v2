
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZagueEF.Core;

namespace AdministradorDeIglesiasV2.Core.Modelos
{
    public partial class ConsolidacionBoleta
    {
        partial void OnAgregar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.CrearBoletasDeConsolidacion);
            validarEntidad(entry);
        }

        partial void OnModificar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.ModificarBoletasDeConsolidacion);
            validarEntidad(entry);
        }

        partial void OnBorrar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.BorrarBoletasDeConsolidacion);
        }

        private void validarEntidad(System.Data.Objects.ObjectStateEntry entry)
        {
            ConsolidacionBoleta entidad = (ConsolidacionBoleta)entry.Entity;
            Validaciones.ValidarEmail(entidad.Email);
            validarRazonDeBorrar(entidad);
            establecerEdad(entidad);
            establecerCategoria(entidad);
            validarUnicidad(entry);
        }

        private void validarRazonDeBorrar(ConsolidacionBoleta entidad)
        {
            if (entidad.BoletaCerrada == true && ((entidad.BoletaRazonCerradaId.HasValue == false) || (entidad.BoletaRazonCerradaId.HasValue && entidad.BoletaRazonCerradaId.Value <= 0)))
            {
                throw new ExcepcionReglaNegocio("Es necesario establecer alguna razón para cerrar la boleta antes de continuar.");
            }
        }

        private void validarUnicidad(System.Data.Objects.ObjectStateEntry entry)
        {
            // Validamos si no existe ya una boleta con el mismo correo
            ConsolidacionBoleta entidadPreexistente = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().ConsolidacionBoleta where o.Email == ((ConsolidacionBoleta)entry.Entity).Email select o).SingleOrDefault();
            
            if (entidadPreexistente != null)
            {
                string registroExistenteMsg = string.Format("Ya existe una boleta para ese email [{0}], la cual su ID es [{1}]. De ser necesario, buscarla y modificarla o utilizar un email distinto.", entidadPreexistente.Email, entidadPreexistente.Id);

                if (entry.State == System.Data.EntityState.Added)
                {
                    throw new ExcepcionReglaNegocio(registroExistenteMsg);
                }
                else if (entry.State == System.Data.EntityState.Modified)
                {
                    if (Validaciones.ValidarCambiosEnCampo(entry, "email")){
                        throw new ExcepcionReglaNegocio(registroExistenteMsg);
                    }
                }
            }
        }

        private void establecerEdad(ConsolidacionBoleta entidad)
        {
            if (entidad.FechaDeNacimiento.HasValue)
            {
                entidad.Edad = DateTime.Now.Subtract(entidad.FechaDeNacimiento.Value).Days / 365;
            }
        }

        private void establecerCategoria(ConsolidacionBoleta entidad)
        {
            int categoriaId;
            // Jovenes
            if (entidad.EstadoCivilId != 2 && entidad.Edad < 38){
                categoriaId = 1;
            }
            // Matrimonios Jovenes
            else if(entidad.EstadoCivilId == 2 && entidad.Edad < 38){
                categoriaId = 2;
            }
            // Familiar
            else if(entidad.Edad >= 38 && entidad.Edad < 50){
                categoriaId = 3;
            }
            // Red Dorada
            else if(entidad.Edad >= 50){
                categoriaId = 4;
            }
            // Otra
            else{
                categoriaId = 99;
            }
            entidad.CategoriaBoletaId = categoriaId;

            // Si la boleta no se encuentra asignada a alguna celula, se asigna a la celula predeterminada de acuerdo a su categoria
            if (!entidad.AsignadaACelulaId.HasValue)
            {
                entidad.AsignadaACelulaId = entidad.ConsolidacionBoletaCategoria.Celula.CelulaId;
            }
        }
    }
}
