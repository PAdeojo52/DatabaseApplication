using DatabaseApplication.Pages.PageRouting;
using DatabaseApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseApplication.Pages
{
    public class DashboardModel : PageModel
    {
public string? RequestId { get; set; }

  public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

  private readonly ILogger<DashboardModel> _logger;

  public DashboardModel(ILogger<DashboardModel> logger)
  {
      _logger = logger;
  }

  public void OnGet()
  {
     
  }
    }
}