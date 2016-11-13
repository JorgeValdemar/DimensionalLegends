using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes.Objetos
{
    public class CardBloco
    {
        private string _idTemp;
        private int _player;
        private bool _usado = false;
        private int _custo = 0;

        public string IdTemp
        {
            get { return _idTemp; }
            set { _idTemp = value; }
        }

        public int Player 
        { 
            get 
            {
                return _player;
            } 
            set 
            {
                _player = value;
            } 
        }

        public bool Usado
        {
            get 
            {
                return _usado;
            }
            set
            {
                _usado = value;
            }
        }


        public int Custo
        {
            get
            {
                return _custo;
            }
            set
            {
                _custo = value;
            }
        }

        public Classes.Objetos.Card Card
        {
            get;
            set;
        }

        public List<Classes.Objetos.Card> ListaCartasArvore
        {
            get;
            set;
        }
    }
}
