﻿namespace Shared.Models.Identity;

public class AppUser
{
    public string Id { get; set; } = "";
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string[] Roles { get; set; } = [];
}