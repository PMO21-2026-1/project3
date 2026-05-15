using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClubManamentSystem
{
    public static class Validator
    {
        public static int ReadInt(string message)
        {
            while (true)
            {
                Console.Write(message);
                if (int.TryParse(Console.ReadLine(), out int result) && result >= 0) return result;
                Console.WriteLine("❌ Помилка: Введи ціле додатне число!");
            }
        }

        public static string ReadEmail(string message)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            while (true)
            {
                Console.Write(message);
                string email = Console.ReadLine() ?? "" ;
                if (Regex.IsMatch(email, pattern)) return email;
                Console.WriteLine("❌ Помилка: Невірний формат email!");
            }
        }

        public static string ReadPhone(string message)
        {
            while (true)
            {
                Console.Write(message);
                string phone = Console.ReadLine() ?? "";
                if (phone.All(char.IsDigit) && phone.Length >= 10 && phone.Length <= 12) return phone;
                Console.WriteLine("❌ Помилка: Номер має містити 10-12 цифр!");
            }
        }

        public static string ReadString(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine() ?? "";
                if (!string.IsNullOrWhiteSpace(input)) return input;
                Console.WriteLine("❌ Помилка: Це поле не може бути порожнім!");
            }
        }

        public static DateTime ReadDate(string message)
        {
            while (true)
            {
                Console.Write(message);
                if (DateTime.TryParse(Console.ReadLine(), out DateTime result)) return result;
                Console.WriteLine("❌ Помилка: Введи дату (РРРР-ММ-ДД)!");
            }
        }

        public static TimeSpan ReadTime(string message)
        {
            while (true)
            {
                Console.Write(message);
                if (TimeSpan.TryParse(Console.ReadLine(), out TimeSpan result)
    && result.TotalHours < 24) return result;
            }
        }
    }
}