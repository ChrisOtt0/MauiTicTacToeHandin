using ServerTicTacToeHandin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTicTacToeHandin.Services
{
    public class GameService
    {
        public Session Session { get; set; } = null;
        public int? PlayerId { get; set; } = null;
        public string PlayerName { get; set; } = string.Empty;
    }
}
