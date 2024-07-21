using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class ShopController : Controller
    {
        private readonly LumiaDbContext _context;

        public ShopController(LumiaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var sizecolor = _context.Products.Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).ToList();
            ViewBag.SizeColor = sizecolor;
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> Index(string filter, string? men = null, string? women = null, string? teens = null, string? sneakers = null, int page = 1)
        {
            List<Product> products = _context.Products
                .Include(p => p.ProductImages).Include(p => p.ProductCategories).ThenInclude(p => p.Category).Include(p => p.ProductSizeColors)
                .ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).Include(p => p.ProductBrands).ThenInclude(p => p.Brand).Include(p => p.ProductVendors).ThenInclude(p => p.Vendor).OrderByDescending(p => p.Id).ToList();


            switch (filter)
            {
                case "men":
                    products = products.Where(x => x.ProductCategories.Any(x => x.Category.Name == "Men")).ToList();
                    break;
                case "women":
                    products = products.Where(x => x.ProductCategories.Any(x => x.Category.Name == "Women")).ToList();
                    break;
                case "teens":
                    products = products.Where(x => x.ProductCategories.Any(x => x.Category.Name == "Teens")).ToList();
                    break;
                case "sneakers":
                    products = products.Where(x => x.ProductCategories.Any(x => x.Category.Name == "Sneakers")).ToList();
                    break;
            }




            ViewBag.TotalPage = Math.Ceiling((double)_context.Products.Count() / 8);
            ViewBag.CurrentPage = page;

            ViewBag.NewArrival = products.Skip((page - 1) * 8).Take(8).ToList();
            var sizecolor = _context.Products.Include(p => p.ProductSizeColors).ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).ToList();
            ViewBag.SizeColor = sizecolor;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FilterData(int[] colorIds, int[] sizeIds, int[] brandIds, int[] vendorIds)
        {
            List<Product> filteredProducts = await GetFilteredData(colorIds, sizeIds, brandIds, vendorIds);
            return PartialView("_ShopPartial", filteredProducts);
        }

        public async Task<List<Product>> GetFilteredData(int[] colorIds, int[] sizeIds, int[] brandIds, int[] vendorIds)
        {
            IQueryable<Product> allProducts = GetAllProducts();
            List<Product> filteredProducts = ApplyFilters(allProducts, colorIds, sizeIds, brandIds, vendorIds);
            return filteredProducts.ToList();
        }

        public IQueryable<Product> GetAllProducts()
        {
            IQueryable<Product> products = _context.Products
                .Include(p => p.ProductImages).Include(p => p.ProductCategories).ThenInclude(p => p.Category).Include(p => p.ProductSizeColors)
                .ThenInclude(p => p.Color).Include(p => p.ProductSizeColors).ThenInclude(p => p.Size).Include(p => p.ProductBrands).ThenInclude(p => p.Brand).AsNoTracking();

            return products;
        }

        private List<Product> ApplyFilters(IQueryable<Product> products, int[] colorIds, int[] sizeIds, int[] brandIds, int[] vendorIds)
        {
            if (colorIds?.Length > 0)
            {
                products = products
                   .Include(p => p.ProductSizeColors)
                      .ThenInclude(psc => psc.Color)
                        .Where(p => p.ProductSizeColors.Any(psc => colorIds.Contains(psc.Color.Id)));

            }
            if (sizeIds?.Length > 0)
            {
                products = products
                   .Include(p => p.ProductSizeColors)
                      .ThenInclude(psc => psc.Size)
                        .Where(p => p.ProductSizeColors.Any(psc => sizeIds.Contains(psc.Size.Id)));

            }
            if (brandIds?.Length > 0)
            {
                products = products
                   .Include(p => p.ProductBrands)
                      .ThenInclude(psc => psc.Brand)
                        .Where(p => p.ProductBrands.Any(psc => brandIds.Contains(psc.Brand.Id)));

            }
            if (vendorIds?.Length > 0)
            {
                products = products
                   .Include(p => p.ProductVendors)
                      .ThenInclude(psc => psc.Vendor)
                        .Where(p => p.ProductVendors.Any(psc => vendorIds.Contains(psc.Vendor.Id)));

            }
            return products.ToList();
        }
    }
}
