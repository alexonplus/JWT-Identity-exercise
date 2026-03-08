using Microsoft.AspNetCore.Identity;

namespace Security.Models;

// inherit from IdentityUser to get all the built-in properties like Email, PasswordHash, etc.
public class ApplicationUser : IdentityUser
{
    //here we can add any custom properties we want for our users. For 
}