using System;
using System.Linq;
using TriggerSol.Boost;
using TriggerSol.JStore;

namespace TriggerSol.XConsole
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //This line of code initialze your datastore 
            //That's all folks!
            new Booster().InitDataStore<CachedJsonFileDataStore>("/Users/trigger/TriggerSolDemo");

            //Create a new transaction for creating and saving objects
            ITransaction transaction = new Transaction();

            //Get noticed when object is commiting
            transaction.ObjectCommiting += (o) =>
            {
                Console.WriteLine("Commiting " + o.ToString());
            };

            //Get noticed when objects are rolling back
            transaction.ObjectRollback += (o) =>
            {
                Console.WriteLine("Rollback " + o.ToString());
            };


            for (int i = 1; i < 11; i++)
            {
               
                //Create some objects
                var foo = transaction.CreateObject<Foo>();
                var bar = transaction.CreateObject<Bar>();
                foo.Text = "Foo " + Guid.NewGuid().ToString();
                foo.Number = i;

                bar.Text = "Bar " + Guid.NewGuid().ToString();
                bar.Number = i;
                foo.FooBar = bar;
            }

            //Now commit
            transaction.Commit();

            //Load from JStore
            var persistents = DataStoreProvider.DataStore.LoadAll<Foo>().OrderBy(p => p.Number).ToList();

            foreach (var model in persistents)
            {
                Console.WriteLine(model.Text + " " + model.Number);
                Console.WriteLine(model.GetType().Name);
                Console.WriteLine(model.FooBar.Text + " " + model.FooBar.Number);
                Console.WriteLine(model.FooBar.GetType().Name);
            }

            Console.ReadKey();
        }
    }

    //This is the foldername where the items of type Bar are saved
    [PersistentName("FOO")]
    public class Foo : PersistentBase
    {
        public string Text
        {
            get;
            set;
        }

        public int Number
        {
            get;
            set;
        }

        public Bar FooBar
        {
            get;
            set;
        }

        //Override Save to store the referenced FooBar item
        public override void Save(bool allowSaving = true)
        {
            if (FooBar != null)
                FooBar.Save();

            base.Save(allowSaving);
        }

        //Override Delete to clean up store from the referenced FooBar item
        public override void Delete()
        {
            if (FooBar != null)
                FooBar.Delete();

            base.Delete();
        }
    }

    //This is the foldername where the items of type Bar are saved
    [PersistentName("BAR")]
    public class Bar : PersistentBase
    {
        public string Text
        {
            get;
            set;
        }

        public int Number
        {
            get;
            set;
        }
    }
}
