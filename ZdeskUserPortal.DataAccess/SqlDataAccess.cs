using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ZdeskUserPortal.DataAccess
{
    public abstract class SqlDataAccess : IDataAccess, IDisposable
    {
        private const string TruncateTableCommand = "TRUNCATE TABLE {0}";

        public IDbConnectionFactory DbConnectionFactory { get; private set; }

        protected SqlDataAccess()
        {
            DbConnectionFactory = new SqlDbConnectionFactory();
        }

        public SqlDataAccess(IDbConnectionFactory dbConnectionFactory)
        {
            DbConnectionFactory = dbConnectionFactory;
        }

        protected IDbConnection CreateDbConnection(string connectionStringName)
        {
            var connection = DbConnectionFactory?.CreateDbConnection(connectionStringName);
            if (connection is null)
            {
                throw new DataException($"Failed to create a connection for connection named {connectionStringName}.");
            }
            return connection;
        }

        #region Non Transactional Methods
        protected T GetValueByProcedure<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            using (var connection = CreateDbConnection(connectionStringName))
            {
                return connection.QueryFirstOrDefault<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        protected T GetValueByProcedure<T, U>(IDbConnection connection, string storedProcedure, U parameters)
        {
            if (connection is null)
                return default;

            return connection.QueryFirstOrDefault<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        protected T GetValueByQuery<T, U>(string sql, U parameters, string connectionStringName)
        {
            using (var connection = CreateDbConnection(connectionStringName))
            {
                return connection.QueryFirstOrDefault<T>(sql, parameters, commandType: CommandType.Text);
            }
        }

        protected T GetValueByQuery<T, U>(IDbConnection connection, string sql, U parameters)
        {
            if (connection is null)
                return default;

            return connection.QueryFirstOrDefault<T>(sql, parameters, commandType: CommandType.Text);
        }

        protected List<T> LoadDataByProcedure<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            using (var connection = CreateDbConnection(connectionStringName))
            {
                return connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        protected List<T> LoadDataByProcedure<T, U>(IDbConnection connection, string storedProcedure, U parameters)
        {
            if (connection is null)
                return default;

            return connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();
        }

        protected List<T> LoadDataByQuery<T, U>(string sql, U parameters, string connectionStringName)
        {
            using (var connection = CreateDbConnection(connectionStringName))
            {
                return connection.Query<T>(sql, parameters, commandType: CommandType.Text).ToList();
            }
        }

        protected List<T> LoadDataByQuery<T, U>(IDbConnection connection, string sql, U parameters)
        {
            if (connection is null)
                return default;

            return connection.Query<T>(sql, parameters, commandType: CommandType.Text).ToList();
        }

        protected void LoadDataSetByProcedure<T>(string storedProcedure, DataSet data, string[] tables, T parameters, string connectionStringName, int? commandTimeoutInSeconds = null)
        {
            using (var connection = CreateDbConnection(connectionStringName))
            {
                var reader = connection.ExecuteReader(storedProcedure, parameters, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeoutInSeconds);
                data.Load(reader, LoadOption.OverwriteChanges, tables);
            }
        }

        protected void LoadDataSetByProcedure<T>(IDbConnection connection, string storedProcedure, DataSet data, string[] tables, T parameters)
        {
            if (connection is null)
                return;

            var reader = connection.ExecuteReader(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            data.Load(reader, LoadOption.OverwriteChanges, tables);
        }

        protected void LoadDataSetByQuery<T>(string sql, DataSet data, string[] tables, T parameters, string connectionStringName)
        {
            using (var connection = CreateDbConnection(connectionStringName))
            {
                var reader = connection.ExecuteReader(sql, parameters, commandType: CommandType.Text);
                data.Load(reader, LoadOption.OverwriteChanges, tables);
            }
        }

        protected void LoadDataSetByQuery<T>(IDbConnection connection, string sql, DataSet data, string[] tables, T parameters)
        {
            if (connection is null)
                return;

            var reader = connection.ExecuteReader(sql, parameters, commandType: CommandType.Text);
            data.Load(reader, LoadOption.OverwriteChanges, tables);
        }

        protected int SaveDataByProcedure<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            using (var connection = CreateDbConnection(connectionStringName))
            {
                return connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        protected void SaveDataByBulkCopy(string stagingTableName, string mergeProcedureName, DataTable dataTable, string connectionStringName)
        {
            using (var connection = CreateDbConnection(connectionStringName) as SqlConnection)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute(string.Format(TruncateTableCommand, stagingTableName), commandType: CommandType.Text, transaction: transaction);

                    using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.DestinationTableName = stagingTableName;
                        bulkCopy.WriteToServer(dataTable);
                    }

                    connection.Execute(mergeProcedureName, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: 3600);

                    transaction.Commit();
                }
            }
        }

        protected void SaveDataByProcedure<T>(IDbConnection connection, string storedProcedure, T parameters)
        {
            connection?.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        protected void SaveDataByQuery<T>(string sql, T parameters, string connectionStringName)
        {
            using (var connection = CreateDbConnection(connectionStringName))
            {
                connection.Execute(sql, parameters, commandType: CommandType.Text);
            }
        }

        protected void SaveDataByQuery<T>(IDbConnection connection, string sql, T parameters)
        {
            connection?.Execute(sql, parameters, commandType: CommandType.Text);
        }
        #endregion

        #region Transactional Methods
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        protected void StartTransaction(string connectionStringName)
        {
            if (_connection is not null || _transaction is not null)
                RollbackTransaction();

            _connection = CreateDbConnection(connectionStringName);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        protected T GetValueInTransaction<T, U>(string storedProcedure, U parameters)
        {
            if (_connection is null)
                return default;

            var result = _connection.QueryFirstOrDefault<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction);
            return result;
        }

        protected List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
            if (_connection is null)
                return default;

            var result = _connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();
            return result;
        }

        protected void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            _connection?.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _transaction = null;

            _connection?.Close();
            _connection = null;
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _transaction = null;

            _connection?.Close();
            _connection = null;
        }
        #endregion

        public void Dispose()
        {
            CommitTransaction();
        }
    }
}
