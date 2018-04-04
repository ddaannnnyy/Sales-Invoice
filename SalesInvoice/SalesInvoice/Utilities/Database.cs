using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using Dapper;

namespace SalesInvoice.Utilities
{
    public static class Database
    {
        const string DB_FILE = "invoices.db";
        const string CREATE_SCRIPT = "db.sql";

        static string MAPPED_DATA;
        static string CONNECTION_STRING;

        static Database()
        {
            MAPPED_DATA = HttpContext.Current.Server.MapPath("~/App_Data/");
            CONNECTION_STRING = $"Data Source={MAPPED_DATA + DB_FILE}";
        }

        /// <summary>
        /// Create new SQLite DB Connection
        /// </summary>
        /// <returns>new SQLiteConnection with CONNECTION_STRING</returns>
        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(CONNECTION_STRING);
        }

        /// <summary>
        /// Runs to create database, using db.sql. Typically run once, on Application start.
        /// </summary>
        public static void CreateDatabase()
        {
            using (var db = GetConnection())
            {
                var query = System.IO.File.ReadAllText(MAPPED_DATA + CREATE_SCRIPT);
                var results = db.Execute(query); //capture to avoid race condition issues
            }
        }
    }
}