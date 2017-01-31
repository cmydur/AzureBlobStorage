using AzureBlobStorage.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace AzureBlobStorage
{
    class AzureTableStorage
    {

        protected CloudTableClient GetContainer()
        {
            var account = CloudStorageAccount.Parse(Constants.StorageConnection);
            return account.CreateCloudTableClient();

        }


        public string GetAnalyzedText(string name)
        {

            CloudTableClient tableClient = GetContainer();


            // Create the CloudTable object that represents the "quotes" table. 
            CloudTable table = tableClient.GetTableReference(ContainerName.analyzedText.ToString());
            

            List<string> t = new List<string>();
            TableOperation to = TableOperation.Retrieve<AnalyzedText>("Image", name);
            

            AnalyzedText tr = (AnalyzedText)table.ExecuteAsync(to).Result.Result;
            if (tr != null)
            {
                return tr.Text;
            }
            else
            {
                return "";
            }
        }
    }
}
