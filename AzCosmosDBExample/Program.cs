using Microsoft.Azure.Cosmos;
using documentClient=Microsoft.Azure.Documents.Client;

namespace AzCosmosDBExample
{
    public class Program
    {
        // Replace <documentEndpoint> with the information created earlier
        private static readonly string EndpointUri = "deletedEndPoint";

        // Set variable to the Primary Key from earlier.
        private static readonly string PrimaryKey = "deletedKey";

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;

        // The names of the database and container we will create
        private string databaseId = "az204Database";
        private string containerId = "az204Container";

        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Beginning operations...\n");
                Program p = new Program();
                //await p.CosmosAsync();
                await p.ExecuteStoreProcedure();
            }
            catch (CosmosException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
            finally
            {
                Console.WriteLine("End of program, press any key to exit.");
                Console.ReadKey();
            }
        }
        //The sample code below gets added below this line

        public async Task CosmosAsync()
        {
            // Create a new instance of the Cosmos Client
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);

            // Runs the CreateDatabaseAsync method
            await this.CreateDatabaseAsync();

            // Run the CreateContainerAsync method
            await this.CreateContainerAsync();
        }

        private async Task CreateDatabaseAsync()
        {
            // Create a new database using the cosmosClient
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Console.WriteLine("Created Database: {0}\n", this.database.Id);
        }

        private async Task CreateContainerAsync()
        {
            // Create a new container
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/LastName");
            Console.WriteLine("Created Container: {0}\n", this.container.Id);
        }

        public async Task ExecuteStoreProcedure()
        {
            //Execute Store Procedure  
            dynamic[] newItems = new dynamic[]
{
    new {
        category = "Personal",
        name = "Groceries",
        description = "Pick up strawberries",
        isComplete = false
    },
    new {
        category = "Personal",
        name = "Doctor",
        description = "Make appointment for check up",
        isComplete = false
    }
};
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);

            var responseMessage=await this.cosmosClient.GetContainer("az204Database", "az204Container").Scripts.
                    ExecuteStoredProcedureStreamAsync("spCreateNewDocument",new PartitionKey("myPartition"), new[] {newItems});
        }
    }
}