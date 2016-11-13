using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.SessionState;
using Classes;
using System.Data;
using System.Web.Services;
using System.IO;

namespace card.Aplicacao.Arena
{
    /// <summary>
    /// Summary description for ConfigurarEntrada
    /// </summary>
    public class ConfigurarEntrada : IHttpHandler, IRequiresSessionState
    {
        private Classes.Objetos.Feedback feed = new Classes.Objetos.Feedback();
        private string conn = ConfigurationManager.ConnectionStrings["sql"].ToString();
        private Random numRand = new Random();
        
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string RequestType = context.Request.RequestType.ToString();

            var data = context.Request["data"];

            if (RequestType != "POST")
            {
                feed.Erro = true;
                feed.ErroDescricao = "Erro desconhecido";

                string jsonErro = JsonConvert.SerializeObject(feed);
                context.Response.Write(jsonErro);
                return;
            }

            if (context.Request.Cookies["UserSessionId"] == null)
            {
                feed.Erro = true;
                feed.ErroDescricao = "Usuário não está logado";

                string jsonErro = JsonConvert.SerializeObject(feed);
                context.Response.Write(jsonErro);
                return;
            }

            if (string.IsNullOrEmpty(data))
            {
                feed.Erro = true;
                feed.ErroDescricao = "dados nulos";

                string jsonErro = JsonConvert.SerializeObject(feed);
                context.Response.Write(jsonErro);
                return;
            }

            Classes.Objetos.PlayerStatus IPlayerStatus = new Classes.Objetos.PlayerStatus();
            IPlayerStatus.InternautaId = context.Request.Cookies["UserSessionId"].Value.ToString();

            // ID da fase
            Classes.Objetos.Fase IFase = new Classes.Objetos.Fase();
            IFase = JsonConvert.DeserializeObject<Classes.Objetos.Fase>(data);

            // lista de cartas selecionadas
            feed = JsonConvert.DeserializeObject<Classes.Objetos.Feedback>(data);
            //feed.ListaCards = JsonConvert.DeserializeObject<List<Classes.Objetos.Card>>(feed.ListaString);

            // Validar informações:
            // Verifica pelo banco se as informações passadas são autenticas.
            SqlConnection conex = new SqlConnection(conn);
            SqlDataReader rs = null;

            conex.Open();
            
            try
            {
                SqlCommand cmd = new SqlCommand("get_player_fase_valida", conex);
                cmd.Parameters.Add("@InternautaId", SqlDbType.VarChar, 60).Value = IPlayerStatus.InternautaId;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = IFase.Id;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    int resultado;
                    resultado = int.Parse(rs["resultado"].ToString());
                    if (resultado == 1)
                    {
                        IFase.LiderId = int.Parse(rs["LiderId"].ToString());
                    }
                    else
                    {
                        feed.Erro = true;
                        feed.ErroDescricao = "ID incorreto encontrado.";

                        string jsonErroID = JsonConvert.SerializeObject(feed);
                        context.Response.Write(jsonErroID);
                        return;
                    }
                }

                rs.Close();
                rs = null;

                /*
                 * ATENÇÃO
                 * Necessário chamar todas as cartas selecionadas novamente, 
                 * pois alguém poderia alterar os atributos
                 * 
                 * */
                
                List<int> numeroCartas = new List<int>();
                
                for (int i = 0; i < feed.ListaCards.Count; i++)
                {
                    //numeroCartas[i] = feed.ListaCards[i].Numero; algum problema de versão entende como index limit 0
                    numeroCartas.Add(feed.ListaCards[i].Numero);
                    
                    SqlCommand cmd2 = new SqlCommand("get_player_card_valido", conex);
                    cmd2.Parameters.Add("@InternautaId", SqlDbType.VarChar, 60).Value = IPlayerStatus.InternautaId;
                    cmd2.Parameters.Add("@Numero", SqlDbType.Int).Value = feed.ListaCards[i].Numero;
                    cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                    rs = cmd2.ExecuteReader();

                    while (rs.Read())
                    {
                        int resultado;
                        resultado = int.Parse(rs["resultado"].ToString());
                        if (resultado == 1)
                        {
                            continue;
                        }
                        else
                        {
                            feed.Erro = true;
                            feed.ErroDescricao = "ID incorreto encontrado.";

                            string jsonErroID = JsonConvert.SerializeObject(feed);
                            context.Response.Write(jsonErroID);
                            return;
                        }
                    }

                    rs.Close();

                }


