using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Four51.APISDK.Enums
{
	public enum CouponType
	{
		Order = 0,
		ProductCategory = 1,
		Decrementing = 2
	}

	public enum CouponDiscountAmountType
	{
		FlatAmountPerOrder = 0,
		FlatAmountPerQuantity,
		PercentagePerOrder,
		PercentagePerQuantity
	}
}
