using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using MyStore.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Products.Commands.IAddProductAdminService
{
    public interface IAddProductAdminService
    {
        ResultDto Execute(RequestAddProduct_Dto request);
    }

    public class AddProductAdminService : IAddProductAdminService
    {
        private readonly IDataBaseContext _context;

        private readonly IHostingEnvironment _environment;

        public AddProductAdminService(IDataBaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public ResultDto Execute(RequestAddProduct_Dto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return new ResultDto
                    {
                        IsSucceed = false,
                        Message = "نام محصول نمی تواند خالی باشذ "
                    };
                }

                if (string.IsNullOrWhiteSpace(request.Description))
                {
                    return new ResultDto
                    {
                        IsSucceed = false,
                        Message = "توضبحات محصول نمی تواند خالی باشد"
                    };
                }

                var category = _context.Categories.Where(p => p.Id == request.CategoryId).FirstOrDefault();

                Product product = new()
                {
                    Title = request.Title,
                    Description = request.Description,
                    Price = request.Price,
                    Category = category,
                    CategoryId = category.Id,
                    InsertTime = DateTime.Now,

                };

                _context.Products.Add(product);

                List<ProductImages> productImages = new List<ProductImages>();
                foreach (var item in request.Images)
                {
                    var uploadedResult = UploadFile(item);
                    productImages.Add(new ProductImages
                    {
                        Product = product,
                        Source = uploadedResult.FileNameAddress,
                    });
                }

                _context.ProductImages.AddRange(productImages);


                //List<ProductSizes> productSizes = new List<ProductSizes>();
                //foreach (var item in request.Sizes)
                //{
                //    productSizes.Add(new ProductSizes
                //    {
                //        Product = product,
                //        Size = item.Size,
                //    });
                //}

                //_context.ProductSizes.AddRange(productSizes);


                //List<ProductColors> productColors = new List<ProductColors>();
                //foreach (var item in request.Colors)
                //{
                //    productColors.Add(new ProductColors
                //    {
                //        Product = product,
                //        Color = item.Color
                //    });
                //}

                //_context.ProductColors.AddRange(productColors);
                _context.SaveChanges();

                return new ResultDto
                {
                    IsSucceed = true,
                    Message = "  محصول با موفقیت افزوده شد"
                };
            }
            catch(Exception)
            {
                return new ResultDto
                {
                    IsSucceed = false,
                    Message = "خطایی رخ داده"
                };
            }
        }

        private UploadDto UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string folder = $@"images\ProductImages\";
                var uploadsRootFolder = Path.Combine(_environment.WebRootPath, folder);
                if (!Directory.Exists(uploadsRootFolder))
                {
                    Directory.CreateDirectory(uploadsRootFolder);
                }


                if (file == null || file.Length == 0)
                {
                    return new UploadDto()
                    {
                        Status = false,
                        FileNameAddress = "",
                    };
                }

                string fileName = DateTime.Now.Ticks.ToString() + file.FileName;
                var filePath = Path.Combine(uploadsRootFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return new UploadDto()
                {
                    FileNameAddress = folder + fileName,
                    Status = true,
                };
            }
            return null;
        }
    }

    public class UploadDto
    {
        public long Id { get; set; }
        public bool Status { get; set; }
        public string FileNameAddress { get; set; }
    }

    public class RequestAddProduct_Dto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public long CategoryId { get; set; }
        public List<IFormFile> Images { get; set; }
        //public List<AddSizes_Dto> Sizes { get; set; }
        //public List<AddColors_Dto> Colors { get; set; }

    }

    //public class AddSizes_Dto
    //{
    //    public string Size { get; set; }
    //}

    //public class AddColors_Dto
    //{
    //    public string Color { get; set; }
    //}
}
