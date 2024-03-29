﻿using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GraphsGenerator.DataAccess
{
    internal class SqlServerDALService : IDataAccessService
    {
        private readonly IConfiguration _configuration;

        public SqlServerDALService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(bool success, GraphWithVectorModel graphWithVectorModel)> TryGetGraphWithVectorModelByG6(string g6)
        {
            try
            {
                await Task.CompletedTask;
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                using var connection = new SqlConnection(connectionString);
                connection.Open();

                SqlCommand cmd = new SqlCommand("usp_SELECT_GraphWithDegreeVectorByG6", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParam = cmd.Parameters.AddWithValue("@G6", g6);

                using var reader = cmd.ExecuteReader();

                if (reader.HasRows && reader.Read())
                {
                    var graphWithVectorModel = new GraphWithVectorModel()
                    {
                        G6 = reader.GetString(0),
                        VertexCount = reader.GetInt16(1),
                        ChromaticNumber = reader.GetInt16(2),
                        DegreeVectorValue = reader.GetInt64(3)
                    };

                    return (true, graphWithVectorModel);
                }

                return (false, null);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task UpsertGraphWithVector(IEnumerable<GraphWithVectorModel> graphWithVectorModels)
        {
            try
            {
                await Task.CompletedTask;
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                DataTable dt = new DataTable();
                dt.Columns.Add("G6", typeof(string));
                dt.Columns.Add("VertexCount", typeof(string));
                dt.Columns.Add("ChromaticNumber", typeof(short));
                dt.Columns.Add("DegreeVectorValue", typeof(long));

                foreach (var item in graphWithVectorModels)
                {
                    dt.Rows.Add(item.G6, item.VertexCount, item.ChromaticNumber, item.DegreeVectorValue);
                }

                using var connection = new SqlConnection(connectionString);
                connection.Open();

                SqlCommand cmd = new SqlCommand("usp_UPSERT_GraphsWithDegreeVector", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParam = cmd.Parameters.AddWithValue("@Graphs", dt);
                sqlParam.SqlDbType = SqlDbType.Structured;

                cmd.ExecuteNonQuery();
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
