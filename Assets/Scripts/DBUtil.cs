using Mono.Data.Sqlite;
using System;
using System.Data;
using UnityEngine;

public class DBUtil
{
    private static IDbConnection dbConn;
    private static IDataReader reader;

    private static string sqlQuery;
    private static string queryResult;

    private const string DatabaseName = "Data.db";
    private static string filepath { get { return Application.persistentDataPath + "/" + DatabaseName; } }
    public static bool dbLoaded { get; private set; }

    public enum Category { Int, Char }

    /// <summary>
    /// Load the Database
    /// </summary>
    public static void LoadDatabase()
    {
        dbLoaded = false;
        string conn = "URI=file:" + filepath;
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
        IDbCommand dbCMD = dbConn.CreateCommand();
        dbCMD.CommandText = sqlQuery;
        reader = dbCMD.ExecuteReader();
        dbCMD.Dispose();
        return reader.GetValue(0).ToString() != "0";
    }

    /// <summary>
    /// Create a new Table
    /// </summary>
    /// <param name="Table">The name of the Table</param>
    /// <param name="pairs">Pairs of (Columns, Data Type) </param>
    public static void CreateTable(string Table, params Tuple<string, Category>[] pairs)
    {
        sqlQuery = $"CREATE TABLE {Table} (";

        switch (pairs[0].Item2)
        {
            case Category.Int:
                sqlQuery += $"{pairs[0].Item1} INTEGER";
                break;
            case Category.Char:
                sqlQuery += $"{pairs[0].Item1} varchar(100)";
                break;
        }

        for (int i = 1; i < pairs.Length; i++)
        {
            switch (pairs[i].Item2)
            {
                case Category.Int:
                    sqlQuery += $", {pairs[i].Item1} INTEGER";
                    break;
                case Category.Char:
                    sqlQuery += $", {pairs[i].Item1} varchar(100)";
                    break;
            }
        }

        sqlQuery += ")";

        IDbCommand dbCMD = dbConn.CreateCommand();
        dbCMD.CommandText = sqlQuery;
        dbCMD.ExecuteReader();
        dbCMD.Dispose();
    }

    /// <summary>
    /// Insert data into a table
    /// </summary>
    /// <param name="table">Table to insert data into</param>
    /// <param name="pairs">Tuple pairs of (Column, Value)</param>
    public static void InsertData(string table, params Tuple<string, string>[] pairs)
    {
        sqlQuery = $"INSERT into {table} ";
        sqlQuery += $"({pairs[0].Item1}";

        for (int i = 1; i < pairs.Length; i++)
            sqlQuery += ", " + pairs[i].Item1;

        sqlQuery += $") VALUES ({pairs[0].Item2}";

        for (int i = 1; i < pairs.Length; i++)
            sqlQuery += ", " + pairs[i].Item2;

        sqlQuery += ")";

        IDbCommand dbCMD = dbConn.CreateCommand();
        dbCMD.CommandText = sqlQuery;
        dbCMD.ExecuteScalar();
        dbCMD.Dispose();
    }

    /// <summary>
    /// Grab all value with a filter
    /// </summary>
    /// <param name="table">The table to filter</param>
    /// <param name="condition">The column to filter</param>
    /// <param name="value">The value to filter for</param>
    /// <returns></returns>
    public static string QueryAllWithFilter(string table, string condition, string value)
    {
        try
        {
            sqlQuery = $"SELECT * FROM {table} WHERE {condition} = {value}";

            IDbCommand dbCMD = dbConn.CreateCommand();
            dbCMD.CommandText = sqlQuery;
            IDataReader reader = dbCMD.ExecuteReader();

            queryResult = string.Empty;

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    queryResult += $"{reader.GetValue(i)} ";
                queryResult += "\n";
            }

            dbCMD.Dispose();
            return queryResult;
        }
        catch { return null; }
    }

    /// <summary>
    /// Standard way to grab value
    /// </summary>
    /// <param name="table">Table :skull:</param>
    /// <param name="verbose">True: Return string also contains the column name; False: Plain string containing only the values</param>
    /// <param name="columns">Columns to get value from</param>
    /// <returns></returns>
    public static string Query(string table, bool verbose = true, params string[] columns)
    {
        try
        {
            int l = columns.Length;

            sqlQuery = "SELECT ";
            sqlQuery += columns[0];

            for (int i = 1; i < l; i++)
                sqlQuery += ", " + columns[i];

            sqlQuery += " FROM ";
            sqlQuery += table;

            IDbCommand dbCMD = dbConn.CreateCommand();
            dbCMD.CommandText = sqlQuery;
            IDataReader reader = dbCMD.ExecuteReader();

            queryResult = string.Empty;

            if (verbose)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < l; i++)
                        queryResult += $"{columns[i]}: {reader.GetValue(i)}\n";
                }
            }
            else
            {
                while (reader.Read())
                {
                    for (int i = 0; i < l; i++)
                        queryResult += $"{reader.GetValue(i)} ";
                }
            }

            dbCMD.Dispose();
            return queryResult;
        }
        catch { return null; }
    }

    public static void Terminate()
    {
        dbConn.Close();
        dbConn.Dispose();
    }
}