using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace ZagueEF.Core
{
    public abstract class ExcepcionBase : ApplicationException
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ExcepcionBase));

        public Exception ExcepcionExtra { get; set; }

        public ExcepcionBase(string msg)
            : base(msg)
        {
        }

        public void Guardar(Nivel nivel)
        {

            switch (nivel)
            {
                case Nivel.ERROR:
                    log.Error(this, this.InnerException);
                    break;
                case Nivel.INFO:
                    log.Info(this, this.InnerException);
                    break;
                case Nivel.WARN:
                    log.Warn(this, this.InnerException);
                    break;
                case Nivel.DEBUG:
                    log.Debug(this, this.InnerException);
                    break;
            }
        }

        public enum Nivel
        {
            ERROR,
            INFO,
            WARN,
            DEBUG
        }
    }
}
