using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes.Objetos
{
    public class Opcoes
    {
        private string _internautaId;
        private int _idioma;
        private int _som;
        private int _chibiAssistente;


        public string InternautaId
        {
            get { return _internautaId; }
            set { _internautaId = value; }
        }

        public int Idioma
        {
            get { return _idioma; }
            set { _idioma = value; }
        }


        public int Som
        {
            get { return _som; }
            set { _som = value; }
        }


        public int ChibiAssistente
        {
            get { return _chibiAssistente; }
            set { _chibiAssistente = value; }
        }

        public Assistente Assistente
        {
            get;
            set;
        }


    }
}
