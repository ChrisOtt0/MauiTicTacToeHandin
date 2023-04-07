using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTicTacToeHandin.Models
{
    public class Highscore
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }

        [MaxLength(100)]
        public string PlayerName { get; set; }

        [MaxLength(100)]
        public string EnemyName { get; set; }

        public DateTime Ended { get; set; }

        public HighscoreStatus Status { get; set; }
    }
}
