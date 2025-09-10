#include <iostream>
#include <string>
#include <vector>
#include <algorithm>
#include <iomanip>
#include <limits>
#include <map>

using namespace std;

// Структура для хранения информации о трате
struct Expense {
    string name;
    double amount;
};




// Функция для безопасного ввода числа
double getValidatedDouble(const string& prompt) {
    double value;
    while (true) {
        cout << prompt;
        cin >> value;
        if (cin.fail() || value <= 0) {
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
            cout << "Ошибка! Введите корректное положительное число: ";
        }
        else {
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
            return value;
        }
    }
}

// Функция для безопасного ввода целого числа
int getValidatedInt(const string& prompt, int min, int max) {
    int value;
    while (true) {
        cout << prompt;
        cin >> value;
        if (cin.fail() || value < min || value > max) {
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
            cout << "Ошибка! Введите число от " << min << " до " << max << ": ";
        }
        else {
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
            return value;
        }
    }
}

// Функция для ввода трат
vector<Expense> inputExpenses(int count) {
    vector<Expense> expenses;

    for (int i = 0; i < count; i++) {
        cout << "\n--- Операция " << i + 1 << " ---" << endl;

        Expense expense;

        cout << "Название товара/услуги: ";
        getline(cin, expense.name);

        expense.amount = getValidatedDouble("Сумма (рубли): ");

        expenses.push_back(expense);
    }

    return expenses;
}

// Функция вывода всех трат
void printExpenses(const vector<Expense>& expenses) {
    cout << "\n=== ВАШИ ТРАТЫ ===" << endl;
    cout << setw(40) << left << "Название" << setw(10) << right << "Сумма (руб)" << endl;
    cout << string(50, '-') << endl;

    double total = 0;
    for (const auto& expense : expenses) {
        cout << setw(40) << left << expense.name
            << setw(10) << right << fixed << setprecision(2) << expense.amount << endl;
        total += expense.amount;
    }

    cout << string(50, '-') << endl;
    cout << setw(40) << left << "ОБЩАЯ СУММА:"
        << setw(10) << right << total << " руб" << endl;
}

// Функция для вывода статистики
void showStatistics(const vector<Expense>& expenses) {
    if (expenses.empty()) {
        cout << "Нет данных для статистики!" << endl;
        return;
    }

    double total = 0;
    double minAmount = expenses[0].amount;
    double maxAmount = expenses[0].amount;
    string minName = expenses[0].name;
    string maxName = expenses[0].name;

    for (const auto& expense : expenses) {
        total += expense.amount;

        if (expense.amount < minAmount) {
            minAmount = expense.amount;
            minName = expense.name;
        }

        if (expense.amount > maxAmount) {
            maxAmount = expense.amount;
            maxName = expense.name;
        }
    }

    double average = total / expenses.size();

    cout << "\n=== СТАТИСТИКА ===" << endl;
    cout << "Общая сумма: " << fixed << setprecision(2) << total << " руб" << endl;
    cout << "Средняя трата: " << average << " руб" << endl;
    cout << "Минимальная трата: " << minAmount << " руб (" << minName << ")" << endl;
    cout << "Максимальная трата: " << maxAmount << " руб (" << maxName << ")" << endl;
}

// Пузырьковая сортировка по цене (по возрастанию)
void bubbleSort(vector<Expense>& expenses) {
    int n = expenses.size();
    for (int i = 0; i < n - 1; i++) {
        for (int j = 0; j < n - i - 1; j++) {
            if (expenses[j].amount > expenses[j + 1].amount) {
                swap(expenses[j], expenses[j + 1]);
            }
        }
    }
    cout << "Сортировка завершена! Траты отсортированы по возрастанию цены." << endl;
}

