using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes.Objetos
{
    public class Musica
    {
        private string _internautaId;
        private int _id;
        private string _nome;
        private string _album;
        private string _autor;
        private string _nomePath;
        private bool _existe;


        public string InternautaId
        {
            get { return _internautaId; }
            set { _internautaId = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }


        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }


        public string Album
        {
            get { return _album; }
            set { _album = value; }
        }


        public string Autor
        {
            get { return _autor; }
            set { _autor = value; }
        }


        public string NomePath
        {
            get { return _nomePath; }
            set { _nomePath = value; }
        }

        public bool Existe
        {
            get { return _existe; }
            set { _existe = value; }
        }

    }
}
