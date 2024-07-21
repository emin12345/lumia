using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FinalProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly LumiaDbContext _context;
        private readonly UserManager<User> _userManager;

        public ProductController(LumiaDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Detail(int id)
        {
            if (id == 0) return RedirectToAction("Index", "NotFound404");
            Product? product = _context.Products
                                         .Include(p => p.ProductImages)
                                           .Include(p => p.ProductDescriptions)
                                             .Include(p => p.ProductVendors).ThenInclude(p => p.Vendor)
                                               .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                                                 .Include(p => p.ProductBrands).ThenInclude(p => p.Brand)
                                                 .Include(p => p.ProductSizeColors).ThenInclude(p => p.Color)
                                                 .Include(p => p.ProductSizeColors).ThenInclude(p => p.Size)
                                                  .Include(p => p.Comments)
                                                    .ThenInclude(p => p.User)
                                                   .FirstOrDefault(p => p.Id == id);
            if (product == null) RedirectToAction("Index", "NotFound404");

            var sizecolor = _context.Products.Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).ToList();
            ViewBag.SizeColor = sizecolor;


            ViewBag.NewArrival = _context.Products
                .Include(p => p.ProductImages).Where(x => x.Id != id).Take(4).ToList();
            return View(product);
        }

        public async Task<IActionResult> AddBasket(int id)
        {

            Product product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(b => b.ProductId == product.Id && b.UserId == user.Id);
                if (basketItem == null)
                {
                    basketItem = new BasketItem()
                    {
                        UserId = user.Id,
                        ProductId = product.Id,
                        Count = 1
                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                {
                    basketItem.Count++;
                }
                _context.SaveChanges();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }


        public async Task<IActionResult> GetTotalPrice()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                decimal totalPrice = _context.BasketItems
                    .Where(b => b.UserId == user.Id)
                    .Join(_context.Products,
                        basketItem => basketItem.ProductId,
                        product => product.Id,
                        (basketItem, product) => new { basketItem, product })
                    .Sum(item => item.basketItem.Count * item.product.Price);

                return Ok(totalPrice);
            }
            return Ok(0);
        }


        public IActionResult ShowProduct()
        {
            List<Product> products = _context.Products.ToList();
            return Json(products);
        }


        public IActionResult ShowBasket()
        {
            string basketStr = HttpContext.Request.Cookies["Basket"];
            if (!string.IsNullOrEmpty(basketStr))
            {
                BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(basketStr);
                return Json(basket);
            }
            return Content("Basket is empty");
        }



        public async Task<IActionResult> RemoveBasketItem(int id)
        {
            Product product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(b => b.ProductId == product.Id && b.UserId == user.Id);
                if (basketItem != null)
                {
                    if (basketItem.Count > 1)
                    {
                        basketItem.Count--;
                    }
                    else
                    {
                        _context.BasketItems.Remove(basketItem);
                    }
                    _context.SaveChanges();
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }


        [Authorize]
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ModelState.IsValid) return RedirectToAction("Detail", "Product", new { id = comment.ProductId });
            if (!_context.Products.Any(f => f.Id == comment.ProductId)) return NotFound();

            Comment cmnt = new Comment
            {
                Text = comment.Text,
                ProductId = comment.ProductId,
                CreatedTime = DateTime.Now,
                UserId = user.Id,
                IsAccess = true,



            };
            _context.Comments.Add(cmnt);
            _context.SaveChanges();
            return Redirect(Request.Headers["Referer"].ToString());
        }




        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {

            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!ModelState.IsValid) return RedirectToAction("Detail", "Product");
            if (_context.Comments.Any(c => c.Id == id && c.IsAccess == false)) return NotFound();
            Comment comment = _context.Comments.FirstOrDefault(c => c.Id == id);
            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());

        }




        public async Task<IActionResult> RemoveWhisList(int id)
        {
            Product product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                WhislistItem basketItem = _context.WhislistItemVMs.FirstOrDefault(b => b.ProductId == product.Id && b.UserId == user.Id);
                if (basketItem != null)
                {
                    if (basketItem.Count > 1)
                    {
                        basketItem.Count--;
                    }
                    else
                    {
                        _context.WhislistItemVMs.Remove(basketItem);
                    }
                    _context.SaveChanges();
                }
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> AddWhislist(int id)
        {

            Product product = _context.Products.FirstOrDefault(p => p.Id == id);



            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                WhislistItem basketItem = _context.WhislistItemVMs.FirstOrDefault(b => b.ProductId == product.Id && b.UserId == user.Id);
                if (basketItem == null)
                {
                    basketItem = new WhislistItem()
                    {
                        UserId = user.Id,
                        ProductId = product.Id,
                        Count = 1
                    };
                    _context.WhislistItemVMs.Add(basketItem);
                }
                else
                {
                    basketItem.Count++;
                }
                _context.SaveChanges();
            }
            else
            {
                string basket = HttpContext.Request.Cookies["List"];

                List<Product> products;

                if (basket == null)
                {

                    WhistListVM basketVM = new WhistListVM()
                    {
                        WhislistItemVMs = new List<WhislistItemVM>(),

                        Count = 1
                    };
                    WhislistItemVM basketItem = new WhislistItemVM
                    {
                        Product = product,
                        Count = 1
                    };
                    basketVM.WhislistItemVMs.Add(basketItem);

                    string basketStr = JsonConvert.SerializeObject(basketVM);

                    HttpContext.Response.Cookies.Append("List", basketStr);

                }
                else
                {
                    WhistListVM basketVM = JsonConvert.DeserializeObject<WhistListVM>(basket);
                    WhislistItemVM basketItem = basketVM.WhislistItemVMs.FirstOrDefault(i => i.Product.Id == product.Id);
                    if (basketItem == null)
                    {
                        basketItem = new WhislistItemVM
                        {
                            Product = product,
                            Count = 1,

                        };
                        basketVM.Count++;
                        basketVM.WhislistItemVMs.Add(basketItem);
                    }
                    else
                    {
                        basketItem.Count++;
                    }

                    string basketStr = JsonConvert.SerializeObject(basketVM);

                    HttpContext.Response.Cookies.Append("List", basketStr);
                }
            }



            return RedirectToAction("Detail", "Product", new { id = product.Id });
        }

        public IActionResult ShowWhistList()
        {
            string basketStr = HttpContext.Request.Cookies["List"];
            if (!string.IsNullOrEmpty(basketStr))
            {
                WhistListVM basket = JsonConvert.DeserializeObject<WhistListVM>(basketStr);
                return Json(basket);
            }
            return Content("List is empty");
        }

    }
}
