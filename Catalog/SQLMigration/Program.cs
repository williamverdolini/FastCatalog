using System;
using SQL2Elastic.Logic;
using SQL2MongoDB.Logic;

namespace SQLMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length.Equals(1))
            {
                switch (args[0])
                {
                    case "Mongo":
                        SQL2Mongo();
                        break;
                    case "Elastic":
                        SQL2Elastic();
                        break;
                    case "All":
                        SQL2Mongo();
                        SQL2Elastic();
                        break;
                    case "help":
                        Console.WriteLine("Type argument to specify wich migration execute. valid argument's values are:");
                        Help();
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Please select:");
                        Help();
                        break;
                }
            }
            else
            {
                Console.WriteLine("Type argument to specify wich migration execute. valid argument's values are:");
                Help();
            }
        }

        static private void SQL2Mongo()
        {
            new Migrator<MongoDbClient>()
                .Initialize()
                .Execute();
        }

        static private void SQL2Elastic()
        {
            new Migrator<ElasticSearchClient>()
                .Initialize()
                .Execute();
        }

        static private void Help()
        {
            Console.WriteLine("'Mongo'   : for migration from SQL to MongoDB");
            Console.WriteLine("'Elastic' : for migration from SQL to ElasticSearch");
            Console.WriteLine("'All'     : for migration from SQL to MongoDB and ElasticSearch");
            Console.WriteLine("");
            Console.WriteLine("Please ensure that chosen DB instances are running before launch the migration.");
        }
    }
}
