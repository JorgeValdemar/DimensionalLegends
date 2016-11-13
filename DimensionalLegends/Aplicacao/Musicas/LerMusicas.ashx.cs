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

namespace card.Aplicacao.Musicas
{
    /// <summary>
    /// Summary description for LerMusica
    /// </summary>
    public class LerMusicas : IHttpHandler
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

            Classes.Objetos.Musica IPlayerMusica = new Classes.Objetos.Musica();
            IPlayerMusica = JsonConvert.DeserializeObject<Classes.Objetos.Musica>(data);


            SqlConnection conex = new SqlConnection(conn);
            SqlDataReader rs = null;

            conex.Open();

            try
            {
                SqlCommand cmd = new SqlCommand("get_player_musica_lista_existentes", conex);
                cmd.Parameters.Add("@InternautaId", SqlDbType.VarChar, 60).Value = IPlayerMusica.InternautaId;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                rs = cmd.ExecuteReader();

                List<Classes.Objetos.Musica> IListaMusica = new List<Classes.Objetos.Musica>();

                while (rs.Read())
                {
                    Classes.Objetos.Musica RMusica = new Classes.Objetos.Musica();
                    RMusica.Id = int.Parse(rs["Id"].ToString());
                    RMusica.Nome = rs["Nome"].ToString();
                    RMusica.Album = rs["Album"].ToString();
                    RMusica.Autor = rs["Autor"].ToString();
                    RMusica.NomePath = rs["NomePath"].ToString();
                    RMusica.Existe = true;

                    IListaMusica.Add(RMusica);
                }

                rs.Close();

                feed.ListaMusicas = IListaMusica;
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