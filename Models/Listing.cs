using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auction.Models
{
    public class Listing
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public string? ImagePath { get; set; }
        public bool IsSold { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }

        [Required]
        public string? IdentityUserId { get; set; }
        [ForeignKey("IdentityUserId")]
        public IdentityUser? user { get; set; }

        public List<Bid>? Bids { get; set; }
        public List<Comment>? Comments { get;set; }
    }
}
