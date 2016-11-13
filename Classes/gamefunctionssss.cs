using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for gamefunctions
/// </summary>

namespace App_Code
{
    public class gamefunctions : System.Web.UI.Page
    {
        public Dictionary<int, int> totalCartas = new Dictionary<int, int>();

        public gamefunctions()
        {

            //
            // TODO: Add constructor logic here
            //
        }

        public object validaSQL(Object o)
        {
            String aux;
            aux = o.ToString();
            Object rtn = aux.Replace("'", "''");

            return rtn;
        }


        public Int32 recebeIntParam(String param)
        {
            if ((param == "") || (param == null))
            {
                return 0;
            }
            else
            {
                return int.Parse(param);
            }
        }


        public Boolean validaParam(String s)
        {
            if ((s == "") || (s == null))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void setQtdCarta(int idCard, int qtdCards)
        {
            totalCartas.Add(idCard, qtdCards);
        }


        public int getQtdCarta(int idCard)
        {

            try
            {
                int aux = int.Parse(totalCartas[idCard].ToString());

                if (aux > 1)
                {
                    return aux;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception e)
            {
                return 1;
            }
        }

        public bool validaNome(string nome)
        {
            Regex regExpNome = new Regex("[^a-zA-Z0-9_ -]+");
            //Regex regExpNome = new Regex("^[a-zA-Z0-9_ -]+$");
            if (regExpNome.IsMatch(nome))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}