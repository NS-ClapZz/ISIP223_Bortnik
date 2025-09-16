using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopInventory
{
    public class InventoryManager
    {
        private List<Product> products;
        private int nextProductId;

        public InventoryManager()
        {
            products = new List<Product>();
            nextProductId = 1001; // Начинаем с 1001 чтобы код начинался с "1"
        }

        public void AddProduct(Product product)
        {
            string code = "1" + nextProductId.ToString().Substring(1);
            product.SetCode(code);
            products.Add(product);
            nextProductId++;
            Console.WriteLine($"Товар добавлен: {product.Name} (код: {product.Code})");
        }

        public bool RemoveProduct(string code)
        {
            var product = products.FirstOrDefault(p => p.Code == code);
            if (product != null)
            {
                products.Remove(product);
                Console.WriteLine($"Товар удален: {product.Name} (код: {product.Code})");
                return true;
            }
            Console.WriteLine($"Товар с кодом {code} не найден.");
            return false;
        }

        public void RestockProduct(string code, int quantity)
        {
            if (quantity <= 0)
            {
                Console.WriteLine("Количество для поставки должно быть положительным.");
                return;
            }

            var product = products.FirstOrDefault(p => p.Code == code);
            if (product != null)
            {
                product.Quantity += quantity;
                Console.WriteLine($"Поставка выполнена: {product.Name}. Новое количество: {product.Quantity}");
            }
            else
            {
                Console.WriteLine($"Товар с кодом {code} не найден.");
            }
        }

        public bool SellProduct(string code, int quantity)
        {
            if (quantity <= 0)
            {
                Console.WriteLine("Количество для продажи должно быть положительным.");
                return false;
            }

            var product = products.FirstOrDefault(p => p.Code == code);
            if (product != null)
            {
                if (product.Quantity >= quantity)
                {
                    product.Quantity -= quantity;
                    decimal total = product.Price * quantity;
                    Console.WriteLine($"Продажа выполнена: {quantity} x {product.Name}");
                    Console.WriteLine($"Общая стоимость: {total:C}");
                    Console.WriteLine($"Остаток на складе: {product.Quantity}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Недостаточно товара на складе. Доступно: {product.Quantity}");
                    return false;
                }
            }
            else
            {
                Console.WriteLine($"Товар с кодом {code} не найден.");
                return false;
            }
        }

        public void DisplayAllProducts()
        {
            if (products.Count == 0)
            {
                Console.WriteLine("Список товаров пуст.");
                return;
            }

            Console.WriteLine("\n=== ВСЕ ТОВАРЫ ===");
            foreach (var product in products)
            {
                Console.WriteLine(product);
            }
        }

        public void SearchByCode(string code)
        {
            var product = products.FirstOrDefault(p => p.Code == code);
            if (product != null)
            {
                Console.WriteLine("\n=== РЕЗУЛЬТАТ ПОИСКА ===");
                Console.WriteLine(product);
            }
            else
            {
                Console.WriteLine($"Товар с кодом {code} не найден.");
            }
        }

        public void SearchByName(string name)
        {
            var results = products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            DisplaySearchResults(results, $"по названию '{name}'");
        }

        public void SearchByCategory(Category category)
        {
            var results = products.Where(p => p.Category == category).ToList();
            DisplaySearchResults(results, $"по категории '{category}'");
        }

        private void DisplaySearchResults(List<Product> results, string searchCriteria)
        {
            if (results.Count > 0)
            {
                Console.WriteLine($"\n=== РЕЗУЛЬТАТЫ ПОИСКА {searchCriteria.ToUpper()} ===");
                foreach (var product in results)
                {
                    Console.WriteLine(product);
                }
            }
            else
            {
                Console.WriteLine($"Товары {searchCriteria} не найдены.");
            }
        }

        public void AddTestData()
        {
            // Добавляем тестовые данные
            AddProduct(new Product("Смартфон Samsung", 25000m, 15, Category.Electronics));
            AddProduct(new Product("Футболка хлопковая", 1500m, 30, Category.Clothing));
            AddProduct(new Product("Шоколад молочный", 100m, 50, Category.Food));
            AddProduct(new Product("Программирование на C#", 1200m, 10, Category.Books));
            AddProduct(new Product("Футбольный мяч", 3000m, 8, Category.Sports));
        }
    }
}