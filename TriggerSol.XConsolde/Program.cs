﻿using System;
using TriggerSol.JStore;
using TriggerSol.Boost;
using System.Linq;

namespace TriggerSol.XConsolde
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Bootstrapping");

            new Bootstrapper().InitDataStore<CachedFileDataStore>("/Users/trigger/TriggerSolDemo");

            var models = DataStoreProvider.DataStore.LoadAll<Foo>().OrderBy(p => p.Number).ToList();

            foreach (var model in models)
            {
                model.Delete();

                Console.WriteLine(model.Number + "\t->\t" + model.Text + " deleted!");
            }

            ITransaction transaction = new Transaction();

            transaction.ObjectCommiting += (o) =>
            {
                Console.WriteLine("Commiting " + o.ToString());
            };

            transaction.ObjectRollback += (o) =>
            {
                Console.WriteLine("Rollback " + o.ToString());
            };

            for (int i = 1; i < 101; i++)
            {
               
                var foo = transaction.CreateObject<Foo>();
                var bar = transaction.CreateObject<Bar>();
                foo.Text = "Foo " + Guid.NewGuid().ToString();
                foo.Number = i;

                bar.Text = "Bar " + Guid.NewGuid().ToString();
                bar.Number = i;
                foo.FooBar = bar;
            }

            transaction.Commit();

            models = DataStoreProvider.DataStore.LoadAll<Foo>().OrderBy(p => p.Number).ToList();

            foreach (var model in models)
            {
                Console.WriteLine(model.Text + " " + model.Number);
                Console.WriteLine(model.GetType().Name);
                Console.WriteLine(model.FooBar.Text + " " + model.FooBar.Number);
                Console.WriteLine(model.FooBar.GetType().Name);
            }

            Console.ReadKey();
        }
    }

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

        public override void Save(bool allowSaving = true)
        {
            FooBar.Save();

            base.Save(allowSaving);
        }

        public override void Delete()
        {
            FooBar.Delete();

            base.Delete();
        }
    }

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
