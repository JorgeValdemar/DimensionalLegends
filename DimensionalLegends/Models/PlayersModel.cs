using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Classes.Objetos;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace DimensionalLegends.Models
{
    public class PlayersModel
    {
        private string conn = ConfigurationManager.ConnectionStrings["sql"].ToString();
        private volatile PlayerStatus _player;
        private static object syncRoot = new Object();

        public PlayerStatus Player
        {
            get
            {
                if (_player == null)
                {
                    lock (syncRoot)
                    {
                        if (_player == null)
                        {
                            _player = new PlayerStatus();
                        }
                    }
                }

                return _player;
            }
        }

        public PlayerStatus getPlayerByNick(string nick)
        {

            SqlConnection conex = new SqlConnection(conn);
            SqlDataReader rs = null;

            conex.Open();


            SqlCommand cmd = new SqlCommand("get_player_status_by_nick", conex);
            cmd.Parameters.Add("@Nick", SqlDbType.VarChar, 20).Value = nick;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            rs = cmd.ExecuteReader();

            while (rs.Read())
            {
                this.Player.InternautaId = rs["InternautaId"].ToString();
                this.Player.Nick = rs["Nick"].ToString();
                this.Player.Imagem = rs["Imagem"].ToString();
                this.Player.Level = int.Parse(rs["Level"].ToString());
                this.Player.MaxHp = int.Parse(rs["MaxHp"].ToString());
                this.Player.MaxMp = int.Parse(rs["MaxMp"].ToString());
                this.Player.MaxSp = int.Parse(rs["MaxSp"].ToString());
                this.Player.Coins = int.Parse(rs["Coins"].ToString());
            }

            rs.Close();
            conex.Close();

            return this.Player;
        }

    }
}
