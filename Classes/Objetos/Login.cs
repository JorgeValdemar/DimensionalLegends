using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes.Objetos
{
    public class Login
    {
        private Random rm;

        private string _id;
        private string _nome;
        private string _email;
        private string _senha;
        private int _codigo = 0;
        private PlayerStatus _ps;

        public string id
        {
            get { return _id; }
            set { _id = value; }
        }


        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }
        

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Senha
        {
            get { return _senha; }
            set { _senha = value; }
        }

        public int Codigo { 
            get {
                if (_codigo == 0)
                {
                    _codigo = rm.Next(100, 1000);
                }

                return _codigo; 
            }
            set { _codigo = value; }
        }

        public PlayerStatus PlayerStatus
        {
            get { return _ps; }
            set { _ps = value; }
        }

        public Login()
        {
            rm = new Random();
        }

    }
}
