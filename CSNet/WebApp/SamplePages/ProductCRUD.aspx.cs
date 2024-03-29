﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using NorthwindSystem.BLL;
using NorthwindSystem.Data;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
#endregion

namespace WebApp.NorthwindPages
{
    public partial class ProductCRUD : System.Web.UI.Page
    {
        //this collection is used by the specialized error handling
        List<string> errormsgs = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //the control being use for error handling display of messages
            //    is a DataList.
            //The clearing of the controls is done by assigning null as
            //   the collection and binding that empty collection to the
            //   control
            Message.DataSource = null;
            Message.DataBind();

            if (!Page.IsPostBack)
            {
                BindProductList();
                //BindCategoryList();
                //BindSupplierList();
            }
        }

        //use this method to discover the inner most error message.
        //this rotuing has been created by the user
        protected Exception GetInnerException(Exception ex)
        {
            //drill down to the inner most exception
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }

        //use this method to load a DataList with a variable
        //number of message lines.
        //each line is a string
        //the strings (lines) are passed to this routine in
        //   a List<string>
        //second parameter is the bootstrap cssclass
        protected void LoadMessageDisplay(List<string> errormsglist, string cssclass)
        {
            Message.CssClass = cssclass;
            Message.DataSource = errormsglist;
            Message.DataBind();
        }

        #region DropDown Loads
        //protected void BindCategoryList()
        //{
        //    //this will be a standard lookup for the Category records
        //    try
        //    {
        //        CategoryController sysmgr = new CategoryController();
        //        List<Category> info = null;
        //        info = sysmgr.Categories_List();
        //        info.Sort((x, y) => x.CategoryName.CompareTo(y.CategoryName));
        //        CategoryList.DataSource = info;
        //        CategoryList.DataTextField = nameof(Category.CategoryName);
        //        CategoryList.DataValueField = nameof(Category.CategoryID);
        //        CategoryList.DataBind();
        //        CategoryList.Items.Insert(0, "select...");
        //    }
        //    catch (Exception ex)
        //    {
        //        //using the specialized error handling DataList control
        //        errormsgs.Add(GetInnerException(ex).ToString());
        //        LoadMessageDisplay(errormsgs, "alert alert-danger");
        //    }
        //}
        //protected void BindSupplierList()
        //{
        //    //this will be a standard lookup for the Supplier records
        //    try
        //    {
        //        SupplierController sysmgr = new SupplierController();
        //        List<Supplier> info = null;
        //        info = sysmgr.Supplier_List();
        //        info.Sort((x, y) => x.ContactName.CompareTo(y.ContactName));
        //        SupplierList.DataSource = info;
        //        SupplierList.DataTextField = nameof(Supplier.ContactName);
        //        SupplierList.DataValueField = nameof(Supplier.SupplierID);
        //        SupplierList.DataBind();
        //        SupplierList.Items.Insert(0, "select...");
        //    }
        //    catch (Exception ex)
        //    {
        //        //using the specialized error handling DataList control
        //        errormsgs.Add(GetInnerException(ex).ToString());
        //        LoadMessageDisplay(errormsgs, "alert alert-danger");
        //    }
        //}
        protected void BindProductList()
        {
            //this will be a standard lookup for the Product records
            try
            {
                ProductController sysmgr = new ProductController();
                List<Product> info = null;
                info = sysmgr.Products_List();
                info.Sort((x, y) => x.ProductName.CompareTo(y.ProductName));
                ProductList.DataSource = info;
                ProductList.DataTextField = nameof(Product.ProductName);
                ProductList.DataValueField = nameof(Product.ProductID);
                ProductList.DataBind();
                ProductList.Items.Insert(0, "select...");
            }
            catch (Exception ex)
            {
                //using the specialized error handling DataList control
                errormsgs.Add(GetInnerException(ex).ToString());
                LoadMessageDisplay(errormsgs, "alert alert-danger");
            }
        }

        #endregion

