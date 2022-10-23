// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Identity;

namespace YourBrand.IdentityService.Models;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
    readonly HashSet<Role> _roles = new HashSet<Role>();
    readonly HashSet<UserRole> _userRoles = new HashSet<UserRole>();

    public IReadOnlyCollection<Role> Roles => _roles;

    public IReadOnlyCollection<UserRole> UserRoles => _userRoles;
}
