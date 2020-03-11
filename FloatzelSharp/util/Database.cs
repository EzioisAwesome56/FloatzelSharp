using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;

namespace FloatzelSharp.util
{
    class Database
    {
        // init rethinkdb in its whole
        public static RethinkDB r = RethinkDB.R;
        public static Connection thonk;

        // copy-paste from java: table names
        private static String banktable = "bank";
        private static String loantable = "loan";
        private static String bloanperm = "bloan";
        private static String stocktable = "stocks";
        private static String stockbuy = "boughtstock";
        private static String tweets = "tweets";
        private static String tagperm = "gtagperm";
        private static String tags = "tags";



        public static void dbinit()
        {
            Connection.Builder builder = r.Connection().Hostname("localhost").Port(28015);
            // connect
            thonk = builder.Connect();
            
            Console.WriteLine("Floatzel is now loading EzioSoft RethinkDB Driver V2...");
            // check if the database exists
            if (!(bool) r.DbList().Contains("floatzel").Run(thonk)) {
                // it doesnt exist! make that database!
                Console.WriteLine("Database not detected! creating new database...");
                r.DbCreate("floatzel").Run(thonk);
                thonk.Use("floatzel");
                Console.WriteLine("Creating tables...");
                makeTables();
                Console.WriteLine("Database created!");
            } else {
                thonk.Use("floatzel");
                Console.WriteLine("Driver loaded!");
            }
        }


        private static void makeTables() {
            // run a bunch of rethink commands
            r.TableCreate(banktable).Run(thonk);
            r.TableCreate(loantable).Run(thonk);
            r.TableCreate(bloanperm).Run(thonk);
            r.TableCreate(stocktable).Run(thonk);
            r.TableCreate(tweets).Run(thonk);
            r.TableCreate(tagperm).Run(thonk);
            r.TableCreate(tags).Run(thonk);
            r.TableCreate(stockbuy).Run(thonk);
        }


        // check if a bank account exists
        public static bool dbCheckIfExist(string id) {
            return (bool) r.Table(banktable).Filter(
                r.HashMap("uid", id)).Count().Eq(1).Run(thonk);
        }

        // load user bank account
        public static Cursor<object> dbLoadInt(string id) {
            return r.Table(banktable).Filter(row => row.GetField("uid").Eq(id)).GetField("bal").Run(thonk);
        }
    }
}