        protected void Clear_Click(object sender, EventArgs e)
        {
            ProductID.Text = "";
            ProductName.Text = "";
            QuantityPerUnit.Text = "";
            UnitPrice.Text = "";
            UnitsInStock.Text = "";
            UnitsOnOrder.Text = "";
            ReorderLevel.Text = "";
            Discontinued.Checked = false;
            ProductList.SelectedIndex = 0;
            CategoryList.SelectedIndex = 0;
            SupplierList.SelectedIndex = 0;
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            //standard query
            if (ProductList.SelectedIndex == 0)
            {
                errormsgs.Add("Select a product to maintain");
                LoadMessageDisplay(errormsgs, "alert alert-info");
            }
            else
            {
                //standard lookup
                try
                {
                    ProductController sysmgr = new ProductController();
                    Product info = null;
                    info = sysmgr.Products_GetByID(int.Parse(ProductList.SelectedValue));
                    if (info == null)
                    {
                        errormsgs.Add("Product no longer on file.");
                        LoadMessageDisplay(errormsgs, "alert alert-info");
                        //optionally
                        Clear_Click(sender, e);
                        //refresh the ProductList control
                        BindProductList();
                    }
                    else
                    {
                        ProductID.Text = info.ProductID.ToString();
                        ProductName.Text = info.ProductName;
                        QuantityPerUnit.Text = 
                            info.QuantityPerUnit == null ? "" : info.QuantityPerUnit;
                        UnitPrice.Text =
                            info.UnitPrice.HasValue ?  string.Format("{0:0.00}", info.UnitPrice.Value) : "";
                        UnitsInStock.Text =
                            info.UnitsInStock.HasValue ? info.UnitsInStock.Value.ToString() : "";
                        UnitsOnOrder.Text =
                            info.UnitsOnOrder.HasValue ? info.UnitsOnOrder.Value.ToString() : "";
                        ReorderLevel.Text =
                            info.ReorderLevel.HasValue ? info.ReorderLevel.Value.ToString() : "";
                        Discontinued.Checked = info.Discontinued;
                        if (info.CategoryID.HasValue)
                        {
                            CategoryList.SelectedValue = info.CategoryID.Value.ToString();
                        }
                        else
                        {
                            CategoryList.SelectedIndex = 0;
                        }
                        if (info.SupplierID.HasValue)
                        {
                            SupplierList.SelectedValue = info.SupplierID.Value.ToString();
                        }
                        else
                        {
                            SupplierList.SelectedIndex = 0;
                        }
                    }
                   

                }
                catch (Exception ex)
                {
                    errormsgs.Add(GetInnerException(ex).ToString());
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }
            }
        }

