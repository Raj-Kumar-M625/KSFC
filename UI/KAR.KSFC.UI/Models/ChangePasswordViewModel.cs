using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.UI.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Current Password")]
        [Required(ErrorMessage = "Current Password is required")]
        [StringLength(12, ErrorMessage = "Current Password must be between 8 and 12 characters", MinimumLength = 8)]
        public string CurrentPassword { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "New Password is required")]
        [StringLength(12, ErrorMessage = "New Password must be between 8 and 12 characters", MinimumLength = 8)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm New Password")]
        [Required(ErrorMessage = "Confirm New Password is required")]
        [StringLength(12, ErrorMessage = "Confirm New Password must be between 8 and 12 characters", MinimumLength = 8)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}
