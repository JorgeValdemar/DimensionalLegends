using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes.Objetos
{
    public class Assistente
    {
        private int _chibiId;
        private string _nome;
        private string _imagem;
        private bool _existe;

        public int ChibiId
        {
            get { return _chibiId; }
            set { _chibiId = value; }
        }

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public string Imagem
        {
            get { return _imagem; }
            set { _imagem = value; }
        }

        public bool Existe
        {
            get { return _existe; }
            set { _existe = value; }
        }

    }
}
