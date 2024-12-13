﻿namespace ProniaProject.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser{ get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal Price { get; set; }

    }
}