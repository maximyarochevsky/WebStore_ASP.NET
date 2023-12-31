﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Domain.ViewModels.Identity
{
	public class LoginViewModel
	{
		[Required]
		[Display(Name = "Имя пользователя")]
		public string UserName { get; set; }
		[Required]
		[Display(Name = "Пароль")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Display(Name = "Запомнить")]
		public bool RememberMe { get; set; }
		[HiddenInput(DisplayValue = false)]
		public string ReturnUrl { get; set; } //адрес откуда перешли на форму входа для обратного редиректа

	}
}
