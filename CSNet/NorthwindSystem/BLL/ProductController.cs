using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using NorthwindSystem.Data;
using NorthwindSystem.DAL;
using System.Data.SqlClient;  //needed  for SqlParameter class
#endregion

namespace NorthwindSystem.BLL
{
    public class ProductController
    {
        //lookup of data from the database using a non-primary key field
        public List<Product> Products_FindByCategory(int categoryid)
        {
            using (var context = new NorthwindContext())
            {
                //syntax
                //context.Database.SqlQuery<T>("sqlprocname [@parameterid[,@parameterid]]"
                //    [, new SqlParameter("parameterid", parametervalue)[,...]]);
                //context.Database.SqlQuery<T>("sqlprocname");  no parameters
                //context.Database.SqlQuery<T>("sqlprocname @parameterid"
                //    , new SqlParameter("parameterid", parametervalue)); one parameter
                //context.Database.SqlQuery<T>("sqlprocname @parameterid,@parameterid"
                //    , new SqlParameter("parameterid", parametervalue)
                //    , new SqlParameter("parameterid", parametervalue)); > 1 parameter

                //the parameterid is the spelling of the parameter name located on your sql procedure
                //the results from this query are returned as a datatype of:  IEnumerable<T>
                IEnumerable<Product> results =
                    context.Database.SqlQuery<Product>("Products_GetByCategories @CategoryID"
                            , new SqlParameter("CategoryID", categoryid));
                return results.ToList();
                
            }
        }

        public List<Product> Products_List()
        {
            using (var context = new NorthwindContext())
            {
                return context.Products.ToList();
            }
        }

        public Product Products_GetByID(int productid)
        {
            using (var context = new NorthwindContext())
            {
                return context.Products.Find(productid);
            }
        }

        public int Products_Add(Product item)
        {
            //at some point in time, your individual product fields
            //   must be placed in an instance of the class
            //this can be done on the web page or within this method

            //start a transaction
            using (var context = new NorthwindContext())
            {
                //Step one
                //Stage the data for execution by the commit statement
                //Staging is done in local memory
                //Staging DOES NOT create an identity value; this is done
                //   at commit time
                context.Products.Add(item);

                //commit your staged record to the database
                //if the commit command is successful, then the new
                //    identity value will exist in your data instance
                //if the commit command is NOT successful, the transaction
                //    is ROLLBACK
                context.SaveChanges();

                //optinally
                //you may decide to return the new identity value to the 
                //   web page
                //if you decide to return the value, then the method has a
                //   returndatatype of int; else the method should be using a
                //   returndatatype of void.
                return item.ProductID;

            }
        }
    }
}
