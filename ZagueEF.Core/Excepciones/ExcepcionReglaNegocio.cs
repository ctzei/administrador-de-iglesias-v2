using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core
{
    public class ExcepcionReglaNegocio : ExcepcionBase
    {
        public ExcepcionReglaNegocio(string message, Exception excepcionExtra): base(message)
        {
            if (excepcionExtra != null)
            {
                this.ExcepcionExtra = excepcionExtra;
                this.Guardar(Nivel.INFO);
            }
        }

        public ExcepcionReglaNegocio(string message): base(message)
        {
            this.Guardar(Nivel.DEBUG);
        }
    }
}
