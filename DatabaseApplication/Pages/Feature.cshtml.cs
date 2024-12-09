using DatabaseApplication.Pages.PageRouting;
using DatabaseApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseApplication.Pages
{
    public class FeatureModel : PageModel
    {
public string? RequestId { get; set; }

  public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

  private readonly ILogger<FeatureModel> _logger;

  public FeatureModel(ILogger<FeatureModel> logger)
  {
      _logger = logger;
  }

  public void OnGet()
  {
     
  }
    }
}