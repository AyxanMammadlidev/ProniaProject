﻿namespace ProniaProject.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int Count { get; set; }
        

        //Relational
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
