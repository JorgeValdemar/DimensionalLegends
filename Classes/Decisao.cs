using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Search;
using QuickGraph.Algorithms.ShortestPath;
using QuickGraph.Algorithms.Observers;

namespace Classes
{
    public class Decisao
    {
        private const char DELIMITADOR_CHAR = '&';
        private const char DELIMITADOR_CHAR_IGUAL = '=';
        private const char DELIMITADOR_CHAR_POSICAO = '#';
        private const string DELIMITADOR_ID_BLOCO = "POS"; //=2#0
        private const string DELIMITADOR_ID_CARTA = "INDEX"; //=4
        private Objetos.ArenaObjItens IArenaObjItens;

        private bool _erro = false;
        private string _erroDescricao = "";

        private int CustoPorAtributo(string valor)
        {
            switch (valor.ToLower())
            {
                case "0":
                    return -8;
                break;
                case "1":
                    return -6;
                break;
                case "2":
                    return -6;
                break;
                case "3":
                    return -4;
                break;
                case "4":
                    return -2;
                break;
                case "5":
                    return -1;
                break;
                case "6":
                    return -1;
                break;
                case "7":
                    return 0;
                break;
                case "8":
                    return 1;
                break;
                case "9":
                    return 2;
                break;
                case "e":
                    return 3;
                break;
                case "s":
                    return 4;
                break;
                default:
                    return 0;
                break;
            }
        }