                // recupera cartas do player

                SqlCommand cmd6 = new SqlCommand("get_player_cards", conex);
                cmd6.Parameters.Add("@InternautaId", SqlDbType.VarChar, 60).Value = context.Request.Cookies["UserSessionId"].Value.ToString();
                cmd6.CommandType = System.Data.CommandType.StoredProcedure;
                rs = cmd6.ExecuteReader();

                List<Classes.Objetos.Card> IListaCard = new List<Classes.Objetos.Card>();

                while (rs.Read())
                {
                    int numeroDeckCard = int.Parse(rs["Numero"].ToString());

                    if (numeroCartas.Contains(numeroDeckCard))
                    {
                        Classes.Objetos.Card RCard = new Classes.Objetos.Card();

                        RCard.Numero = numeroDeckCard;
                        RCard.Rank = int.Parse(rs["Rank"].ToString());
                        RCard.Nome = rs["Nome"].ToString();

                        RCard.A1 = rs["A1"].ToString();
                        RCard.A2 = rs["A2"].ToString();
                        RCard.A3 = rs["A3"].ToString();
                        RCard.B1 = rs["B1"].ToString();
                        RCard.B2 = rs["B2"].ToString();
                        RCard.C1 = rs["C1"].ToString();
                        RCard.C2 = rs["C2"].ToString();
                        RCard.C3 = rs["C3"].ToString();

                        RCard.Elemento = new Classes.Objetos.Elementos();
                        RCard.Elemento.ElementoId = int.Parse(rs["ElementoId"].ToString());
                        RCard.Elemento.Elemento = rs["Elemento"].ToString();

                        RCard.EmUso = bool.Parse(rs["EmUso"].ToString());

                        IListaCard.Add(RCard);

                    }

                }

                rs.Close();




                // recupera 10 cartas do arcade

                SqlCommand cmd3 = new SqlCommand("get_arcade_cartas", conex);
                cmd3.Parameters.Add("@Id", SqlDbType.Int).Value = IFase.Id;
                cmd3.CommandType = System.Data.CommandType.StoredProcedure;
                rs = cmd3.ExecuteReader();

                List<Classes.Objetos.Card> IListaArcadeCartas = new List<Classes.Objetos.Card>();

                while (rs.Read())
                {
                    Classes.Objetos.Card IArcadeCarta = new Classes.Objetos.Card();
                    IArcadeCarta.Numero = int.Parse(rs["Numero"].ToString());
                    IArcadeCarta.Rank = int.Parse(rs["Rank"].ToString());
                    IArcadeCarta.Nome = rs["Nome"].ToString();
                    IArcadeCarta.A1 = rs["A1"].ToString();
                    IArcadeCarta.A2 = rs["A2"].ToString();
                    IArcadeCarta.A3 = rs["A3"].ToString();
                    IArcadeCarta.B1 = rs["B1"].ToString();
                    IArcadeCarta.B2 = rs["B2"].ToString();
                    IArcadeCarta.C1 = rs["C1"].ToString();
                    IArcadeCarta.C2 = rs["C2"].ToString();
                    IArcadeCarta.C3 = rs["C3"].ToString();

                    IArcadeCarta.Elemento = new Classes.Objetos.Elementos();
                    IArcadeCarta.Elemento.ElementoId = int.Parse(rs["ElementoId"].ToString());
                    IArcadeCarta.Elemento.Elemento = rs["Elemento"].ToString();

                    IListaArcadeCartas.Add(IArcadeCarta);
                }

                rs.Close();
                rs = null;


                // recupera a musica da fase

                SqlCommand cmd4 = new SqlCommand("get_arcade_musica", conex);
                cmd4.Parameters.Add("@Id", SqlDbType.Int).Value = IFase.Id;
                cmd4.CommandType = System.Data.CommandType.StoredProcedure;
                rs = cmd4.ExecuteReader();

                Classes.Objetos.Musica IMusica = new Classes.Objetos.Musica();

                if (rs.Read())
                {
                    IMusica.Id           = int.Parse(rs["Id"].ToString());
                    IMusica.Nome         = rs["Nome"].ToString();
                    IMusica.Album        = rs["Album"].ToString();
                    IMusica.Autor        = rs["Autor"].ToString();
                    IMusica.NomePath     = rs["NomePath"].ToString();
                }

                rs.Close();
                rs = null;

