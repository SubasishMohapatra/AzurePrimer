// See https://aka.ms/new-console-template for more information
using System;

Console.WriteLine("Azure Blob Storage exercise\n");

// Run the examples asynchronously, wait for the results before proceeding
BlobStorageExample.ProcessAsync().GetAwaiter().GetResult();

Console.WriteLine("Press enter to exit the sample application.");
Console.ReadLine();
