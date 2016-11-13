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
        private Classes.Objetos.ArenaObjItens IArenaObjItens;

        private bool IsFirstTurn()
        {
            // verificar a quantia de cartas para cada lado.
            if (
                IArenaObjItens.p1deckListCard.Count() == 5
                &&
                IArenaObjItens.p2deckListCard.Count() == 5
                )
                return true;

            return false;
        }

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
                if (IArenaObjItens.arenasituacao2Dobject[xVa, yVa].Usado)
                {
                    string[][] blocoAttrAdjacente = IArenaObjItens.arenasituacao2Dobject[xVa, yVa].Card.Atributos;


                    if (this.AtributoIsLetra(blocoAttrAdjacente[xaPos][yaPos]) && this.AtributoIsLetra(valorAtributo))
                    {
                        // se os dois forem letras

                        if (valorAtributo.ToUpper() == "S")
                        {
                            if (blocoAttrAdjacente[xaPos][yaPos].ToUpper() == "S")
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

                            if(blocoAttrAdjacente[xaPos][yaPos].ToUpper() == "S")
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
                    else if (this.AtributoIsLetra(blocoAttrAdjacente[xaPos][yaPos]) || this.AtributoIsLetra(valorAtributo))
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
                        
                        switch (this.ConflitoNumerico(valorAtributo, blocoAttrAdjacente[xaPos][yaPos]))
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

        public void Arcade()
        {
            /*
             * leia a arena e as cartas.
             * gere um custo para cada um bloco e carta, 
             * adicione arvore para a IA trabalhar e percorrer tanto com o bloco quanto a carta.
             */
            UndirectedGraph<object, Edge<object>> graph = new UndirectedGraph<object, Edge<object>>(false);
//            AdjacencyGraph<object, Edge<object>> graph = new AdjacencyGraph<object, Edge<object>>(false);
            List<Edge<object>> aresta = new List<Edge<object>>();
            List<double> arestaCusto = new List<double>();
            List<object> vertice = new List<object>();

            arestaCusto.Add(0);
            vertice.Add("raiz");

            for (int xPos = 0; xPos < Objetos.ArenaObjItens.COLUMNS; xPos++)
            {
                for (int yPos = 0; yPos < Objetos.ArenaObjItens.ROWS; yPos++)
                {
                    if (IArenaObjItens.arenasituacao2Dobject[xPos, yPos].Usado)
                        continue;

                    int auxCusto = 0;

                    // vamos preencher o que vamos usar!
                    IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore = new List<Objetos.Card>();
                    IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore.AddRange(IArenaObjItens.p2deckListCard);

                    // sendo assim teremos 5 valores para cada um dos blocos
                    for (int i = 0; i < IArenaObjItens.p2deckListCard.Count(); i++)
                    {
                        IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore[i].CustoBusca = this.CustoFinal(this.CustoPorBloco(xPos, yPos, i));

                        string id = Guid.NewGuid().ToString();
                        // cria os atributos para os graficos
                        //graph.AddVertex(IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore[i]);

                        if (auxCusto > IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore[i].CustoBusca)
                            auxCusto = IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore[i].CustoBusca;

                        IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore[i].IdTemp = id;
                        
                        aresta.Add(new Edge<object>(IArenaObjItens.arenasituacao2Dobject[xPos, yPos], IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore[i].IdTemp));
                        arestaCusto.Add(IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore[i].CustoBusca);

                        IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore[i].IdTemp = id;
                        vertice.Add(IArenaObjItens.arenasituacao2Dobject[xPos, yPos].ListaCartasArvore[i].IdTemp);
                        
                    }

                    IArenaObjItens.arenasituacao2Dobject[xPos, yPos].Custo = auxCusto;


                    aresta.Add(new Edge<object>("raiz", IArenaObjItens.arenasituacao2Dobject[xPos, yPos]));
                    arestaCusto.Add(IArenaObjItens.arenasituacao2Dobject[xPos, yPos].Custo);
                    vertice.Add(IArenaObjItens.arenasituacao2Dobject[xPos, yPos]);


                    // cria os atributos para os graficos
                    //graph.AddVertex(IArenaObjItens.arenasituacao2Dobject[xPos, yPos]);
                }
            }


            // adicionamos todas as informações acima dentro do grafico
            graph.AddVertexRange(vertice);
            graph.AddEdgeRange(aresta);

            Dictionary<Edge<object>, double> edgeCostDictionary = new Dictionary<Edge<object>, double>(graph.EdgeCount);

            for (int i = 0; i < aresta.Count; i++)
                edgeCostDictionary.Add(aresta[i], arestaCusto[i+1]);

            //var d = edgeCostDictionary[x1_y1_c0]; // funciona
            
            Func<Edge<object>, double> edgeCost = AlgorithmExtensions.GetIndexer(edgeCostDictionary);
            
            // PATHFIND
            // A Star Algorithm

            // ......

            // inicia variavel tempo

            // Dijkstra Algorithm
           // var dijkstra = new DijkstraShortestPathAlgorithm<object, Edge<object>>(graph, edgeCost);



//            var dijkstra = new UndirectedDijkstraShortestPathAlgorithm<object, Edge<object>>(graph, edgeCost);
            var dijkstra = new QuickGraph.Algorithms.ShortestPath.UndirectedDijkstraShortestPathAlgorithm<object, Edge<object>>(graph, edgeCost);
//            dijkstra.
 
            
            /*
            VertexDistanceRecorderObserver<object, Edge<object>> distObserver = new VertexDistanceRecorderObserver<object, Edge<object>>(edgeCost);
            distObserver.Attach(dijkstra);

            // Attach a Vertex Predecessor Recorder Observer to give us the paths
            VertexPredecessorRecorderObserver<object, Edge<object>> predecessorObserver = new VertexPredecessorRecorderObserver<object, Edge<object>>();
            predecessorObserver.Attach(dijkstra);

            // Run the algorithm with A set to be the source
            dijkstra.Compute("raiz");

            foreach (KeyValuePair<object, Edge<object>> kvp in distObserver.Distances)
                Console.WriteLine("Distance from root to node {0} is {1}", kvp.Key, kvp.Value);

            foreach (KeyValuePair<object, Edge<object>> kvp in predecessorObserver.VertexPredecessors)
                Console.WriteLine("If you want to get to {0} you have to enter through the in edge {1}", kvp.Key, kvp.Value);

            // Remember to detach the observers
            distObserver.Detach(dijkstra);
            predecessorObserver.Detach(dijkstra);

            */


            UndirectedVertexPredecessorRecorderObserver<object, Edge<object>> distObserver = new UndirectedVertexPredecessorRecorderObserver<object, Edge<object>>();
            
//            VertexDistanceRecorderObserver<object, Edge<object>> distObserver = new VertexDistanceRecorderObserver<object, Edge<object>>(edgeCost);
            distObserver.Attach(dijkstra);


            // Attach a Vertex Predecessor Recorder Observer to give us the paths
            var predecessors = new VertexPredecessorRecorderObserver<object, Edge<object>>();
           // predecessors.Attach(dijkstra);

            var PathObserver = new VertexPredecessorPathRecorderObserver<object, Edge<object>>(predecessors.VertexPredecessors);
            //PathObserver.Attach(dijkstra);



            dijkstra.Compute("raiz"); // valor padrão, root.

            
            IEnumerable<Edge<object>> path;
            if (distObserver.TryGetPath(null, out path))
                Console.WriteLine(path);


            QuickGraph.Algorithms.Services.IService fim;
//            IUndirectedGraph<object, Edge<object>> fim;
            while (dijkstra.TryGetService(out fim))
            {
                Console.WriteLine(fim);
            }
            var baba = PathObserver.EndPathVertices;

            //using (predecessors.Attach(dijkstra))
            // Run the algorithm with A set to be the source //dijkstra.Compute("A ROOT");


//            foreach (KeyValuePair<object, double> kvp in distObserver.Distances)
  //              Console.WriteLine("Distance from root to node {0} is {1}", kvp.Key, kvp.Value);


            foreach (var v in graph.Vertices)
            {
                double distance = 0;
                object vertex = v;
                Edge<object> predecessor;
                while (predecessors.VertexPredecessors.TryGetValue(vertex, out predecessor))
                {
                    distance += edgeCostDictionary[predecessor];
                    vertex = predecessor.Source;
                }
                Console.WriteLine("A -> {0}: {1}", v, distance);
            }

            

            // recebe o tempo gasto
        }

        public void Player(int p)
        {
            /*
             * no primeiro turno o player 1 tem tudo o que precisa,
             * não tem necessidade de processamento.
             */
            if (this.IsFirstTurn()) return;



        }

        // construtor
        public Decisao(Classes.Objetos.ArenaObjItens arenaObjItens)
        {
            IArenaObjItens = arenaObjItens;
        }

    }
}
