using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Classes.Objetos
{
    public class BattleInfo
    {
        private string _battleId;
        private string _player1Id;
        private string _player2Id;
        private int _arcadeLiderId;
        private DateTime _dataInicio;
        private DateTime _dataFim;
        private bool _ativa;

        public BattleInfo(string battleId = null, string player1Id = null, string player2Id = null, int arcadeLiderId = 0, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            _battleId = battleId;
            _player1Id = player1Id;
            _player2Id = player2Id;
            _arcadeLiderId = arcadeLiderId;
            _dataInicio = dataInicio == null ? DateTime.Now : (DateTime)dataInicio;
            _dataFim = dataFim == null ? DateTime.MaxValue : (DateTime)dataFim;
        }

        public SqlConnection ConexaoDB { get; set; }

        public string BattleId { 
            get
            {
                return _battleId;
            } 
            set 
            {
                _battleId = value;
            }
        }

        public string Player1Id
        {
            get
            {
                return _player1Id;
            }
            set
            {
                _player1Id = value;
            }
        }

        public string Player2Id
        {
            get
            {
                return _player2Id;
            }
            set
            {
                _player2Id = value;
            }
        }

        public int ArcadeLiderId
        {
            get
            {
                return _arcadeLiderId;
            }
            set
            {
                _arcadeLiderId = value;
            }
        }

        public DateTime DataInicio
        {
            get
            {
                return _dataInicio;
            }
            set
            {
                _dataInicio = value;
            }
        }

        public DateTime DataFim
        {
            get
            {
                return _dataFim;
            }
            set
            {
                _dataFim = value;
            }
        }

        public bool Ativa
        {
            get
            {
                return _ativa;
            }
            set
            {
                _ativa = value;
            }
        }

        public bool Insert()
        {
            if (!this.MudancaPermitida())
                return false;

            SqlCommand cmd = new SqlCommand("ex_battle_info", this.ConexaoDB);
            cmd.Parameters.Add("@Player1Id", SqlDbType.VarChar).Value = this.Player1Id;
            cmd.Parameters.Add("@Player2Id", SqlDbType.VarChar).Value = this.Player2Id;
            cmd.Parameters.Add("@ArcadeLiderId", SqlDbType.Int).Value = this.ArcadeLiderId;
            cmd.Parameters.Add("@DataInicio", SqlDbType.DateTime).Value = this.DataInicio;
            cmd.Parameters.Add("@DataFim", SqlDbType.DateTime).Value = this.DataFim;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            _battleId = (string) cmd.ExecuteScalar(); // retorna o ID e adiciona a classe

            return true;
        }

        public bool Select(string battleId)
        {
            // sem conexão não tem conversa
            if (this.ConexaoDB == null)
                return false;

            this.ConexaoDB.Open();
            
            SqlCommand cmd = new SqlCommand("get_battle_info", this.ConexaoDB);
            cmd.Parameters.Add("@BattleId", SqlDbType.VarChar).Value = battleId;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataReader rs = cmd.ExecuteReader();

            if (rs.Read())
            {
                _battleId = rs["BattleId"].ToString();
                _player1Id = rs["Player1Id"].ToString();
                _player2Id = rs["Player2Id"].ToString();
                _arcadeLiderId = int.Parse(rs["ArcadeLiderId"].ToString());
                _dataInicio = DateTime.Parse(rs["DataInicio"].ToString());
                _dataFim = DateTime.Parse(rs["DataFim"].ToString());

                this.ConexaoDB.Close();
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool MudancaPermitida()
        {
            // sem conexão não tem conversa
            if (this.ConexaoDB == null)
                return false;

            // sempre precisa ter um player 1
            if (string.IsNullOrEmpty(_player1Id))
                return false;

            // sempre precisa ter o player 2 ou o player arcade
            if (string.IsNullOrEmpty(_player2Id) && _arcadeLiderId == 0)
                return false;
            
            return true;
        }

    }
}
