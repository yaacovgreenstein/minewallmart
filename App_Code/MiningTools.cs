using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

public interface IMineProduct
{
    Task<string> MineProduct(string url);
}
/// <summary>
/// Summary description for MiningTools
/// </summary>
  

    public class MineWallMart : IMineProduct
    {
        public async Task<string> MineProduct(string url)
        {
            string pageContent = await HttpTools.DownloadPage(url);
            //get wallmart number
            int productNumberIndex = pageContent.IndexOf("\"walmartItemNumber\"") + 22;
            int productNumberEndIndex = pageContent.IndexOf("\"brand\"", productNumberIndex) - 3;
            string productNumber = pageContent.Substring(productNumberIndex, productNumberEndIndex - productNumberIndex);
            
            using (DatabaseRpoundForestEntities db = new DatabaseRpoundForestEntities())
            {
                Product product = db.Products.Where((x) => x.SellerInternalId == productNumber).FirstOrDefault();
                if (product == null) //add it
                {
                    product = GetAndAddProduct(pageContent, productNumber, db);
                }

            //delete all reviews and re-fill
            DeleteGetAndAddReviews(product.Id, pageContent, db);
            }
            return productNumber;
        }

        private Product GetAndAddProduct(string pageContent, string productNumber, DatabaseRpoundForestEntities db)
        {
            int selectedProductIdIndex = pageContent.IndexOf("\"selectedProductId\"");
            int productTitleIndex = pageContent.IndexOf("\"title\"", selectedProductIdIndex) + 9;
            int productTitleEndIndex = pageContent.IndexOf("\"brand\"", selectedProductIdIndex) - 2;
            string productTitle = pageContent.Substring(productTitleIndex, productTitleEndIndex - productTitleIndex);
            Product newProduct = new Product { SellerInternalId = productNumber, ProductName = productTitle, Price = 22.33M };
            db.Products.Add(newProduct); //and whatever the price is.
            db.SaveChanges();
            return newProduct;
        }

        private void DeleteGetAndAddReviews(int productId,string pageContent, DatabaseRpoundForestEntities db)
        {
            //remove
            db.Reviews.RemoveRange(db.Reviews.Where(x => x.ProductId == productId));
            db.SaveChanges();
            //getcustomerReviews
            int customerReviewsIndex = pageContent.IndexOf("\"customerReviews\"")+18;
            int customerReviewsEndIndex = pageContent.IndexOf("\"selected\"", customerReviewsIndex) - 3;
            string reviewList = pageContent.Substring(customerReviewsIndex, customerReviewsEndIndex - customerReviewsIndex);
            var reviewList2 = JsonConvert.DeserializeObject <List<ReviewClass>>(reviewList);
            //List<ReviewShow> reviews = new List<ReviewShow>();
            foreach(ReviewClass rc in reviewList2)
            {
            var thisReview = new Review
            {
                ProductId = productId,
                ReviewTitle = rc.reviewTitle,
                ReviewText = rc.reviewText,
                Stars = Convert.ToInt32(rc.rating)
            };
            db.Reviews.Add(thisReview);
            //reviews.Add(new ReviewShow { rating = rc.rating, reviewText = rc.reviewText, reviewTitle = rc.reviewTitle } );
            }
            db.SaveChanges();
        }
}

public class ReviewClass
{
    public string reviewId { get; set; }
    public string authorId { get; set; } 
    public string recommended { get; set; }
    public string showRecommended { get; set; }
    public string negativeFeedback { get; set; }
    public string positiveFeedback { get; set; }
    public string rating { get; set; }
    public string reviewTitle { get; set; }
    public string reviewText { get; set; }
}


