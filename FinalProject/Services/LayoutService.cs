using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FinalProject.Services
{
    public class LayoutService
    {
        private readonly LumiaDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public LayoutService(LumiaDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }
        public List<Setting> GetSettings()
        {
            List<Setting> settings = _context.Settings.ToList();
            return settings;
        }

        public BasketVM ShowBasket()
        {
            string basket = _accessor.HttpContext.Request.Cookies["Basket"];
            BasketVM basketVM = new BasketVM();
            BasketVM basketData = new BasketVM()
            {
                TotalPrice = 0,
                BasketItems = new List<BasketItemVM>(),
                Count = 0
            };
            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {

                List<BasketItem> basketItems = _context.BasketItems.Include(b => b.User).Where(b => b.User.UserName == _accessor.HttpContext.User.Identity.Name).ToList();
                foreach (BasketItem item in basketItems)
                {
                    Product product = _context.Products.FirstOrDefault(f => f.Id == item.ProductId);
                    if (product != null)
                    {
                        BasketItemVM basketItemVM = new BasketItemVM()
                        {
                            Product = product,
                            Count = item.Count
                        };
                        basketItemVM.Product.Price = product.Price;///
						basketData.BasketItems.Add(basketItemVM);
                        basketData.Count++;
                        basketData.TotalPrice += item.Product.Price * item.Count;///
					}
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(basket))
                {
                    basketVM = JsonConvert.DeserializeObject<BasketVM>(basket);
                    foreach (BasketItemVM item in basketVM.BasketItems)
                    {
                        item.Product = _context.Products.FirstOrDefault(s => s.Id == item.Product.Id);
                        if (item.Product != null)
                        {
                            basketData.BasketItems.Add(item);
                            basketData.TotalPrice += item.Product.Price * item.Count;

                        }
                        basketData.Count++;
                    }

                }
            }

            return basketData;
        }

        public WhistListVM ShowList()
        {
            string basket = _accessor.HttpContext.Request.Cookies["List"];
            WhistListVM basketVM = new WhistListVM();
            WhistListVM data = new WhistListVM()
            {
                WhislistItemVMs = new List<WhislistItemVM>(),
                Count = 0
            };
            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {

                List<WhislistItem> basketItems = _context.WhislistItemVMs.Include(b => b.User).Where(b => b.User.UserName == _accessor.HttpContext.User.Identity.Name).ToList();
                foreach (WhislistItem item in basketItems)
                {
                    Product product = _context.Products.FirstOrDefault(f => f.Id == item.ProductId);
                    if (product != null)
                    {
                        WhislistItemVM basketItemVM = new WhislistItemVM()
                        {
                            Product = product,
                            Count = item.Count
                        };
                        basketItemVM.Product.Price = product.Price;///
						data.WhislistItemVMs.Add(basketItemVM);
                        data.Count++;
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(basket))
                {
                    basketVM = JsonConvert.DeserializeObject<WhistListVM>(basket);
                    foreach (WhislistItemVM item in basketVM.WhislistItemVMs)
                    {
                        item.Product = _context.Products.FirstOrDefault(s => s.Id == item.Product.Id);
                        if (item.Product != null)
                        {
                            data.WhislistItemVMs.Add(item);

                        }
                        data.Count++;
                    }

                }
            }

            return data;
        }

    }
}
