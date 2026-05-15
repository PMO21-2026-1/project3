using System;
using System.Collections.Generic;
using System.Linq;
using ClubManamentSystem.DataStorage;
using ClubManamentSystem.Models;
using ClubManamentSystem.Services;

namespace ClubManamentSystem
{
    class Program
    {
        static readonly ClubService _clubService = new ClubService();
        static readonly TeacherService _teacherService = new TeacherService();
        static readonly StudentService _studentService = new StudentService();
        static readonly SessionService _sessionService = new SessionService();
        static readonly AttendanceService _attendanceService = new AttendanceService();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            DataStore.Initialize();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("==========================================================");
                Console.WriteLine("   ІНФОРМАЦІЙНА СИСТЕМА УПРАВЛІННЯ ГУРТКАМИ");
                Console.WriteLine("==========================================================");
                Console.WriteLine("1. Учень / Батько");
                Console.WriteLine("2. Викладач");
                Console.WriteLine("3. Адміністратор");
                Console.WriteLine("0. Вихід");
                Console.WriteLine("----------------------------------------------------------");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1": StudentMenu(); break;
                    case "2": TeacherMenu(); break;
                    case "3": AdminMenu(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("❌ Невірний вибір. Натисніть клавішу...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void StudentMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- МЕНЮ: УЧЕНЬ / БАТЬКО ---");
                Console.WriteLine("1. Пошук за адресою");
                Console.WriteLine("2. Список усіх гуртків");
                Console.WriteLine("3. Записатися на гурток");
                Console.WriteLine("4. Мій розклад");
                Console.WriteLine("5. Моя відвідуваність");
                Console.WriteLine("0. Назад");

                Console.Write("\nДія: ");
                string choice = Console.ReadLine() ?? "";
                if (choice == "0") return;

                switch (choice)
                {
                    case "1":
                        string search = Validator.ReadString("Введіть адресу для пошуку: ");
                        var results = _clubService.SearchByAddress(search);
                        if (results.Count == 0)
                        {
                            Console.WriteLine("Нічого не знайдено.");
                        }
                        else
                        {
                            Console.WriteLine($"\nЗнайдено {results.Count} гурток(-ів):");
                            foreach (var c in results)
                            {
                                var teacher = _teacherService.GetById(c.TeacherId);
                                string tName = teacher?.FullName ?? "Не призначено";
                                Console.WriteLine($"  [ID: {c.Id}] {c.Name} | {c.Address} | Викладач: {tName} | Розклад: {c.Schedule}");
                            }
                        }
                        break;

                    case "2":
                        var allClubs = _clubService.GetAll();
                        if (allClubs.Count == 0)
                        {
                            Console.WriteLine("Гуртків ще немає в системі.");
                        }
                        else
                        {
                            Console.WriteLine("\n--- Всі доступні гуртки ---");
                            foreach (var c in allClubs)
                            {
                                var teacher = _teacherService.GetById(c.TeacherId);
                                string tName = teacher?.FullName ?? "Не призначено";
                                Console.WriteLine($"  [ID: {c.Id}] {c.Name} | Адреса: {c.Address} | Викладач: {tName} | Розклад: {c.Schedule}");
                            }
                        }
                        break;

                    case "3":
                        Console.WriteLine("\n--- Кого записуємо? ---");
                        foreach (var s in DataStore.Students)
                            Console.WriteLine($"  [ID: {s.Id}] {s.FullName}");

                        int sId = Validator.ReadInt("Введіть ID учня: ");

                        Console.WriteLine("\n--- Куди записуємо? ---");
                        foreach (var c in _clubService.GetAll())
                            Console.WriteLine($"  [ID: {c.Id}] {c.Name}");

                        int cId = Validator.ReadInt("Введіть ID гуртка: ");

                        if (_studentService.EnrollStudent(sId, cId))
                            Console.WriteLine("✅ Успішно записано!");
                        else
                            Console.WriteLine("❌ Помилка запису (учень або гурток не існує, або вже записаний).");
                        break;

                    case "4":
                        Console.WriteLine("\n--- Чий розклад шукаємо? ---");
                        foreach (var s in DataStore.Students)
                            Console.WriteLine($"  [ID: {s.Id}] {s.FullName}");

                        int myId = Validator.ReadInt("Введіть ваш ID: ");

                        var schedule = _studentService.GetStudentSchedule(myId);
                        if (schedule.Count == 0)
                        {
                            Console.WriteLine("У вас немає жодних занять.");
                        }
                        else
                        {
                            Console.WriteLine("\n--- Ваш розклад ---");
                            foreach (var s in schedule)
                            {
                                var clubName = DataStore.Clubs.FirstOrDefault(c => c.Id == s.ClubId)?.Name ?? "Невідомо";
                                Console.WriteLine($"  [{s.Date.ToShortDateString()}] {clubName} | {s.StartTime}-{s.EndTime} | Кімн. {s.Room}");
                            }
                        }
                        break;

                    case "5":
                        Console.WriteLine("\n--- Перевірка відвідуваності ---");
                        foreach (var s in DataStore.Students)
                            Console.WriteLine($"  [ID: {s.Id}] {s.FullName}");

                        int studId = Validator.ReadInt("Введіть ваш ID: ");

                        var myAtt = DataStore.Attendances.Where(a => a.StudentId == studId).ToList();
                        if (myAtt.Count == 0)
                        {
                            Console.WriteLine("Немає жодних записів про відвідуваність.");
                        }
                        else
                        {
                            int presentCount = myAtt.Count(a => a.Status == AttendanceStatus.Present);
                            Console.WriteLine($"\nВідвідано: {presentCount} з {myAtt.Count} занять\n");
                            foreach (var a in myAtt)
                            {
                                var session = DataStore.Sessions.FirstOrDefault(s => s.Id == a.SessionId);
                                var club = DataStore.Clubs.FirstOrDefault(c => c.Id == session?.ClubId);
                                string statusStr = a.Status == AttendanceStatus.Present ? "✅ Був" : "❌ Прогуляв";
                                Console.WriteLine($"  [{session?.Date.ToShortDateString()}] {club?.Name} | {statusStr}");
                            }
                        }
                        break;

                    default:
                        Console.WriteLine("❌ Невірний пункт меню.");
                        break;
                }
                Console.WriteLine("\nНатисніть будь-яку клавішу...");
                Console.ReadKey();
            }
        }
        static void TeacherMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- ПАНЕЛЬ ВИКЛАДАЧА ---");
                Console.WriteLine("1. Відмітити відвідуваність на занятті");
                Console.WriteLine("2. Список учнів у гуртку");
                Console.WriteLine("3. Дізнатися телефон учня (якщо не прийшов)");
                Console.WriteLine("4. Статистика відвідуваності гуртка");
                Console.WriteLine("0. Назад");

