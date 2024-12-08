using DatabaseApplication.Pages.PageRouting;
using DatabaseApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseApplication.Pages
{
    public class ContactModel : PageModel
    {
public string? RequestId { get; set; }

  public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

  private readonly ILogger<ContactModel> _logger;

  public ContactModel(ILogger<ContactModel> logger)
  {
      _logger = logger;
  }

  public void OnGet()
  {
     
  }
    }
}