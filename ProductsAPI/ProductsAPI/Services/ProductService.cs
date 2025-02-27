using Microsoft.EntityFrameworkCore;
using ProductsAPI.Data;
using ProductsAPI.Models;

namespace ProductsAPI.Services
{

    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly INotifyService _notifyService;

        public ProductService(AppDbContext context, INotifyService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        public async Task<(bool IsValid, string ErrorMessage)> ValidateProductAsync(Product product)
        {
            // Check if SKU is unique
            var existingProduct = await _context.Products.AnyAsync(p => p.SKU == product.SKU);

            if (existingProduct)
            {
                return (false, "SKU must be unique.");
            }

            // Check if BuyerId exists
            var buyerExists = await _context.Buyers.AnyAsync(b => b.Id == product.BuyerId);

            if (!buyerExists)
            {
                return (false, "BuyerId does not exist.");
            }

            return (true, string.Empty);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            var validation = await ValidateProductAsync(product);

            if (!validation.IsValid)
            {
                throw new System.Exception(validation.ErrorMessage);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> UpdateProductAsync(int id, Product updatedProduct)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return null;
            }

            // Check if BuyerId exists
            var newBuyer = await _context.Buyers.FirstOrDefaultAsync(b => b.Id == updatedProduct.BuyerId);
            if (newBuyer == null)
            {
                throw new System.Exception("BuyerId does not exist.");
            }

            // Check if Product de-activated

            bool deactivated = false;
            if ((updatedProduct.Active != product.Active) && (updatedProduct.Active == false))
            {
                deactivated = true;
            }

            var oldBuyer = await _context.Buyers.FirstOrDefaultAsync(b => b.Id == product.BuyerId);

            product.Title = updatedProduct.Title;
            product.Description = updatedProduct.Description;
            product.BuyerId = updatedProduct.BuyerId;
            product.Active = updatedProduct.Active;

            await _context.SaveChangesAsync();

            if (newBuyer != oldBuyer && oldBuyer != null)
            {
                _notifyService.Notify(oldBuyer.Email, $"Product '{product.SKU}' has been reassigned to a new buyer.");
                _notifyService.Notify(newBuyer.Email, $"You have been assigned a new product: '{product.SKU}'");
            }

            if (deactivated)
            {
                _notifyService.Notify(newBuyer.Email, $"Product {product.SKU} has been deactivated.");
            }

            return product;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return false;
            }

            // Check if BuyerId exists
            var buyer = await _context.Buyers.FirstOrDefaultAsync(b => b.Id == product.BuyerId);
            if (buyer != null)
            {
                _notifyService.Notify(buyer.Email, $"Product {product.SKU} has been deactivated.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}