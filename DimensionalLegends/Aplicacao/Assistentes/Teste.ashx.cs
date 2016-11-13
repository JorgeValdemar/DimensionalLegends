using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace card.Aplicacao.Assistentes
{
    /// <summary>
    /// Summary description for Teste
    /// </summary>
    public class Teste : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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