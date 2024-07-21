using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.Extensions;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;
using System.Net.Mail;
using System.Net;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly LumiaDbContext _context;

        public ProductController(LumiaDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            ViewBag.TotalPage = Math.Ceiling((double)_context.Products.Count() / 3);
            ViewBag.CurrentPage = page;

            IEnumerable<Product>? products = _context.Products.Include(p => p.ProductImages)
                                                        .Include(p => p.ProductSizeColors).ThenInclude(p => p.Size)
                                                          .Include(p => p.ProductSizeColors).ThenInclude(p => p.Color)
                                                           .Include(p => p.Comments)
                                                         .AsNoTracking().Skip((page - 1) * 3).Take(3).AsEnumerable();
            return View(products);
        }

        public IActionResult Create()
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            ViewBag.Brands = _context.Brands.AsEnumerable();
            ViewBag.Vendors = _context.Vendors.AsEnumerable();
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Sizes = _context.Sizes.AsEnumerable();
            ViewBag.Colors = _context.Colors.AsEnumerable();
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductVM newProduct)
        {
            ViewBag.Brands = _context.Brands.AsEnumerable();
            ViewBag.Vendors = _context.Vendors.AsEnumerable();
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Sizes = _context.Sizes.AsEnumerable();
            ViewBag.Colors = _context.Colors.AsEnumerable();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!newProduct.HoverPhoto.IsValidFile("image/") || !newProduct.MainPhoto.IsValidFile("image/"))
            {
                ModelState.AddModelError(string.Empty, "Please choose image file");
                return View();
            }
            if (!newProduct.HoverPhoto.IsValidLength(1) || !newProduct.MainPhoto.IsValidLength(1))
            {
                ModelState.AddModelError(string.Empty, "Please choose image which size is maximum 1MB");
                return View();
            }

            Product product = new()
            {
                Name = newProduct.Name,
                MiniDesc = newProduct.MiniDesc,
                Price = newProduct.Price,
                InStock = newProduct.InStock,
                BasketPhoto = newProduct.MainPhoto.FileName
            };

            var imagefolderPath = Path.Combine(_env.WebRootPath, "images");
            foreach (var image in newProduct.Images)
            {
                if (!image.IsValidFile("image/") || !image.IsValidLength(1))
                {
                    return View();
                }
                ProductImage productImage = new()
                {
                    IsMain = false,
                    Path = await image.CreateImage(imagefolderPath, "products")
                };
                product.ProductImages.Add(productImage);
            }


            ProductImage main = new()
            {
                IsMain = true,
                Path = await newProduct.MainPhoto.CreateImage(imagefolderPath, "products")
            };
            product.ProductImages.Add(main);
            ProductImage hover = new()
            {
                IsMain = null,
                Path = await newProduct.HoverPhoto.CreateImage(imagefolderPath, "products")
            };
            product.ProductImages.Add(hover);


            foreach (int id in newProduct.VendorIds)
            {
                ProductVendor vendor = new()
                {
                    VendorId = id
                };
                product.ProductVendors.Add(vendor);
            }
            foreach (int id in newProduct.CategoriesIds)
            {
                ProductCategory cateogry = new()
                {
                    CategoryId = id
                };
                product.ProductCategories.Add(cateogry);
            }
            foreach (int id in newProduct.BrandIds)
            {
                ProductBrand brand = new()
                {
                    BrandId = id
                };
                product.ProductBrands.Add(brand);
            }

            if (newProduct.ColorsSizesQuantity is null)
            {
                ModelState.AddModelError("", "Please Select Color,Size and Quantity");
                return View();
            }
            else
            {
                string[] colorSizeQuantities = newProduct.ColorsSizesQuantity.Split(',');
                foreach (string colorSizeQuantity in colorSizeQuantities)
                {
                    string[] datas = colorSizeQuantity.Split('-');
                    ProductSizeColor productSizeColor = new()
                    {
                        SizeId = int.Parse(datas[0]),
                        ColorId = int.Parse(datas[1]),
                        Quantity = int.Parse(datas[2])
                    };
                    if (productSizeColor.Quantity > 0)
                    {
                        product.InStock = true;
                    }
                    product.ProductSizeColors.Add(productSizeColor);
                }
            }

            _context.Products.Add(product);
            _context.SaveChanges();
            //return RedirectToAction("Index", "Product");
            return RedirectToAction("SendMail", new { urlMessage = Url.Action("Create") });
        }



        public IActionResult Edit(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            if (id == 0) return BadRequest();
            ProductVM? model = EditedModel(id);
            ViewBag.Brands = _context.Brands.AsEnumerable();
            ViewBag.Vendors = _context.Vendors.AsEnumerable();
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Sizes = _context.Sizes.AsEnumerable();
            ViewBag.Colors = _context.Colors.AsEnumerable();

            List<ProductSizeColor> productSizeColors = _context.ProductSizeColors.Where(psc => psc.ProductId == id).ToList();
            ViewBag.ProducSC = productSizeColors;
            model.ProductSizeColors = productSizeColors;

            if (model is null) return BadRequest();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductVM edited)
        {
            ViewBag.Brands = _context.Brands.AsEnumerable();
            ViewBag.Vendors = _context.Vendors.AsEnumerable();
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Sizes = _context.Sizes.AsEnumerable();
            ViewBag.Colors = _context.Colors.AsEnumerable();


            ProductVM? model = EditedModel(id);//

            Product? product = await _context.Products.Include(p => p.ProductImages).
                Include(pc => pc.ProductCategories).Include(pt => pt.ProductVendors).
                    Include(psc => psc.ProductSizeColors).
                        ThenInclude(pc => pc.Size).
                          Include(psc => psc.ProductSizeColors).
                        ThenInclude(pc => pc.Color).
                        Include(p => p.ProductBrands).
                    FirstOrDefaultAsync(p => p.Id == id);

            if (product is null) return BadRequest();

            IEnumerable<string> removables = product.ProductImages.Where(p => !edited.ImagesId.Contains(p.Id)).Select(i => i.Path).AsEnumerable();
            var imagefolderPath = Path.Combine(_env.WebRootPath, "images");

            foreach (string removable in removables)
            {
                string path = Path.Combine(imagefolderPath, "products", removable);
                ExtensionMethods.DeleteImage(path);
            }

            if (edited.MainPhoto is not null)
            {
                if (!edited.MainPhoto.IsValidFile("image/"))
                {
                    ModelState.AddModelError(string.Empty, "Please choose image file");
                    return View();
                }
                if (!edited.MainPhoto.IsValidLength(2))
                {
                    ModelState.AddModelError(string.Empty, "Please choose image which size is maximum 2MB");
                    return View();
                }
                await AdjustPlantPhoto(true, edited.MainPhoto, product);
            }
            if (edited.HoverPhoto is not null)
            {
                if (!edited.HoverPhoto.IsValidFile("image/"))
                {
                    ModelState.AddModelError(string.Empty, "Please choose image file");
                    return View();
                }
                if (!edited.HoverPhoto.IsValidLength(2))
                {
                    ModelState.AddModelError(string.Empty, "Please choose image which size is maximum 2MB");
                    return View();
                }
                await AdjustPlantPhoto(null, edited.HoverPhoto, product);
            }

            product.ProductImages.RemoveAll(p => !edited.ImagesId.Contains(p.Id));
            if (edited.Images is not null)
            {
                foreach (var item in edited.Images)
                {
                    if (!item.IsValidFile("image/") || !item.IsValidLength(2))
                    {
                        TempData["NonSelect"] += item.FileName;
                        continue;
                    }
                    ProductImage productImage = new()
                    {
                        IsMain = false,
                        Path = await item.CreateImage(imagefolderPath, "products")
                    };
                    product.ProductImages.Add(productImage);
                }
            }





            // Brand Edit
            product.ProductBrands.Clear();
            if (edited.BrandIds != null)
            {
                foreach (int brandId in edited.BrandIds)
                {
                    Brand brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == brandId);
                    if (brand is not null)
                    {
                        ProductBrand productBrand = new ProductBrand { Brand = brand };
                        product.ProductBrands.Add(productBrand);
                    }
                }
            }

            // Category Edit
            product.ProductCategories.Clear();
            if (edited.CategoriesIds != null)
            {
                foreach (int categoryId in edited.CategoriesIds)
                {
                    Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                    if (category is not null)
                    {
                        ProductCategory productCategory = new ProductCategory { Category = category };
                        product.ProductCategories.Add(productCategory);
                    }
                }
            }
            // Vendor Edit

            if (edited.VendorIds != null)
            {
                product.ProductVendors.RemoveAll(pt => !edited.VendorIds.Contains(pt.VendorId));
                foreach (int vendorid in edited.VendorIds)
                {
                    Vendor tag = await _context.Vendors.FirstOrDefaultAsync(c => c.Id == vendorid);
                    if (tag is not null)
                    {
                        ProductVendor productTag = new() { Vendor = tag };
                        product.ProductVendors.Add(productTag);
                    }
                }
            }


            if (edited.ColorsSizesQuantity is not null)
            {
                string[] colorSizeQuantities = edited.ColorsSizesQuantity.Split(',');
                foreach (string colorSizeQuantityLoop in colorSizeQuantities)
                {
                    string[] datas = colorSizeQuantityLoop.Split('-');
                    ProductSizeColor productSizeColor = new()
                    {
                        SizeId = int.Parse(datas[0]),
                        ColorId = int.Parse(datas[1]),
                        Quantity = int.Parse(datas[2])
                    };
                    if (productSizeColor.Quantity <= 0)
                    {
                        product.InStock = false;
                    }
                    var existingItem = product.ProductSizeColors.FirstOrDefault(p => p.SizeId == productSizeColor.SizeId && p.ColorId == productSizeColor.ColorId);
                    if (existingItem != null)
                    {
                        existingItem.Quantity = productSizeColor.Quantity;
                    }
                    else
                    {
                        product.ProductSizeColors.Add(productSizeColor);
                    }
                }
            }




            if (edited.ProductSizeColorDelete is not null)
            {
                string[] plantSizeColorsToDeleteIds = edited.ProductSizeColorDelete.Split(',');
                foreach (string rid in plantSizeColorsToDeleteIds)
                {
                    int productSizeColorId = int.Parse(rid);
                    var itemToDelete = product.ProductSizeColors.FirstOrDefault(p => p.Id == productSizeColorId);
                    if (itemToDelete != null)
                    {
                        product.ProductSizeColors.Remove(itemToDelete);
                    }

                }
            }

            product.Name = edited.Name;
            product.Price = edited.Price;
            product.MiniDesc = edited.MiniDesc;
            product.InStock = edited.InStock;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private ProductVM? EditedModel(int id)
        {
            ProductVM? model = _context.Products.Include(p => p.ProductBrands)
                                               .Include(P => P.ProductCategories)
                                            .Include(p => p.ProductSizeColors).ThenInclude(psc => psc.Color)
                                            .Include(p => p.ProductVendors)
                                            .Include(p => p.ProductImages).
                                            Include(p => p.ProductSizeColors).ThenInclude(pss => pss.Size)
                                            .Select(p =>
                                                new ProductVM
                                                {
                                                    Id = p.Id,
                                                    Name = p.Name,
                                                    InStock = p.InStock,
                                                    MiniDesc = p.MiniDesc,
                                                    Price = p.Price,
                                                    CategoriesIds = p.ProductCategories.Select(pc => pc.CategoryId).ToList(),
                                                    VendorIds = p.ProductVendors.Select(pc => pc.VendorId).ToList(),
                                                    BrandIds = p.ProductBrands.Select(pc => pc.BrandId).ToList(),
                                                    AllImages = p.ProductImages.Select(pi => new ProductImage
                                                    {
                                                        Id = pi.Id,
                                                        Path = pi.Path,
                                                        IsMain = pi.IsMain
                                                    }).ToList(),
                                                    ProductSizeColors = p.ProductSizeColors.Select(psc => new ProductSizeColor
                                                    {
                                                        ColorId = psc.ColorId,
                                                        SizeId = psc.SizeId,
                                                        Quantity = psc.Quantity
                                                    }).ToList()
                                                })
                                                .FirstOrDefault(p => p.Id == id);
            return model;
        }

        private async Task AdjustPlantPhoto(bool? ismain, IFormFile image, Product product)
        {
            var imagefolderPath = Path.Combine(_env.WebRootPath, "images");
            string filepath = Path.Combine(imagefolderPath, "products", product.ProductImages.FirstOrDefault(p => p.IsMain == ismain).Path);
            ExtensionMethods.DeleteImage(filepath);
            product.ProductImages.FirstOrDefault(p => p.IsMain == ismain).Path = await image.CreateImage(imagefolderPath, "products");
        }
        public IActionResult SendMail(string urlMessage)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            List<Subscriber> subscribes = _context.Subscribers.ToList();
            foreach (Subscriber email in subscribes)
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("hellojob440@gmail.com", "Lumia");
                mailMessage.To.Add(new MailAddress(email.Email));


                mailMessage.Subject = "New Product";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = string.Empty;
                string body = string.Empty;
                using (StreamReader reader = new StreamReader("wwwroot/assets/template/newproduct.html"))
                {
                    body = reader.ReadToEnd();
                }
                mailMessage.Body = body;



                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 587;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("hellojob440@gmail.com", "eomddhluuxosvnoy");
                smtpClient.Send(mailMessage);

            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            var product = _context.Products.FirstOrDefault(c => c.Id == id);

            if (product == null)
            {

                return NotFound();
            }


            return View(product);
        }

        public IActionResult DeleteConfirmed(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            var product = _context.Products.FirstOrDefault(c => c.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Comments(int ProductId)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            if (!_context.Comments.Any(c => c.ProductId == ProductId)) return RedirectToAction("Index", "Product");

            List<Comment> comment = _context.Comments.Include(c => c.User).Where(c => c.ProductId == ProductId).ToList();
            return View(comment);
        }
        public IActionResult CommentAccept(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            if (!_context.Comments.Any(c => c.Id == id)) return RedirectToAction("Index", "Product");
            Comment comment = _context.Comments.SingleOrDefault(c => c.Id == id);
            comment.IsAccess = true;
            _context.SaveChanges();
            return RedirectToAction("Comments", "Product", new { ProductId = comment.ProductId });
        }
        public IActionResult CommentReject(int id)
        {
            var blogaccept = _context.Blogs.Where(c => c.Accept == false).ToList();
            ViewBag.Blogaccept = blogaccept;
            if (!_context.Comments.Any(c => c.Id == id)) return RedirectToAction("Index", "Product");
            Comment comment = _context.Comments.SingleOrDefault(c => c.Id == id);
            comment.IsAccess = false;
            _context.SaveChanges();
            return RedirectToAction("Comments", "Product", new { ProductId = comment.ProductId });
        }
    }
}

