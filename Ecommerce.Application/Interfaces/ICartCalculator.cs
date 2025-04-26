//Logica de calculo de totales del carrito
using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
	public interface ICartCalculator
	{
		void CalculateTotal(Cart cart);
	}
}