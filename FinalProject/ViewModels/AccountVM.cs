﻿using FinalProject.Entities;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class AccountVM
    {
        public User User { get; set; }
        public string Token { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}
