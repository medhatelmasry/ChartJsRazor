using ChartJsRazor.Data;
using ChartJsRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChartJsRazor.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _context;

    [BindProperty]
    public List<SchoolCount> ChartData { get; set; } = default!;


    public IndexModel(ILogger<IndexModel> logger,
    ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public void OnGet()
    {
        List<SchoolCount> studentsInSchool = new List<SchoolCount>();

        var data = _context.Students
           .GroupBy(_ => _.School)
           .Select(g => new
           {
               Name = g.Key,
               Count = g.Count()
           })
           .OrderByDescending(cp => cp.Count)
           .ToList();

        foreach (var item in data)
        {
            studentsInSchool.Add(new SchoolCount()
            {
                Name = item.Name,
                Count = item.Count
            });
        }

        ChartData = studentsInSchool;
    }
}
