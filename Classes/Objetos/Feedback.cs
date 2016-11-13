using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes.Objetos
{
    public class Feedback
    {
        private bool _erro = true;
        private string _usuarioMensagem;
        private string _erroDescricao;
        private bool _nomeExistente;
        private string _internautaId;
        private string _listaString;
        private bool _turnoResposta = false; // uso futuro

        public bool Erro
        {
            get { return _erro; }
            set { _erro = value; }
        }

        public string UsuarioMensagem
        {
            get { return _usuarioMensagem; }
            set { _usuarioMensagem = value; }
        }

        public string ErroDescricao
        {
            get { return _erroDescricao; }
            set { _erroDescricao = value; }
        }

        public bool NomeExistente
        {
            get { return _nomeExistente; }
            set { _nomeExistente = value; }
        }

        public string InternautaId
        {
            get { return _internautaId; }
            set { _internautaId = value; }
        }


        public string ListaString
        {
            get { return _listaString; }
            set { _listaString = string.Concat("[",value, "]").Replace(@"\",""); }
        }

        public List<Classes.Objetos.Elementos> ListaElementos
        {
            get;
            set;
        }

        public Login Login
        {
            get;
            set;
        }


        public PlayerStatus PlayerStatus
        {
            get;
            set;
        }

        public Opcoes Opcoes
        {
            get;
            set;
        }

        public Musica Musica
        {
            get;
            set;
        }

        public ArenaConfig ArenaConfig
        {
            get;
            set;
        }

        public object[ , ] ArenaSituacao
        {
            get;
            set;
        }

        public object[] arenasituacaoY1
        {
            get;
            set;
        }

        public object[] arenasituacaoY2
        {
            get;
            set;
        }

        public object[] arenasituacaoY3
        {
            get;
            set;
        }

        public object[] arenasituacaoY4
        {
            get;
            set;
        }

        public List<Classes.Objetos.Musica> ListaMusicas
        {
            get;
            set;
        }

        public List<Classes.Objetos.Card> ListaCards
        {
            get;
            set;
        }


        public List<Classes.Objetos.Card> ListaCardsOponente
        {
            get;
            set;
        }

        public List<Classes.Objetos.Assistente> ListaAssistentes
        {
            get;
            set;
        }

        public List<Classes.Objetos.Fase> ListaFases
        {
            get;
            set;
        }

        public List<Classes.Objetos.HistoricoPassos> ListaHistoricoPassosP1
        {
            get;
            set;
        }

        public List<Classes.Objetos.HistoricoPassos> ListaHistoricoPassosP2
        {
            get;
            set;
        }

        public bool TurnoResposta
        {
            get { return _turnoResposta; }
            set { _turnoResposta = value; }
        }

    }
}
