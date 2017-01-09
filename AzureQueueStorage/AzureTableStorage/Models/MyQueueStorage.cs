using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace AzureTableStorage.Models
{
    public class MyQueueStorage
    {
        private string Account { get; set; }
        private string Key { get; set; }

        public MyQueueStorage()
        {
            this.Account = "testaccounttoday";
            this.Key = "Z/ddM9f7mIl1bm9yVRnzmENUPbW6yg8o0zEMgRtK4Mn1iSau5y4xuwK7pe2z/nzzxhwBFt4OTBLeSBu7+hycAA==";
        }

        public void Create_a_Queue()
        {
            // Retrieve storage account from connection string.
            StorageCredentials Credentials = new StorageCredentials(this.Account, this.Key);
            CloudStorageAccount storageAccount = new CloudStorageAccount(Credentials, false);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Create the queue if it doesn't already exist
            queue.CreateIfNotExists();
        }

        public void Insert_a_message_into_a_queue()
        {
            // Retrieve storage account from connection string.
            StorageCredentials Credentials = new StorageCredentials(this.Account, this.Key);
            CloudStorageAccount storageAccount = new CloudStorageAccount(Credentials, false);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Create the queue if it doesn't already exist.
            queue.CreateIfNotExists();

            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage("Hello, World");
            queue.AddMessage(message);
        }

        public void Peek_at_the_next_message()
        {
            // Retrieve storage account from connection string
            StorageCredentials Credentials = new StorageCredentials(this.Account, this.Key);
            CloudStorageAccount storageAccount = new CloudStorageAccount(Credentials, false);

            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Peek at the next message
            CloudQueueMessage peekedMessage = queue.PeekMessage();

            // Display message.
            Console.WriteLine(peekedMessage.AsString);
        }

        public void Change_the_contents_of_a_queued_message()
        {
            // Retrieve storage account from connection string
            StorageCredentials Credentials = new StorageCredentials(this.Account, this.Key);
            CloudStorageAccount storageAccount = new CloudStorageAccount(Credentials, false);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Get the message from the queue and update the message contents.
            CloudQueueMessage message = queue.GetMessage();
            message.SetMessageContent("Updated contents.");
            queue.UpdateMessage(message,
                TimeSpan.FromSeconds(60.0),  // Make it visible for another 60 seconds.
                MessageUpdateFields.Content | MessageUpdateFields.Visibility);
        }

        public void Dequeue_the_next_message()
        {
            // Retrieve storage account from connection string
            StorageCredentials Credentials = new StorageCredentials(this.Account, this.Key);
            CloudStorageAccount storageAccount = new CloudStorageAccount(Credentials, false);

            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Get the next message
            CloudQueueMessage retrievedMessage = queue.GetMessage();
            
            //Process the message in less than 30 seconds, and then delete the message
            if (retrievedMessage!=null)
                queue.DeleteMessage(retrievedMessage);
        }

        public async void Use_async_wait_pattern()
        {
            // Retrieve storage account from connection string
            StorageCredentials Credentials = new StorageCredentials(this.Account, this.Key);
            CloudStorageAccount storageAccount = new CloudStorageAccount(Credentials, false);

            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Create the queue if it doesn't already exist
            if (await queue.CreateIfNotExistsAsync())
            {
                Console.WriteLine("Queue '{0}' Created", queue.Name);
            }
            else
            {
                Console.WriteLine("Queue '{0}' Exists", queue.Name);
            }

            // Create a message to put in the queue
            CloudQueueMessage cloudQueueMessage = new CloudQueueMessage("My message");

            // Async enqueue the message
            await queue.AddMessageAsync(cloudQueueMessage);
            Console.WriteLine("Message added");

            // Async dequeue the message
            CloudQueueMessage retrievedMessage = await queue.GetMessageAsync();
            Console.WriteLine("Retrieved message with content '{0}'", retrievedMessage.AsString);

            // Async delete the message
            await queue.DeleteMessageAsync(retrievedMessage);
            Console.WriteLine("Deleted message");
        }

        public void Leverage_additional_options_for_dequeuing_messages()
        {
            // Retrieve storage account from connection string
            StorageCredentials Credentials = new StorageCredentials(this.Account, this.Key);
            CloudStorageAccount storageAccount = new CloudStorageAccount(Credentials, false);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            foreach (CloudQueueMessage message in queue.GetMessages(20, TimeSpan.FromMinutes(5)))
            {
                // Process all messages in less than 5 minutes, deleting each message after processing.
                queue.DeleteMessage(message);
            }
        }

        public void Get_the_queue_length()
        {
            // Retrieve storage account from connection string
            StorageCredentials Credentials = new StorageCredentials(this.Account, this.Key);
            CloudStorageAccount storageAccount = new CloudStorageAccount(Credentials, false);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Fetch the queue attributes.
            queue.FetchAttributes();

            // Retrieve the cached approximate message count.
            int? cachedMessageCount = queue.ApproximateMessageCount;

            // Display number of messages.
            Console.WriteLine("Number of messages in queue: " + cachedMessageCount);
        }

        public void Delete_a_queue()
        {
            // Retrieve storage account from connection string.
            StorageCredentials Credentials = new StorageCredentials(this.Account, this.Key);
            CloudStorageAccount storageAccount = new CloudStorageAccount(Credentials, false);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Delete the queue.
            queue.Delete();
        }
    }
}