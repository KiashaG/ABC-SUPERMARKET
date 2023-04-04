using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Microsoft.Azure.Cosmos;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Windows;
//using Azure.Storage.Blobs;
//using Microsoft.Azure.Cosmos.Table;

namespace ABCSupermarkertTask2.BlobHandler
{
    public class BlobManager
    {
        // connect to storage account
        // container name pulled from controller so this file simply accpets return values

        private CloudBlobContainer blobContainer;

        public BlobManager(string ContainerName)
        {
            // check if conatiner name is null or empty
            if (string.IsNullOrEmpty(ContainerName))
            {
                throw new ArgumentNullException("Container", "Container name cannot be empty");
            }
            try
            {
                // get azure storage account connection string

                string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=abcsupermarketsakg;AccountKey=ZcdVeZpTZRxEll9LpvHn0oDbomBuFU2XrxbV1hyiG2R9Jo3JGJt8O8Cao+sBIB3VY6mI9kPHnJlFIZSyHJosjw==;EndpointSuffix=core.windows.net";
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

                // create the blob client
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                blobContainer = cloudBlobClient.GetContainerReference(ContainerName);

                // create container if does not exists
                if (blobContainer.CreateIfNotExists())
                {
                    blobContainer.SetPermissions(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        });
                }

            }

            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }
    

        // upload method to insert and or update from blob storage
        public string UploadFile(HttpPostedFileBase FileToUpload)
        {
            string AbsoluteUri;
            // check if file base is null or empty
            if (FileToUpload == null|| FileToUpload.ContentLength == 0)
           {
               
               return null;
           }
            try
            {
                // get the file to upload file name
                string FileName = Path.GetFileName(FileToUpload.FileName);

                // create a block blob
                CloudBlockBlob blockblob;
                blockblob = blobContainer.GetBlockBlobReference(FileName);

                // set the blob content type
                blockblob.Properties.ContentType = FileToUpload.ContentType;

                // upload the blob
                blockblob.UploadFromStream(FileToUpload.InputStream);

                // get the uri
                AbsoluteUri = blockblob.Uri.AbsoluteUri;


            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }

            return AbsoluteUri;
        }

        // retrieve blob products
        public List<string> BlobList()
        {
            List<string> _blobList = new List<string>();
            foreach(IListBlobItem item in blobContainer.ListBlobs())
            {
                if(item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob _blobpage = (CloudBlockBlob)item;
                    _blobList.Add(_blobpage.Uri.AbsoluteUri.ToString());
                        
                }
            }
            return _blobList;
        }

        // delete blob
        public bool DeleteBlob(string AbsoluteUri)
        {
            try
            {
                Uri uriObj = new Uri(AbsoluteUri);
                string BlobName = Path.GetFileName(uriObj.LocalPath);

                // get reference to block blob
                CloudBlockBlob blockblob = blobContainer.GetBlockBlobReference(BlobName);

                // delete the blob
                blockblob.Delete();
                return true;

            }
            catch(Exception ExceptionObj)
            {
                throw ExceptionObj;
            }

        }
    }
}