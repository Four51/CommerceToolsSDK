using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Four51.APISDK.Four51Address;

namespace Four51.APISDK.Services
{
	public struct Four51WebAddressAssignment
	{
		public string AddressInteropID { get; set; }
		public bool UseForShipping { get; set; }
		public bool UseForBilling { get; set; }
	}
}
