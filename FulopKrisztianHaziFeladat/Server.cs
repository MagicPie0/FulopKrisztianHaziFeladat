using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows;

namespace FulopKrisztianHaziFeladat
{
    public class DatabaseHelper
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Konstruktor
        public DatabaseHelper()
        {
            Initialize();
        }

        //Kapcsolat inicializálása
        private void Initialize()
        {
            server = "localhost"; 
            database = "library"; 
            uid = "LibraryUse"; 
            password = "Premo800"; 
            string connectionString;
            connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";

            connection = new MySqlConnection(connectionString);
        }

        //Kapcsolat lekérdezése
        public MySqlConnection GetConnection()
        {
            return connection;
        }

        //Kapcsolat megnyitása
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Kapcsolat bezárása
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Bejelentkezés
        public enum AuthenticationResult
        {
            Success,
            InvalidUsername,
            InvalidPassword,
            Error
        }

        public static AuthenticationResult AuthenticateUser(string username, string password)
        {
            AuthenticationResult result = AuthenticationResult.Error;
            DatabaseHelper dbHelper = new DatabaseHelper();

            try
            {
                string query = $"SELECT COUNT(*) FROM user WHERE Name='{username}' AND Password='{password}'";
                MySqlCommand cmd = new MySqlCommand(query, dbHelper.GetConnection());
                dbHelper.OpenConnection();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                dbHelper.CloseConnection();

                if (count > 0)
                {
                    result = AuthenticationResult.Success;
                }
                else
                {
                    query = $"SELECT COUNT(*) FROM user WHERE Name='{username}'";
                    cmd = new MySqlCommand(query, dbHelper.GetConnection());
                    dbHelper.OpenConnection();
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                    dbHelper.CloseConnection();

                    if (count > 0)
                    {
                        result = AuthenticationResult.InvalidPassword;
                    }
                    else
                    {
                        result = AuthenticationResult.InvalidUsername;
                    }
                }
            }
            catch (Exception)
            {
                result = AuthenticationResult.Error;
            }

            return result;
        }

        //Regisztráció
        public static bool AddUserToDatabase(string id, string username, string password, string email)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            try
            {
                
                if (UserExistsWithEmail(email))
                {
                    MessageBox.Show("Ez az e-mail cím már használatban van!");
                    return false;
                }
                else
                {
                    string query = $"INSERT INTO user (ID, Name, Email, Password) VALUES ('{id}', '{username}', '{email}', '{password}')";
                    MySqlCommand cmd = new MySqlCommand(query, dbHelper.GetConnection());
                    dbHelper.OpenConnection();
                    cmd.ExecuteNonQuery();
                    dbHelper.CloseConnection();
                    return true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt az adatbázishoz történő hozzáadás során: " + ex.Message);
                return false;
            }
        }
        private static bool UserExistsWithEmail(string email)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            try
            {
                string query = $"SELECT COUNT(*) FROM user WHERE Email='{email}'";
                MySqlCommand cmd = new MySqlCommand(query, dbHelper.GetConnection());
                dbHelper.OpenConnection();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                dbHelper.CloseConnection();

                return count > 0; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt az adatbázis ellenőrzése során: " + ex.Message);
                return false;
            }
        }
    }
}