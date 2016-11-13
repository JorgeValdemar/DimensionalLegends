using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes.Objetos
{
    public class Card
    {
        private string _idTemp;
        private string _cardId;
        private int _numero;
        private int _rank;
        private string _nome;
        private string _A1;
        private string _A2;
        private string _A3;
        private string _B1;
        private string _B2;
        private string _C1;
        private string _C2;
        private string _C3;
        private int _custo; // usado pelo jogo como preço
        private int _custoBusca; // usado pelo IA como custo de uso
        private bool _emUso;
        private bool _nova = false;

        public bool Nova
        {
            get { return _nova; }
            set { _nova = value; }
        }

        public string IdTemp
        {
            get { return _idTemp; }
            set { _idTemp = value; }
        }

        public string CardId
        {
            get { return _cardId; }
            set { _cardId = value; }
        }


        public int Numero
        {
            get { return _numero; }
            set { _numero = value; }
        }


        public int Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }



        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public string A1
        {
            get { return _A1; }
            set { _A1 = value; } 
        }


        public string A2
        {
            get { return _A2; }
            set { _A2 = value; }
        }


        public string A3
        {
            get { return _A3; }
            set { _A3 = value; }
        }

        public string B1
        {
            get { return _B1; }
            set { _B1 = value; }
        }

        public string B2
        {
            get { return _B2; }
            set { _B2 = value; }
        }


        public string C1
        {
            get { return _C1; }
            set { _C1 = value; }
        }

        public string C2
        {
            get { return _C2; }
            set { _C2 = value; }
        }

        public string C3
        {
            get { return _C3; }
            set { _C3 = value; }
        }

        public int Custo
        {
            get { return _custo; }
            set { _custo = value; }
        }

        public int CustoBusca
        {
            get { return _custoBusca; }
            set { _custoBusca = value; }
        }
        
        public bool EmUso
        {
            get { return _emUso; }
            set { _emUso = value; }
        }

        public Classes.Objetos.Elementos Elemento { get; set; }

        // não foi feito para ser usado pelo JSON
        public string[][] Atributos
        {
            get
            {
                string[][] cardAttrs = new string[][]
                        {
                            new string[] { 
                                this.A1, 
                                this.B1, 
                                this.C1
                            },
                            new string[] {
                                this.A2, 
                                "0", // não tem nada aqui, deverá ser ignorado
                                this.C2
                            },
                            new string[] { 
                                this.A3,
                                this.B2,
                                this.C3
                            }
                        };
                

                return cardAttrs;

            }
        }

    }
}
