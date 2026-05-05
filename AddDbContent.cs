using Microsoft.EntityFrameworkCore;
using EmployeeDirectory.Models;

namespace EmployeeDirectory.Data;

public class AppDbContext : DbContext
{
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Department> Departments => Set<Department>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // База данных будет создана в папке с запущенным приложением
        var dbPath = Path.Combine(AppContext.BaseDirectory, "employees.db");
        
        optionsBuilder
            .UseSqlite($"Data Source={dbPath}")
            .UseSeeding((context, _) =>
            {
                // Заполнение отделов, если их нет
                if (!context.Set<Department>().Any())
                {
                    var dev = new Department { Name = "Разработка" };
                    var qa  = new Department { Name = "Тестирование" };
                    var hr  = new Department { Name = "HR" };
                    
                    context.Set<Department>().AddRange(dev, qa, hr);
                    context.SaveChanges();
                }

                // Заполнение контактов, если их нет
                if (!context.Set<Contact>().Any())
                {
                    context.Set<Contact>().AddRange(
                        new Contact { FullName = "Анна Петрова",   Email = "anna@test.com",  DepartmentId = 1, IsActive = true  },
                        new Contact { FullName = "Иван Соколов",   Email = "ivan@test.com",  DepartmentId = 1, IsActive = true  },
                        new Contact { FullName = "Мария Орлова",   Email = "maria@test.com", DepartmentId = 2, IsActive = false },
                        new Contact { FullName = "Алексей Новиков", Email = "alex@test.com",  DepartmentId = 2, IsActive = true  },
                        new Contact { FullName = "Елена Козлова",  Email = "elena@test.com", DepartmentId = 3, IsActive = true  },
                        new Contact { FullName = "Дмитрий Фёдоров", Email = "dima@test.com",  DepartmentId = 3, IsActive = false }
                    );
                    context.SaveChanges();
                }
            });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Описание связи Один-ко-Многим
        modelBuilder.Entity<Contact>()
            .HasOne(c => c.Department)
            .WithMany(d => d.Contacts)
            .HasForeignKey(c => c.DepartmentId);
    }
}
