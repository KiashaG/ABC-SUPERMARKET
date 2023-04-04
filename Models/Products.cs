using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Table;


namespace ABCSupermarkertTask2.Models
{
    public class Products : TableEntity
    {
        public Products() { }

        public string FilePath { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public double ProductPrice { get; set; }
    }
}