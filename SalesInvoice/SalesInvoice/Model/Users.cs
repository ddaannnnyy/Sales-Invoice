using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;

namespace SalesInvoice.Model
{
    public class Users
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Pass { get; set; }
        public string Location { get; set; } //user's store (E.G. Brisbane)

        /// <summary>
        /// Blank User constructor
        /// </summary>
        public Users()
        {
            Id = 0;
            Username = string.Empty;
            Pass = string.Empty;
            Location = string.Empty;
        }

        /// <summary>
        /// Checks to see if the database has users
        /// </summary>
        /// <returns>true if at least one user is registered</returns>
        public static bool HasUsers()
        {
            using (var db = Utilities.Database.GetConnection())
            {
                var query = "SELECT COUNT(*) FROM users";
                var results = db.ExecuteScalar(query);
                return (long)results > 0; // returns true if the database has at least one user.                   
            }
        }

        /// <summary>
        /// Encryption Method to Salt and Hash passwords
        /// </summary>
        /// <param name="data">text to salt and hash</param>
        /// <returns>encrypted string as var hashed.</returns>
        public static string Encrypt(string data)
        {
            const string SHA_KEY = "kashdjkhadaydad89asdu90asd0aus6y8172h";

            string saltedPassword = SHA_KEY + data + SHA_KEY;
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(saltedPassword);
            bytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(bytes);
            string hashed = System.Text.Encoding.ASCII.GetString(bytes);

            return hashed;
        }

        /// <summary>
        /// Creates user with supplied info
        /// </summary>
        /// <param name="username">Username of user</param>
        /// <param name="pass">Chosen password of user</param>
        /// <param name="location">Store location of user</param>
        public static void CreateUser(string username, string pass, string location)
        {
            using (var db = Utilities.Database.GetConnection().OpenAndReturn())
            using (var transaction = db.BeginTransaction())
            {
                try
                {
                    var query = "INSERT INTO users (username, pass, location) VALUES (@un, @up, @loc);";
                    var param = new { un = username, up = Encrypt(pass), loc = location };
                    var results = db.Execute(query, param, transaction);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }


    }
}