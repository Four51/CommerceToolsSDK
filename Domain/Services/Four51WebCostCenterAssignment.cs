using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Four51.APISDK.Four51Address;

namespace Four51.APISDK.Services
{
	public struct Four51WebCostCenterAssignment
	{
		public string CostCenterInteropID { get; set; }
		public bool UserInteropID { get; set; }
	}
}
