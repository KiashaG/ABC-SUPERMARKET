using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABCSupermarkertTask2.Models;
using ABCSupermarkertTask2.TableHandler;
using ABCSupermarkertTask2.BlobHandler;
using System.Windows;

namespace ABCSupermarkertTask2.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(string id)
        {

            // for the edit
            if (!string.IsNullOrEmpty(id))
            {
                // set the name of the table for table storage
                TableManager TableManagerObj = new TableManager("products");

                // retireve the product object that matches the row key
                List<Products> ProductsListObj = TableManagerObj.RetrieveEntity<Products>("RowKey eq'" + id + "'");
                Products ProductsObj = ProductsListObj.FirstOrDefault();
                
                return View(ProductsObj);
                

            }


            return View(new Products());
        }
        // for insert products
        [HttpPost]
        public ActionResult Index(string id, HttpPostedFileBase uploadFile, FormCollection formData)
        {
            

            Products ProductsObj = new Products();
            ProductsObj.ProductName = formData["ProductName"] == "" ? null : formData["ProductName"];
            ProductsObj.ProductDescription = formData["ProductDescription"] == "" ? null : formData["ProductDescription"];
            double ProductPrice;
            if (double.TryParse(formData["ProductPrice"], out ProductPrice))
            {
                ProductsObj.ProductPrice = double.Parse(formData["ProductPrice"] == "" ? null : formData["ProductPrice"]);

            }
            else
            {
            
                return View(new Products());
            }

            // upload blob(product picture)
            foreach (string file in Request.Files)
            {
                uploadFile = Request.Files[file];
            }

            // create blob container
            BlobManager BlobManagerObj = new BlobManager("productpictures");
                string fileAbsoluteUri = BlobManagerObj.UploadFile(uploadFile);

            ProductsObj.FilePath = fileAbsoluteUri.ToString(); // fix this

            // Insert into table
            if (string.IsNullOrEmpty(id))
            {
               
                ProductsObj.PartitionKey = ProductsObj.ProductName;
                ProductsObj.RowKey = Guid.NewGuid().ToString();

                TableManager TableManagerObj = new TableManager("products");
                TableManagerObj.InsertEntity<Products>(ProductsObj, true);
            }
            // Update
            else
            {
              
                ProductsObj.PartitionKey = ProductsObj.ProductName;
                ProductsObj.RowKey = id;

                TableManager TableManagerObj = new TableManager("products");
                TableManagerObj.InsertEntity<Products>(ProductsObj, false);
            }
            return RedirectToAction("Get");

        }

        // get products (retrieve)
        public ActionResult Get()
        {
            TableManager tableManagerObj = new TableManager("products");
            List<Products> ProductObj = tableManagerObj.RetrieveEntity<Products>(null);

            return View(ProductObj);
        }

        // delete products 
        public ActionResult Delete(string id)
        {
            // retrieve product to be deleted
            TableManager TableManagerObj = new TableManager("products");
            List<Products> ProductListObj = TableManagerObj.RetrieveEntity<Products>("RowKey eq'" + id + "'");

            Products ProductsObj = ProductListObj.FirstOrDefault();

            // Delete the product
            TableManagerObj.DeleteEntity<Products>(ProductsObj);
            return RedirectToAction("Get");


        
        }
    }

   
}