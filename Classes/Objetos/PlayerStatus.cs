using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes.Objetos
{
    public class PlayerStatus
    {
        private string _internautaId;
        private int _level;
        private int _coins;
        private int _maxHp;
        private int _maxMp;
        private int _maxSp;
        private string _nick;
        private string _imagem;


        public string InternautaId
        {
            get;
            set;
        }

        public int Level
        {
            get;
            set;
        }

        public int Coins
        {
            get;
            set;
        }

        public int MaxHp
        {
            get;
            set;
        }

        public int MaxMp
        {
            get;
            set;
        }

        public int MaxSp
        {
            get;
            set;
        }

        public string Nick
        {
            get;
            set;
        }

        public string Imagem
        {
            get;
            set;
        }




    }
}
