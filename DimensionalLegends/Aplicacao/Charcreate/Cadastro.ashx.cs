using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.SessionState;
using Classes;
using System.Data;
using System.Configuration;

namespace card.Aplicacao.Charcreate
{
    /// <summary>
    /// Summary description for Cadastro
    /// </summary>
    public class Cadastro : IHttpHandler, IRequiresSessionState
    {
        Classes.Objetos.Feedback feed = new Classes.Objetos.Feedback();
        private string conn = ConfigurationManager.ConnectionStrings["sql"].ToString();
        private gamefunctions gameFunc = new gamefunctions();


        private String _nick;
        private String _imagem;
        private int _deck;
        private String _internautaId;
        
        private String Nick
        {
            get
            {
                return _nick;
            }
            set
            {
                _nick = value;
            }
        }
        
        
        private String Imagem
        {
            get
            {
                return _imagem;
            }
            set
            {
                _imagem = value;
            }
        }


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


        private String InternautaId
        {
            get
            {
                return _internautaId;
            }
            set
            {
                _internautaId = value;
            }
        }


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            //context.Session
            if (context.Request.Cookies["UserSessionIdTemp"] == null)
            {
                feed.Erro = true;
                feed.ErroDescricao = "Usuário não existe no sistema, cadastro de Avatar só possível com um usuário cadastrado.";

                string jsonErro = JsonConvert.SerializeObject(feed);
                context.Response.Write(jsonErro);
                return;
            }

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
            
            Nick = o["nomeChar"].ToString();
            Imagem = o["imagemChar"].ToString();
            Deck = int.Parse(o["deckChar"].ToString());
            InternautaId = context.Request.Cookies["UserSessionIdTemp"].Value.ToString();

            if (gameFunc.validaParam(Nick) && gameFunc.validaParam(Deck.ToString()) && gameFunc.validaParam(Imagem))
            {
                if (gameFunc.validaNome(Nick))
                {

                    // classe de conexão
                    SqlConnection conex = new SqlConnection(conn);

                    // data readers
                    SqlDataReader rsCard = null;

                    conex.Open(); //ABRIR CONEXÃO

                    try
                    {
                        SqlCommand cmd2 = new SqlCommand("get_char_existe", conex);
                        cmd2.Parameters.Add("@Nome", SqlDbType.VarChar, 30).Value = Nick;
                        cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                        rsCard = cmd2.ExecuteReader();

                        if (rsCard.Read())
                        {
                            feed.Erro = true;
                            feed.NomeExistente = true;
                        }
                        else
                        {
                            feed.NomeExistente = false;
                            cadastrarDeck(context);
                        }

                        rsCard.Close();
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
                }
                else
                {
                    feed.Erro = true;
                    feed.ErroDescricao = "Avatar inválido, deve conter apenas: letras, numeros, espaço, _ ou -";
                }
            }
            else
            {
                feed.Erro = true;
                feed.ErroDescricao = "Dados incompletos, preencha todas as informações";
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


        private void cadastrarDeck(HttpContext context)
        {
            // classe de conexão
            SqlConnection conex = new SqlConnection(conn);

            conex.Open(); //ABRIR CONEXÃO

            try
            {
                
                SqlCommand command = new SqlCommand("ex_cadastrar_player", conex);
                command.Parameters.Add("@InternautaId", SqlDbType.VarChar, 60).Value = InternautaId;
                command.Parameters.Add("@Nick", SqlDbType.VarChar, 20).Value = Nick;
                command.Parameters.Add("@Imagem", SqlDbType.VarChar, 50).Value = Imagem;
                command.Parameters.Add("@Deck", SqlDbType.Int).Value = Deck;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.ExecuteNonQuery();


                feed.Erro = false;


                var cookie = new HttpCookie("UserSessionId", InternautaId);
                cookie.Expires.AddYears(1);
                context.Response.SetCookie(cookie);
            }
            catch (Exception ex)
            {
                feed.Erro = true;
                feed.ErroDescricao = ex.GetType().ToString() + "++----++" + ex.Message;
            }
            finally
            {
                conex.Close();
            }
        }

    }
}