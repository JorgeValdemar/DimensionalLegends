using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes.Objetos;

namespace WebSocketTurnoAcao.Entity
{
    public class Chat
    {
        public string WhisperToName { get; set; }

        public string WhisperToId { get; set; }

        private string mensagem;

        public string Mensagem
        {
            get { return mensagem; }
            set { mensagem = value; }
        }

        private PlayerStatus playerStatus;

        public PlayerStatus PlayerStatus
        {
            get { return playerStatus; }
            set { playerStatus = value; }
        }
        
    }
}
