using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SabrasSmoothie.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Calories { get; set; }
        public bool IsVegan { get; set; }
        public string ImagePath { get; set; }

        public ICollection<OrderProduct> ProductOrders { get; set; }
    }

    public class ProductDbContext : SabrasDbContext
    {

        public IQueryable<Product> GetProductsIncludeProductOrders()
        {
            return this.Products.Include(product => product.ProductOrders);
        }

        public IEnumerable<Product> SortByOrders(IEnumerable<Product> products = null)
        {
            if(products == null)
            {
                products = Products.ToList();
            }

            return products.OrderBy(x => x.ProductOrders != null ? x.ProductOrders.Count : 0);
        }

        public IEnumerable<Product> FindByAll(string message, IEnumerable<Product> products = null)
        {
            if (products == null)
            {
                products = Products.ToList();
            }

            IEnumerable<Product> allNames = null, allPrices = null, allCalories = null;
            int result;
            if (int.TryParse(message, out result))
            {
                allPrices = Products.Where(x => x.Price == result) ?? Enumerable.Empty<Product>();
                allCalories = Products.Where(x => x.Calories == result) ?? Enumerable.Empty<Product>();
                
            }

            allNames = Products.Where(x => x.Name == message) ?? Enumerable.Empty<Product>();
            

            return allNames.Union(allPrices).Union(allCalories).Distinct();
        }

        public List<List<Product>> GroupByPrices(IEnumerable<Product> products = null) {
            var productsOrderByPrice = Products.OrderBy(x => x.Price).ToList();
            return GroupingByNumber(productsOrderByPrice, 3);
        }

        public List<List<Product>> GroupByCalories(IEnumerable<Product> products = null)
        {
            var productsOrderByPrice = Products.OrderBy(x => x.Calories).ToList();
            return GroupingByNumber(productsOrderByPrice, 3);
        }

        public IEnumerable<Product> RangePrice(int min, int max, IEnumerable<Product> products = null) {
            return Products.Where(x => x.Price >= min && x.Price <= max).ToList();
        }

        public IEnumerable<Product> RangeCalories(int min, int max)
        {
            return Products.Where(x => x.Calories >= min && x.Price <= max);
        }

        private List<List<Product>> GroupingByNumber(List<Product> sortedProducts, int numberOfGroup)
        {
            int length = sortedProducts.Count;
            int ProductsPerGroup = length / numberOfGroup;
            List<List<Product>> result = new List<List<Product>>();
            result.Add(sortedProducts.GetRange(0, ProductsPerGroup));
            result.Add(sortedProducts.GetRange(ProductsPerGroup, ProductsPerGroup));
            result.Add(sortedProducts.GetRange(ProductsPerGroup * 2, length - ProductsPerGroup * 2));
            return result;
        }

        public IDictionary<bool, IEnumerable<Product>> GroupByVegan()
        {
            return Products.GroupBy(x => x.IsVegan).ToDictionary(x =>x.Key, y=>y.AsEnumerable());
        }
    }
}