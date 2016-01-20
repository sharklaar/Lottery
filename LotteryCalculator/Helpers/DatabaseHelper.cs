using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LotteryCalculator.Models;

namespace LotteryCalculator.Helpers
{
    public class DatabaseHelper
    {
        private string _connectionString;

        public void AddResult(Result result)
        {

            _connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            try
            {
                var cmd = new SqlCommand("AddDrawResult", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DrawDate", result.Date);
                cmd.Parameters.AddWithValue("@DrawType", result.DrawType.ToString());
                cmd.Parameters.AddWithValue("@Ball1", result.Numbers[0]);
                cmd.Parameters.AddWithValue("@Ball2", result.Numbers[1]);
                cmd.Parameters.AddWithValue("@Ball3", result.Numbers[2]);
                cmd.Parameters.AddWithValue("@Ball4", result.Numbers[3]);
                cmd.Parameters.AddWithValue("@Ball5", result.Numbers[4]);
                cmd.Parameters.AddWithValue("@Ball6", result.Numbers[5]);
                cmd.Parameters.AddWithValue("@Booster", result.Numbers[6]);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }

        public List<Result> GetAllResults()
        {
            var results = new List<Result>();

            _connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            try
            {
                var cmd = new SqlCommand("GetAllResults", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var result = new Result();
                    result.Numbers = new List<int>
                    {
                        (int)reader[3],
                        (int)reader[4],
                        (int)reader[5],
                        (int)reader[6],
                        (int)reader[7],
                        (int)reader[8],
                        (int)reader[9],
                    };
                    result.Date = (DateTime) reader[1];
                    result.DrawType = reader[2].ToString().ToUpper() == "LUNCHTIME" ? DrawEnum.Lunchtime : DrawEnum.Teatime;
                    results.Add(result);
                }
            }
            finally
            {
                connection.Close();
            }

            return results;
        }
    }
}