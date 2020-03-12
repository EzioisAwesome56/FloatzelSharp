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
        private static RethinkDB r = RethinkDB.R;
        private static Connection thonk;
        // for compatibility reasons
        private static Connection oldthonk;
        private static bool hasOld = false;

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
            if (!(bool) r.DbList().Contains("FloatzelSharp").Run(thonk)) {
                // it doesnt exist! make that database!
                Console.WriteLine("Database not detected! creating new database...");
                r.DbCreate("FloatzelSharp").Run(thonk);
                thonk.Use("FloatzelSharp");
                Console.WriteLine("Creating tables...");
                makeTables();
                Console.WriteLine("Database created!");
            } else {
                thonk.Use("FloatzelSharp");
                Console.WriteLine("Driver loaded!");
            }
            // check for legacy database stuff
            Console.WriteLine("Floatzel is now checking for 2.x database...");
            if ((bool) r.DbList().Contains("floatzel").Run(thonk)) {
                oldthonk = builder.Connect();
                oldthonk.Use("floatzel");
                Console.WriteLine("Floatzel found 2.x database! Will convert data as its accessed");
                hasOld = true;
            } else {
                Console.WriteLine("Floatzel did not find 2.x databse!");
            }
        }


        private static void makeTables() {
            // run a bunch of rethink commands
            r.TableCreate(banktable).OptArg("primary_key", "uid").Run(thonk);
            r.TableCreate(loantable).OptArg("primary_key", "uid").Run(thonk);
            r.TableCreate(bloanperm).OptArg("primary_key", "uid").Run(thonk);
            r.TableCreate(stocktable).OptArg("primary_key", "sid").Run(thonk);
            r.TableCreate(tweets).OptArg("primary_key", "tid").Run(thonk);
            r.TableCreate(tagperm).OptArg("primary_key", "gid").Run(thonk);
            //r.TableCreate(tags).Run(thonk);
            r.TableCreate(stockbuy).OptArg("primary_key", "uid").Run(thonk);
        }


        // check if a bank account exists; also serves as account converter from 2.x db to 3.x
        public static bool dbCheckIfExist(string id) {
            var dank = r.Table(banktable).Get(id).Run(thonk);
            if (dank == null) {
                // is olddb present?
                if (hasOld) {
                    // step 1: check if they have an account
                    if ((bool) r.Table(banktable).Filter(r.HashMap("uid", id)).Count().Eq(1).Run(oldthonk)) {
                        // they do! load the value.
                        var cursor = r.Table(banktable).Filter(row => row.GetField("uid").Eq(id)).GetField("bal").Run(oldthonk);
                        foreach (var i in cursor) {
                            r.Table(banktable).Insert(r.Array(
                            r.HashMap("uid", id)
                                    .With("bal", i)
                             )).Run(thonk);
                        }
                        // cool, data converted! return true
                        return true;
                    }
                }
            } else {
                // if its not null, theres a value! return true
                return true;
            }
            return false;
        }

        // load user bank account
        public static int dbLoadInt(string id) {
            return (int) r.Table(banktable).Get(id).GetField("bal").Run(thonk);
        }

        // save user bank account
        public static void dbSaveInt(string id, int bal) {
            // patch integer overflow error
            if (bal < -100) {
                bal = int.MaxValue;
            }
            // save it
            r.Table(banktable).Get(id).Update(r.HashMap("bal", bal)).Run(thonk);
        }

        // self-explanitory: make a new bank account for a person
        public static void dbCreateAccount(string id) {
            r.Table(banktable).Insert(r.HashMap("uid", id).With("bal", 0)).Run(thonk);
        }

        // check if a user already has a loan or not
        public static bool dbCheckIfLoan(string id) {
            var dank = r.Table(loantable).Get(id).Run(thonk);
            if (dank == null) {
                // insert a blank db entry
                r.Table(loantable).Insert(r.HashMap("uid", id).With("time", 0L)).Run(thonk);
                return false;
            } else {
                return true;
            }
        }

        // save current loan time into db
        public static void dbSaveLoan(string id, double time) {
            r.Table(loantable).Update(r.HashMap("uid", id).With("time", time)).Run(thonk);
        }

        // load last loan time from database
        public static double dbLoadLoan(string id) {
            return (double) r.Table(loantable).Get(id).GetField("time").Run(thonk);
        }
    }
}
