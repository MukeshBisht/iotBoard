using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using IOT.Models;
using MySql.Data.MySqlClient;

namespace IOT.DB {

    public class SensorDataRepository {

        public readonly AppDB Db;
        public SensorDataRepository(AppDB db)
        {
            Db = db;
        }

        public async Task InsertAsync(SensorData data)
        {
            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO sensordata (Temperature, Pressure, Humidity, SoilMoisture, Light, Altitude)
                                 VALUES (@Temperature, @Pressure, @Humidity, @SoilMoisture, @Light, @Altitude);";
            BindParams(cmd, data);
            if(Db.Connection.State==ConnectionState.Closed)
                Db.Connection.Open();
            await cmd.ExecuteNonQueryAsync();
            //var Id = (int) cmd.LastInsertedId;
        }

        public async Task<List<SensorData>> TelemetryAsync(DateTime date)
        {
            var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT Temperature, Pressure, Humidity, SoilMoisture, Light, TimeStamp FROM sensordata where DATE(TimeStamp)='"+date.ToString("yyyy/MM/dd")+"' limit 500;";
            //Console.WriteLine(cmd.CommandText);
            if(Db.Connection.State==ConnectionState.Closed)
                Db.Connection.Open();
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<SensorData>> ReadAllAsync(DbDataReader dbDataReader)
        {
            var data = new List<SensorData>();
            using (dbDataReader)
            {
                while (await dbDataReader.ReadAsync())
                {
                    var d = new SensorData
                    {
                        Temperature = await dbDataReader.GetFieldValueAsync<double>(0),
                        Pressure = await dbDataReader.GetFieldValueAsync<double>(1),
                        Humidity = await dbDataReader.GetFieldValueAsync<double>(2),
                        SoilMoisture = await dbDataReader.GetFieldValueAsync<double>(3),
                        Light = await dbDataReader.GetFieldValueAsync<double>(4),
                        TimeStamp = await dbDataReader.GetFieldValueAsync<DateTime>(5),
                    };
                    data.Add(d);
                }
            }
            return data;
        }

        private void BindParams(MySqlCommand cmd, SensorData data)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Temperature",
                DbType = DbType.Double,
                Value = data.Temperature,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Pressure",
                DbType = DbType.Double,
                Value = data.Pressure,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@SoilMoisture",
                DbType = DbType.Double,
                Value = data.SoilMoisture,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Humidity",
                DbType = DbType.Double,
                Value = data.Humidity,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Light",
                DbType = DbType.Double,
                Value = data.Light,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Altitude",
                DbType = DbType.Double,
                Value = data.Altitude,
            });
        }
    }
}