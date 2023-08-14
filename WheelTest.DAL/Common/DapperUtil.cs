using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using static Dapper.SqlMapper;

namespace WheelTest.DAL.Common
{
    /// <summary>
    /// dapper扩展工具类
    /// </summary>
    public class DapperUtil
    {
        private bool useDbTransaction = false;
        private string dbConnectionString = null;
        private IDbConnection dbConnection = null;
        private IDbTransaction dbTransaction = null;
        private Dictionary<string, object> paramDic;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connName"></param>
        public DapperUtil(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        /// <summary>
        /// 使用事务
        /// </summary>
        public void UseDbTransaction()
        {
            useDbTransaction = true;
        }

        /// <summary>
        /// 获取一个值，param可以是SQL参数也可以是匿名对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public T GetValue<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return UseDbConnection((dbConn) =>
            {
                return dbConn.ExecuteScalar<T>(sql, param, dbTransaction, commandTimeout, commandType);
            });
        }

        /// <summary>
        /// 获取第一行的所有值，param可以是SQL参数也可以是匿名对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Dictionary<string, dynamic> GetFirstValues(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return UseDbConnection((dbConn) =>
            {
                Dictionary<string, dynamic> firstValues = new Dictionary<string, dynamic>();
                List<string> indexColNameMappings = new List<string>();
                int rowIndex = 0;
                using (var reader = dbConn.ExecuteReader(sql, param, dbTransaction, commandTimeout, commandType))
                {
                    while (reader.Read())
                    {
                        if ((++rowIndex) > 1) break;
                        if (indexColNameMappings.Count == 0)
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                indexColNameMappings.Add(reader.GetName(i));
                            }
                        }

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            firstValues[indexColNameMappings[i]] = reader.GetValue(i);
                        }
                    }
                    reader.Close();
                }
                return firstValues;
            });
        }

        /// <summary>
        /// 获取一个数据模型实体类，param可以是SQL参数也可以是匿名对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public T GetModel<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null) where T : class
        {
            return UseDbConnection((dbConn) =>
            {
                return dbConn.QueryFirstOrDefault<T>(sql, param, dbTransaction, commandTimeout, commandType);
            });
        }

        /// <summary>
        /// 获取符合条件的所有数据模型实体类列表，param可以是SQL参数也可以是匿名对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public List<T> GetModelList<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) where T : class
        {
            return UseDbConnection((dbConn) =>
            {
                return dbConn.Query<T>(sql, param, dbTransaction, buffered, commandTimeout, commandType).ToList();
            });
        }

        /// <summary>
        /// 获取符合条件的所有数据并根据动态构建Model类委托来创建合适的返回结果（适用于临时性结果且无对应的模型实体类的情况）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buildModelFunc"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public T GetDynamicModel<T>(Func<IEnumerable<dynamic>, T> buildModelFunc, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            var dynamicResult = UseDbConnection((dbConn) =>
            {
                return dbConn.Query(sql, param, dbTransaction, buffered, commandTimeout, commandType);
            });
            return buildModelFunc(dynamicResult);
        }

        /// <summary>
        /// 获取符合条件的所有指定返回结果对象的列表(复合对象【如：1对多，1对1】)，param可以是SQL参数也可以是匿名对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="types"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>

        public List<T> GetMultModelList<T>(string sql, Type[] types, Func<object[], T> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return UseDbConnection((dbConn) =>
            {
                return dbConn.Query<T>(sql, types, map, param, dbTransaction, buffered, splitOn, commandTimeout, commandType).ToList();
            });
        }
        public List<TReturn> GetMultModelList<TFirst, TSecond, TReturn>(
            string sql, Func<TFirst, TSecond, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return UseDbConnection((dbConn) =>
            {
                return dbConn.Query(sql, map, param, dbTransaction, buffered, splitOn, commandTimeout, commandType).ToList();
            });
        }
        public List<TReturn> GetMultModelList<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return UseDbConnection((dbConn) =>
            {
                return dbConn.Query(sql, map, param, dbTransaction, buffered, splitOn, commandTimeout, commandType).ToList();
            });
        }
        public List<TReturn> GetMultModelList<TFirst, TSecond, TThird, TFourth, TReturn>(
            string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return UseDbConnection((dbConn) =>
            {
                return dbConn.Query(sql, map, param, dbTransaction, buffered, splitOn, commandTimeout, commandType).ToList();
            });
        }
        public List<TReturn> GetMultModelList<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return UseDbConnection((dbConn) =>
            {
                return dbConn.Query(sql, map, param, dbTransaction, buffered, splitOn, commandTimeout, commandType).ToList();
            });
        }
        public List<TReturn> GetMultModelList<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
            string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return UseDbConnection((dbConn) =>
            {
                return dbConn.Query(sql, map, param, dbTransaction, buffered, splitOn, commandTimeout, commandType).ToList();
            });
        }
        public List<TReturn> GetMultModelList<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
           string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return UseDbConnection((dbConn) =>
            {
                return dbConn.Query(sql, map, param, dbTransaction, buffered, splitOn, commandTimeout, commandType).ToList();
            });
        }
        /// <summary>
        /// 多表查询
        /// </summary>
        /// <typeparam name="TTFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public TReturn GetMultiple<TTFirst, TSecond, TReturn>(string sql, Func<List<TTFirst>, List<TSecond>, TReturn> map, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return UseDbConnection((dbConn) =>
             {
                 var reader = dbConn.QueryMultiple(sql, param, dbTransaction, commandTimeout, commandType);
                 if (!reader.IsConsumed)
                     return map(reader.Read<TTFirst>().ToList(), reader.Read<TSecond>().ToList());
                 return default(TReturn);
             });
        }/// <summary>
         /// 多表查询
         /// </summary>
         /// <typeparam name="TTFirst"></typeparam>
         /// <typeparam name="TSecond"></typeparam>
         /// <typeparam name="TReturn"></typeparam>
         /// <param name="sql"></param>
         /// <param name="map"></param>
         /// <param name="param"></param>
         /// <param name="buffered"></param>
         /// <param name="commandTimeout"></param>
         /// <param name="commandType"></param>
         /// <returns></returns>
        public TReturn GetMultiple<T1, T2, T3, TReturn>(string sql, Func<List<T1>, List<T2>, List<T3>, TReturn> map, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return UseDbConnection((dbConn) =>
            {
                var reader = dbConn.QueryMultiple(sql, param, dbTransaction, commandTimeout, commandType);
                if (!reader.IsConsumed)
                    return map(reader.Read<T1>().ToList(), reader.Read<T2>().ToList(), reader.Read<T3>().ToList());
                return default(TReturn);
            });
        }
        /// <summary>
        /// 多表查询
        /// </summary>
        /// <typeparam name="TTFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public TReturn GetMultiple<T1, T2, T3, T4, TReturn>(string sql, Func<List<T1>, List<T2>, List<T3>, List<T4>, TReturn> map, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return UseDbConnection((dbConn) =>
             {
                 var reader = dbConn.QueryMultiple(sql, param, dbTransaction, commandTimeout, commandType);
                 if (!reader.IsConsumed)
                 {
                     var t1 = reader.Read<T1>().ToList();
                     var t2 = reader.Read<T2>().ToList();
                     var t3 = reader.Read<T3>().ToList();
                     var t4 = reader.Read<T4>().ToList();
                     return map(t1, t2, t3, t4);
                 }
                 return default(TReturn);
             });
        }
        /// <summary>
        /// 多表查询
        /// </summary>
        /// <typeparam name="TTFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public TReturn GetMultiple<T1, T2, T3, T4, T5, TReturn>(string sql, Func<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, TReturn> map, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return UseDbConnection((dbConn) =>
            {
                var reader = dbConn.QueryMultiple(sql, param, dbTransaction, commandTimeout, commandType);
                if (!reader.IsConsumed)
                {
                    var t1 = reader.Read<T1>().ToList();
                    var t2 = reader.Read<T2>().ToList();
                    var t3 = reader.Read<T3>().ToList();
                    var t4 = reader.Read<T4>().ToList();
                    var t5 = reader.Read<T5>().ToList();
                    return map(t1, t2, t3, t4, t5);
                }
                return default(TReturn);
            });
        }
        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return UseDbConnection((dbConn) =>
            {
                int tableCount = sql.Count(c => c == ';') + 1;
                DataSet ds = new DataSet();
                IDataReader reader = dbConn.ExecuteReader(sql, param, dbTransaction, commandTimeout, commandType);
                ds.Load(reader, LoadOption.Upsert, new string[tableCount]);
                return ds;
            });
        }

        /// <summary>
        /// 执行SQL命令（CRUD），param可以是SQL参数也可以是要添加的实体类
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public bool ExecuteCommand(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return UseDbConnection((dbConn) =>
            {
                int result = dbConn.Execute(sql, param, dbTransaction, commandTimeout, commandType);
                return result > 0;
            });
        }

        /// <summary>
        /// 执行SQL命令（CRUD），param可以是SQL参数也可以是要添加的实体类，返回影响行数或自增长值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public int ExecuteCommandInt(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return UseDbConnection((dbConn) =>
            {
                int result = dbConn.ExecuteScalar<int>(sql, param, dbTransaction, commandTimeout, commandType);
                return result;
            });
        }

        /// <summary>
        /// 批量转移数据(利用SqlBulkCopy实现快速大批量插入到指定的目的表及SqlDataAdapter的批量删除)
        /// </summary>
        public bool BatchMoveData(string srcSelectSql, string srcTableName, List<SqlParameter> srcPrimarykeyParams, string destConnName, string destTableName)
        {
            using (SqlDataAdapter srcSqlDataAdapter = new SqlDataAdapter(srcSelectSql, dbConnectionString))
            {
                DataTable srcTable = new DataTable();
                SqlCommand deleteCommand = null;
                try
                {
                    srcSqlDataAdapter.AcceptChangesDuringFill = true;
                    srcSqlDataAdapter.AcceptChangesDuringUpdate = false;
                    srcSqlDataAdapter.Fill(srcTable);

                    if (srcTable == null || srcTable.Rows.Count <= 0)
                        return true;

                    string notExistsDestSqlWhere = null;
                    string deleteSrcSqlWhere = null;
                    for (int i = 0; i < srcPrimarykeyParams.Count; i++)
                    {
                        string keyColName = srcPrimarykeyParams[i].ParameterName.Replace("@", "");
                        notExistsDestSqlWhere += string.Format(" AND told.{0}=tnew.{0}", keyColName);
                        deleteSrcSqlWhere += string.Format(" AND {0}=@{0}", keyColName);
                    }

                    string dbProviderName2 = null;
                    using (var destConn = new SqlConnection(dbConnectionString))
                    {
                        destConn.Open();
                        string tempDestTableName = "#temp_" + destTableName;
                        destConn.Execute(string.Format("select top 0 * into {0} from {1}", tempDestTableName, destTableName));
                        string destInsertCols = null;
                        using (var destSqlBulkCopy = new SqlBulkCopy(destConn))
                        {
                            try
                            {
                                destSqlBulkCopy.BulkCopyTimeout = 120;
                                destSqlBulkCopy.DestinationTableName = tempDestTableName;
                                foreach (DataColumn col in srcTable.Columns)
                                {
                                    destSqlBulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                                    destInsertCols += "," + col.ColumnName;
                                }
                                destSqlBulkCopy.BatchSize = 1000;
                                destSqlBulkCopy.WriteToServer(srcTable);
                            }
                            catch (Exception ex)
                            {
                                //LogUtil.Error("DapperUtil.Bat/*chMoveData.SqlBulkCopy:" + ex.ToString(), "DapperUtil.BatchMoveData");*/
                            }
                            destInsertCols = destInsertCols.Substring(1);
                            destConn.Execute(string.Format("insert into {1}({0}) select {0} from {2} tnew where not exists(select 1 from {1} told where {3})", destInsertCols, destTableName, tempDestTableName, notExistsDestSqlWhere.Trim().Substring(3)), null, null, 100);
                        }
                        destConn.Close();
                    }

                    deleteCommand = new SqlCommand(string.Format("DELETE FROM {0} WHERE {1}", srcTableName, deleteSrcSqlWhere.Trim().Substring(3)), srcSqlDataAdapter.SelectCommand.Connection);
                    deleteCommand.Parameters.AddRange(srcPrimarykeyParams.ToArray());
                    deleteCommand.UpdatedRowSource = UpdateRowSource.None;
                    deleteCommand.CommandTimeout = 200;

                    srcSqlDataAdapter.DeleteCommand = deleteCommand;
                    foreach (DataRow row in srcTable.Rows)
                    {
                        row.Delete();
                    }
                    srcSqlDataAdapter.UpdateBatchSize = 1000;
                    srcSqlDataAdapter.Update(srcTable);
                    srcTable.AcceptChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    //LogUtil.Error("DapperUtil.BatchMoveData:" + ex.ToString(), "DapperUtil.BatchMoveData");
                    return false;
                }
                finally
                {
                    if (deleteCommand != null)
                    {
                        deleteCommand.Parameters.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// 批量复制数据（把源DB中根据SQL语句查出的结果批量COPY插入到目的DB的目的表中）
        /// </summary>
        public TResult BatchCopyData<TResult>(string srcSelectSql, string destConnName, string destTableName, IDictionary<string, string> colMappings, Func<IDbConnection, TResult> afterCoppyFunc)
        {
            using (SqlDataAdapter srcSqlDataAdapter = new SqlDataAdapter(srcSelectSql, dbConnectionString))
            {
                DataTable srcTable = new DataTable();
                TResult copyResult = default(TResult);
                try
                {
                    srcSqlDataAdapter.AcceptChangesDuringFill = true;
                    srcSqlDataAdapter.AcceptChangesDuringUpdate = false;
                    srcSqlDataAdapter.Fill(srcTable);
                    if (srcTable == null || srcTable.Rows.Count <= 0) return copyResult;

                    string dbProviderName2 = null;
                    using (var destConn = new SqlConnection(dbConnectionString))
                    {
                        destConn.Open();
                        string tempDestTableName = "#temp_" + destTableName;
                        destConn.Execute(string.Format("select top 0 * into {0} from {1}", tempDestTableName, destTableName));
                        bool bcpResult = false;
                        using (var destSqlBulkCopy = new SqlBulkCopy(destConn))
                        {
                            try
                            {
                                destSqlBulkCopy.BulkCopyTimeout = 120;
                                destSqlBulkCopy.DestinationTableName = tempDestTableName;
                                foreach (var col in colMappings)
                                {
                                    destSqlBulkCopy.ColumnMappings.Add(col.Key, col.Value);
                                }

                                destSqlBulkCopy.BatchSize = 1000;
                                destSqlBulkCopy.WriteToServer(srcTable);
                                bcpResult = true;
                            }
                            catch (Exception ex)
                            {
                                //LogUtil.Error("DapperUtil.BatchMoveData.SqlBulkCopy:" + ex.ToString(), "DapperUtil.BatchMoveData");
                            }
                        }

                        if (bcpResult)
                        {
                            copyResult = afterCoppyFunc(destConn);
                        }
                        destConn.Close();
                    }
                    return copyResult;
                }
                catch (Exception ex)
                {
                    //LogUtil.Error("DapperUtil.BatchCopyData:" + ex.ToString(), "DapperUtil.BatchCopyData");
                    return copyResult;
                }
            }
        }

        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList">数据</param>
        /// <param name="connName">数据库连接</param>
        /// <param name="tableName">表名</param>
        public void BulkInsert<T>(List<T> dataList, string connName, string tableName)
        {
            string dbProviderName = null;
            using (var conn = new SqlConnection(dbConnectionString))
            {
                conn.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = dataList.Count;
                    bulkCopy.WriteToServer(dataList.ToDataTable());
                }
                conn.Close();
            }
        }

        /// <summary>
        /// 带事务功能的批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="connName"></param>
        /// <param name="tableName"></param>
        public void BulkInsertWithTransaction<T>(List<T> dataList, string connName, string tableName)
        {
            string dbProviderName = null;
            using (var conn = new SqlConnection(dbConnectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans))
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = dataList.Count;
                    bulkCopy.WriteToServer(dataList.ToDataTable());
                }
                trans.Commit();
                conn.Close();
            }
        }


        /// <summary>
        /// 当使用了事务，则最后需要调用该方法以提交所有操作
        /// </summary>
        public void Commit()
        {
            try
            {
                if (dbTransaction.Connection != null && dbTransaction.Connection.State != ConnectionState.Closed)
                {
                    dbTransaction.Commit();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dbTransaction.Connection != null)
                {
                    CloseDbConnection(dbTransaction.Connection);
                }
                dbTransaction.Dispose();
                dbTransaction = null;
                useDbTransaction = false;
                if (dbConnection != null)
                {
                    CloseDbConnection(dbConnection);
                }
            }
        }

        /// <summary>
        /// 当使用了事务，如果报错或需要中断执行，则需要调用该方法执行回滚操作
        /// </summary>
        public void Rollback()
        {
            if (dbTransaction == null)  // 还没执行就异常，因此dbTransaction为NULL
            {
                useDbTransaction = false;
                return;
            }
            try
            {
                if (dbTransaction.Connection != null && dbTransaction.Connection.State != ConnectionState.Closed)
                {
                    dbTransaction.Rollback();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dbTransaction.Connection != null)
                {
                    CloseDbConnection(dbTransaction.Connection);
                }
                dbTransaction.Dispose();
                dbTransaction = null;
                useDbTransaction = false;
            }
        }

        /// <summary>
        /// 析构函数释放资源
        /// </summary>
        ~DapperUtil()
        {
            try
            {
                CloseDbConnection(dbConnection, true);
            }
            catch
            { }
        }

        #region 私有方法
        /// <summary>
        /// 获取数据库连接对象
        /// </summary>
        /// <returns></returns>
        private IDbConnection GetDbConnection()
        {
            if (dbConnection == null)
            {
                var dbProviderFactory = DbProviderFactories.GetFactory(new SqlConnection(dbConnectionString));
                dbConnection = dbProviderFactory.CreateConnection();
                dbConnection.ConnectionString = dbConnectionString;
            }

            if (dbConnection.State == ConnectionState.Closed)
            {
                if (string.IsNullOrEmpty(dbConnection.ConnectionString))
                    dbConnection.ConnectionString = this.dbConnectionString;

                dbConnection.Open();
            }

            return dbConnection;
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="dbConnName"></param>
        /// <param name="dbProviderName"></param>
        /// <returns></returns>
        //private string GetDbConnectionString(string dbConnName, out string dbProviderName)
        //{
        //    ConnectionStringSettings connectionStrings = ConfigurationManager.ConnectionStrings[dbConnName];
        //    //dbProviderName = connectionStrings.ProviderName;
        //    //return connectionStrings.ConnectionString;
        //    dbProviderName = "";
        //    return PubConstant.ConnectionString;
        //}


        /// <summary>
        /// 使用数据库连接
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryOrExecSqlFunc"></param>
        /// <returns></returns>
        private T UseDbConnection<T>(Func<IDbConnection, T> queryOrExecSqlFunc)
        {
            IDbConnection dbConn = null;
            try
            {
                dbConn = GetDbConnection();
                if (useDbTransaction && dbTransaction == null)
                {
                    dbTransaction = GetDbTransaction();
                }
                return queryOrExecSqlFunc(dbConn);
            }
            finally
            {
                if (dbTransaction == null && dbConn != null)
                {
                    CloseDbConnection(dbConn);
                }
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="disposed"></param>
        private void CloseDbConnection(IDbConnection dbConn, bool disposed = false)
        {
            if (dbConn != null)
            {
                if (disposed && dbTransaction != null)
                {
                    dbTransaction.Rollback();
                    dbTransaction.Dispose();
                    dbTransaction = null;
                }

                if (dbConn.State != ConnectionState.Closed)
                {
                    dbConn.Close();
                }
                dbConn.Dispose();
                dbConn = null;
            }
        }

        /// <summary>
        /// 获取一个事务对象（如果需要确保多条执行语句的一致性，必需使用事务）
        /// </summary>
        /// <param name="il"></param>
        /// <returns></returns>
        private IDbTransaction GetDbTransaction(IsolationLevel il = IsolationLevel.Unspecified)
        {
            return GetDbConnection().BeginTransaction(il);
        }

        /// <summary>
        /// 生成动态参数
        /// </summary>
        /// <param name="paramDic"></param>
        /// <returns></returns>
        public object ToParameters()
        {
            var dyparams = paramDic;
            paramDic = new Dictionary<string, object>();
            return new DynamicParameters(dyparams);
        }
        /// <summary>
        /// 添加动态参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DapperUtil AddParameter(string key, object value)
        {
            if (paramDic == null)
                paramDic = new Dictionary<string, object>();
            paramDic[key] = value;
            return this;

        }
        /// <summary>
        /// 生成动态参数
        /// </summary>
        /// <param name="paramDic"></param>
        /// <returns></returns>
        public object ToParameters(Dictionary<string, object> paramDic)
        {
            return new DynamicParameters(paramDic);
        }
        /// <summary>
        /// 添加动态参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>返回点位符</returns>
        public string AddParameter(object value)
        {
            if (paramDic == null)
                paramDic = new Dictionary<string, object>();
            string key = "@p_" + paramDic.Count;
            paramDic[key] = value;
            return key;

        }

        #endregion
    }
}