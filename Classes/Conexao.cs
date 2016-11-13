using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;


/// <summary>
/// Summary description for connection
/// </summary>

namespace Classes
{
    public class Conexao
    {
        public static String variaveisDct = "inicio=true";
        public static String erro = "&erro=false";
        public static String exErro = "&exception=";

        public static SqlConnection connectToDatabase()
        {
            SqlConnection connectionStr = new SqlConnection( "server=.\\SQLEXPRESS; database=cardgame; Integrated Security=SSPI;");

            return connectionStr;
        }

    }
}