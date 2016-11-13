using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Sql;
using System.Data.SqlClient;
using Classes;
using System.Data;

namespace card.Aplicacao.Opcoes
{
    /// <summary>
    /// Summary description for Atualizar
    /// </summary>
    public class Atualizar : IHttpHandler
    {
        private string conn = ConfigurationManager.ConnectionStrings["sql"].ToString();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            Classes.Objetos.Feedback feed = new Classes.Objetos.Feedback();

            var data = context.Request.Form["data"];

            if (string.IsNullOrEmpty(data))
            {
                feed.Erro = true;
                feed.ErroDescricao = "dados nulos";

                string jsonErro = JsonConvert.SerializeObject(feed);
                context.Response.Write(jsonErro);
                return;
            }

            Classes.Objetos.Opcoes IPlayerOpcoes = new Classes.Objetos.Opcoes();
            IPlayerOpcoes = JsonConvert.DeserializeObject<Classes.Objetos.Opcoes>(data);


            SqlConnection conex = new SqlConnection(conn);
            SqlDataReader rs = null;

            conex.Open();

            try
            {
                SqlCommand cmd = new SqlCommand("up_player_opcoes", conex);
                cmd.Parameters.Add("@InternautaId", SqlDbType.VarChar, 60).Value = IPlayerOpcoes.InternautaId;
                cmd.Parameters.Add("@Idioma", SqlDbType.VarChar, 60).Value = IPlayerOpcoes.Idioma;
                cmd.Parameters.Add("@Som", SqlDbType.VarChar, 60).Value = IPlayerOpcoes.Som;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    Classes.Objetos.Opcoes ROpcoes = new Classes.Objetos.Opcoes();
                    ROpcoes.Idioma = int.Parse(rs["Idioma"].ToString());
                    ROpcoes.Som = int.Parse(rs["Som"].ToString());

                    feed.Opcoes = ROpcoes;

                    rs.Close();
                }

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