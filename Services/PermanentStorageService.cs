using System;
using System.Threading.Tasks;
using Piller.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using SQLite;
using MvvmCross.Platform;
using MvvmCross.Plugins.File;

namespace Piller.Services
{
    public class PermanentStorageService : IPermanentStorageService
    {
        private readonly IMvxFileStore fileStore = Mvx.Resolve<IMvxFileStore> ();

        private readonly string databaseFileName = "piller.db";
        private SQLiteAsyncConnection connection;

        public PermanentStorageService ()
        {
            this.connection = new SQLiteAsyncConnection (this.fileStore.NativePath ("Piller") + databaseFileName);

            this.connection.GetConnection ().CreateTable<MedicationDosage> ();
        }

        public async Task<T> GetAsync<T> (int id) where T : new()
        {
            return await this.connection.FindAsync<T> (id);
        }

        public async Task SaveAsync<T> (T entity)
        {
            await this.connection.InsertOrReplaceAsync(entity);
            //tu nie trzeba robić oddzielnej metody Update - connection ma opcję InsertOrReplace, która wstawi rekord jak go nie ma lub zrobi update jak jest (po kluczu glownym)
        }

        public async Task<List<T>> List<T> (Expression<Func<T, bool>> predicate = null) where T : new()
        {
            if (predicate == null) {
                var query = this.connection.Table<T> ();
                return await query.ToListAsync ();
            }
            else {
                var query = this.connection.Table<T> ().Where (predicate);
                return await query.ToListAsync ();
            }
        }


        public async Task DeleteAsync<T> (T entity)
        {
            await this.connection.DeleteAsync(entity);
        }
    }
}
