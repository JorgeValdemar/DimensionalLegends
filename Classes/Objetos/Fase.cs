using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes.Objetos
{
    public class Fase
    {
        private int _id;
        private string _fase;
        private int _liderId;
        private int _musicaId;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string FaseNome
        {
            get { return _fase; }
            set { _fase = value; }
        }

        public int LiderId
        {
            get { return _liderId; }
            set { _liderId = value; }
        }

        public int MusicaId
        {
            get { return _musicaId; }
            set { _musicaId = value; }
        }

        public Classes.Objetos.PlayerStatus ArcadeLiderStatus
        {
            get;
            set;
        }

        public Classes.Objetos.Musica Musica
        {
            get;
            set;
        }

        public List<Classes.Objetos.Card> ListaDrop
        {
            get;
            set;
        }

    }
}
