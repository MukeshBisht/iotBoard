using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using IOT.BL;
using IOT.Models;
using MySql.Data.MySqlClient;

namespace IOT.DB
{

    public class ActionRepository
    {
        public readonly AppDB Db;
        public ActionRepository(AppDB db)
        {
            Db = db;
        }

         public async Task InsertAsync(Models.Action act)
        {
            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO action (text, action)
                                 VALUES (@text, @action);";
            BindParams(cmd, act);
            if(Db.Connection.State==ConnectionState.Closed)
                Db.Connection.Open();
            if(ActionQueue.lastTask != act.Act  && act.Act == 3)
                await cmd.ExecuteNonQueryAsync();
            //var Id = (int) cmd.LastInsertedId; 
            ActionQueue.lastTask = act.Act;
        }

        private void BindParams(MySqlCommand cmd, Models.Action data)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@text",
                DbType = DbType.String,
                Value = data.Text,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@action",
                DbType = DbType.Int32,
                Value = data.Act,
            });
        }

        public async Task<List<Models.Action>> ReadActionsAsync()
        {
            var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT Text, Action, TimeStamp FROM action order by id desc limit 15;";
            if(Db.Connection.State==ConnectionState.Closed)
                Db.Connection.Open();
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<Models.Action>> ReadAllAsync(DbDataReader dbDataReader)
        {
            var data = new List<Models.Action>();
            using (dbDataReader)
            {
                while (await dbDataReader.ReadAsync())
                {
                    var d = new Models.Action
                    {
                        Text = await dbDataReader.GetFieldValueAsync<String>(0),
                        Act = await dbDataReader.GetFieldValueAsync<int>(1),
                        timeStamp = await dbDataReader.GetFieldValueAsync<DateTime>(2)
                    };
                    data.Add(d);
                }
            }
            return data;
        }
    }
}