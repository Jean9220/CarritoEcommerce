using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
	// CartService implementa ambas interfaces ICartQuery (para obtener datos) e ICartCommand (para modificar datos)
	public class CartService : ICartQuery, ICartCommand
	{
		private readonly ICartRepository _repository;
		private readonly IProductRepository _productRepository;
		private readonly ICartCalculator _cartCalculator;  // Aquí estamos manteniendo la dependencia del CartCalculator

		// Constructor que inyecta las dependencias
		public CartService(ICartRepository repository, IProductRepository productRepository, ICartCalculator cartCalculator)
		{
			_repository = repository;
			_productRepository = productRepository;
			_cartCalculator = cartCalculator;  // Inyectamos el CartCalculator
		}

		// Implementación de ICartQuery - Obtener el carrito de un usuario o sesión
		public async Task<Cart> ObtenerCarritoAsync(int? userId, string sessionId)
		{
			Cart? carrito = userId.HasValue
				? await _repository.GetCartByUserIdAsync(userId.Value)
				: await _repository.GetCartBySessionIdAsync(sessionId);

			if (carrito == null)
			{
				carrito = new Cart
				{
					UserId = userId,
					SessionId = userId == null ? sessionId : null,
					CreatedDate = DateTime.Now,
					CartItems = new List<CartItem>()
				};
				await _repository.AddCartAsync(carrito);
			}
			return carrito;
		}

		// Implementación de ICartQuery - Obtener un producto por su ID
		public async Task<Product?> ObtenerProductoAsync(int productId)
		{
			return await _productRepository.GetByIdAsync(productId);
		}

		// Implementación de ICartCommand - Agregar producto al carrito
		public async Task AgregarProductoAsync(int? userId, string sessionId, Product producto, int cantidad)
		{
			var cart = await ObtenerCarritoAsync(userId, sessionId);
			var item = cart.CartItems.FirstOrDefault(x => x.ProductId == producto.ProductId);

			if (item != null)
			{
				item.Quantity += cantidad;
				item.Subtotal = item.UnitPrice * item.Quantity;
			}
			else
			{
				cart.CartItems.Add(new CartItem
				{
					ProductId = producto.ProductId,
					Quantity = cantidad,
					UnitPrice = producto.Price,
					Subtotal = producto.Price * cantidad
				});
			}

			// Usamos el CartCalculator para actualizar el total
			_cartCalculator.CalculateTotal(cart);
			await _repository.UpdateCartAsync(cart);
		}

		// Implementación de ICartCommand - Eliminar producto del carrito
		public async Task EliminarProductoAsync(int? userId, string sessionId, int productId)
		{
			var cart = await ObtenerCarritoAsync(userId, sessionId);
			var item = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);
			if (item != null)
			{
				cart.CartItems.Remove(item);
				// Usamos el CartCalculator para actualizar el total después de la eliminación
				_cartCalculator.CalculateTotal(cart);
				await _repository.UpdateCartAsync(cart);
			}
		}

		// Implementación de ICartCommand - Actualizar la cantidad de un producto en el carrito
		public async Task ActualizarCantidadAsync(int? userId, string sessionId, int productId, int nuevaCantidad)
		{
			var cart = await ObtenerCarritoAsync(userId, sessionId);
			var item = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);
			if (item != null)
			{
				item.Quantity = nuevaCantidad;
				item.Subtotal = item.UnitPrice * nuevaCantidad;

				// Usamos el CartCalculator para actualizar el total después de actualizar la cantidad
				_cartCalculator.CalculateTotal(cart);
				await _repository.UpdateCartAsync(cart);
			}
		}
	}
}