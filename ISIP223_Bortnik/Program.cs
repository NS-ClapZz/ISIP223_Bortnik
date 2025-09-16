// Добавляем в класс InventoryManager дополнительные методы проверки

public bool ProductExists(string code)
{
    return products.Any(p => p.Code == code);
}

public Product GetProductByCode(string code)
{
    return products.FirstOrDefault(p => p.Code == code);
}

// В класс Program добавляем дополнительные проверки

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

// Добавляем проверку перед удалением
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

    // Подтверждение удаления
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