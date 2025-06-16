using Auction.Data;
using Auction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auction.Controllers
{
    [Authorize]
    public class BidsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BidsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceBid(int listingId, double bidAmount)
        {
            var listing = await _context.Listings
                .Include(l => l.Bids)
                .FirstOrDefaultAsync(l => l.Id == listingId);

            if (listing == null)
            {
                return NotFound();
            }

            if (listing.EndTime < DateTime.Now)
            {
                TempData["BidError"] = "This auction has ended.";
                return RedirectToAction("Details", "Listings", new { id = listingId });
            }

            // Check if bid is higher than starting price or current highest bid
            var highestBid = listing.Bids.Any() ? listing.Bids.Max(b => b.Price) : listing.Price;
            if (bidAmount <= highestBid)
            {
                TempData["BidError"] = "Your bid must be higher than the current price.";
                return RedirectToAction("Details", "Listings", new { id = listingId });
            }
            
            var bid = new Bid
            {
                ListingId = listingId,
                Price = bidAmount,
                IdentityUserId = _userManager.GetUserId(User)
            };

            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Listings", new { id = listingId });
        }
    }
}