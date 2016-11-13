using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes
{
    public class Arvore
    {
        public Arvore(object valor = null, List<Arvore> filhos = null)
        {
            this.Valor = valor;
            this.Filhos = filhos != null ? filhos : new List<Arvore>();
        }

        public object Valor { get; set; }
        public List<Arvore> Filhos { get; set; }

        public void AddValor(object valor)
        {
            this.Valor = valor;
        }

        public void AddFilhos(List<Arvore> filhos)
        {
            this.Filhos = filhos;
        }

    }
}
