using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Sql;
using System.Data.SqlClient;
using Classes;
using System.Data;
using System.Web.Services;
using System.IO;

namespace card.Aplicacao.Arena
{
    /// <summary>
    /// Summary description for TurnoAcao
    /// </summary>
    public class TurnoAcao : IHttpHandler, IRequiresSessionState
    {
        private Classes.Objetos.Feedback feed = new Classes.Objetos.Feedback();
        private Classes.Objetos.BattleInfo IBattleInfo = new Classes.Objetos.BattleInfo();
        private Classes.Objetos.ArenaObjItens IArenaObjItens = new Classes.Objetos.ArenaObjItens();
        private string conn = ConfigurationManager.ConnectionStrings["sql"].ToString();
        private ArquivoLog IArquivoLog;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string RequestType = context.Request.RequestType.ToString();

            var data = context.Request["data"];

            if (RequestType != "POST")
            {
                feed.Erro = true;
                feed.ErroDescricao = "Erro desconhecido";

                string jsonErro = JsonConvert.SerializeObject(feed);
                context.Response.Write(jsonErro);
                return;
            }

            // TODO: ver onde o ID se encaixa aqui
            if (context.Request.Cookies["UserSessionId"] == null)
            {
                feed.Erro = true;
                feed.ErroDescricao = "Usuário não está logado";

                string jsonErro = JsonConvert.SerializeObject(feed);
                context.Response.Write(jsonErro);
                return;
            }

            if (string.IsNullOrEmpty(data))
            {
                feed.Erro = true;
                feed.ErroDescricao = "dados nulos";

                string jsonErro = JsonConvert.SerializeObject(feed);
                context.Response.Write(jsonErro);
                return;
            }

            Classes.Objetos.ArenaConfig IArenaConfig = new Classes.Objetos.ArenaConfig();
            IArenaConfig = JsonConvert.DeserializeObject<Classes.Objetos.ArenaConfig>(data);

            SqlConnection conex = new SqlConnection(conn);

            IBattleInfo.ConexaoDB = conex;
            IBattleInfo.Select(IArenaConfig.Id);

            IArquivoLog = new ArquivoLog(IBattleInfo.BattleId);
            string dadosArquivo = IArquivoLog.Select();

            if (string.IsNullOrEmpty(dadosArquivo))
            {
                feed.Erro = true;
                feed.ErroDescricao = "Batalha não existe";

                string jsonErro = JsonConvert.SerializeObject(feed);
                context.Response.Write(jsonErro);
                return;
            }

            /*
             * Vamos receber o conteúdo do arquivo de LOG para continuar a partida
             * A principio é recomendado melhorias de segurança com o tempo, assim reduzimos o risco de cheater
             */
            IArenaObjItens = JsonConvert.DeserializeObject<Classes.Objetos.ArenaObjItens>(dadosArquivo);

            IArenaObjItens.arenaConfig.CardIndexEscolhido = IArenaConfig.CardIndexEscolhido;
            IArenaObjItens.arenaConfig.PosXEscolhido = IArenaConfig.PosXEscolhido;
            IArenaObjItens.arenaConfig.PosYEscolhido = IArenaConfig.PosYEscolhido;

            if (IArenaObjItens.p1deckListCard.Count() == 0 && IArenaObjItens.p2deckListCard.Count() == 0)
                IArenaObjItens.arenaConfig.Encerrada = true;


            if (!IArenaObjItens.arenaConfig.Encerrada)
            {
                /*
                 * CHAMADA DA IA
                 */
                feed.Erro = false;
                this.InicioAcao();

                // se houve um erro não precisa continuar o processamento
                if (feed.Erro)
                {
                    string jsonFim = JsonConvert.SerializeObject(feed);
                    context.Response.Write(jsonFim);
                    return;
                }
            }

            if (IArenaObjItens.p1deckListCard.Count() == 0 && IArenaObjItens.p2deckListCard.Count() == 0)
                IArenaObjItens.arenaConfig.Encerrada = true;

            IArquivoLog.Update
                (
                    IArenaObjItens.p1deckListCard, 
                    IArenaObjItens.p2deckListCard, 
                    IArenaObjItens.arenasituacaoY1,
                    IArenaObjItens.arenasituacaoY2,
                    IArenaObjItens.arenasituacaoY3,
                    IArenaObjItens.arenasituacaoY4,
                    IArenaObjItens.arenaConfig
                );