                Console.Write("\nДія: ");
                string choice = Console.ReadLine() ?? "";
                if (choice == "0") return;

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("\n--- Доступні заняття ---");
                        foreach (var s in DataStore.Sessions)
                        {
                            var club = DataStore.Clubs.FirstOrDefault(c => c.Id == s.ClubId);
                            Console.WriteLine($"  [ID: {s.Id}] {club?.Name} | {s.Date.ToShortDateString()} {s.StartTime} | Кімн. {s.Room}");
                        }

                        int sessId = Validator.ReadInt("Введіть ID сесії: ");
                        var session = DataStore.Sessions.FirstOrDefault(s => s.Id == sessId);

                        if (session == null)
                        {
                            Console.WriteLine("❌ Сесію з таким ID не знайдено.");
                            break;
                        }

                        Console.WriteLine("\n--- Учні в цьому гуртку ---");
                        var students = _clubService.GetStudentsInClub(session.ClubId);
                        if (students.Count == 0)
                        {
                            Console.WriteLine("У цьому гуртку ще немає учнів.");
                            break;
                        }

                        foreach (var st in students)
                            Console.WriteLine($"  [ID: {st.Id}] {st.FullName}");

                        Console.Write("\nВведіть ID присутніх через пробіл (або Enter якщо всі відсутні): ");
                        string line = Console.ReadLine() ?? "";
                        List<int> present = new List<int>();
                        foreach (var part in line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                            if (int.TryParse(part, out int parsed)) present.Add(parsed);
                            else Console.WriteLine($"  ⚠️ '{part}' — не число, пропущено.");

                        _attendanceService.MarkSessionAttendance(sessId, present);
                        Console.WriteLine("✅ Відвідуваність збережена!");
                        break;

                    case "2":
                        Console.WriteLine("\n--- Гуртки ---");
                        foreach (var c in _clubService.GetAll())
                            Console.WriteLine($"  [ID: {c.Id}] {c.Name}");

                        int clubId = Validator.ReadInt("Введіть ID гуртка: ");
                        var clubStudents = _clubService.GetStudentsInClub(clubId);

                        if (clubStudents.Count == 0)
                            Console.WriteLine("У цьому гуртку поки немає учнів.");
                        else
                            foreach (var s in clubStudents)
                                Console.WriteLine($"  [ID: {s.Id}] {s.FullName} | Тел: {s.Phone ?? "—"}");
                        break;

                    case "3":
                        Console.WriteLine("\n--- Всі учні ---");
                        foreach (var s in DataStore.Students)
                            Console.WriteLine($"  [ID: {s.Id}] {s.FullName}");

                        int stId = Validator.ReadInt("Введіть ID учня: ");
                        var student = DataStore.Students.FirstOrDefault(s => s.Id == stId);

                        if (student == null)
                            Console.WriteLine("❌ Учня з таким ID не знайдено.");
                        else
                            Console.WriteLine($"  Учень: {student.FullName} | Телефон: {student.Phone ?? "не вказано"} | Email: {student.Email ?? "не вказано"}");
                        break;

                    case "4":
                        Console.WriteLine("\n--- Гуртки ---");
                        foreach (var c in _clubService.GetAll())
                            Console.WriteLine($"  [ID: {c.Id}] {c.Name}");

                        int statId = Validator.ReadInt("Введіть ID гуртка для статистики: ");
                        PrintAttendanceStats(statId);
                        break;

                    default:
                        Console.WriteLine("❌ Невірний пункт меню.");
                        break;
                }
                Console.WriteLine("\nНатисніть будь-яку клавішу...");
                Console.ReadKey();
            }
        }

        static void AdminMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- ПАНЕЛЬ АДМІНІСТРАТОРА ---");
                Console.WriteLine("1. Додати гурток");
                Console.WriteLine("2. Створити заняття (сесію)");
                Console.WriteLine("3. Управління викладачами (Список / Додати / Редагувати)");
                Console.WriteLine("4. Змінити кабінет для заняття");
                Console.WriteLine("5. Статистика відвідуваності гуртка");
                Console.WriteLine("0. Назад");

                Console.Write("\nВаш вибір: ");
                string choice = Console.ReadLine() ?? "";
                if (choice == "0") return;

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("\n--- Додавання гуртка ---");
                        string name = Validator.ReadString("Назва гуртка: ");
                        string desc = Validator.ReadString("Опис: ");
                        string sched = Validator.ReadString("Розклад (наприклад: Пн-Ср 18:00): ");
                        string addr = Validator.ReadString("Адреса: ");

                        Console.WriteLine("\n--- Викладачі ---");
                        foreach (var t in _teacherService.GetAll())
                            Console.WriteLine($"  [ID: {t.Id}] {t.FullName} | {t.Specialization}");

                        int tId = Validator.ReadInt("Введіть ID викладача: ");

                        int newClubId = _clubService.AddClub(name, desc, sched, addr, tId);
                        Console.WriteLine($"✅ Гурток додано! Присвоєно ID: {newClubId}");
                        break;

                    case "2":
                        Console.WriteLine("\n--- Гуртки ---");
                        foreach (var c in _clubService.GetAll())
                            Console.WriteLine($"  [ID: {c.Id}] {c.Name}");

                        int cid = Validator.ReadInt("Введіть ID гуртка: ");
                        string room = Validator.ReadString("Кімната: ");
                        DateTime date = Validator.ReadDate("Дата (РРРР-ММ-ДД): ");

                        TimeSpan start = Validator.ReadTime("Час початку (ГГ:ХХ): ");
                        TimeSpan end;
                        while (true)
                        {
                            end = Validator.ReadTime("Час закінчення (ГГ:ХХ): ");
                            if (end > start) break;
                            Console.WriteLine("❌ Час закінчення має бути пізніше початку!");
                        }

                        int newSessId = _sessionService.AddSession(cid, room, date, start, end);
                        Console.WriteLine($"✅ Заняття створено! ID: {newSessId}");
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("--- УПРАВЛІННЯ ВИКЛАДАЧАМИ ---");
                        Console.WriteLine("1. Переглянути всіх");
                        Console.WriteLine("2. Додати нового");
                        Console.WriteLine("3. Редагувати існуючого");
                        Console.Write("\nДія: ");
                        string subChoice = Console.ReadLine() ?? "";

                        if (subChoice == "1")
                        {
                            var all = _teacherService.GetAll();
                            if (all.Count == 0) Console.WriteLine("Викладачів ще немає.");
                            else foreach (var t in all)
                                    Console.WriteLine($"  [ID: {t.Id}] {t.FullName} | {t.Specialization} | Тел: {t.Phone ?? "—"} | Email: {t.Email ?? "—"}");
                        }
                        else if (subChoice == "2")
                        {
                            string tName = Validator.ReadString("ПІБ викладача: ");
                            string tEmail = Validator.ReadEmail("Email: ");
                            string tSpec = Validator.ReadString("Спеціалізація: ");
                            string tPhone = Validator.ReadPhone("Телефон: ");

                            int newId = _teacherService.AddTeacher(tName, tEmail, tSpec, tPhone);
                            Console.WriteLine($"✅ Викладача додано! ID: {newId}");
                        }
                        else if (subChoice == "3")
                        {
                            foreach (var t in _teacherService.GetAll())
                                Console.WriteLine($"  [ID: {t.Id}] {t.FullName}");

                            int idToEdit = Validator.ReadInt("ID викладача для редагування: ");
                            string newName = Validator.ReadString("Нове ПІБ: ");
                            string newEmail = Validator.ReadEmail("Новий Email: ");
                            string newSpec = Validator.ReadString("Нова спеціалізація: ");
                            string newPhone = Validator.ReadPhone("Новий телефон: ");

                            if (_teacherService.Update(idToEdit, newName, newEmail, newSpec, newPhone))
                                Console.WriteLine("✅ Дані оновлено!");
                            else
                                Console.WriteLine("❌ Викладача з таким ID не знайдено.");
                        }
                        else
                        {
                            Console.WriteLine("❌ Невірний пункт.");
                        }
                        break;

                    case "4":
                        Console.WriteLine("\n--- Список занять ---");
                        foreach (var s in DataStore.Sessions)
                            Console.WriteLine($"  [ID: {s.Id}] {s.Date.ToShortDateString()} | Кімн. {s.Room}");

                        int sessId = Validator.ReadInt("Введіть ID заняття: ");
                        string newRoom = Validator.ReadString("Номер нового кабінету: ");

                        if (_sessionService.UpdateRoom(sessId, newRoom))
                            Console.WriteLine("✅ Кабінет змінено.");
                        else
                            Console.WriteLine("❌ Заняття з таким ID не знайдено.");
                        break;

                    case "5":
                        Console.WriteLine("\n--- Гуртки ---");
                        foreach (var c in _clubService.GetAll())
                            Console.WriteLine($"  [ID: {c.Id}] {c.Name}");

                        int stId = Validator.ReadInt("Введіть ID гуртка: ");
                        PrintAttendanceStats(stId);
                        break;

                    default:
                        Console.WriteLine("❌ Невірний пункт меню.");
                        break;
                }
                Console.WriteLine("\nНатисніть будь-яку клавішу...");
                Console.ReadKey();
            }
        }

        static void PrintAttendanceStats(int clubId)
        {
            var sessions = _sessionService.GetSessionsByClub(clubId);
            if (sessions.Count == 0)
            {
                Console.WriteLine("Для цього гуртка ще немає занять.");
                return;
            }

            Console.WriteLine("\n--- Статистика відвідуваності ---");
            int totalPresent = 0, totalAbsent = 0;

            foreach (var s in sessions)
            {
                var att = _attendanceService.GetAttendanceBySession(s.Id);
                int p = att.Count(a => a.Status == AttendanceStatus.Present);
                int ab = att.Count(a => a.Status == AttendanceStatus.Absent);
                totalPresent += p;
                totalAbsent += ab;
                Console.WriteLine($"  [{s.Date.ToShortDateString()}] Кімн. {s.Room} | ✅ Присутні: {p} | ❌ Відсутні: {ab}");
            }

            Console.WriteLine($"\n  Загалом по гуртку: ✅ {totalPresent} | ❌ {totalAbsent}");
        }
    }
}