using Foundatio.Caching;
using Newtonsoft.Json;

namespace Caching.Demo
{
    class Program
    {
        private static InMemoryCacheClient _cache;
        private static HttpClient _httpClient;

        static void Main()
        {
            _cache = new InMemoryCacheClient();

            _httpClient = new HttpClient();

            while (true)
            {
                Console.WriteLine("Please enter an id: ");
                var id = Convert.ToInt32(Console.ReadLine());

                if (id == 0)
                {
                    var products = GetProducts();
                    Console.WriteLine($"Products\n{string.Join(", \n", products)}");
                } else
                {
                    var product  = GetById(id);
                    Console.WriteLine($"Product: ${product}");
                }

            }
        }

        public static List<Product> GetProducts()
        {
            Console.WriteLine("Fetching from service");

            var response = _httpClient.GetAsync("https://localhost:7012/data").Result;
            var json = response.Content.ReadAsStringAsync().Result;

            var products = JsonConvert.DeserializeObject<List<Product>>(json);

            return products;
        }

        public static Product GetById(int id)
        {
            var cacheProduct = _cache.GetAsync<Product>(id.ToString()).Result;

            if (cacheProduct.HasValue)
            {
                Console.WriteLine("Fetching from cache");
                return cacheProduct.Value;
            }

            Console.WriteLine("Fetching from service");

            var response = _httpClient.GetAsync("https://localhost:7012/data/" + id).Result;
            var json = response.Content.ReadAsStringAsync().Result;

            var product = JsonConvert.DeserializeObject<Product>(json);

            _cache.AddAsync(id.ToString(), product).Wait();

            return product;
        }
    }

    internal record Product(int Id, string Name, double Price, int Quantity);
}