        protected void AddProduct_Click(object sender, EventArgs e)
        {
            //check the page validation
            if (Page.IsValid)
            {
                //event code validation that was not accomplished on 
                //   the web form
                //example
                // Assume that the CategoryID is required
                if (CategoryList.SelectedIndex == 0)
                {
                    errormsgs.Add("Category is required.");
                }
                if (QuantityPerUnit.Text.Length > 20)
                {
                    errormsgs.Add("Quantity per Unit is limited to 20 characters");
                }

                //check if click event validation is good
                if (errormsgs.Count > 0)
                {
                    LoadMessageDisplay(errormsgs, "alert alert-info");
                }
                else
                {
                    //assume that the data is validation to our knowledge
                    try
                    {
                        //standard add to a database
                        //connect to the appropriate controller 
                        ProductController sysmgr = new ProductController();
                        //create and load an instance of the entity record
                        //  since there was no constructor placed in the
                        //  entity, when one creates the instance the
                        //  default system constructor will be used
                        Product item = new Product();
                        //what about ProductiD??
                        //   since ProductID is an identity field it does NOT
                        //   need to be loaded into the new instance
                        item.ProductName = ProductName.Text.Trim();
                        if (CategoryList.SelectedIndex == 0)
                        {
                            item.CategoryID = null;
                        }
                        else
                        {
                            item.CategoryID = int.Parse(CategoryList.SelectedValue);
                        }
                        if (SupplierList.SelectedIndex == 0)
                        {
                            item.SupplierID = null;
                        }
                        else
                        {
                            item.SupplierID = int.Parse(SupplierList.SelectedValue);
                        }
                        item.QuantityPerUnit =
                            string.IsNullOrEmpty(QuantityPerUnit.Text) ? null : QuantityPerUnit.Text;
                        if (string.IsNullOrEmpty(UnitPrice.Text))
                        {
                            item.UnitPrice = null;
                        }
                        else
                        {
                            item.UnitPrice = decimal.Parse(UnitPrice.Text);
                        }
                        if (string.IsNullOrEmpty(UnitsInStock.Text))
                        {
                            item.UnitsInStock = null;
                        }
                        else
                        {
                            item.UnitsInStock = Int16.Parse(UnitsInStock.Text);
                        }
                        if (string.IsNullOrEmpty(UnitsOnOrder.Text))
                        {
                            item.UnitsOnOrder = null;
                        }
                        else
                        {
                            item.UnitsOnOrder = Int16.Parse(UnitsOnOrder.Text);
                        }
                        if (string.IsNullOrEmpty(ReorderLevel.Text))
                        {
                            item.ReorderLevel = null;
                        }
                        else
                        {
                            item.ReorderLevel = Int16.Parse(ReorderLevel.Text);
                        }
                        //what about Discontinue??
                        item.Discontinued = false;
                        //issue the BLL call
                        int newProductID = sysmgr.Products_Add(item);
                        //give feedback
                        //if you get to execute the following code, it means
                        //   that the product has been successfully added
                        //   to the database
                        ProductID.Text = newProductID.ToString();
                        errormsgs.Add("Product has been added");
                        LoadMessageDisplay(errormsgs, "alert alert-success");
                        //is there any other controls on the form that need to be refreshed
                        BindProductList();  //by default, list will be at index 0
                        ProductList.SelectedValue = ProductID.Text;

                    }
                    catch (DbUpdateException ex)
                    {
                        UpdateException updateException = (UpdateException)ex.InnerException;
                        if (updateException.InnerException != null)
                        {
                            errormsgs.Add(updateException.InnerException.Message.ToString());
                        }
                        else
                        {
                            errormsgs.Add(updateException.Message);
                        }
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                errormsgs.Add(validationError.ErrorMessage);
                            }
                        }
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }

                    catch (Exception ex)
                    {
                        errormsgs.Add(GetInnerException(ex).ToString());
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                }
                
            }
        }

        protected void UpdateProduct_Click(object sender, EventArgs e)
        {
            //check the page validation
            if (Page.IsValid)
            {
                //event code validation that was not accomplished on 
                //   the web form
                //example
                // Assume that the CategoryID is required
                if (CategoryList.SelectedIndex == 0)
                {
                    errormsgs.Add("Category is required.");
                }
                if (QuantityPerUnit.Text.Length > 20)
                {
                    errormsgs.Add("Quantity per Unit is limited to 20 characters");
                }

                //on update, ensure you have your primary key value
                int productid = 0;
                if (string.IsNullOrEmpty(ProductID.Text))
                {
                    errormsgs.Add("Search for a product to update");
                }
                else if (!int.TryParse(ProductID.Text, out productid))
                {
                    errormsgs.Add("Product id is invalid");
                }
                else if (productid < 1)
                {
                    errormsgs.Add("Product id is invalid");
                }

                //check if click event validation is good
                if (errormsgs.Count > 0)
                {
                    LoadMessageDisplay(errormsgs, "alert alert-info");
                }
                else
                {
                    //assume that the data is validation to our knowledge
                    try
                    {
                        //standard update to a database
                        //connect to the appropriate controller 
                        ProductController sysmgr = new ProductController();
                        //create and load an instance of the entity record
                        //  since there was no constructor placed in the
                        //  entity, when one creates the instance the
                        //  default system constructor will be used
                        Product item = new Product();
                        //ensure you include the primary key
                        item.ProductID = productid;
                        item.ProductName = ProductName.Text.Trim();
                        if (CategoryList.SelectedIndex == 0)
                        {
                            item.CategoryID = null;
                        }
                        else
                        {
                            item.CategoryID = int.Parse(CategoryList.SelectedValue);
                        }
                        if (SupplierList.SelectedIndex == 0)
                        {
                            item.SupplierID = null;
                        }
                        else
                        {
                            item.SupplierID = int.Parse(SupplierList.SelectedValue);
                        }
                        item.QuantityPerUnit =
                            string.IsNullOrEmpty(QuantityPerUnit.Text) ? null : QuantityPerUnit.Text;
                        if (string.IsNullOrEmpty(UnitPrice.Text))
                        {
                            item.UnitPrice = null;
                        }
                        else
                        {
                            item.UnitPrice = decimal.Parse(UnitPrice.Text);
                        }
                        if (string.IsNullOrEmpty(UnitsInStock.Text))
                        {
                            item.UnitsInStock = null;
                        }
                        else
                        {
                            item.UnitsInStock = Int16.Parse(UnitsInStock.Text);
                        }
                        if (string.IsNullOrEmpty(UnitsOnOrder.Text))
                        {
                            item.UnitsOnOrder = null;
                        }
                        else
                        {
                            item.UnitsOnOrder = Int16.Parse(UnitsOnOrder.Text);
                        }
                        if (string.IsNullOrEmpty(ReorderLevel.Text))
                        {
                            item.ReorderLevel = null;
                        }
                        else
                        {
                            item.ReorderLevel = Int16.Parse(ReorderLevel.Text);
                        }
                        //actually current value of Discontinued
                        item.Discontinued = Discontinued.Checked;
                        //issue the BLL call
                        int rowsaffected = sysmgr.Products_Update(item);
                        //give feedback
                        if (rowsaffected > 0)
                        {
                            errormsgs.Add("Product has been updated");
                            LoadMessageDisplay(errormsgs, "alert alert-success");
                            //is there any other controls on the form that need to be refreshed
                            BindProductList();  //by default, list will be at index 0
                            ProductList.SelectedValue = ProductID.Text;
                        }
                        else
                        {
                            errormsgs.Add("Product has not been updated. Product not found");
                            LoadMessageDisplay(errormsgs, "alert alert-info");
                            //is there any other controls on the form that need to be refreshed
                            BindProductList();  //by default, list will be at index 0

                            //optionally you could clear your fields
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        UpdateException updateException = (UpdateException)ex.InnerException;
                        if (updateException.InnerException != null)
                        {
                            errormsgs.Add(updateException.InnerException.Message.ToString());
                        }
                        else
                        {
                            errormsgs.Add(updateException.Message);
                        }
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                errormsgs.Add(validationError.ErrorMessage);
                            }
                        }
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }

                    catch (Exception ex)
                    {
                        errormsgs.Add(GetInnerException(ex).ToString());
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                }

            }
        }

        protected void RemoveProduct_Click(object sender, EventArgs e)
        {
            //on delete, ensure you have your primary key value
            int productid = 0;
            if (string.IsNullOrEmpty(ProductID.Text))
            {
                errormsgs.Add("Search for a product to update");
            }
            else if (!int.TryParse(ProductID.Text, out productid))
            {
                errormsgs.Add("Product id is invalid");
            }
            else if (productid < 1)
            {
                errormsgs.Add("Product id is invalid");
            }

            //check if click event validation is good
            if (errormsgs.Count > 0)
            {
                LoadMessageDisplay(errormsgs, "alert alert-info");
            }
            else
            {
                //assume that the data is validation to our knowledge
                try
                {
                    //standard delete to a database
                    //connect to the appropriate controller 
                    ProductController sysmgr = new ProductController();
                    //create and load an instance of the entity record
                    //  since there was no constructor placed in the
                    //  entity, when one creates the instance the
                    //  default system constructor will be used
                        
                    //issue the BLL call
                    int rowsaffected = sysmgr.Products_Delete(productid);
                    //give feedback
                    if (rowsaffected > 0)
                    {
                        errormsgs.Add("Product has been discontinued");
                        LoadMessageDisplay(errormsgs, "alert alert-success");
                        //is there any other controls on the form that need to be refreshed
                        BindProductList();  //by default, list will be at index 0
                        ProductList.SelectedValue = ProductID.Text;
                    Discontinued.Checked = true;
                    }
                    else
                    {
                        errormsgs.Add("Product has not been discontinued. Product not found");
                        LoadMessageDisplay(errormsgs, "alert alert-warning");
                        //is there any other controls on the form that need to be refreshed
                        BindProductList();  //by default, list will be at index 0

                        //optionally you could clear your fields
                    }
                }
                catch (DbUpdateException ex)
                {
                    UpdateException updateException = (UpdateException)ex.InnerException;
                    if (updateException.InnerException != null)
                    {
                        errormsgs.Add(updateException.InnerException.Message.ToString());
                    }
                    else
                    {
                        errormsgs.Add(updateException.Message);
                    }
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            errormsgs.Add(validationError.ErrorMessage);
                        }
                    }
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }

                catch (Exception ex)
                {
                    errormsgs.Add(GetInnerException(ex).ToString());
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }
            }

        }

    }
}