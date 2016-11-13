using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for gamefunctions
/// </summary>

namespace Classes
{
    public class gamefunctions
    {
        public Dictionary<int, int> totalCartas = new Dictionary<int, int>();

        public gamefunctions()
        {

            //
            // TODO: Add constructor logic here
            //
        }

        public static string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }

        public static string Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);
            return System.Text.Encoding.UTF8.GetString(encoded);
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
            if (string.IsNullOrEmpty(param) || param == "null")
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
            if (string.IsNullOrEmpty(s) || (s == "null"))
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