using MauiTicTacToeHandin.Models;
using MauiTicTacToeHandin.Tools;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTicTacToeHandin.Services
{
    public class HighscoreDatabase
    {
        SQLiteAsyncConnection Database;
        public HighscoreDatabase() { }

        async Task Init()
        {
            if (Database != null) return;

            SQLitePCL.Batteries.Init();
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await Database.CreateTableAsync<Highscore>();
        }

        public async Task<List<Highscore>> GetHighscoresAsync()
        {
            await Init();
            return await Database.Table<Highscore>().ToListAsync();
        }

        public async Task<int> SaveHighscoreAsync(Highscore highscore)
        {
            await Init();
            if (highscore.ID != 0)
                return await Database.UpdateAsync(highscore);
            else
                return await Database.InsertAsync(highscore);
        }


    }
}
