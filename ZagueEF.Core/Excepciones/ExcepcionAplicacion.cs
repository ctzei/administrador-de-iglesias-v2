using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core
{
    public class ExcepcionAplicacion : ExcepcionBase
    {
        public ExcepcionAplicacion(String message, Exception excepcionExtra) : base(message)
        {
            if (excepcionExtra != null)
            {
                this.ExcepcionExtra = excepcionExtra;
                this.Guardar(Nivel.ERROR);
            }
        }

        public ExcepcionAplicacion(String message) : base(message)
        {
            this.Guardar(Nivel.ERROR);
        }
    }
}
