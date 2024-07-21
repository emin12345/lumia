﻿using FinalProject.ViewModels;
using System.Drawing;

namespace FinalProject.Entities
{
    public class ProductSizeColor : BaseEntity
    {
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }
        public Product? Product { get; set; } = null!;
        public Color? Color { get; set; } = null!;
        public Size? Size { get; set; } = null!;
        public List<BasketItem>? BasketItems { get; set; }

        public ProductSizeColor()
        {
            BasketItems = new();
        }
    }
}
