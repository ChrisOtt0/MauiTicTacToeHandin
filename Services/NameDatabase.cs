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
    public class NameDatabase
    {
        SQLiteAsyncConnection Database;

        public NameDatabase() { }

        async Task Init()
        {
            if (Database != null) return;

            SQLitePCL.Batteries.Init();
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await Database.CreateTableAsync<NameSave>();
        }

        public async Task<NameSave> GetNameSave(int id)
        {
            await Init();
            return await Database.Table<NameSave>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveNameSave(NameSave ns)
        {
            await Init();

            var result = await Database.Table<NameSave>().Where(i => i.ID == ns.ID).FirstOrDefaultAsync();
            if (result == null)
                return await Database.InsertAsync(ns);
            else
                return await Database.UpdateAsync(ns);
        }
    }
}
