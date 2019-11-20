using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using NorthwindSystem.Data;
using NorthwindSystem.DAL;
#endregion

namespace NorthwindSystem.BLL
{
    public class CategoryController
    {
        public List<Category> Categories_List()
        {
            //connect to the context class that will handle the data request
            //most of CRUD requires using a transaction
            //To ensure that your data request is handled as a transaction
            //   we will encase all controller action within a transaction
            using (var context = new NorthwindContext())
            {
                //transaction code
                return context.Categories.ToList();
            }

        }
        public List<Category> Category_List()
        {
            //need to connect to the Context class
            //this connection will be done in a transaction coding group
            using (var context = new NorthwindContext())
            {
                //via EnityFrame, make a request for all records,
                //all attributes from the specified DbSet property
                return context.Categories.ToList();
            }
        }
    }
}
