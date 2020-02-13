using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Bride_Frankenstein.Services
{
    public class BaseRepository : IDisposable
    {
        internal IDbConnection Connection => new SqlConnection(@"data source=FIGUEREDO;initial catalog=NOVOTESTE;password=cloudmed;user id=sa");

        public int Execute(string query)
        {
            int ret;
            using (var cn = Connection)
            {
                cn.Open();
                ret = cn.Execute(query);
                cn.Close();
            }

            return ret;
        }

        public IEnumerable<T> Find<T>(string query)
        {
            IEnumerable<T> items;
            using (var cn = Connection)
            {
                cn.Open();
                items = cn.Query<T>(query);
                cn.Close();
            }

            return items;
        }

        public void Dispose()
        {
            if (Connection.State == ConnectionState.Open)
                Connection.Close();
        }
    }
}
