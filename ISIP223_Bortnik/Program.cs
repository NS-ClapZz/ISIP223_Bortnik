using System;

namespace ShopInventory
{
    class Program
    {
        private static InventoryManager inventory = new InventoryManager();

        static void Main(string[] args)
        {
            Console.WriteLine("=== СИСТЕМА УЧЕТА ТОВАРОВ МАГАЗИНА ===\n");

            // Добавляем тестовые данные
            inventory.AddTestData();

            bool exit = false;
            while (!exit)
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
                        exit = true;
                        Console.WriteLine("Выход из программы...");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
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
            Console.WriteLine("7. Выход");
            Console.Write("Выберите действие: ");
        }

        static void AddProduct()
        {
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ ТОВАРА ===");

            try
            {
                Console.Write("Введите название товара: ");
                string name = Console.ReadLine();

                Console.Write("Введите цену: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
                {
                    Console.WriteLine("Неверная цена. Должна быть положительным числом.");
                    return;
                }

                Console.Write("Введите количество: ");
                if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
                {
                    Console.WriteLine("Неверное количество. Должно быть положительным числом.");
                    return;
                }

                Console.WriteLine("Выберите категорию:");
                Console.WriteLine("1. Electronics");
                Console.WriteLine("2. Clothing");
                Console.WriteLine("3. Food");
                Console.WriteLine("4. Books");
                Console.WriteLine("5. Sports");
                Console.Write("Введите номер категории: ");

                if (!int.TryParse(Console.ReadLine(), out int categoryChoice) || categoryChoice < 1 || categoryChoice > 5)
                {
                    Console.WriteLine("Неверный выбор категории.");
                    return;
                }

                Category category = (Category)(categoryChoice - 1);
                var product = new Product(name, price, quantity, category);
                inventory.AddProduct(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void RemoveProduct()
        {
            Console.WriteLine("\n=== УДАЛЕНИЕ ТОВАРА ===");
            Console.Write("Введите код товара для удаления: ");
            string code = Console.ReadLine();
            inventory.RemoveProduct(code);
        }

        static void RestockProduct()
        {
            Console.WriteLine("\n=== ПОСТАВКА ТОВАРА ===");
            Console.Write("Введите код товара: ");
            string code = Console.ReadLine();

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
                    Console.WriteLine("1. Electronics");
                    Console.WriteLine("2. Clothing");
                    Console.WriteLine("3. Food");
                    Console.WriteLine("4. Books");
                    Console.WriteLine("5. Sports");
                    Console.Write("Введите номер категории: ");

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