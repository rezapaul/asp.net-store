using Microsoft.AspNetCore.Mvc;
using MyStore.Application.Services.Wishlists.Commands.IAddToWishlistService;
using MyStore.Application.Services.Wishlists.Queries.IGetWishlistsService;
using Microsoft.AspNetCore.Authorization;
using MyStore.Application.Services.Wishlists.Commands.IDeleteFromWishList;

namespace EndPoint.Website.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IAddToWishlistService _addToWishlist;
        private readonly IGetWishlistsService _getWishlists;
        private readonly IDeleteFromWishList _deleteFromWish;

        public UserController(IAddToWishlistService addToWishlist,
                              IGetWishlistsService getWishlists,
                              IDeleteFromWishList deleteFromWish)
        {
            _addToWishlist = addToWishlist;
            _getWishlists = getWishlists;
            _deleteFromWish = deleteFromWish;
        }

        [HttpGet]
        public IActionResult Favorite()
        {
            var result = _getWishlists.Execute(ClaimUtility.ClaimUtility.GetUserId(User));
            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Addwish(RequestAddToWishlistDto request)
        {
            request.UserId = ClaimUtility.ClaimUtility.GetUserId(User);
            var result = _addToWishlist.Execute(request);
            TempData["AlertMessage"] = result.Message;
            return RedirectToAction("Favorite");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(long WishId)
        {
            var result = _deleteFromWish.Execute(WishId);
            TempData["AlertMessage"] = result.Message;
            return RedirectToAction("Favorite");
        }
    }
}
