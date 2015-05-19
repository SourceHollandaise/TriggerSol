using System;
using System.Linq;
using TriggerSol.Boost;
using TriggerSol.JStore;
using System.Collections.Generic;

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
                var category = transaction.CreateObject<Category>();
                category.Name = Guid.NewGuid().ToString();

                for (int x = 1; x < 11; x++)
                {
                    var item = transaction.CreateObject<Item>();
                    item.Name = "Item_" + x + "_" + category.Name;
                    item.ParentCategory = category;
                }

            }

            //Now commit
            transaction.Commit();
            transaction.Dispose();

            //Load from JStore
            var categories = DataStoreProvider.DataStore.LoadAll<Category>().OrderBy(p => p.Name).ToList();

            foreach (var model in categories)
            {
                Console.WriteLine(model.Name);
                Console.WriteLine("With items");
                foreach (var item in model.Items.OrderBy(p => p.Name))
                {
                    Console.WriteLine("\t" + item.Caption);
                }
            }

            Console.ReadKey();
        }
    }

    //This is the foldername where the items of type Category are saved
    [PersistentName("CATEGORIES")]
    public class Category : PersistentBase
    {
        string _Name;

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetPropertyValue(ref _Name, value);
            }
        }

        public IList<Item> Items
        {
            get
            {
                return GetAssociatedCollection<Item>(Fields<Item>.GetName(p => p.ParentCategory));
            }
        }

        //Override Delete for custom delete handling
        public override void Delete()
        {
            foreach (var item in Items)
            {
                item.Delete();
            }
            base.Delete();
        }

        //Override Save for custom save handling
        public override void Save(bool allowSaving = true)
        {
            foreach (var item in Items)
            {
                item.ParentCategory.Name = this.Name;
                item.Save();
            }

            base.Save(allowSaving);
        }
    }

    [PersistentName("ITEMS")]
    public class Item : PersistentBase
    {
        public string Caption
        {
            get
            {
                return ParentCategory.Name + "-" + Name;
            }
        }

        string _Name;

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetPropertyValue(ref _Name, value);
            }
        }

        int _Id;

        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                SetPropertyValue(ref _Id, value);
            }
        }

        Category _ParentCategory;

        public Category ParentCategory
        {
            get
            {
                return _ParentCategory;
            }
            set
            {
                SetPropertyValue(ref _ParentCategory, value);
            }
        }
    }
}