                List<Classes.Objetos.Card> ICartasArcadeEscolhidas = new List<Classes.Objetos.Card>();
                ICartasArcadeEscolhidas = this.EscolherCartas(IListaArcadeCartas);

                Classes.Objetos.ArenaConfig config = new Classes.Objetos.ArenaConfig();

                Classes.Objetos.CardBloco[,] arenaTabuleiro = new Classes.Objetos.CardBloco[,]    
                {
                    { new Classes.Objetos.CardBloco(), new Classes.Objetos.CardBloco(), new Classes.Objetos.CardBloco() }, 
                    { new Classes.Objetos.CardBloco(), new Classes.Objetos.CardBloco(), new Classes.Objetos.CardBloco() }, 
                    { new Classes.Objetos.CardBloco(), new Classes.Objetos.CardBloco(), new Classes.Objetos.CardBloco() }, 
                    { new Classes.Objetos.CardBloco(), new Classes.Objetos.CardBloco(), new Classes.Objetos.CardBloco() }
                };

                Classes.Objetos.CardBloco[] linhaTabuleiro = Classes.Objetos.ArenaObjItens.RetornarColuna(0, arenaTabuleiro);

                Classes.Objetos.BattleInfo IBattleInfo = new Classes.Objetos.BattleInfo
                    (
                        null,
                        IPlayerStatus.InternautaId,
                        null,
                        IFase.LiderId
                    );

                IBattleInfo.ConexaoDB = conex;
                if (!IBattleInfo.Insert())
                {
                    feed.Erro = true;
                    feed.ErroDescricao = "Falha ao criar batalha nova";

                    context.Response.Write(JsonConvert.SerializeObject(feed));
                    return;
                }

                // ATENÇÃO: ID DO ARQUIVO AQUI PRESENTE
                config.Id = IBattleInfo.BattleId;

                // TURNO É ESCOLHIDO
                config.Turno = this.TurnoVez();


                Classes.ArquivoLog IArquivoLog = new ArquivoLog
                    (
                        IListaCard, 
                        ICartasArcadeEscolhidas,
                        linhaTabuleiro,
                        linhaTabuleiro,
                        linhaTabuleiro,
                        linhaTabuleiro,
                        config
                    );

                IArquivoLog.Name = IBattleInfo.BattleId;
                IArquivoLog.Update();

                // já temos o objeto feed.ListaCards do player 1 preenchido no inicio desta classe
                // portanto só é necessário as do player 2, na verdade é provisória para teste esta necessidade
                feed.ListaCardsOponente = ICartasArcadeEscolhidas;
                feed.ArenaConfig = config;
                feed.arenasituacaoY1 = linhaTabuleiro;
                feed.arenasituacaoY2 = linhaTabuleiro;
                feed.arenasituacaoY3 = linhaTabuleiro;
                feed.arenasituacaoY4 = linhaTabuleiro;
                feed.Musica = IMusica;
                feed.Erro = false;

            }
            catch (Exception ex)
            {
                feed.Erro = true;
                feed.ErroDescricao = ex.ToString();
                //feed.ErroDescricao = "Exception error: ERRO FATAL, PROBLEMA EM CRIAR PARTIDA.";
            }
            finally
            {
                conex.Close();
            }

            string json = JsonConvert.SerializeObject(feed);

            context.Response.Write(json);

            context.Response.End();
        }

        private List<Classes.Objetos.Card> EscolherCartas(List<Classes.Objetos.Card> lista)
        {
            List<int> num = new List<int>();
            List<Classes.Objetos.Card> novaLista = new List<Classes.Objetos.Card>();

            num = this.NumeroNaoRepetido(5);

            for (int i = 0; i < num.Count; i++)
            {
                novaLista.Add(lista[num[i]]);
            }

            return novaLista;
        }

        private List<int> NumeroNaoRepetido(int qtdNumeros, List<int> num = null)
        {
            if (num == null) num = new List<int>();

            int rand = numRand.Next(0, 10);

            for (int i = 0; i < num.Count; i++)
            {
                if (rand == num[i])
                {
                    NumeroNaoRepetido(qtdNumeros, num);
                }
            }

            if (num.Count != qtdNumeros)
            {
                num.Add(rand);
                NumeroNaoRepetido(qtdNumeros, num);
            }

            return num;
        }

        private int TurnoVez()
        {
            Random numRand = new Random();
            int rand = 1;//numRand.Next(1, 3);

            return rand;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}