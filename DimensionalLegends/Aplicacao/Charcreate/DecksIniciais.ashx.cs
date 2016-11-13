using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;
using Classes;
using System.Data;
using System.Configuration;
using Newtonsoft.Json;

namespace card.Aplicacao.Charcreate
{
    /// <summary>
    /// Summary description for DecksIniciais
    /// </summary>
    public class DecksIniciais : IHttpHandler
    {
        private string conn = ConfigurationManager.ConnectionStrings["sql"].ToString();
        private gamefunctions gameFunc = new gamefunctions();
        private int _deck = 1;

        private int Deck
        {
            get
            {
                return _deck;
            }
            set
            {
                _deck = value;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            Classes.Objetos.Feedback feed = new Classes.Objetos.Feedback();

            var data = context.Request.Form["data"];

            if (string.IsNullOrEmpty(data))
            {
                feed.Erro = true;
                feed.ErroDescricao = "dado nulo";

                string jsonErro = JsonConvert.SerializeObject(feed);
                context.Response.Write(jsonErro);
                return;
            }

            Dictionary<string, string> o = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            Deck = int.Parse(o["Deck"].ToString());

            // RETORNA OS DECKS INICIAIS
            //gameFunc.setQtdCarta(56, 3);
            //gameFunc.setQtdCarta(62, 3);

            SqlConnection conex = new SqlConnection(conn);
            SqlDataReader rsCard = null;

            conex.Open();

            try
            {
                SqlCommand cmd2 = new SqlCommand("get_deck_inicial", conex);
                cmd2.Parameters.Add("@deckNum", SqlDbType.Int).Value = Deck;
                cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                rsCard = cmd2.ExecuteReader();

                List<Classes.Objetos.Card> listaCartas = new List<Classes.Objetos.Card>();

                while (rsCard.Read())
                {
                    Classes.Objetos.Card ICarta = new Classes.Objetos.Card();

                    ICarta.Numero = int.Parse(rsCard["Numero"].ToString());
                    ICarta.Rank = int.Parse(rsCard["Rank"].ToString());
                    ICarta.Nome = rsCard["Nome"].ToString();

                    ICarta.A1 = rsCard["A1"].ToString();
                    ICarta.A2 = rsCard["A2"].ToString();
                    ICarta.A3 = rsCard["A3"].ToString();
                    ICarta.B1 = rsCard["B1"].ToString();
                    ICarta.B2 = rsCard["B2"].ToString();
                    ICarta.C1 = rsCard["C1"].ToString();
                    ICarta.C2 = rsCard["C2"].ToString();
                    ICarta.C3 = rsCard["C3"].ToString();

                    ICarta.Elemento = new Classes.Objetos.Elementos();
                    string elemid = rsCard["ElementoId"].ToString();

                    ICarta.Elemento.ElementoId = elemid == "0" ? 0 : int.Parse(elemid);
                    ICarta.Elemento.Elemento = rsCard["Elemento"].ToString();

                    listaCartas.Add(ICarta);

                }

                
                rsCard.Close();

                feed.ListaCards = listaCartas;
                feed.Erro = false;

            }
            catch (Exception ex)
            {
                feed.Erro = true;
                feed.ErroDescricao = ex.ToString();
            }
            finally
            {
                conex.Close();
            }

            string json = JsonConvert.SerializeObject(feed);

            context.Response.Write(json);
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