            // as do player 2 na verdade é provisória para teste
            feed.ListaCards = IArenaObjItens.p1deckListCard;
            feed.ListaCardsOponente = IArenaObjItens.p2deckListCard;
            feed.ArenaConfig = IArenaObjItens.arenaConfig;
            feed.ArenaSituacao = IArenaObjItens.arenasituacao2Dobject;
            feed.arenasituacaoY1 = Classes.Objetos.ArenaObjItens.RetornarColuna(0, IArenaObjItens.arenasituacao2Dobject);
            feed.arenasituacaoY2 = Classes.Objetos.ArenaObjItens.RetornarColuna(1, IArenaObjItens.arenasituacao2Dobject);
            feed.arenasituacaoY3 = Classes.Objetos.ArenaObjItens.RetornarColuna(2, IArenaObjItens.arenasituacao2Dobject);
            feed.arenasituacaoY4 = Classes.Objetos.ArenaObjItens.RetornarColuna(3, IArenaObjItens.arenasituacao2Dobject);
            feed.ListaHistoricoPassosP1 = IArenaObjItens.ListaHostoricosP1;
            feed.ListaHistoricoPassosP2 = IArenaObjItens.ListaHostoricosP2;
            feed.Erro = false;


            string json = JsonConvert.SerializeObject(feed);

            context.Response.Write(json);

        }

        private bool AcaoPlayerValida()
        {
            if (
                (IArenaObjItens.arenaConfig.CardIndexEscolhido == -1)
                ||
                (IArenaObjItens.arenaConfig.PosXEscolhido == -1)
                ||
                (IArenaObjItens.arenaConfig.PosYEscolhido == -1)
                )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool IsFirstTurn()
        {
            // verificar a quantia de cartas para cada lado.
            if (
                IArenaObjItens.p1deckListCard.Count() == 5
                &&
                IArenaObjItens.p2deckListCard.Count() == 5
                )
            {
                if (!AcaoPlayerValida())
                {
                    return true;
                } 
            }
            return false;
        }

        public void InicioAcao()
        {
            Decisao IDecisao = new Decisao(IArenaObjItens);

            if (IArenaObjItens.arenaConfig.Turno == 1)
            {
                // SE O TURNO FOR DO PLAYER 1
                IArenaObjItens = IDecisao.Player(1);

                if (IDecisao.Erro)
                {
                    feed.Erro = true;
                    feed.ErroDescricao = IDecisao.ErroDescricao;
                }
                else
                {
                    if (!this.IsFirstTurn())
                    {

                        IArenaObjItens.arenaConfig.Turno = 2;

                        // SE O TURNO FOR DO ARCADE, VAMOS LOGO FAZER TUDO FUNCIONAR
                        IArenaObjItens = IDecisao.Arcade();

                        if (IDecisao.Erro)
                        {
                            feed.Erro = true;
                            feed.ErroDescricao = IDecisao.ErroDescricao;
                        }
                        else
                        {
                            IArenaObjItens.arenaConfig.Turno = 1;
                        }
                    }
                }
            }
            else if (IArenaObjItens.arenaConfig.Turno == 2 && string.IsNullOrEmpty(IBattleInfo.Player2Id))
            {
                // SE O TURNO FOR DO ARCADE, POIS O PLAYER 2 NÃO EXISTE
                IArenaObjItens = IDecisao.Arcade();

                if (IDecisao.Erro)
                {
                    feed.Erro = true;
                    feed.ErroDescricao = IDecisao.ErroDescricao;
                }
                else
                {
                    IArenaObjItens.arenaConfig.Turno = 1;
                }
            }
            else if (IArenaObjItens.arenaConfig.Turno == 2)
            {
                // SE O TURNO FOR DO PLAYER 2
                IArenaObjItens = IDecisao.Player(2);

                if (IDecisao.Erro)
                {
                    feed.Erro = true;
                    feed.ErroDescricao = IDecisao.ErroDescricao;
                }
                else
                {
                    if (!this.IsFirstTurn())
                    {
                        IArenaObjItens.arenaConfig.Turno = 1;
                    }
                }
            }
            else
            {
                // ERRO
                feed.Erro = true;
                feed.ErroDescricao = "Erro em definir o turno corretamente";
            }


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