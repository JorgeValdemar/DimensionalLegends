using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes.Objetos
{
    public class ArenaConfig
    {
        private string _id;
        private int _p1_pontos;
        private int _p2_pontos;
        private int _turno;
        private int _cardIndexEscolhido = -1;
        private int _posXEscolhido = -1;
        private int _posYEscolhido = -1;
        private bool _encerrada = false;

        public ArenaConfig()
        {
            _p1_pontos = 0;
            _p2_pontos = 0;
            _turno = new Random().Next(0, 1);
        }

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int P1_pontos {
            get { return _p1_pontos; }
            set { _p1_pontos = value; }
        }

        public int P2_pontos
        {
            get { return _p2_pontos; }
            set { _p2_pontos = value; }
        }

        public int Turno
        {
            get { return _turno; }
            set { _turno = value; }
        }

        public int CardIndexEscolhido
        {
            get { return _cardIndexEscolhido; }
            set { _cardIndexEscolhido = value; }
        }

        public int PosXEscolhido
        {
            get { return _posXEscolhido; }
            set { _posXEscolhido = value; }
        }

        public int PosYEscolhido
        {
            get { return _posYEscolhido; }
            set { _posYEscolhido = value; }
        }

        public bool Encerrada
        {
            get { return _encerrada; }
            set { _encerrada = value; }
        }
    }
}
