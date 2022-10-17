using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

public class BlobStorageExample
{
    public static async Task ProcessAsync()
    {
        try
        {
            // Copy the connection string from the portal in the variable below.
            //string storageConnectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=storageazblob;AccountKey=Fvr4AWlBtoylzOrH+BbLBJaw5BduqETL3w2sHyHUDY9BhK56sizgHyLxajd7pFaEYQfNrrObOwxZ+AStY+8FVQ==;EndpointSuffix=core.windows.net";

            // Create a client that can authenticate with a connection string
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);
            //Create a unique name for the container
            string containerName = "wtblob" + Guid.NewGuid().ToString();

            // Create the container and return a container client object
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
            Console.WriteLine("A container named '" + containerName + "' has been created. " +
                "\nTake a minute and verify in the portal." +
                "\nNext a file will be created and uploaded to the container.");
            Console.WriteLine("Press 'Enter' to continue.");
            Console.ReadLine();

            // Create a local file in the ./data/ directory for uploading and downloading
            string localPath = "./data/";
            string fileName = "wtfile" + Guid.NewGuid().ToString() + ".txt";
            string localFilePath = Path.Combine(localPath, fileName);

            // Write text to the file
            await File.WriteAllTextAsync(localFilePath, "Hello, World!");

            // Get a reference to the blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            // Open the file and upload its data
            using FileStream uploadFileStream = File.OpenRead(localFilePath);
            await blobClient.UploadAsync(uploadFileStream, true);

            Console.WriteLine("\nThe file was uploaded. We'll verify by listing" +
                    " the blobs next.");
            Console.WriteLine("Press 'Enter' to continue.");
            Console.ReadLine();

            // List blobs in the container
            Console.WriteLine("Listing blobs...");
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                Console.WriteLine("\t" + blobItem.Name);
            }

            Console.WriteLine("\nYou can also verify by looking inside the " +
                    "container in the portal." +
                    "\nNext the blob will be downloaded with an altered file name.");
            Console.WriteLine("Press 'Enter' to continue.");
            Console.ReadLine();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error -{ex.Message}");

        }
    }
}