        // verifica se o numero desafiante vence
        private int ConflitoNumerico(string desafiante, string desafiado)
        {
            if (int.Parse(desafiante) > int.Parse(desafiado))
            {
                return 1;
            }
            else if (int.Parse(desafiante) == int.Parse(desafiado))
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        private int TabelaElemental(int desafiante, int desafiado)
        {
            return Objetos.Elementos.TabelaElemental[desafiante-1][desafiado-1];
        }

        private bool AtributoIsLetra(string atributo)
        {
            if (atributo.ToUpper() == "S" || atributo.ToUpper() == "E")
                return true;
            else
                return false;
        }

        private int RefletirPosicaoAtributo(int posicaoAtributo)
        {
            switch (posicaoAtributo)
            {
                case 0:
                    return 2;
                    break;
                case 1:
                    return 1;
                    break;
                case 2:
                    return 0;
                    break;
                default:
                    return 1;
                    break;
            }
        }

        private void SomarPontos(int playerNum)
        {
            if (playerNum == 1)
            {
                IArenaObjItens.arenaConfig.P1_pontos++;
                IArenaObjItens.arenaConfig.P2_pontos--;
            }
            else
            {
                IArenaObjItens.arenaConfig.P1_pontos--;
                IArenaObjItens.arenaConfig.P2_pontos++;
            }
        }

        

        /*
         * Avalia o quanto vale um atributo da carta de acordo com a casa adjacente
         * int xPos = posição x do tabuleiro
         * int yPos = posição y do tabuleiro
         * int xaPos = posição x do atributo
         * int yaPos = posição y do atributo
         * string valorAtributo = valor do atributo
         * int elementoId = id do elemento da carta
         * int poderMaximo = soma de todos os atributos numericos
         */
        private int CustoRegraAdjacente(int xPos, int yPos, int xaPos, int yaPos, string valorAtributo, int elementoId, int poderMaximo)
        {
            int xVa = xPos + (xaPos - 1);
            int yVa = yPos + (yaPos - 1);
            int custo = 0;

            // IF PAREDE: quando isso acontece o valorAtributo é substituido por um custo da parede.
            if ((xVa < 0) || (xVa >= Objetos.ArenaObjItens.COLUMNS)
                ||
                (yVa < 0) || (yVa >= Objetos.ArenaObjItens.ROWS))
            {
                
                if (this.CustoPorAtributo(valorAtributo) <= Objetos.ArenaObjItens.CUSTO_RUIM)
                {
                    custo = Objetos.ArenaObjItens.CUSTO_PAREDE;
                }
                else
                {
                    custo = Objetos.ArenaObjItens.CUSTO_PAREDE * -1;
                }
            }
            else
            {
                // TODO: FAZER A IA DIFERENCIAR QUEM É INIMIGO, aliados podem ser como paredes com menos custo.
                //if (IArenaObjItens.arenasituacao2Dobject[xVa, yVa].Usado && IArenaObjItens.arenasituacao2Dobject[xVa, yVa].Player == 1)
                if (IArenaObjItens.arenasituacao2Dobject[xVa, yVa].Usado)
                {
                    string[][] blocoAttrAdjacente = IArenaObjItens.arenasituacao2Dobject[xVa, yVa].Card.Atributos;
                    int xaPosReflect = this.RefletirPosicaoAtributo(xaPos);
                    int yaPosReflect = this.RefletirPosicaoAtributo(yaPos);


                    if (this.AtributoIsLetra(blocoAttrAdjacente[xaPosReflect][yaPosReflect]) && this.AtributoIsLetra(valorAtributo))
                    {
                        // se os dois forem letras

                        if (valorAtributo.ToUpper() == "S")
                        {
                            if (blocoAttrAdjacente[xaPosReflect][yaPosReflect].ToUpper() == "S")
                            {
                                // empate
                                custo = this.CustoPorAtributo(valorAtributo) + 1;
                            }
                            else
                            {
                                // vence
                                custo = this.CustoPorAtributo(valorAtributo) + 10;
                            }
                        }
                        else
                        {
                            // assumindo letra E

                            if(blocoAttrAdjacente[xaPosReflect][yaPosReflect].ToUpper() == "S")
                            {
                                // perde
                                custo = this.CustoPorAtributo(valorAtributo) - 5;
                            } 
                            else 
                            {
                                // batalha elemental E x E
                                switch (this.TabelaElemental(elementoId, IArenaObjItens.arenasituacao2Dobject[xVa, yVa].Card.Elemento.ElementoId))
                                {
                                    case -1:
                                        // perde
                                        custo = this.CustoPorAtributo(valorAtributo) - 5;
                                        break;
                                    case 0:
                                        // empata
                                        custo = this.CustoPorAtributo(valorAtributo) + 1;
                                        break;
                                    case 1:
                                        // ganha
                                        custo = this.CustoPorAtributo(valorAtributo) + 10;
                                        break;
                                    case 2:
                                        // batalha luz x trevas
                                        int blocoPoderMaximo = this.PoderMaximo(IArenaObjItens.arenasituacao2Dobject[xVa, yVa].Card.Atributos);

                                        if (poderMaximo > blocoPoderMaximo)
                                        {
                                            // ganha
                                            custo = this.CustoPorAtributo(valorAtributo) + 10;
                                        }
                                        else if (poderMaximo == blocoPoderMaximo)
                                        {
                                            // empata
                                            custo = this.CustoPorAtributo(valorAtributo) + 1;
                                        }
                                        else
                                        {
                                            // perde
                                            custo = this.CustoPorAtributo(valorAtributo) - 5;
                                        }

                                        break;
                                }
                            }

                        }
                    }
                    else if (this.AtributoIsLetra(blocoAttrAdjacente[xaPosReflect][yaPosReflect]) || this.AtributoIsLetra(valorAtributo))
                    {
                        // se apenas um dos dois forem letras assumimos que um deles já perdeu pois letra é sempre maior q numero

                        if (this.AtributoIsLetra(valorAtributo))
                        {
                            // vence
                            custo = this.CustoPorAtributo(valorAtributo) + 8;
                        }
                        else
                        {
                            // perde
                            custo = this.CustoPorAtributo(valorAtributo) - 10;
                        }
                    }
                    else
                    {
                        //assumiremos que só existem números
                        
                        switch (this.ConflitoNumerico(valorAtributo, blocoAttrAdjacente[xaPosReflect][yaPosReflect]))
                        {
                            case 1:
                                custo = this.CustoPorAtributo(valorAtributo) + 8; // vence
                                break;
                            case 0:
                                custo = this.CustoPorAtributo(valorAtributo) - 1; // empate
                                break;
                            case -1:
                                custo = this.CustoPorAtributo(valorAtributo) - 10; // perde
                                break;
                        }
                    }

                }
                else
                {
                    custo = this.CustoPorAtributo(valorAtributo);
                }
            }

            return custo;
        }

        /*
         * Avalia a situação do atributo atual com a casa adjacente e atualiza o resultado
         * int xPos = posição x do tabuleiro
         * int yPos = posição y do tabuleiro
         * int xaPos = posição x do atributo
         * int yaPos = posição y do atributo
         * string valorAtributo = valor do atributo
         * int elementoId = id do elemento da carta
         * int poderMaximo = soma de todos os atributos numericos
         */
        private void RegraAdjacente(int playerNum, int xPos, int yPos, int xaPos, int yaPos, string valorAtributo, int elementoId, int poderMaximo)
        {
            int adversarioNum = playerNum == 1 ? 2 : 1;
            int xVa = xPos + (xaPos - 1);
            int yVa = yPos + (yaPos - 1);
            bool ganha = false;

            // IF PAREDE DE FODA DO TABULEIRO.
            if ((xVa < 0) || (xVa > Objetos.ArenaObjItens.COLUMNS)
                ||
                (yVa < 0) || (yVa > Objetos.ArenaObjItens.ROWS))
            {
                // uma parede não faz mau algum, pelo menos até hoje.
                return;
            }
            else if (IArenaObjItens.arenasituacao2Dobject[xPos, yPos].Player == adversarioNum)
            {
                // perdi minha carta...já era...
                return;
            }
            else
            {
                // Se a casa adjacente está sendo usada por uma carta do adversário.
                if (IArenaObjItens.arenasituacao2Dobject[xVa, yVa].Usado && IArenaObjItens.arenasituacao2Dobject[xVa, yVa].Player == adversarioNum)
                {
                    string[][] blocoAttrAdjacente = IArenaObjItens.arenasituacao2Dobject[xVa, yVa].Card.Atributos;
                    int xaPosReflect = this.RefletirPosicaoAtributo(xaPos);
                    int yaPosReflect = this.RefletirPosicaoAtributo(yaPos);


                    if (this.AtributoIsLetra(blocoAttrAdjacente[xaPosReflect][yaPosReflect]) && this.AtributoIsLetra(valorAtributo))
                    {
                        // se os dois forem letras

                        if (valorAtributo.ToUpper() == "S")
                        {
                            if (blocoAttrAdjacente[xaPosReflect][yaPosReflect].ToUpper() == "S")
                            {
                                // empate, não faz mau a nenhum dos lados.
                                return;
                            }
                            else
                            {
                                // vence, então ponto para p1
                                ganha = true;
                            }
                        }
                        else
                        {
                            // assumindo letra E

                            if (blocoAttrAdjacente[xaPosReflect][yaPosReflect].ToUpper() == "S")
                            {
                                // perde, então ponto para p2
                                ganha = false;
                            }
                            else
                            {
                                // batalha elemental E x E
                                switch (this.TabelaElemental(elementoId, IArenaObjItens.arenasituacao2Dobject[xVa, yVa].Card.Elemento.ElementoId))
                                {
                                    case -1:
                                        // perde
                                        ganha = false;
                                        break;
                                    case 0:
                                        // empata
                                        return;
                                        break;
                                    case 1:
                                        // ganha
                                        ganha = true;
                                        break;
                                    case 2:
                                        // batalha luz x trevas
                                        int blocoPoderMaximo = this.PoderMaximo(IArenaObjItens.arenasituacao2Dobject[xVa, yVa].Card.Atributos);

                                        if (poderMaximo > blocoPoderMaximo)
                                        {
                                            // ganha
                                            ganha = true;
                                        }
                                        else if (poderMaximo == blocoPoderMaximo)
                                        {
                                            // empata
                                            return;
                                        }
                                        else
                                        {
                                            // perde
                                            ganha = false;
                                        }

                                        break;
                                }
                            }

                        }
                    }
                    else if (this.AtributoIsLetra(blocoAttrAdjacente[xaPosReflect][yaPosReflect]) || this.AtributoIsLetra(valorAtributo))
                    {
                        // se apenas um dos dois forem letras assumimos que um deles já perdeu pois letra é sempre maior q numero

                        if (this.AtributoIsLetra(valorAtributo))
                        {
                            // vence
                            ganha = true;
                        }
                        else
                        {
                            // perde
                            ganha = false;
                        }
                    }
                    else
                    {
                        //assumiremos que só existem números

                        switch (this.ConflitoNumerico(valorAtributo, blocoAttrAdjacente[xaPosReflect][yaPosReflect]))
                        {
                            case 1:
                                ganha = true; // vence
                                break;
                            case 0:
                                return; // empate
                                break;
                            case -1:
                                ganha = false; // perde
                                break;
                        }
                    }

                }
                else
                {
                    // um bloco vazio vizinho não faz mau, pelo menos até hoje.
                    return;
                }

                Objetos.HistoricoPassos historico = new Objetos.HistoricoPassos();

                if (ganha)
                {
                    if (IArenaObjItens.arenasituacao2Dobject[xVa, yVa].Player != playerNum)
                    {
                        historico.NewPlayer = playerNum;
                        historico.PosX = xVa;
                        historico.PosY = yVa;
                        historico.ExecutorPosX = xPos;
                        historico.ExecutorPosY = yPos;
                        IArenaObjItens.arenasituacao2Dobject[xVa, yVa].Player = playerNum;
                        this.SomarPontos(playerNum);
                    }
                }
                else
                {
                    if (IArenaObjItens.arenasituacao2Dobject[xPos, yPos].Player != adversarioNum)
                    {
                        historico.NewPlayer = adversarioNum;
                        historico.PosX = xPos;
                        historico.PosY = yPos;
                        historico.ExecutorPosX = xVa;
                        historico.ExecutorPosY = yVa;
                        IArenaObjItens.arenasituacao2Dobject[xPos, yPos].Player = adversarioNum;
                        this.SomarPontos(adversarioNum);
                    }
                }

                if (playerNum == 1)
                {
                    IArenaObjItens.ListaHostoricosP1.Add(historico);
                }
                else
                {
                    IArenaObjItens.ListaHostoricosP2.Add(historico);
                }

            }

        }

        /*
         * Avalia o quanto vale cada atributo de uma determinada posição para uma carta
         */
        private int PoderMaximo(string[][] atributos)
        {
            int pontos = 0;

            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if (i == 1 && j == 1)
                    {
                        pontos += 0;
                    }
                    else
                    {
                        if (!this.AtributoIsLetra(atributos[i][j]))
                        {
                            pontos += int.Parse(atributos[i][j]);
                        }
                    }
                }
            }
            return pontos;
        }

