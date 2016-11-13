using Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Data;

namespace DimensionalLegends.Aplicacao.Outros
{
    /// <summary>
    /// Summary description for HandlerContato
    /// </summary>
    public class HandlerContato : IHttpHandler
    {
        Classes.Objetos.Feedback feed = new Classes.Objetos.Feedback();
        private string conn = ConfigurationManager.ConnectionStrings["sql"].ToString();
        private gamefunctions gameFunc = new gamefunctions();

        private void EnviarEmail(Classes.Objetos.Login ILogin)
        {
            string corpo = String.Concat("Oi o email funciona.");

            corpo = corpo.Replace("'", @""""); // GAMBIARRA

            MailAddress from = new MailAddress("tsuru.massoterapia@gmail.com");
            MailAddress to = new MailAddress("tsuru.massoterapia@gmail.com");
            MailMessage msg = new MailMessage(from, to);
            msg.Subject = "Contato recebido do site Tsuru";
            msg.Body = corpo;
            msg.IsBodyHtml = true;
            msg.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
            msg.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient("envio.redehost.com.br"); //servidor smtp da redehost ja tentei com o mail.meusmtp.com.br
            //            SmtpClient smtp = new SmtpClient("mail32.redehost.com.br"); //servidor smtp da redehost ja tentei com o mail.meusmtp.com.br
            //            smtp.Credentials = new NetworkCredential("contato@meuemail.com.br", "minhasenha");
            //smtp.Port = 25;
            //smtp.Port = 587;

            smtp.Send(msg);
            if (msg != null) msg.Dispose();


        }

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

            Classes.Objetos.Login ILogin = new Classes.Objetos.Login();
            ILogin = JsonConvert.DeserializeObject<Classes.Objetos.Login>(data);


            SqlConnection conex = new SqlConnection(conn);
            SqlDataReader rs = null;


            conex.Open();

            try
            {

                SqlCommand cmd = new SqlCommand("ex_create_login", conex);
                /*
                 * meio difícil rodar injection mas mesmo assim vamos usar um validador SQL da gameFunc
                 */
                cmd.Parameters.Add("@Nome", SqlDbType.VarChar, 60).Value = gameFunc.validaSQL(ILogin.Nome);
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 60).Value = gameFunc.validaSQL(ILogin.Email);
                cmd.Parameters.Add("@Codigo", SqlDbType.Int).Value = ILogin.Codigo;
                cmd.Parameters.Add("@Senha", SqlDbType.VarChar, 60).Value = gameFunc.validaSQL(ILogin.Senha);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    string id = rs["id"].ToString();

                    if (id == "0")
                    {
                        throw new Exception("Já existe um usuário com este E-mail.");
                    }
                    else
                    {
                        ILogin.id = id;
                        this.EnviarEmail(ILogin);
                    }

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

    class Contato
    {

    }

}