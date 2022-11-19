using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChartJsRazor.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();

    private static IEnumerable<Student> GetStudents()
    {
        string[] p = { Directory.GetCurrentDirectory(), "Data", "students.csv" };
        var csvFilePath = Path.Combine(p);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToLower(),
        };

        var data = new List<Student>().AsEnumerable();
        using (var reader = new StreamReader(csvFilePath))
        {
            using (var csvReader = new CsvReader(reader, config))
            {
                data = (csvReader.GetRecords<Student>()).ToList();
            }
        }

        return data;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Student>().HasData(GetStudents());
    }

}
