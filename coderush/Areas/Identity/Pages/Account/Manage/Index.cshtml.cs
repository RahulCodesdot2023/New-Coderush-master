using coderush.Models;
using coderush.Services.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IEmailSender _emailSender;
        private readonly IdentityDefaultOptions _identityDefaultOptions;
        private readonly SuperAdminDefaultOptions _superAdminDefaultOptions;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            //IEmailSender emailSender,
            IOptions<IdentityDefaultOptions> identityDefaultOptions,
            IOptions<SuperAdminDefaultOptions> superAdminDefaultOptions
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_emailSender = emailSender;
            _identityDefaultOptions = identityDefaultOptions.Value;
            _superAdminDefaultOptions = superAdminDefaultOptions.Value;
        }

        public string Username { get; set; }
        public bool IsEmailConfirmed { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            [Phone]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Last Name")]
            public string lastName { get; set; }
            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Date of Birth")]
            public DateTime? DOB { get; set; }
            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Date Of Joning ")]
            public DateTime? DOJ { get; set; }
            [DataType(DataType.Text)]
            [Display(Name = "Profile Pic ")]
            public string ProfilePicture { get; set; }
            [Display(Name = "Aadhar Card Frontside Photo")]
            public byte[] AadharFront { get; set; }
            [Display(Name = "Aadhar Card Backside Photo")]
            public byte[] AadharBack { get; set; }
            [Display(Name = "PAN Card Photo")]
            public byte[] PanCard { get; set; }
            [Display(Name = "Profile Picture ")]
            public byte[] ProfilePic { get; set; }

            public string PanCardNo { get; set; }
            public string AadharCardNo { get; set; }
            public string BankName { get; set; }
            public string BankBranchName { get; set; }
            public string AccountNumber { get; set; }
            public string IFSCode { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Email = email,
                FirstName = user.FirstName,
                DOB = user.DOB,
                DOJ = user.JoiningDate,
                lastName = user.LastName,
                PhoneNumber = phoneNumber,
                ProfilePicture = user.ProfilePicture,
                PanCard = user.PanCard,
                AadharFront = user.AadharFront,
                AadharBack = user.AadharBack,
                ProfilePic = user.ProfilePic,
                PanCardNo = user.PanCardNo,
                AadharCardNo = user.AadharCardNo,
                AccountNumber = user.AccountNumber,
                BankBranchName = user.BankBranchName,
                BankName = user.BankName,
                IFSCode = user.IFSCode
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnPostAsync(ApplicationUser formData)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //check to see demo or not, if demo so it can not change the super@admin.com data
            if (_identityDefaultOptions.IsDemo && _superAdminDefaultOptions.Email == user.Email)
            {
                StatusMessage = "Error: Demo mode can not change super@admin.com data.";
                return RedirectToPage();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }

                //profile change by its own, no need to verification
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
            }

            if (Input.DOB != user.DOB)
            {
                user.DOB = Input.DOB;
            }

            if (Input.DOJ != user.JoiningDate)
            {
                user.JoiningDate = Input.DOJ;
            }

            if (Input.lastName != user.LastName)
            {
                user.LastName = Input.lastName;
            }

            if (Input.PanCardNo != user.PanCardNo)
            {
                user.PanCardNo = Input.PanCardNo;
            }

            if (Input.AadharCardNo != user.AadharCardNo)
            {
                user.AadharCardNo = Input.AadharCardNo;
            }

            if (Input.BankName != user.BankName)
            {
                user.BankName = Input.BankName;
            }

            if (Input.BankBranchName != user.BankBranchName)
            {
                user.BankBranchName = Input.BankBranchName;
            }

            if (Input.IFSCode != user.IFSCode)
            {
                user.IFSCode = Input.IFSCode;
            }

            if (Input.AccountNumber != user.AccountNumber)
            {
                user.AccountNumber = Input.AccountNumber;
            }

            if (Request.Form.Files.Count > 0)
            {
                _ = new List<IFormFile>();
                List<IFormFile> file = Request.Form.Files.ToList();

                for (int i = 0; i < Request.Form.Files.Count; i++)
                {
                    var uploadefile = file[i];

                    using var dataStream = new MemoryStream();
                    uploadefile.CopyTo(dataStream);
                    if (uploadefile.Name.ToLower() == "Input.ProfilePic".ToLower())
                    {
                        user.ProfilePic = dataStream.ToArray();
                    }
                    else if (uploadefile.Name.ToLower() == "Input.AadharFront".ToLower())
                    {
                        user.AadharFront = dataStream.ToArray();
                    }
                    else if (uploadefile.Name.ToLower() == "Input.AadharBack".ToLower())
                    {
                        user.AadharBack = dataStream.ToArray();
                    }
                    else
                    {
                        user.PanCard = dataStream.ToArray();
                    }
                }
                await _userManager.UpdateAsync(user);
            }
            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            //await _emailSender.SendEmailAsync(
            //    email,
            //    "Confirm your email",
            //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
