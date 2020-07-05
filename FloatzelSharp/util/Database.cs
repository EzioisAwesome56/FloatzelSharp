using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using FloatzelSharp.types;
using System.Linq;

namespace FloatzelSharp.util {
    class Database {
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

        // profiles??
        private static String account = "profile";



        public static void dbinit() {
            Connection.Builder builder = r.Connection().Hostname("localhost").Port(28015);
            // connect
            Console.WriteLine("Floatzel is now loading EzioSoft RethinkDB Driver V2...");
            thonk = builder.Connect();
            // check if the database exists
            if (!(bool)r.DbList().Contains("FloatzelSharp").Run(thonk)) {
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
            if ((bool)r.DbList().Contains("floatzel").Run(thonk)) {
                oldthonk = builder.Connect();
                oldthonk.Use("floatzel");
                Console.WriteLine("Floatzel found 2.x database! Will convert data as its accessed");
                hasOld = true;
            } else {
                Console.WriteLine("Floatzel did not find 2.x databse!");
            }
        }

        // check if we have Floatzel 2.x tweets stored that need to be converted
        public static bool dbCheckForOldTweets() => hasOld && r.TableList().Contains(tweets).Run<bool>(oldthonk) && r.Table(tweets).Count().Run<int>(oldthonk) > 0;

        // check if we have Floatzel 2.x stocks saved
        public static bool dbCheckForOldStocks() => hasOld && r.TableList().Contains(stocktable).Run<bool>(oldthonk) && r.Table(stocktable).Count().Run<int>(oldthonk) > 0;

        // Floatzel 2.x stocks converter
        public static void dbConvertStocks() {
            Console.WriteLine("Floatzel has found stocks stored in Legacy 2.x format");
            Console.WriteLine("Floatzel will now attempt to convert these stocks into the new format");
            // step 1: load how many stocks are present
            int total = r.Table(stocktable).Count().Run<int>(oldthonk);
            Console.WriteLine($"Total number of stocks is {total.ToString()}");
            var count = 1;
            while (count <= total) {
                Cursor<int> cur;
                List<int> list;
                // step 2: create new Stock object
                Stock dank = new Stock();
                // step 3: load all data required into stock object
                dank.sid = count.ToString();
                // step 4: load diff
                cur = r.Table(stocktable).Filter(r.HashMap("sid", count)).GetField("diff").Run<int>(oldthonk);
                list = cur.ToList<int>();
                dank.diff = list.Single<int>();
                // step 5: load price
                cur = r.Table(stocktable).Filter(r.HashMap("sid", count)).GetField("price").Run<int>(oldthonk);
                list = cur.ToList<int>();
                dank.price = list.Single<int>();
                // step 6: load units
                cur = r.Table(stocktable).Filter(r.HashMap("sid", count)).GetField("units").Run<int>(oldthonk);
                list = cur.ToList<int>();
                dank.units = list.Single<int>();
                // step 7: load name
                Cursor<string> namecur = r.Table(stocktable).Filter(r.HashMap("sid", count)).GetField("name").Run<string>(oldthonk);
                List<string> namelist = namecur.ToList<string>();
                dank.name = namelist.Single<string>();
                // step 8: save stock object to rethinkdb
                r.Table(stocktable).Insert(dank).Run(thonk);
                // step 9: inc counter
                Console.WriteLine($"Converted Stock {count.ToString()}");
                count++;
            }
            // step 10: drop the old ass table
            r.TableDrop(stocktable).Run(oldthonk);
            // done!
            Console.WriteLine("Floatzel has finished converting the stocks!");
        }

        // Floatzel 2.x tweet converter
        public static void dbConvertTweets() {
            Console.WriteLine("Floatzel has found tweets stored in Floatzel 2.x format.");
            Console.WriteLine("Floatzel will now convert tweets to newer 3.0 format. This may take some time");
            // step 1: load the total amount of tweets we need to convert
            int total = (int)r.Table(tweets).Count().Run(oldthonk);
            Console.WriteLine($"Total number of tweets is {total.ToString()}");
            var count = 1;
            while (count <= total) {
                // step 2: obain tweet
                Cursor<string> cursor = r.Table(tweets).Filter(r.HashMap("tid", count)).GetField("txt").Run<string>(oldthonk);
                // step 3: create new tweet object
                Tweet bird = new Tweet();
                bird.tid = count.ToString();
                List<string> list = cursor.ToList<string>();
                bird.txt = list[0];
                // step 4: save tweet object into new database
                r.Table(tweets).Insert(bird).Run(thonk);
                Console.WriteLine($"Tweet ID {count} converted!");
                // step 5: increment the counter
                count++;
            }
            // step 6: delete the old tweets table
            r.TableDrop(tweets).Run(oldthonk);
            Console.WriteLine("Floatzel has converted all tweets!");
        }


        private static void makeTables() {
            // run a bunch of rethink commands
            //r.TableCreate(banktable).OptArg("primary_key", "uid").Run(thonk);
            r.TableCreate(account).OptArg("primary_key", "uid").Run(thonk);
            r.TableCreate(tweets).OptArg("primary_key", "tid").Run(thonk);
            r.TableCreate(stocktable).OptArg("primary_key", "sid").Run(thonk);
            /*r.TableCreate(loantable).OptArg("primary_key", "uid").Run(thonk);
            r.TableCreate(bloanperm).OptArg("primary_key", "uid").Run(thonk);
            r.TableCreate(tagperm).OptArg("primary_key", "gid").Run(thonk);
            r.TableCreate(tags).Run(thonk);
            r.TableCreate(stockbuy).OptArg("primary_key", "uid").Run(thonk);*/
        }

        // <summary>
        // check if a user profile exists; also serves as bank account importer from 2.x db to 3.x
        // </summary>
        public static async Task<bool> dbCheckIfExist(string id) {
            var dank = await r.Table(account).Get(id).RunAsync(thonk);
            if (dank == null) {
                // is olddb present?
                if (hasOld) {
                    // step 1: check if they have an account
                    if ((bool)await r.Table(banktable).Filter(r.HashMap("uid", id)).Count().Eq(1).RunAsync(oldthonk)) {
                        // they do! load the value.
                        Cursor<int> cursor = await r.Table(banktable).Filter(row => row.GetField("uid").Eq(id)).GetField("bal").RunAsync<int>(oldthonk);
                        List<int> list = cursor.ToList<int>();
                        await Database.dbCreateProfile(id, list[0]);
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

        // self-explanitory: make a new profile for a user
        public static async Task dbCreateProfile(string id, int bal = 0, double time = 0) {
            Profile dank = new Profile {
                uid = id,
                bal = bal,
                loantime = time,
                bloan = false,
                boxes = new int[] { 0, 0, 0, 0 },
                stock = new int[] { 0, 0, 0 },
                admin = false
            };
            await r.Table(account).Insert(dank).RunAsync(thonk);
        }

        // load a user profile
        public static async Task<Profile> dbLoadProfile(string id) {
            return await r.Table(account).Get(id).RunAsync<Profile>(thonk);
        }

        // Save a user profile
        public static async Task dbSaveProfile(Profile dank) {
            await r.Table(account).Get(dank.uid).Update(dank).RunAsync(thonk);
        }

        // OWNER SHIT:
        // do you absolutely hate someone?
        // DELETE THEIR PROFILE with this command!
        public static async Task dbDeleteProfile(string uid) {
            await r.Table(account).Get(uid).Delete().RunAsync(thonk);
        }

        // convert old permissions into new permissions
        public static async Task<Profile> dbConvertPerms(Profile dank) {
            if (await r.Table(bloanperm).Filter(r.HashMap("uid", dank.uid)).Count().Eq(1).RunAsync<bool>(oldthonk)) {
                dank.bloan = true;
            }
            return dank;
        }

        // count how many stocks are in the stock table
        public static async Task<int> dbCountStocks() {
            return await r.Table(stocktable).Count().RunAsync<int>(thonk);
        }

        // load a stock
        public static async Task<Stock> dbLoadStock(string id) {
            return await r.Table(stocktable).Get(id).RunAsync<Stock>(thonk);
        }

        // check if a stock DOES NOT exist
        public static async Task<bool> dbCheckForStock(string id) {
            return await r.Table(stocktable).Filter(r.HashMap("sid", id)).Count().Eq(0).RunAsync<bool>(thonk);
        }

        // save a stock
        public static async Task dbSaveStock(Stock st) {
            await r.Table(stocktable).Get(st.sid).Update(st).RunAsync(thonk);
        }
    }
}
