using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes.Objetos
{
    public class ArenaObjItens
    {
        public static int COLUMNS = 3; // posicoes X EQUIVALE A: 4
        public static int ROWS = 2; // posicoes Y EQUIVALE A: 3
        public static int CUSTO_RUIM = -3; // QUAL ATRIBUTO JÁ É CONSIDERADO RUIM?
        public static int CUSTO_BOM = 0; // NÃO É USADO ATUALMENTE
        public static int CUSTO_PAREDE = 2; // ADICIONAL DE CUSTO PARA PAREDES
        public static int MARGEM_CUSTO_MAXIMO = 200; // MARGEM DE CUSTO MAXIMO USADO EM CALCULO PARA GERAR CUSTO POR MENOR VALOR


        public List<Classes.Objetos.HistoricoPassos> ListaHostoricosP1
        {
            get;
            set;
        }

        public List<Classes.Objetos.HistoricoPassos> ListaHostoricosP2
        {
            get;
            set;
        }

        public List<Classes.Objetos.Card> p1deckListCard
        {
            get;
            set;
        }

        public List<Classes.Objetos.Card> p2deckListCard
        {
            get;
            set;
        }

        public Classes.Objetos.CardBloco[] arenasituacaoY1
        {
            get;
            set;
        }

        public Classes.Objetos.CardBloco[] arenasituacaoY2
        {
            get;
            set;
        }

        public Classes.Objetos.CardBloco[] arenasituacaoY3
        {
            get;
            set;
        }

        public Classes.Objetos.CardBloco[] arenasituacaoY4
        {
            get;
            set;
        }

        // não deve ser usado em transferência de dados
        public static Classes.Objetos.CardBloco[] RetornarColuna(int linhaY, Classes.Objetos.CardBloco[,] arenaTabuleiro)
        {
            Classes.Objetos.CardBloco[] cTabuleiro = new Classes.Objetos.CardBloco[ArenaObjItens.COLUMNS];

            for (int i = 0; i < ArenaObjItens.COLUMNS; i++)
                cTabuleiro[i] = arenaTabuleiro[linhaY, i];

            return cTabuleiro;
        }

        // não deve ser usado em transferência de dados
        public Classes.Objetos.CardBloco[,] arenasituacao2Dobject
        {
            get {
                return new Classes.Objetos.CardBloco[,] { 
                    {arenasituacaoY1[0], arenasituacaoY1[1], arenasituacaoY1[2]},   //coluna 0
                    {arenasituacaoY2[0], arenasituacaoY2[1], arenasituacaoY2[2]},   //coluna 1
                    {arenasituacaoY3[0], arenasituacaoY3[1], arenasituacaoY3[2]},   //coluna 2
                    {arenasituacaoY4[0], arenasituacaoY4[1], arenasituacaoY4[2]}    //coluna 3
                    //linha 0               linha 1             linha 2
                };
            }

            /*
             
             | 0,0 | 1,0 | 2,0 |
             | 0,1 | 1,1 | 2,1 |
             | 0,2 | 1,2 | 2,2 |
             | 0,3 | 1,3 | 2,3 |
             
             */

        }

        public Classes.Objetos.ArenaConfig arenaConfig
        {
            get;
            set;
        }
    }
}
