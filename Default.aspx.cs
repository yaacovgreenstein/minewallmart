using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;



public partial class _Default : Page
{
    List<ReviewShow> reviews;
    protected void Page_Load(object sender, EventArgs e)
    {
         
    }

    protected async void ButtonDownloadPage_Click(object sender, EventArgs e)
    {
        IMineProduct mp = new MineWallMart(); //some infrastucture for dependency injection in the future to support other suppliers
        string productNumber = await mp.MineProduct(TextBoxProductUrl.Text);
        Product product = null;
        List<ReviewShow> myReviews;
        using (DatabaseRpoundForestEntities db = new DatabaseRpoundForestEntities())
        {
            product = db.Products.Where((x) => x.SellerInternalId == productNumber).FirstOrDefault();
            myReviews = db.Reviews
                .Where((x) => x.ProductId == product.Id)
                .Select((x)=> new ReviewShow {  reviewTitle = x.ReviewTitle, reviewText = x.ReviewText, rating = x.Stars.ToString() } )
                .ToList<ReviewShow>();
            
        }
        Session["productid"] = product.Id;
        LabelProductInfo.Text = String.Format("Product Seller Number: {0} Product name: {1}",product.SellerInternalId,product.ProductName);
        GridView1.DataSource = myReviews;
        GridView1.DataBind();

    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataSource = reviews;
        GridView1.DataBind();
    }

    protected void TextBoxSeek_TextChanged(object sender, EventArgs e)
    {
        
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        using (DatabaseRpoundForestEntities db = new DatabaseRpoundForestEntities())
        {
            int productId = Convert.ToInt32(Session["productid"]);
            reviews = db.Reviews
               .Where((x) => x.ProductId == productId && x.ReviewText.Contains(TextBoxSeek.Text))
               .Select((x) => new ReviewShow { reviewTitle = x.ReviewTitle, reviewText = x.ReviewText, rating = x.Stars.ToString() })
               .ToList<ReviewShow>();
            GridView1.DataSource = reviews;
            GridView1.DataBind();
        }
    }

    protected void ButtonClear_Click(object sender, EventArgs e)
    {
        TextBoxSeek.Text = "";
        Button1_Click(sender, e);
    }
}

public class ReviewShow
{
    public string reviewTitle { get; set; }
    public string reviewText { get; set; }
    public string rating { get; set; }
}