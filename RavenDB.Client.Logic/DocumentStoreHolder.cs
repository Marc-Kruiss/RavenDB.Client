using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RavenDB.Client.Logic
{
    public static class DocumentStoreHolder
    {
        private static readonly string DbName = "Pets";
        // Use Lazy<IDocumentStore> to initialize the document store lazily. 
        // This ensures that it is created only once - when first accessing the public `Store` property.
        private static Lazy<IDocumentStore> store = new Lazy<IDocumentStore>(CreateStore);

        public static IDocumentStore Store => store.Value;

        private static IDocumentStore CreateStore()
        {
            IDocumentStore store = new DocumentStore()
            {
                // Define the cluster node URLs (required)
                Urls = new[] { "http://localhost:8080", 
                           /*some additional nodes of this cluster*/ },


                // Define a default database (optional)
                Database = DbName,

                // Initialize the Document Store
            }.Initialize();

            EnsureDatabaseIsCreated(store);

            return store;
        }

        private static void EnsureDatabaseIsCreated(IDocumentStore store)
        {
            try
            {
                // check if database exists
                store.Maintenance.ForDatabase(DbName).Send(new GetStatisticsOperation());
            }
            catch (DatabaseDoesNotExistException)
            {
                // create database
                store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(DbName)));
                Console.WriteLine($"Database '{DbName}' created!");
            }
        }
    }
}