// Функция конвертации валюты
void convertCurrency(vector<Expense>& expenses) {
    cout << "\n=== КОНВЕРТАЦИЯ ВАЛЮТЫ ===" << endl;
    cout << "1. Доллары (USD)" << endl;
    cout << "2. Евро (EUR)" << endl;
    cout << "3. Юани (CNY)" << endl;
    cout << "4. Другая валюта (ввести курс вручную)" << endl;

    int choice = getValidatedInt("Выберите валюту (1-4): ", 1, 4);

    double rate;
    string currencyName;

    switch (choice) {
    case 1:
        rate = getValidatedDouble("Введите курс USD к RUB: ");
        currencyName = "USD";
        break;
    case 2:
        rate = getValidatedDouble("Введите курс EUR к RUB: ");
        currencyName = "EUR";
        break;
    case 3:
        rate = getValidatedDouble("Введите курс CNY к RUB: ");
        currencyName = "CNY";
        break;
    case 4:
        cout << "Введите название валюты: ";
        getline(cin, currencyName);
        rate = getValidatedDouble("Введите курс " + currencyName + " к RUB: ");
        break;
    }

    cout << "\n=== ТРАТЫ В " << currencyName << " ===" << endl;
    cout << setw(40) << left << "Название"
        << setw(15) << right << "Сумма (" + currencyName + ")" << endl;
    cout << string(55, '-') << endl;

    double totalRub = 0;
    for (const auto& expense : expenses) {
        double converted = expense.amount / rate;
        cout << setw(40) << left << expense.name
            << setw(15) << right << fixed << setprecision(2) << converted << endl;
        totalRub += expense.amount;
    }

    cout << string(55, '-') << endl;
    cout << setw(40) << left << "ОБЩАЯ СУММА:"
        << setw(15) << right << totalRub / rate << " " << currencyName << endl;
}

// Функция поиска по названию
void searchByName(const vector<Expense>& expenses) {
    cout << "\n=== ПОИСК ПО НАЗВАНИЮ ===" << endl;
    cout << "Введите часть названия для поиска: ";
    string searchTerm;
    getline(cin, searchTerm);

    // Преобразуем поисковый запрос в нижний регистр
    transform(searchTerm.begin(), searchTerm.end(), searchTerm.begin(), ::tolower);

    vector<Expense> results;

    for (const auto& expense : expenses) {
        string lowerName = expense.name;
        transform(lowerName.begin(), lowerName.end(), lowerName.begin(), ::tolower);

        if (lowerName.find(searchTerm) != string::npos) {
            results.push_back(expense);
        }
    }

    if (results.empty()) {
        cout << "Ничего не найдено по запросу: '" << searchTerm << "'" << endl;
    }
    else {
        cout << "\nНайдено " << results.size() << " совпадений:" << endl;
        cout << setw(40) << left << "Название" << setw(10) << right << "Сумма (руб)" << endl;
        cout << string(50, '-') << endl;

        for (const auto& result : results) {
            cout << setw(40) << left << result.name
                << setw(10) << right << fixed << setprecision(2) << result.amount << endl;
        }
    }
}

// Главное меню
void showMenu() {
    cout << "\n=== ГЛАВНОЕ МЕНЮ ===" << endl;
    cout << "1. Вывод данных" << endl;
    cout << "2. Статистика (среднее, максимальное, минимальное, сумма)" << endl;
    cout << "3. Сортировка по цене (пузырьковая сортировка)" << endl;
    cout << "4. Конвертация валюты" << endl;
    cout << "5. Поиск по названию" << endl;
    cout << "0. Выход" << endl;
}

int main() {
    setlocale(LC_ALL, "Russian");

    cout << "=== УЧЕТ ЕЖЕДНЕВНЫХ РАСХОДОВ ===" << endl;

    // Ввод количества операций
    int operationCount = getValidatedInt("Введите количество операций (2-40): ", 2, 40);

    // Ввод трат
    vector<Expense> expenses = inputExpenses(operationCount);

    // Основной цикл меню
    int choice;
    do {
        showMenu();
        choice = getValidatedInt("Выберите пункт меню (0-5): ", 0, 5);

        switch (choice) {
        case 1:
            printExpenses(expenses);
            break;
        case 2:
            showStatistics(expenses);
            break;
        case 3:
            bubbleSort(expenses);
            break;
        case 4:
            convertCurrency(expenses);
            break;
        case 5:
            searchByName(expenses);
            break;
        case 0:
            cout << "Выход из программы. Хорошего дня!" << endl;
            break;
        }

    } while (choice != 0);

    return 0;
}