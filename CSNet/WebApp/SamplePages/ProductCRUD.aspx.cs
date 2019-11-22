﻿using NorthwindSystem.BLL;
using NorthwindSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                BindCategoryList();
                BindSupplierList();
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
        protected void BindCategoryList()
        {
            //this will be a standard lookup for the Category records
            try
            {
                CategoryController sysmgr = new CategoryController();
                List<Category> info = null;
                info = sysmgr.Categories_List();
                info.Sort((x, y) => x.CategoryName.CompareTo(y.CategoryName));
                CategoryList.DataSource = info;
                CategoryList.DataTextField = nameof(Category.CategoryName);
                CategoryList.DataValueField = nameof(Category.CategoryID);
                CategoryList.DataBind();
                CategoryList.Items.Insert(0, "select...");
            }
            catch (Exception ex)
            {
                //using the specialized error handling DataList control
                errormsgs.Add(GetInnerException(ex).ToString());
                LoadMessageDisplay(errormsgs, "alert alert-danger");
            }
        }
        protected void BindSupplierList()
        {
            //this will be a standard lookup for the Supplier records
            try
            {
                SupplierController sysmgr = new SupplierController();
                List<Supplier> info = null;
                info = sysmgr.Supplier_List();
                info.Sort((x, y) => x.ContactName.CompareTo(y.ContactName));
                SupplierList.DataSource = info;
                SupplierList.DataTextField = nameof(Supplier.ContactName);
                SupplierList.DataValueField = nameof(Supplier.SupplierID);
                SupplierList.DataBind();
                SupplierList.Items.Insert(0, "select...");
            }
            catch (Exception ex)
            {
                //using the specialized error handling DataList control
                errormsgs.Add(GetInnerException(ex).ToString());
                LoadMessageDisplay(errormsgs, "alert alert-danger");
            }
        }
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
    }
}