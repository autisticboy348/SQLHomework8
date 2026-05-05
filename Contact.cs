namespace EmployeeDirectory.Models;

public class Contact
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? BirthDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Внешний ключ
    public int DepartmentId { get; set; }

    // Навигационное свойство
    public Department? Department { get; set; }
}
