using GreenSale.Domain.Entites.BuyerPosts;
using GreenSale.Domain.Enums.BuyerPosts;
using System.ComponentModel;

namespace GreenSale.DataAccess.ViewModels.BuyerPosts
{
    public class BuyerPostViewModel
    {
        public long Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public long UserId { get; set; }
        public string UserPhoneNumber { get; set; } = string.Empty;
        public string PostPhoneNumber { get; set; } = string.Empty;
        public long CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Capacity { get; set; }
        public string CapacityMeasure { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public BuyerPostEnum Status { get; set; }
        public double AverageStars { get; set; }
        public int UserStars { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<BuyerPostImage> BuyerPostsImages { get; set; }
        public string MainImage { get; set; } = string.Empty;
    }
}