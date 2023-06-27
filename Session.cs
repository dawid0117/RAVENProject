using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;

namespace BookingManagement___Projekt_zaliczeniowy
{
    public class Session
    {
        private static Lazy<IDocumentStore> store = new Lazy<IDocumentStore>(CreateStore);

        public static IDocumentStore Store => store.Value;


        private static IDocumentStore CreateStore()
        {            
            IDocumentStore store = new DocumentStore()
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "BookingManagement"
            }.Initialize();

            return store;
        }
    }
}
