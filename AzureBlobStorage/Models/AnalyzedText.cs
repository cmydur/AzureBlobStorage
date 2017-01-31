using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureBlobStorage.Models
{
    public class AnalyzedText : TableEntity
    { 
        public string Text { get; set; }

    }
}
