using System;
using System.Data;
using System.Data.OleDb;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // The menu of when the program is run
            while (true)
            {
                Console.WriteLine("1. Inserting data");
                Console.WriteLine("2. Read data");
                Console.WriteLine("3. Update data");
                Console.WriteLine("4. Delete data");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    //Inserting data into the database
                    case 1:
                        Console.Write("Enter SQL for Insert: ");
                        string insertSql = Console.ReadLine();
                        DBCon.InsertData(insertSql);
                        break;
                    //Reading data from the database
                    case 2:
                        Console.Write("Enter SQL for Read: ");
                        string readSql = Console.ReadLine();
                        DBCon.GetData(readSql);
                        break;
                    // Updating the data that is present in the database
                    case 3:
                        Console.Write("Enter SQL for Update: ");
                        string updateSql = Console.ReadLine();
                        DBCon.UpdateData(updateSql);
                        break;
                    // Deleting data that is present in the database
                    case 4:
                        Console.Write("Enter SQL for Delete: ");
                        string deleteSql = Console.ReadLine();
                        DBCon.DeleteData(deleteSql);
                        break;
                    case 5:
                        return;
                    // This will tell the user that their number is invalid at will have to enter in their option again
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }

    public static class DBCon
    {
        // Make sure to update the ConnectionString with the correct path
        private static string ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Database project\Inventory Management System\Inventory Management System\bin\Debug\Inventory.accdb;Persist Security Info=False;";


        //Database connector function
        public static OleDbConnection Connect()
        {
            OleDbConnection con = null;
            try
            {
                con = new OleDbConnection(ConnectionString);
                con.Open();
                Console.WriteLine("Connected to database successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
                // This will help in debugging path issues
                Console.WriteLine("Check if the database path is correct: " + ConnectionString);
            }
            return con;
        }

        // Function to execute a SQL query and return a dataset
        public static DataSet ExecuteQuery(string sql)
        {
            using (OleDbConnection con = Connect())
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, con);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }

        // Function to execute SQL commands that do not return any result
        public static void ExecuteNonQuery(string sql)
        {
            using (OleDbConnection con = Connect())
            {
                if (con != null)
                {
                    using (OleDbCommand cmd = new OleDbCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        //Function for inserting data from the database
        public static void InsertData(string sql)
        {
            try
            {
                ExecuteNonQuery(sql);
                Console.WriteLine("Data has been successfully inserted into database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data insert failed: " + ex.Message);
            }
        }

        //Function for retriving data from the database
        public static void GetData(string sql)
        {
            try
            {
                DataSet ds = ExecuteQuery(sql);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            foreach (DataColumn dc in dt.Columns)
                            {
                                Console.WriteLine($"{dc.ColumnName}: {dr[dc]}");
                            }
                            Console.WriteLine();
                        }
                    }
                }
                else 
                {
                    Console.WriteLine("No data is found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data: " + ex.Message);
            }
        }

        //Function for updating the data
        public static void UpdateData(string sql)
        {
            try
            {
                ExecuteNonQuery(sql);
                Console.WriteLine("Data has been successfully updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating data: " + ex.Message);
            }
        }

        //Function for deleting the data
        public static void DeleteData(string sql)
        {
            try
            {
                ExecuteNonQuery(sql);
                Console.WriteLine("Data has been successfully deleted from database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting data: " + ex.Message);
            }
        }

    }
}
