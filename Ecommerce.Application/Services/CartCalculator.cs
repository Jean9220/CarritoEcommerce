//Depende de la interfas ICartCalculator
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
	public class CartCalculator : ICartCalculator
	{
		public void CalculateTotal(Cart cart)
		{
			cart.TotalAmount = cart.CartItems?.Sum(x => x.Subtotal) ?? 0;
			cart.UpdatedDate = DateTime.Now;
		}
	}
}