        /*
         * Avalia o quanto vale cada atributo de uma determinada posição para uma carta
         */
        private int CustoPorBloco(int xPos, int yPos, int itemNum)
        {
            string[][] cardAttrs = IArenaObjItens.p2deckListCard[itemNum].Atributos;
            int elementoId = IArenaObjItens.p2deckListCard[itemNum].Elemento.ElementoId;
            int poderMaximo = this.PoderMaximo(IArenaObjItens.p2deckListCard[itemNum].Atributos);

            int custo = 0;

            // para cada atributo da carta adicione custo 0 ou CustoRegraAdjacente()
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if (i == 1 && j == 1)
                        custo += 0;
                    else
                        custo += this.CustoRegraAdjacente(xPos, yPos, i, j, cardAttrs[i][j], elementoId, poderMaximo);
                }
            }

            return custo;
        }

        private int CustoFinal(int custo)
        {
            return Objetos.ArenaObjItens.MARGEM_CUSTO_MAXIMO - custo;
        }

        private Objetos.ArenaObjItens AtualizarArenaPorDelimitador(List<string> listaDelimitadores)
        {
            int px = 0;
            int py = 0;
            int cp = 0;

            // removemos a posicao 0 que representa a raiz, ela é inútil agora
            listaDelimitadores.RemoveAt(0);

            foreach (string s in listaDelimitadores)
            {
                string[] delimitChar = s.Split(DELIMITADOR_CHAR);
                string[] delimit = delimitChar[0].Split(DELIMITADOR_CHAR_IGUAL);

                string chave = delimit[0];
                string valor = delimit[1];

                switch (chave)
                {
                    case DELIMITADOR_ID_BLOCO:
                        string[] delimitPos = valor.Split(DELIMITADOR_CHAR_POSICAO);

                        px = int.Parse(delimitPos[0]);
                        py = int.Parse(delimitPos[1]);

                        break;
                    case DELIMITADOR_ID_CARTA:
                        cp = int.Parse(valor);
                        break;
                }
            }

            this.SituarBatalha(2, cp, px, py);

            return IArenaObjItens;
        }

        private string GerarIdBloco(int xPos, int yPos)
        {
            string idBloco = string.Concat(
                    DELIMITADOR_ID_BLOCO, 
                    DELIMITADOR_CHAR_IGUAL, 
                    xPos.ToString(), 
                    DELIMITADOR_CHAR_POSICAO, 
                    yPos.ToString(), 
                    DELIMITADOR_CHAR, 
                    Guid.NewGuid().ToString()
                );

            return idBloco;
        }

        private string GerarIdCarta(int cardIndex)
        {
            string id = string.Concat(
                    DELIMITADOR_ID_CARTA,
                    DELIMITADOR_CHAR_IGUAL,
                    cardIndex.ToString(),
                    DELIMITADOR_CHAR, 
                    Guid.NewGuid().ToString()
                );

            return id;
        }

        public Classes.Objetos.ArenaObjItens Arcade()
        {
            /*
             * leia a arena e as cartas.
             * gere um custo para cada um bloco e carta, 
             * adicione arvore para a IA trabalhar e percorrer tanto com o bloco quanto a carta.
             */
            AdjacencyGraph<string, Edge<string>> graph = new AdjacencyGraph<string, Edge<string>>(false);
            List<Edge<string>> aresta = new List<Edge<string>>();
            List<double> arestaCusto = new List<double>();
            List<string> vertice = new List<string>();

            arestaCusto.Add(0);
            vertice.Add("raiz");

            for (int xPos = 0; xPos <= Objetos.ArenaObjItens.COLUMNS; xPos++)
            {
                for (int yPos = 0; yPos <= Objetos.ArenaObjItens.ROWS; yPos++)
                {
                    if (IArenaObjItens.arenasituacao2Dobject[xPos, yPos].Usado)
                        continue;

                    string idBloco = this.GerarIdBloco(xPos, yPos);
                    int auxCusto = 0;

                    // vamos preencher o que vamos usar!
                    IArenaObjItens.arenasituacao2Dobject[xPos, yPos].IdTemp = idBloco;
                    IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore = new List<Objetos.Card>();
                    IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore.AddRange(IArenaObjItens.p2deckListCard);

                    // sendo assim teremos 5 valores para cada um dos blocos
                    for (int i = 0; i < IArenaObjItens.p2deckListCard.Count(); i++)
                    {
                        IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore[i].CustoBusca = this.CustoFinal(this.CustoPorBloco(xPos, yPos, i));

                        string id = this.GerarIdCarta(i);

                        if (auxCusto > IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore[i].CustoBusca)
                            auxCusto = IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore[i].CustoBusca;

                        IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore[i].IdTemp = id;

                        aresta.Add(new Edge<string>(IArenaObjItens.arenasituacao2Dobject[xPos, yPos].IdTemp, IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore[i].IdTemp));
                        arestaCusto.Add(IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore[i].CustoBusca);

                        vertice.Add(IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore[i].IdTemp);
                        
                    }


                    IArenaObjItens.arenasituacao2Dobject[xPos, yPos].Custo = auxCusto;

                    aresta.Add(new Edge<string>("raiz", IArenaObjItens.arenasituacao2Dobject[xPos, yPos].IdTemp));
                    arestaCusto.Add(IArenaObjItens.arenasituacao2Dobject[xPos, yPos].Custo);
                    vertice.Add(IArenaObjItens.arenasituacao2Dobject[xPos, yPos].IdTemp);

                }
            }


            // adicionamos todas as informações acima dentro do grafico
            graph.AddVertexRange(vertice);
            graph.AddEdgeRange(aresta);

            Dictionary<Edge<string>, double> edgeCostDictionary = new Dictionary<Edge<string>, double>(graph.EdgeCount);

            for (int i = 0; i < aresta.Count; i++)
                edgeCostDictionary.Add(aresta[i], arestaCusto[i+1]);
            
            Func<Edge<string>, double> edgeCost = AlgorithmExtensions.GetIndexer(edgeCostDictionary);
            
            // PATHFIND
            // A Star Algorithm

            // ......

            // inicia variavel tempo

            // Dijkstra Algorithm
            var dijkstra = new DijkstraShortestPathAlgorithm<string, Edge<string>>(graph, edgeCost);

            // Attach a Vertex Predecessor Recorder Observer to give us the paths
            var predecessors = new VertexPredecessorRecorderObserver<string, Edge<string>>();
            predecessors.Attach(dijkstra);

            var PathObserver = new VertexPredecessorPathRecorderObserver<string, Edge<string>>(predecessors.VertexPredecessors);
            PathObserver.Attach(dijkstra);
            
            dijkstra.Compute("raiz");


            List<string> endPathVertices = PathObserver.EndPathVertices.ToList<string>();
            List<Edge<string>> endPathEdges = new List<Edge<string>>();
            
            foreach (var pred in predecessors.VertexPredecessors)
                if(endPathVertices.Contains(pred.Key))
                    endPathEdges.Add(pred.Value);

            double bestCost = -1;
            string bestCostVertex = "";

            foreach (var fimEdge in endPathEdges)
            {
                if ((bestCost == -1) || (bestCost > edgeCostDictionary[fimEdge]))
                {
                    bestCost = edgeCostDictionary[fimEdge];
                    bestCostVertex = fimEdge.Target;
                }
                else
                {
                    continue;
                }
            }

            IEnumerable<Edge<string>> bestPath;
            List<string> listBestPath = new List<string>();
            if (predecessors.TryGetPath(bestCostVertex, out bestPath))
            {
                foreach (var e in bestPath)
                {
                    listBestPath.Add(e.Source);
                }
            }
            
            // após adicionar o melhor caminho devemos adicionar o objetivo na lista
            listBestPath.Add(bestCostVertex);

            // recebe o tempo gasto

            IArenaObjItens = this.AtualizarArenaPorDelimitador(listBestPath);

            return IArenaObjItens;
        }

        private bool IsFirstTurn()
        {
            // verificar a quantia de cartas para cada lado.
            if (
                IArenaObjItens.p1deckListCard.Count() == 5
                &&
                IArenaObjItens.p2deckListCard.Count() == 5
                )
            {
                if (!AcaoPlayerValida())
                {
                    return true;
                }
            }
            return false;
        }

        private bool AcaoPlayerValida()
        {
            if (
                (IArenaObjItens.arenaConfig.CardIndexEscolhido == -1)
                ||
                (IArenaObjItens.arenaConfig.PosXEscolhido == -1)
                ||
                (IArenaObjItens.arenaConfig.PosYEscolhido == -1)
                )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Objetos.ArenaObjItens AtualizarArenaPorHumano(int cartaEscolhida, int posX, int posY)
        {
            if (IArenaObjItens.arenasituacao2Dobject[posX, posY].Usado)
            {
                this.Erro = true;
                this.ErroDescricao = "bloco inválido";
                return IArenaObjItens;
            }

            this.SituarBatalha(1, cartaEscolhida, posX, posY);

            return IArenaObjItens;
        }

        private void SituarBatalha(int playerNum, int cartaEscolhidaId, int posX, int posY)
        {
            Objetos.Card carta = new Objetos.Card();
            switch (playerNum)
            {
                case 1:
                    for (int i = 0; i < IArenaObjItens.p1deckListCard.Count(); i++ )
                    {
                        if (IArenaObjItens.p1deckListCard[i].Numero == cartaEscolhidaId)
                        {
                            carta = IArenaObjItens.p1deckListCard[i];
                            IArenaObjItens.p1deckListCard.RemoveAt(i);
                        }
                    }
                    break;
                case 2:
                    /*
                     * ATENÇÃO ATENÇÃO!!!!
                     * ESTAMOS CONTANDO QUE ESTA SENDO RECEBIDO PELA IA, 
                     * POIS A IA MANDA A POSIÇÃO E NÃO O NUMERO DA CARTA!!!!!!!!!!!!
                     * */
                        carta = IArenaObjItens.p2deckListCard[cartaEscolhidaId];
                        IArenaObjItens.p2deckListCard.RemoveAt(cartaEscolhidaId);
                    break;
                default:
                    this.Erro = true;
                    this.ErroDescricao = "sem id player? programador...está dormindo bem?";
                    break;
            }

            carta.Nova = true;
            IArenaObjItens.arenasituacao2Dobject[posX, posY].Card = carta;
            IArenaObjItens.arenasituacao2Dobject[posX, posY].Usado = true;
            IArenaObjItens.arenasituacao2Dobject[posX, posY].Player = playerNum;

            int poderMaximo = this.PoderMaximo(IArenaObjItens.arenasituacao2Dobject[posX, posY].Card.Atributos);

            // para cada atributo da carta...
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == 1 && j == 1)
                    {
                        continue;
                    } else {
                        this.RegraAdjacente(playerNum, posX, posY, i, j, IArenaObjItens.arenasituacao2Dobject[posX, posY].Card.Atributos[i][j], IArenaObjItens.arenasituacao2Dobject[posX, posY].Card.Elemento.ElementoId, poderMaximo);
                    }
                }
            }

        }

        public Classes.Objetos.ArenaObjItens Player(int p)
        {
            /*
             * no primeiro turno o player 1 tem tudo o que precisa,
             * não tem necessidade de processamento.
             */
            if (this.IsFirstTurn()) return IArenaObjItens;

            if (!AcaoPlayerValida()){
                this.Erro = true;
                this.ErroDescricao = "Erro em especificar a ação do player";
                return IArenaObjItens;
            }

            IArenaObjItens = this.AtualizarArenaPorHumano(
                    IArenaObjItens.arenaConfig.CardIndexEscolhido, 
                    IArenaObjItens.arenaConfig.PosXEscolhido,
                    IArenaObjItens.arenaConfig.PosYEscolhido
                );

            return IArenaObjItens;
        }

        public bool Erro
        {
            get
            {
                return _erro;
            }
            set
            {
                _erro = value;
            }
        }

        public string ErroDescricao
        {
            get
            {
                return string.Concat("Erro Decisão: ", _erroDescricao);
            }
            set
            {
                _erroDescricao = value;
            }
        }


        // construtor
        public Decisao(Classes.Objetos.ArenaObjItens arenaObjItens)
        {
            IArenaObjItens = arenaObjItens;
            IArenaObjItens.ListaHostoricosP1 = new List<Objetos.HistoricoPassos>();
            IArenaObjItens.ListaHostoricosP2 = new List<Objetos.HistoricoPassos>();
        }

    }
}




/*
 * APENAS IGNORAR ESSE CÓDIGO, SERVE COMO EXEMPLO CASO NECESSÁRIO.
foreach (var v in graph.Vertices)
{
    double distance = 0;
    object vertex = v;
    Edge<object> predecessor;
    while (predecessors.VertexPredecessors.TryGetValue(vertex, out predecessor))
    {
        distance += edgeCostDictionary[predecessor];
        vertex = predecessor.Source;
        if(endPathVertices.Contains(vertex))
            endPathEdges.Add(predecessor);
    }
    Console.WriteLine("A -> {0}: {1}", v, distance);
}
*/