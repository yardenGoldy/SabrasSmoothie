using Mono.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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

        public IEnumerable<Product> SortByOrders(Expression<Func<Product, bool>> query)
        {
            return Products.Where(query).ToList().OrderBy(x => x.ProductOrders != null ? x.ProductOrders.Count : 0);
        }

        public Expression<Func<Product, bool>> FindByAll(Expression<Func<Product, bool>> query, string message)
        {
            Expression<Func<Product, bool>> newQuery = PredicateBuilder.False<Product>();

            int result;
            if (int.TryParse(message, out result))
            {
                newQuery = newQuery.OrElse(Product => Product.Price == result).OrElse(product => product.Calories == result);
            }

            newQuery = newQuery.OrElse(Product => Product.Name == message);

            return newQuery.AndAlso(query);
        }

        public List<List<Product>> GroupByPrices(Expression<Func<Product, bool>> query)
        {
            var productsOrderByPrice = Products.Where(query).OrderBy(x => x.Price).ToList();
            return GroupingByNumber(productsOrderByPrice, 3);
        }

        public List<List<Product>> GroupByCalories(Expression<Func<Product, bool>> query)
        {
            var productsOrderByPrice = Products.Where(query).OrderBy(x => x.Calories).ToList();
            return GroupingByNumber(productsOrderByPrice, 3);
        }

        public Expression<Func<Product, bool>> RangePrice(Expression<Func<Product, bool>> query, int min, int max)
        {
            return query.AndAlso(x => x.Price >= min && x.Price <= max);
        }

        public Expression<Func<Product, bool>> RangeCalories(Expression<Func<Product, bool>> query, int min, int max)
        {
            return query.AndAlso(x => x.Calories >= min && x.Price <= max);
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
            return Products.GroupBy(x => x.IsVegan).ToDictionary(x => x.Key, y => y.AsEnumerable());
        }
    }
}