﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Domain.Entities.Orders;

public class Order
{
    [Required]
    public User User { get; set; } = null;

    [Required]
    [MaxLength(200)]
    public string Phone { get; set; }

    [Required]
    [MaxLength(200)]
    public string Adress { get; set; }

    public string Descripction { get; set; }

    public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;

    public ICollection<OrderItem> Items { get; set} = new List<OrderItem>();

    [NotMapped]
    public decimal TotalPrice => Items.Sum(item => item.TotalItemPrice);
}
