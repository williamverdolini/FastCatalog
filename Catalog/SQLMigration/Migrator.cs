using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using SQLCommon.Logic;
using SQLCommon.Models;

namespace SQLMigration
{
    public class Migrator<T> where T : IDbClient, new()
    {
        private IDbClient dbClient;
        private bool IsInitialized = false;
        private int commitStep = 0;

        public Migrator<T> Initialize()
        {
            dbClient = (new T()).Initialize();
            IsInitialized = true;
            commitStep = int.Parse(Resources.CommitStep);
            return this;
        }

        public Migrator<T> Execute()
        {
            if (IsInitialized)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                int counter = 0;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[Resources.ConnectionStringKey].ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(Resources.InitialPopulate, conn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SQLProduct dbProduct = reader[0].ToString().FromXmlTo<SQLProduct>();
                            dbClient.Save(dbProduct);
                            dbClient.FlushProducts(commitStep);
                            Console.WriteLine("#{0} - code: {1}", (++counter), dbProduct.Data.Code);
                        }
                    }
                    cmd.Dispose();
                    dbClient.FlushProducts();
                }
                sw.Stop();
                Console.WriteLine("Elapsed: {0}", sw.Elapsed);
                Console.WriteLine("Total Records inserted: {0}", counter);
                Console.WriteLine("Insert Rate: {0} rec/sec", (counter / (sw.ElapsedMilliseconds / 1000)));                                
            }
            return this;
        }

        public void PostMigration()
        {
            Console.WriteLine("Start executing post-migration logic");
            dbClient.PostMigration();
            Console.WriteLine("Post-migration logic completed.");
        }
    }
}
