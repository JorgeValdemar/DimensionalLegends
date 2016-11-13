using Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DimensionalLegends.Aplicacao.Home
{
    /// <summary>
    /// Summary description for Ativacao
    /// </summary>
    public class Ativacao : IHttpHandler
    {
        private string conn = ConfigurationManager.ConnectionStrings["sql"].ToString();
        private gamefunctions gameFunc = new gamefunctions();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            var data = context.Request.Params;

            if (string.IsNullOrEmpty(data["id"]) || string.IsNullOrEmpty(data["code"]))
            {
                context.Response.Write("Houve uma falha no link");
                return;
            }

            string id = data["id"].ToString();
            int codigo = int.Parse(data["code"].ToString());

            // classe de conexão
            SqlConnection conex = new SqlConnection(conn);

            // data readers
            SqlDataReader rs = null;

            conex.Open();

            try
            {

                // verifica login valido
                SqlCommand cmd = new SqlCommand("ex_ativa_login", conex);
                cmd.Parameters.Add("@id", SqlDbType.VarChar, 200).Value = gameFunc.validaSQL(id).ToString();
                cmd.Parameters.Add("@Codigo", SqlDbType.Int, 200).Value = int.Parse(gameFunc.validaSQL(codigo).ToString());
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    int resp = int.Parse(rs["resposta"].ToString());

                    if (resp == 0)
                    {
                        context.Response.Write("Este código de ativação está inválido.");
                    }

                    rs.Close();
                }

            }
            catch (Exception ex)
            {
                context.Response.Write(String.Concat("Houve um erro, favor entrar em contato: ", ex));
                return;
            }
            finally
            {
                conex.Close();
            }


            context.Response.Redirect("http://dimensionallegends.com/Login");
            return;
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