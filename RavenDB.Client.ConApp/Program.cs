using Raven.Client.Documents;
using RavenDB.Client.Logic;

namespace RavenDB.Client.ConApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        private static void Main(string[] args)
        {
            
            using (IDocumentStore store = DocumentStoreHolder.Store)
            {

                string command;
                do
                {
                    Console.Clear();
                    DbHandler.DisplayMenu();

                    command = Console.ReadLine().ToUpper();
                    switch (command)
                    {
                        case "C":
                            DbHandler.Creation(store);
                            break;
                        case "G":
                            DbHandler.GetOwnerById(store);
                            break;
                        case "N":
                            DbHandler.QueryOwnersByName(store);
                            break;
                        case "P":
                            DbHandler.QueryOwnersByPetsName(store);
                            break;
                        case "R":
                            DbHandler.RenameOwnerById(store);
                            break;
                        case "D":
                            DbHandler.DeleteOwnerById(store);
                            break;
                        case "Q":
                            break;
                        default:
                            Console.WriteLine("Unknown command.");
                            break;
                    }
                } while (command != "Q");
            }
        }
    }
}