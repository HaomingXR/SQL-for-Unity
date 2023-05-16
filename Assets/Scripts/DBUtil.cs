using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace HaomingSQL
{
    public class DBUtil
    {
        private static IDbConnection dbConn;

        private static string sqlQuery;
        private static List<string> queryResult;

        public static bool dbLoaded { get; private set; }

        public static string ToStr(string input) => $"\"{input}\"";

        /// <summary>
        /// Load the Database
        /// </summary>
        public static void LoadDatabase(string database)
        {
            dbLoaded = false;
            string conn = "URI=file:" + Application.persistentDataPath + "/" + database;

#if UNITY_EDITOR
            Debug.Log(conn);
#endif

            dbConn = new SqliteConnection(conn);
            dbConn.Open();
            dbLoaded = true;
        }

        /// <summary>
        /// Check if the Database is already populated
        /// </summary>
        /// <returns>True if DB Exists; False otherwise</returns>
        public static bool Verify()
        {
            sqlQuery = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table'";
            bool result = false;

            using (IDbCommand dbCMD = dbConn.CreateCommand())
            {
                dbCMD.CommandText = sqlQuery;
                using (IDataReader reader = dbCMD.ExecuteReader())
                    result = reader.GetValue(0).ToString() != "0";
            }

            return result;
        }

        /// <summary>
        /// Check if a value exists in the database
        /// </summary>
        public static bool CheckEntry(string Table, ColumnStruct pair)
        {
            sqlQuery = $"SELECT EXISTS(SELECT 1 FROM {Table} WHERE {pair.Name} = {pair.Data}) as exist";
            bool result = false;

            using (IDbCommand dbCMD = dbConn.CreateCommand())
            {
                dbCMD.CommandText = sqlQuery;
                using (IDataReader reader = dbCMD.ExecuteReader())
                    result = reader.GetValue(0).ToString() != "0";
            }

            return result;
        }

        /// <summary>
        /// Create a new Table
        /// </summary>
        /// <param name="Table">The name of the Table</param>
        /// <param name="pairs">
        /// Tuple pairs of (Column, Value)
        /// eg. INTEGER, BOOL, VARCHAR(size), DATE
        /// </param>
        public static void CreateTable(string Table, params ColumnStruct[] pairs)
        {
            sqlQuery = $"CREATE TABLE {Table} (";

            sqlQuery += $"{pairs[0].Name} {pairs[0].Data}";

            for (int i = 1; i < pairs.Length; i++)
                sqlQuery += $", {pairs[i].Name} {pairs[i].Data}";

            sqlQuery += ")";

            using (IDbCommand dbCMD = dbConn.CreateCommand())
            {
                dbCMD.CommandText = sqlQuery;
                dbCMD.ExecuteReader();
            }
        }

        /// <summary>
        /// Insert data into a table
        /// </summary>
        /// <param name="table">Table to insert data into</param>
        /// <param name="pairs">Pairs of (Column, Value) to insert</param>
        public static void InsertData(string table, params ColumnStruct[] pairs)
        {
            sqlQuery = $"INSERT into {table} ";
            sqlQuery += $"({pairs[0].Name}";

            for (int i = 1; i < pairs.Length; i++)
                sqlQuery += ", " + pairs[i].Name;

            sqlQuery += $") VALUES ({pairs[0].Data}";

            for (int i = 1; i < pairs.Length; i++)
                sqlQuery += ", " + $"{pairs[i].Data}";

            sqlQuery += ")";

            using (IDbCommand dbCMD = dbConn.CreateCommand())
            {
                dbCMD.CommandText = sqlQuery;
                dbCMD.ExecuteScalar();
            }
        }

        /// <summary>
        /// Modify the data of a specific row
        /// </summary>
        /// <param name="table">Table to modify data from</param>
        /// <param name="change">(Column to modify, new Value)</param>
        /// <param name="condition">(Column to filter, condition value)</param>
        /// <returns>True if Successful; False otherwise</returns>
        public static void ModifyData(string table, ColumnStruct change, ColumnStruct condition)
        {
            sqlQuery = $"UPDATE {table} SET {change.Name} = {change.Data} WHERE {condition.Name} = {condition.Data}";

            using (IDbCommand dbCMD = dbConn.CreateCommand())
            {
                dbCMD.CommandText = sqlQuery;
                dbCMD.ExecuteReader();
            }
        }

        /// <summary>
        /// Grab a specific value with a condition
        /// </summary>
        /// <param name="table">The table to filter</param>
        /// <param name="target">The column to return</param>
        /// <param name="condition">The column to filter</param>
        /// <param name="value">The value to filter for</param>
        public static string QuerySingleWithFilter(string table, string target, string condition, string value)
        {
            sqlQuery = $"SELECT {target} FROM {table} WHERE {condition} = {value}";
            string result = string.Empty;

            using (IDbCommand dbCMD = dbConn.CreateCommand())
            {
                dbCMD.CommandText = sqlQuery;
                using (IDataReader reader = dbCMD.ExecuteReader())
                    result = reader.GetValue(0).ToString();
            }

            return result;
        }

        /// <summary>
        /// Grab all value with a filter
        /// </summary>
        /// <param name="table">The table to filter</param>
        /// <param name="condition">The column to filter</param>
        /// <param name="value">The value to filter for</param>
        public static string[] QueryAllWithFilter(string table, string condition, string value)
        {
            sqlQuery = $"SELECT * FROM {table} WHERE {condition} = {value}";
            queryResult = new List<string>();

            using (IDbCommand dbCMD = dbConn.CreateCommand())
            {
                dbCMD.CommandText = sqlQuery;
                using (IDataReader reader = dbCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                            queryResult.Add(reader.GetValue(i).ToString());
                    }
                }
            }

            return queryResult.ToArray();
        }

        /// <summary>
        /// Standard way to grab value
        /// </summary>
        /// <param name="table">Table :skull:</param>
        /// <param name="columns">Columns to get value from</param>
        public static string[] Query(string table, params string[] columns)
        {
            int l = columns.Length;

            sqlQuery = "SELECT ";
            sqlQuery += columns[0];

            for (int i = 1; i < l; i++)
                sqlQuery += ", " + columns[i];

            sqlQuery += $" FROM {table}";
            queryResult = new List<string>();

            using (IDbCommand dbCMD = dbConn.CreateCommand())
            {
                dbCMD.CommandText = sqlQuery;
                using (IDataReader reader = dbCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < l; i++)
                            queryResult.Add(reader.GetValue(i).ToString());
                    }
                }
            }

            return queryResult.ToArray();
        }

        /// <summary>
        /// Run commands not provided by the APIs
        /// </summary>
        public static void RunCustomCommand(string cmd)
        {
            using (IDbCommand dbCMD = dbConn.CreateCommand())
            {
                dbCMD.CommandText = cmd;
                dbCMD.ExecuteReader();
            }
        }

        /// <summary>
        /// End the connection to the Database
        /// </summary>
        public static void Terminate()
        {
            dbConn.Close();
            dbConn.Dispose();
            dbConn = null;
            SqliteConnection.ClearAllPools();
        }
    }

    public struct ColumnStruct
    {
        public string Name;
        public string Data;

        public ColumnStruct(string c, string v)
        {
            Name = c;
            Data = v;
        }
    }
}