using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopInventory
{
    public enum Category
    {
        Electronics,
        Clothing,
        Food,
        Books,
        Sports
    }

    public class Product
    {
        public string Code { get; private set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool InStock => Quantity > 0;
        public Category Category { get; set; }

        public Product(string name, decimal price, int quantity, Category category)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название товара не может быть пустым");

            if (price < 0)
                throw new ArgumentException("Цена не может быть отрицательной");

            if (quantity < 0)
                throw new ArgumentException("Количество не может быть отрицательным");

            Name = name.Trim();
            Price = price;
            Quantity = quantity;
            Category = category;
        }

        public void SetCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || !code.StartsWith("1"))
                throw new ArgumentException("Код должен начинаться с '1'");

            Code = code;
        }

        public override string ToString()
        {
            return $"Код: {Code}, Название: {Name}, Цена: {Price:C}, Количество: {Quantity}, " +
                   $"В наличии: {(InStock ? "Да" : "Нет")}, Категория: {Category}";
        }
    }

    public class InventoryManager
    {
        private List<Product> products;
        private int nextProductId;

        public InventoryManager()
        {
            products = new List<Product>();
            nextProductId = 1001;
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
                Console.WriteLine($"Найдено товаров: {results.Count}");
            }
            else
            {
                Console.WriteLine($"Товары {searchCriteria} не найдены.");
            }
        }

        public void AddTestData()
        {
            AddProduct(new Product("Смартфон Samsung", 25000m, 15, Category.Electronics));
            AddProduct(new Product("Футболка хлопковая", 1500m, 30, Category.Clothing));
            AddProduct(new Product("Шоколад молочный", 100m, 50, Category.Food));
            AddProduct(new Product("Программирование на C#", 1200m, 10, Category.Books));
            AddProduct(new Product("Футбольный мяч", 3000m, 8, Category.Sports));
        }

        public bool ProductExists(string code)
        {
            return products.Any(p => p.Code == code);
        }

        public Product GetProductByCode(string code)
        {
            return products.FirstOrDefault(p => p.Code == code);
        }

        public void DisplayStatistics()
        {
            Console.WriteLine("\n=== СТАТИСТИКА ===");
            Console.WriteLine($"Общее количество товаров: {products.Count}");
            Console.WriteLine($"Товаров в наличии: {products.Count(p => p.InStock)}");
            Console.WriteLine($"Общая стоимость инвентаря: {products.Sum(p => p.Price * p.Quantity):C}");

            foreach (Category category in Enum.GetValues(typeof(Category)))
            {
                var categoryProducts = products.Where(p => p.Category == category).ToList();
                if (categoryProducts.Count > 0)
                {
                    Console.WriteLine($"\n{category}: {categoryProducts.Count} товаров");
                    Console.WriteLine($"Общая стоимость: {categoryProducts.Sum(p => p.Price * p.Quantity):C}");
                }
            }
        }
    }

    class Program
    {
        private static InventoryManager inventory = new InventoryManager();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            DisplayWelcomeMessage();

            inventory.AddTestData();

            bool exit = false;
            while (!exit)
            {
                try
                {
                    DisplayMenu();
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            AddProduct();
                            break;
                        case "2":
                            RemoveProduct();
                            break;
                        case "3":
                            RestockProduct();
                            break;
                        case "4":
                            SellProduct();
                            break;
                        case "5":
                            inventory.DisplayAllProducts();
                            break;
                        case "6":
                            SearchProducts();
                            break;
                        case "7":
                            inventory.DisplayStatistics();
                            break;
                        case "8":
                            exit = true;
                            Console.WriteLine("Спасибо за использование системы! Выход...");
                            break;
                        default:
                            Console.WriteLine("Неверный выбор. Пожалуйста, выберите от 1 до 8.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
                }

                if (!exit)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    Console.Clear();
                    DisplayWelcomeMessage();
                }
            }
        }

        static void DisplayWelcomeMessage()
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("    СИСТЕМА УЧЕТА ТОВАРОВ МАГАЗИНА");
            Console.WriteLine("==========================================");
            Console.WriteLine("Добро пожаловать! Загружены тестовые данные.");
            Console.WriteLine("==========================================\n");
        }

        static void DisplayMenu()
        {
            Console.WriteLine("МЕНЮ:");
            Console.WriteLine("1. Добавить товар");
            Console.WriteLine("2. Удалить товар");
            Console.WriteLine("3. Заказать поставку товара");
            Console.WriteLine("4. Продать товар");
            Console.WriteLine("5. Показать все товары");
            Console.WriteLine("6. Поиск товаров");
            Console.WriteLine("7. Статистика");
            Console.WriteLine("8. Выход");
            Console.Write("Выберите действие: ");
        }

        static void AddProduct()
        {
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ ТОВАРА ===");

            try
            {
                Console.Write("Введите название товара: ");
                string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Название товара не может быть пустым.");
                    return;
                }

                Console.Write("Введите цену: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
                {
                    Console.WriteLine("Неверная цена. Должна быть положительным числом.");
                    return;
                }

                Console.Write("Введите количество: ");
                if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
                {
                    Console.WriteLine("Неверное количество. Должно быть неотрицательным числом.");
                    return;
                }

                Console.WriteLine("Выберите категорию:");
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"{i + 1}. {(Category)i}");
                }
                Console.Write("Введите номер категории (1-5): ");

                if (!int.TryParse(Console.ReadLine(), out int categoryChoice) || categoryChoice < 1 || categoryChoice > 5)
                {
                    Console.WriteLine("Неверный выбор категории. Должен быть от 1 до 5.");
                    return;
                }

                Category category = (Category)(categoryChoice - 1);
                var product = new Product(name, price, quantity, category);
                inventory.AddProduct(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении товара: {ex.Message}");
            }
        }

        static void RemoveProduct()
        {
            Console.WriteLine("\n=== УДАЛЕНИЕ ТОВАРА ===");
            Console.Write("Введите код товара для удаления: ");
            string code = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(code))
            {
                Console.WriteLine("Код товара не может быть пустым.");
                return;
            }

            var product = inventory.GetProductByCode(code);
            if (product != null)
            {
                Console.WriteLine($"Вы уверены, что хотите удалить товар: {product.Name}? (y/n)");
                if (Console.ReadLine().ToLower() == "y")
                {
                    inventory.RemoveProduct(code);
                }
                else
                {
                    Console.WriteLine("Удаление отменено.");
                }
            }
            else
            {
                Console.WriteLine($"Товар с кодом {code} не найден.");
            }
        }

        static void RestockProduct()
        {
            Console.WriteLine("\n=== ПОСТАВКА ТОВАРА ===");
            Console.Write("Введите код товара: ");
            string code = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(code))
            {
                Console.WriteLine("Код товара не может быть пустым.");
                return;
            }

            Console.Write("Введите количество для поставки: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Неверное количество. Должно быть положительным числом.");
                return;
            }

            inventory.RestockProduct(code, quantity);
        }

        static void SellProduct()
        {
            Console.WriteLine("\n=== ПРОДАЖА ТОВАРА ===");
            Console.Write("Введите код товара: ");
            string code = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(code))
            {
                Console.WriteLine("Код товара не может быть пустым.");
                return;
            }

            Console.Write("Введите количество для продажи: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Неверное количество. Должно быть положительным числом.");
                return;
            }

            inventory.SellProduct(code, quantity);
        }

        static void SearchProducts()
        {
            Console.WriteLine("\n=== ПОИСК ТОВАРОВ ===");
            Console.WriteLine("1. По коду");
            Console.WriteLine("2. По названию");
            Console.WriteLine("3. По категории");
            Console.Write("Выберите тип поиска: ");

            string searchType = Console.ReadLine();

            switch (searchType)
            {
                case "1":
                    Console.Write("Введите код товара: ");
                    string code = Console.ReadLine();
                    inventory.SearchByCode(code);
                    break;
                case "2":
                    Console.Write("Введите название товара: ");
                    string name = Console.ReadLine();
                    inventory.SearchByName(name);
                    break;
                case "3":
                    Console.WriteLine("Выберите категорию:");
                    for (int i = 0; i < 5; i++)
                    {
                        Console.WriteLine($"{i + 1}. {(Category)i}");
                    }
                    Console.Write("Введите номер категории (1-5): ");

                    if (int.TryParse(Console.ReadLine(), out int categoryChoice) && categoryChoice >= 1 && categoryChoice <= 5)
                    {
                        Category category = (Category)(categoryChoice - 1);
                        inventory.SearchByCategory(category);
                    }
                    else
                    {
                        Console.WriteLine("Неверный выбор категории.");
                    }
                    break;
                default:
                    Console.WriteLine("Неверный выбор типа поиска.");
                    break;
            }
        }
    }
}