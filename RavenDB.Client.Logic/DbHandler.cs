using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Session;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using RavenDB.Client.Contracts;

namespace RavenDB.Client.Logic
{
    public class DbHandler
    {
        // Returns the entered string if it is not empty, otherwise, keeps asking for it.
        private static string ReadNotEmptyString(string message)
        {
            Console.WriteLine(message);
            string res;
            do
            {
                res = Console.ReadLine().Trim();
                if (res == string.Empty)
                {
                    Console.WriteLine("Entered value cannot be empty.");
                }
            } while (res == string.Empty);

            return res;
        }

        // Will use this to prevent text from being cleared before we've read it.
        private static void PressAnyKeyToContinue()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        // Prepends the 'owners/' prefix to the id if it is not present (more on it later)
        private static string NormalizeOwnerId(string id)
        {
            if (!id.ToLower().StartsWith("owners/"))
            {
                id = "owners/" + id;
            }

            return id;
        }

        // Displays the menu
        public static void DisplayMenu()
        {
            Console.WriteLine("Select a command");
            Console.WriteLine("C - Create an owner with pets");
            Console.WriteLine("G - Get an owner with its pets by Owner Id");
            Console.WriteLine("N - Query owners whose name starts with...");
            Console.WriteLine("P - Query owners who have a pet whose name starts with...");
            Console.WriteLine("R - Rename an owner by Id");
            Console.WriteLine("D - Delete an owner by Id");
            Console.WriteLine();
        }
        // Creation
        private static Owner CreateOwner()
        {
            string name = ReadNotEmptyString("Enter the owner's name.");

            return new Owner { Name = name };
        }

        private static Pet CreatePet()
        {
            string name = ReadNotEmptyString("Enter the name of the pet.");
            string race = ReadNotEmptyString("Enter the race of the pet.");
            string color = ReadNotEmptyString("Enter the color of the pet.");

            return new Pet
            {
                Color = color,
                Race = race,
                Name = name
            };
        }

        public static void Creation(IDocumentStore store)
        {
            // create owner entity
            Owner owner = CreateOwner();
            Console.WriteLine(
                "Do you want to create a pet and assign it to {0}? (Y/y: yes, anything else: no)",
                owner.Name);

            bool createPets = Console.ReadLine().ToLower() == "y";
            while (createPets)
            {
                // create Pet Entity and add to owner's pet list
                owner.Pets.Add(CreatePet());

                Console.WriteLine("Do you want to create a pet and assign it to {0}?", owner.Name);
                createPets = Console.ReadLine().ToLower() == "y";
            }

            using (IDocumentSession session = store.OpenSession())
            {
                session.Store(owner);
                session.SaveChanges();
            }
        }
        

        // Retrieval by Id
        public static void GetOwnerById(IDocumentStore store)
        {
            Owner owner;
            string id = NormalizeOwnerId(ReadNotEmptyString("Enter the Id of the owner to display."));

            using (IDocumentSession session = store.OpenSession())
            {
                owner = session.Load<Owner>(id);
            }

            if (owner == null)
            {
                Console.WriteLine("Owner not found.");
            }
            else
            {
                Console.WriteLine(owner);
            }

            PressAnyKeyToContinue();
        }

        // Query 1
        public static void QueryOwnersByName(IDocumentStore store)
        {
            string namePart = ReadNotEmptyString("Enter a name to filter by.");

            List<Owner> result;
            using (IDocumentSession session = store.OpenSession())
            {
                result = session.Query<Owner>()
                   .Where(ow => ow.Name.StartsWith(namePart))
                   .Take(10)
                   .ToList();
            }

            if (result.Count > 0)
            {
                result.ForEach(ow => Console.WriteLine(ow));
            }
            else
            {
                Console.WriteLine("No matches.");
            }
            PressAnyKeyToContinue();
        }

        // Query 2
        public static void QueryOwnersByPetsName(IDocumentStore store)
        {
            string namePart = ReadNotEmptyString("Enter a name to filter by.");

            List<Owner> result;
            using (IDocumentSession session = store.OpenSession())
            {
                result = session.Query<Owner>()
                    .Where(ow => ow.Pets.Any(p => p.Name.StartsWith(namePart)))
                    .Take(10)
                    .ToList();
            }

            if (result.Count > 0)
            {
                result.ForEach(ow => Console.WriteLine(ow));
            }
            else
            {
                Console.WriteLine("No matches.");
            }
            PressAnyKeyToContinue();
        }

        // Deletion
        public static void DeleteOwnerById(IDocumentStore store)
        {
            string id = NormalizeOwnerId(ReadNotEmptyString("Enter the Id of the owner to delete."));

            using (IDocumentSession session = store.OpenSession())
            {
                session.Delete(id);
                session.SaveChanges();
            }
        }
        // Updating
        public static void RenameOwnerById(IDocumentStore store)
        {
            string id = NormalizeOwnerId(ReadNotEmptyString("Enter the Id of the owner to rename."));
            string newName = ReadNotEmptyString("Enter the new name.");

            using (IDocumentSession session = store.OpenSession())
            {
                // Load a document
                Owner owner = session.Load<Owner>(id);

                // Update the company's type
                owner.Name = newName;

                // Apply changes
                session.SaveChanges();
            }

            
        }
    }


}