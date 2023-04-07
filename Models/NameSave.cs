using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTicTacToeHandin.Models
{
    public class NameSave
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
