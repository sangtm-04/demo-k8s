using Demo.WebApp.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebApp.DL
{
    public class DatabaseContext : IDisposable
    {
        /// <summary>
        /// Chuỗi kết nối
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// MySqlConnection
        /// </summary>
        private MySqlConnection _sqlConnection { get; set; }

        public MySqlConnection SqlConnection { get => _sqlConnection; }

        /// <summary>
        /// MySqlCommand
        /// </summary>
        private MySqlCommand _sqlCommand { get; set; }

        public MySqlCommand SqlCommand { get => _sqlCommand; }


        MySqlTransaction _sqlTransaction;
        /// <summary>
        /// Khởi tạo kết nối
        /// </summary>
        /// <param name="connectionString"></param>
        /// Created Bt : NVMANH (20/10/2019)
        public DatabaseContext()
        {
            _sqlConnection = new MySqlConnection(ConnectionString);
            _sqlConnection.Open();
            _sqlCommand = _sqlConnection.CreateCommand();
            _sqlCommand.CommandTimeout = 7200;
            _sqlCommand.CommandType = CommandType.StoredProcedure;
        }

        public DatabaseContext(CommandType commandType)
        {
            _sqlConnection = new MySqlConnection(ConnectionString);
            _sqlConnection.Open();
            _sqlCommand = _sqlConnection.CreateCommand();
            _sqlCommand.CommandTimeout = 7200;
            _sqlCommand.CommandType = commandType;
        }

        /// <summary>
        /// Đối tượng kết nối với Database
        /// </summary>
        /// <returns></returns>
        /// Created By : NVMANH (20/10/2019)
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        /// <summary>
        /// Thực hiện map giá trị giữa tham số và đối số
        /// </summary>
        /// <returns></returns>
        /// Created By : NVMANH (20/10/2019)
        public void MapValueToStorePram(string commandText, object[] parameters)
        {
            var paramsFromStore = GetParamFromStore(commandText);
            if (paramsFromStore.Count >= 1 && parameters != null)
            {
                for (int i = 0; i < paramsFromStore.Count; i++)
                {
                    if (i <= parameters.Length)
                    {
                        var value = parameters[i].ToString();
                        var result = Utility.ConvertType(value, paramsFromStore[i].MySqlDbType);
                        paramsFromStore[i].Value = result;
                    }
                }
            }
        }

        /// <summary>
        /// Lấy toàn bộ tham số đầu vào được khai báo trong store
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        /// Created By : NVMANH (20/10/2019)
        public MySqlParameterCollection GetParamFromStore(string commandText)
        {
            _sqlCommand.CommandText = commandText;
            MySqlCommandBuilder.DeriveParameters(_sqlCommand);
            return _sqlCommand.Parameters;
        }

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        /// Created By : NVMANH (20/10/2019)
        public MySqlDataReader ExecuteReader(string commandText)
        {
            _sqlCommand.CommandText = commandText;
            return _sqlCommand.ExecuteReader();
        }

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        /// Created By : NVMANH (20/10/2019)
        public async Task<MySqlDataReader> ExecuteReaderAsync(string commandText)
        {
            _sqlCommand.CommandText = commandText;
            return (MySqlDataReader)await _sqlCommand.ExecuteReaderAsync();
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        /// Created By : NVMANH (20/10/2019)
        public int ExecuteNonQuery()
        {
            _sqlTransaction = _sqlConnection.BeginTransaction();
            _sqlCommand.Transaction = _sqlTransaction;
            var result = _sqlCommand.ExecuteNonQuery();
            _sqlTransaction.Commit();
            return result;
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        /// Created By : NVMANH (20/10/2019)
        public async Task<int> ExecuteNonQueryAsync()
        {
            _sqlTransaction = _sqlConnection.BeginTransaction();
            _sqlCommand.Transaction = _sqlTransaction;
            var result = await _sqlCommand.ExecuteNonQueryAsync();
            _sqlTransaction.Commit();
            return result;
        }


        public object ExecuteScalar()
        {
            object result = null;
            _sqlTransaction = _sqlConnection.BeginTransaction();
            _sqlCommand.Transaction = _sqlTransaction;
            result = _sqlCommand.ExecuteScalar();
            _sqlTransaction.Commit();
            return result;
        }

        public async Task<object> ExecuteScalarAsync()
        {
            object result = null;
            _sqlTransaction = _sqlConnection.BeginTransaction();
            _sqlCommand.Transaction = _sqlTransaction;
            result = await _sqlCommand.ExecuteScalarAsync();
            _sqlTransaction.Commit();
            return result;
        }

        public void Dispose()
        {
            _sqlConnection.Close();
        }
    }
}
