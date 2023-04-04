using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Controls;


namespace ABCSupermarkertTask2.TableHandler
{
    public class TableManager
    {
        // connect to storage account
        // table name pulled from controller so this file simply accpets return values
        private CloudTable table;

        public TableManager(string _CloudTablename)
        {
            // check if table name is null or empty
            if (string.IsNullOrEmpty (_CloudTablename))
            {
                throw new ArgumentNullException("Table","Table name cannot be empty");
            }
            try
            {
                // get azure storage account connection string
              
                string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=abcsupermarketsakg;AccountKey=ZcdVeZpTZRxEll9LpvHn0oDbomBuFU2XrxbV1hyiG2R9Jo3JGJt8O8Cao+sBIB3VY6mI9kPHnJlFIZSyHJosjw==;EndpointSuffix=core.windows.net";
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

                // create table if does not exists
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                table = tableClient.GetTableReference(_CloudTablename);
                table.CreateIfNotExists();


            }

            catch(StorageException StorageExceptionObj)
            {
                throw StorageExceptionObj;

            }
            catch(Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }

        // CRUD METHODS
        // retrieve products list RETRIEVE from table storage , R
        public List<T> RetrieveEntity<T> (String Query = null) where T:TableEntity, new()
        {
            try
            {
                TableQuery<T> DataTableQuery = new TableQuery<T>();
                if (!string.IsNullOrEmpty(Query))
                {
                    DataTableQuery = new TableQuery<T>().Where(Query);
                }
                IEnumerable<T> IDataList = table.ExecuteQuery(DataTableQuery);
                List<T> DataList = new List<T>();
                foreach (var singleData in IDataList)
                 DataList.Add(singleData);
                 return DataList;
                

            }
            catch(Exception ExceptionObj) {
                throw ExceptionObj;
            }
        }
        // INSERT AND UPDATE PRODUCTS FROM TABLE STORAGE C, U
        public void InsertEntity<T> (T entity, bool forInsert = true) where T : TableEntity , new() 
        {
            try
            {
                if (forInsert)
                {
                    var InsertOperation = TableOperation.Insert(entity);
                    table.Execute(InsertOperation);
                    
                }
                //update
                else
                {
                    var InsertOrReplaceOperation = TableOperation.InsertOrReplace(entity);
                    table.Execute(InsertOrReplaceOperation);
                }
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }


        // DELETE PRODUCT FROM TABLE STORAGE , D
        public bool DeleteEntity<T> (T entity) where T : TableEntity, new()
        {
            try
            {
                var DeleteOperation = TableOperation.Delete(entity);
                table.Execute(DeleteOperation);
                return true;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }
      


    }
}