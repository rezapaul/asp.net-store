using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Wishlists.Commands.IDeleteFromWishList
{
    public interface IDeleteFromWishList
    {
        ResultDto Execute(long WishId);
    }

    public class DeleteFromWishList : IDeleteFromWishList
    {
        private readonly IDataBaseContext _context;

        public DeleteFromWishList(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(long WishId)
        {
            var wish = _context.Wishlists.Find(WishId);
            if(wish == null)
            {
                return new ResultDto
                {
                    IsSucceed = false,
                    Message = "چنین علافه مندی یافت نشد"
                };
            }

            wish.IsRemoved = true;
            wish.RemoveTime = DateTime.Now;
            _context.SaveChanges();

            return new ResultDto
            {
                IsSucceed = true,
                Message = "با موفقیت از لیست علاقه مندی شما پاک شد"
            };
        }
    }
}
