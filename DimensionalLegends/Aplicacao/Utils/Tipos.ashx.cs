using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Sql;
using System.Data.SqlClient;
using Classes;
using System.Data;

namespace card.Aplicacao.Utils
{
    /// <summary>
    /// Summary description for Tipos
    /// </summary>
    public class Tipos : IHttpHandler
    {
        private string conn = ConfigurationManager.ConnectionStrings["sql"].ToString();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            Classes.Objetos.Feedback feed = new Classes.Objetos.Feedback();

            // classe de conexão
            SqlConnection conex = new SqlConnection(conn);

            // data readers
            SqlDataReader rs = null;

            conex.Open();

            try
            {
                SqlCommand cmd = new SqlCommand("get_tipos", conex);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                rs = cmd.ExecuteReader();

                List<Classes.Objetos.Tipos> listaTipos = new List<Classes.Objetos.Tipos>();

                while (rs.Read())
                {
                    Classes.Objetos.Tipos ITipos = new Classes.Objetos.Tipos();
                    ITipos.TipoId = int.Parse(rs["TipoId"].ToString());
                    ITipos.Tipo = rs["Tipo"].ToString();

                    listaTipos.Add(ITipos);
                }

                rs.Close();

                feed.ListaTipos = listaTipos;
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