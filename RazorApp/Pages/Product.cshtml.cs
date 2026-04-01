using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorApp.Models;

namespace RazorApp.Pages
{
    [Authorize(Roles = "Admin,OrderAssistant")]
    public class ProductModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public List<Product> Products { get; set; } = new List<Product>();

        [BindProperty]
        public Product NewProduct { get; set; } = new Product();


        [BindProperty]
        public IFormFile NewProductImage { get; set; }

        [BindProperty]
        public Product EditProduct { get; set; } = new Product();

        [BindProperty]
        public IFormFile EditProductImage { get; set; }


        public ProductModel(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public void OnGet()
        {
            Products = _context.Product.ToList();
        }

        // CREATE
        public IActionResult OnPost()
        {
            NewProduct.ImagePath = SaveImage(NewProductImage);

            _context.Product.Add(NewProduct);
            _context.SaveChanges();
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var product = _context.Product.Find(id);

            if (product != null)
            {
                _context.Product.Remove(product);
                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostEdit()
        {
            var product = _context.Product.Find(EditProduct.ProductID);

            if (product != null)
            {
                product.ProductName = EditProduct.ProductName;
                product.Stock = EditProduct.Stock;

                if (EditProductImage != null)
                    product.ImagePath = SaveImage(EditProductImage);

                _context.SaveChanges();
            }

            return RedirectToPage();
        }
        private string SaveImage(IFormFile image)
        {
            if (image == null) return "";

            var imagesPath = Path.Combine(_env.WebRootPath, "images");
            if (!Directory.Exists(imagesPath)) Directory.CreateDirectory(imagesPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(imagesPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            image.CopyTo(stream);

            return "/images/" + fileName;
        }
    }
}
