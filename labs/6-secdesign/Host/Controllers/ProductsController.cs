using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecureByDesign.Host.Application;
using SecureByDesign.Host.Domain;

namespace SecureByDesign.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpGet]
        public async Task<ActionResult<Product>> GetById(string id)
        {
            if (!ProductId.IsValidId(id))
            {
                return BadRequest(); // https://stackoverflow.com/q/3290182/291299
            }

            var productId = new ProductId(id);

            var productResult = await productsService.GetById(User, productId);

            if (productResult.Result == ServiceResult.NotFound)
            {
                return NotFound();
            }

            if (productResult.Result == ServiceResult.Forbidden)
            {
                return Forbid();
            }

            return Ok(productResult.Value);
        }
    }
}