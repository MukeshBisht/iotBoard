using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using IOT.Models;
using MySql.Data.MySqlClient;

namespace IOT.DB {
    public class WeatherRepository {

        public readonly AppDB Db;
        public WeatherRepository(AppDB db)
        {
            Db = db;
        }

        public async Task InsertAsync(Condition condition, bool forecast=false)
        {
            string _table = forecast ? "forecast" : "weather";
            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO " + _table + @" (`main`, `description`, `icon`, `date`, `place`, `clouds`, `temp`, `minTemp`, `maxTemp`, `windSpeed`)
                                 VALUES (@main, @description, @icon, @date, @place, @clouds, @temp, @minTemp, @maxTemp, @windSpeed);";
            BindParams(cmd, condition);
            if(Db.Connection.State==ConnectionState.Closed)
                Db.Connection.Open();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task InsertAsync(List<Condition> condition, bool forecast=false)
        {
            foreach (var item in condition)
            {
               await InsertAsync(item, forecast);
            }
        }
        private void BindParams(MySqlCommand cmd, Condition condition)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@main",
                DbType = DbType.String,
                Value = condition.weather,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@description",
                DbType = DbType.String,
                Value = condition.description,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@icon",
                DbType = DbType.String,
                Value = condition.icon,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@date",
                DbType = DbType.DateTime,
                Value = condition.date,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@place",
                DbType = DbType.String,
                Value = condition.place,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@clouds",
                DbType = DbType.Double,
                Value = condition.clouds,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@temp",
                DbType = DbType.Double,
                Value = condition.temp,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@minTemp",
                DbType = DbType.Double,
                Value = condition.minTemp,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@maxTemp",
                DbType = DbType.Double,
                Value = condition.maxTemp,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@windSpeed",
                DbType = DbType.Double,
                Value = condition.windSpeed,
            });
        }

        public async Task<List<Condition>> LatestConditionAsync(bool forecast=false)
        {
            string _table = forecast ? "forecast" : "weather";
            string lim = forecast ? "40" : "1";
            var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT   id,  main,  description,  icon,  date,  place,  clouds,  temp,  minTemp,  maxTemp,  windSpeed,  timeStamp FROM " + _table + "  ORDER BY Id DESC LIMIT "+ lim +";";
            if(Db.Connection.State==ConnectionState.Closed)
                Db.Connection.Open();
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<Condition>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Condition>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var con = new Condition
                    {
                        id = await reader.GetFieldValueAsync<int>(0),
                        weather = await reader.GetFieldValueAsync<string>(1),
                        description = await reader.GetFieldValueAsync<string>(2),
                        icon = await reader.GetFieldValueAsync<string>(3),
                        date = await reader.GetFieldValueAsync<DateTime>(4),
                        place = await reader.GetFieldValueAsync<string>(5),
                        clouds = await reader.GetFieldValueAsync<double>(6),
                        temp = await reader.GetFieldValueAsync<double>(7),
                        minTemp = await reader.GetFieldValueAsync<double>(8), 
                        maxTemp = await reader.GetFieldValueAsync<double>(9),
                        windSpeed = await reader.GetFieldValueAsync<double>(10),
                        timeStamp = await reader.GetFieldValueAsync<DateTime>(11),
                    };
                    posts.Add(con);
                }
            }
            return posts;
        }
    }

}