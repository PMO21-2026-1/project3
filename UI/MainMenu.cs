using System;
using System.Collections.Generic;
using ClubManamentSystem.DataStorage;
using ClubManamentSystem.Models;
using ClubManamentSystem.Services;

namespace ClubManamentSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ініціалізація початкових даних
            DataStore.Initialize();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("==========================================================");
                [cite_start] Console.WriteLine("   ІНФОРМАЦІЙНА СИСТЕМА УПРАВЛІННЯ ГУРТКАМИ [cite: 1]");
                Console.WriteLine("==========================================================");
                Console.WriteLine("Оберіть вашу роль для входу:");
                Console.WriteLine("1. Учень / Батько");
                Console.WriteLine("2. Викладач");
                Console.WriteLine("3. Адміністратор");
                Console.WriteLine("0. Вихід");
                Console.WriteLine("----------------------------------------------------------");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": StudentMenu(); break;
                    case "2": TeacherMenu(); break;
                    case "3": AdminMenu(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Невірний вибір. Натисніть будь-яку клавішу...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void StudentMenu()
        {
            ClubService clubService = new ClubService();
            Console.Clear();
            Console.WriteLine("--- МЕНЮ: УЧЕНЬ / БАТЬКО ---");
            [cite_start] Console.WriteLine("1. Пошук за районом/адресою [cite: 61, 62]");
            [cite_start] Console.WriteLine("2. Переглянути всі гуртки [cite: 14]");
            [cite_start] Console.WriteLine("3. Записатися на гурток [cite: 64]");
            Console.WriteLine("0. Назад");

            Console.Write("\nОберіть дію: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Введіть район або вулицю: ");
                string search = Console.ReadLine();
                var results = clubService.SearchByAddress(search);

                Console.WriteLine("\nЗнайдені гуртки:");
                foreach (var c in results) Console.WriteLine($"- {c.Name} ({c.Address})");
                Console.ReadKey();
            }
        }

        static void TeacherMenu()
        {
            Console.Clear();
            Console.WriteLine("--- ПАНЕЛЬ ВИКЛАДАЧА ---");
            [cite_start] Console.WriteLine("1. Відмітити відвідуваність [cite: 15, 66]");
            [cite_start] Console.WriteLine("2. Переглянути контакт учня [cite: 67]");
            Console.WriteLine("0. Назад");
            // Тут додається логіка виклику методів з ClubManager або AttendanceService
            Console.ReadKey();
        }

        static void AdminMenu()
        {
            ClubService clubService = new ClubService();
            Console.Clear();
            Console.WriteLine("--- ПАНЕЛЬ АДМІНІСТРАТОРА ---");
            [cite_start] Console.WriteLine("1. Додати новий гурток [cite: 15, 69]");
            [cite_start] Console.WriteLine("2. Змінити кабінет заняття [cite: 70]");
            Console.WriteLine("0. Назад");

            string choice = Console.ReadLine();
            if (choice == "1")
            {
                Console.Write("Назва гуртка: ");
                string name = Console.ReadLine();
                Console.Write("Адреса: ");
                string addr = Console.ReadLine();
                clubService.AddClub(name, "Опис", "Пн-Ср 15:00", addr, 1);
                Console.WriteLine("Гурток успішно додано!");
                Console.ReadKey();
            }
        }
    }
}