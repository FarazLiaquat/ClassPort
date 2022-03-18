using System.ComponentModel.DataAnnotations;

namespace ClassPort.Web.Areas.Identity.Models.AccountViewModels;

public class ExternalLoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}