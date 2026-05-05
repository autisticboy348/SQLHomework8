using Microsoft.EntityFrameworkCore;
using EmployeeDirectory.Data;
using EmployeeDirectory.Models;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

using var db = new AppDbContext();

// Применяем миграции при старте
db.Database.Migrate();

Console.WriteLine("=== СПИСОК ОТДЕЛОВ ===");
ShowDepartments(db);

Console.WriteLine("\n=== ВСЕ СОТРУДНИКИ ===");
ShowContacts(db);

Console.WriteLine("\n=== ТОЛЬКО АКТИВНЫЕ ===");
ShowActiveContacts(db);

Console.WriteLine("\n=== ВЫВОД ПО ОТДЕЛУ (ID: 1) ===");
ShowContactsByDepartment(db, 1);

Console.WriteLine("\n=== ПРИМЕНЕННЫЕ МИГРАЦИИ ===");
ShowAppliedMigrations(db);

// --- Методы реализации ---

void ShowDepartments(AppDbContext ctx)
{
    var deps = ctx.Departments.ToList();
    Console.WriteLine($"Всего отделов: {deps.Count}");
    foreach (var d in deps)
    {
        Console.WriteLine($"  [{d.Id}] {d.Name}");
    }
}

void ShowContacts(AppDbContext ctx)
{
    var contacts = ctx.Contacts
        .Include(c => c.Department) // Загружаем связанные данные отдела
        .OrderBy(c => c.FullName)
        .ToList();

    foreach (var c in contacts)
    {
        Console.WriteLine($"  {c.FullName} | Отдел: {c.Department?.Name} | Активен: {c.IsActive}");
    }
}

void ShowActiveContacts(AppDbContext ctx)
{
    var active = ctx.Contacts.Where(c => c.IsActive).ToList();
    Console.WriteLine($"Активных сотрудников: {active.Count}");
    foreach (var c in active)
    {
        Console.WriteLine($"  {c.FullName}");
    }
}

void ShowContactsByDepartment(AppDbContext ctx, int departmentId)
{
    var dept = ctx.Departments.Find(departmentId);
    var contacts = ctx.Contacts
        .Where(c => c.DepartmentId == departmentId)
        .OrderBy(c => c.FullName)
        .ToList();

    Console.WriteLine($"Отдел: {dept?.Name ?? "Не найден"}");
    foreach (var c in contacts)
    {
        Console.WriteLine($"  {c.FullName} | Email: {c.Email}");
    }
}

void ShowAppliedMigrations(AppDbContext ctx)
{
    var migrations = ctx.Database.GetAppliedMigrations().ToList();
    Console.WriteLine($"Применено миграций: {migrations.Count}");
    foreach (var m in migrations)
    {
        Console.WriteLine($"  - {m}");
    }
}
