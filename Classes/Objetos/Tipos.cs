using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes.Objetos
{
    public class Tipos
    {
        private int _tipoId;
        private string _tipo;

        public int TipoId
        {
            get { return _tipoId; }
            set { _tipoId = value; }
        }

        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }
    }
}
