using System.ComponentModel.DataAnnotations;

namespace ClassPort.Web.Areas.Identity.Models.AccountViewModels;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}