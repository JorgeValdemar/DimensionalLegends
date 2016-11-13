using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Summary description for connection
/// </summary>

namespace App_Code
{
    public class connection : System.Web.UI.Page 
    {
        public const String variaveisDct = "inicio=true"; 
        public const String erro = "&erro=false";
        public String exErro = "&exception=";

        public SqlConnection connectionStr = new SqlConnection("server=.\\SQLEXPRESS; database=cardgame; Integrated Security=SSPI;");

        public SqlDataReader openSelectConnection(String cmdSql)
        {
            try
            {
                using (DataSet dataSet = new DataSet())
                {
                    SqlCommand commando = new SqlCommand(cmdSql, connectionStr);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(commando);
                    commando.CommandType = CommandType.Text;
                    SqlDataReader rs = commando.ExecuteReader();

                    return rs;
                }
            }
            catch (Exception ex)
            {
                exErro = exErro + ex;
                return null;
            }
        }


        public Dictionary<int, Object> selectConnectionArr(String cmdSql, Array[] param)
        {
            connectionStr.Open(); //ABRIR CONEXÃO
            try
            {
                using (DataSet dataSet = new DataSet())
                {
                    SqlCommand commando = new SqlCommand(cmdSql, connectionStr);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(commando);
                    commando.CommandType = CommandType.Text;
                    SqlDataReader rs = commando.ExecuteReader();
                    
                    Dictionary<String, String> dados = new Dictionary<String, String>();
                    Dictionary<int, Object> listaRS = new Dictionary<int, Object>();
                    int aux = 0;

                    while (rs.Read())
                    {
                        for (Int32 i = 0; i <= param.Length; i++)
                        {
                            dados.Add(param[i].ToString(), rs.GetString(0));
                        }
                        listaRS.Add(aux, dados);
                        aux++;
                    }

                    return listaRS;
                }
            }
            catch (Exception ex)
            {
                exErro = exErro + ex;
                connectionStr.Close(); //FECHAR CONEXAO!!!
                return null;
            }
        }


        public bool openInsertConnection(String cmdSql)
        {
            try
            {
                using (DataSet dataSet = new DataSet())
                {
                    DataSet insertDataDetails = new DataSet();
                    SqlCommand commando = new SqlCommand(cmdSql, connectionStr);
                    SqlDataAdapter insertDataAdapter = new SqlDataAdapter(commando);
                    insertDataAdapter.Fill(insertDataDetails);
                }
                return true;
            }
            catch (Exception ex)
            {
                exErro = exErro + ex;
                return false;
            }
        }

    }
}