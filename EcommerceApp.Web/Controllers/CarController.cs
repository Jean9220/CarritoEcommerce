// Maneja las solicitudes HTTP y delega las solicitudes del servicio
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

[Route("cart")]
public class CartController : Controller
{
	private readonly ICartQuery _cartQuery;
	private readonly ICartCommand _cartCommand;

	// Se inyectan las dependencias de las interfaces ICartQuery e ICartCommand
	public CartController(ICartQuery cartQuery, ICartCommand cartCommand)
	{
		_cartQuery = cartQuery;
		_cartCommand = cartCommand;
	}

	// Simulación: null para invitado
	private int? GetUserId() => null;

	// Este es un ID autogenerado por el sistema de ASP.NET Core que sirve para identificar la sesión del usuario.
	// Si el usuario no está autenticado, con este ID se podrá guardar y recuperar el carrito.
	private string GetSessionId() => HttpContext.Session.Id;

	[HttpGet("view")]
	public async Task<IActionResult> ViewCart()
	{
		var cart = await _cartQuery.ObtenerCarritoAsync(GetUserId(), GetSessionId());
		return View(cart);
	}

	[HttpPost("add")]
	public async Task<IActionResult> AddProduct(int productId, int cantidad)
	{
		var producto = await _cartQuery.ObtenerProductoAsync(productId); 
		if (producto == null)
		{
			return NotFound("Producto no encontrado.");
		}

		await _cartCommand.AgregarProductoAsync(GetUserId(), GetSessionId(), producto, cantidad);
		return RedirectToAction("ViewCart");
	}

	[HttpPost("remove")]
	public async Task<IActionResult> RemoveProduct(int productId)
	{
		await _cartCommand.EliminarProductoAsync(GetUserId(), GetSessionId(), productId);
		return RedirectToAction("ViewCart");
	}

	[HttpPost("update")]
	public async Task<IActionResult> UpdateQuantity(int productId, int cantidad)
	{
		await _cartCommand.ActualizarCantidadAsync(GetUserId(), GetSessionId(), productId, cantidad);
		return RedirectToAction("ViewCart");
	}
}
