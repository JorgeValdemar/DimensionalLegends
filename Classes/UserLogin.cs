using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Classes
{
    public class UserLogin
    {
        public static string teste()
        {
            SqlConnection conex = Conexao.connectToDatabase();
            SqlDataReader rd = null;

            try
            {
                conex.Open();
                string total = "";
                SqlCommand cmd = new SqlCommand("cards_by_number", conex);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                rd = cmd.ExecuteReader();
                
                
                while (rd.Read())
                {
                    total += rd["Numero"].ToString() + rd["Rank"].ToString() + rd["Nome"].ToString() + "<br /><br />";
                }

                return "CONECTADO!!!"+total;
                conex.Close();
                rd.Close();
            }
            catch (Exception ex)
            {
                return "ERRO: " + ex;
            }
            finally
            {
                if (conex != null)
                {
                    conex.Close();
                }
                if (rd != null)
                {
                    rd.Close();
                }
            }
        }
    }
}
