using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Four51.APISDK.Four51CostCenter;

namespace Four51.APISDK.Services
{
	public class Four51WebCostCenter : BaseService {
		private CostCenter _costcenter;

		public Four51WebCostCenter(string SharedSecret, string ServiceId)
		{
			_costcenter = new CostCenter() {
				Url = String.Format("http://www.four51.com/services/CostCenter.asmx?id={0}", ServiceId)
			};
			this.ServiceId = ServiceId;
			this.SharedSecret = SharedSecret;
		}

		#region Properties
		public string InteropID { get; set; }
		public string CompanyInteropID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		#endregion

		public void Save() {
			CostCenterProperties[] costcenter = new CostCenterProperties[1];
			costcenter[0] = new CostCenterProperties() {
				InteropID = InteropID,
				CompanyInteropID = CompanyInteropID,
				Name = Name,
				Description = Description
			};
			var error = _costcenter.Save(costcenter, this.SharedSecret);
			if (error.Length > 0)
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
		}

		public void Delete() {
			CostCenterID[] delete = new CostCenterID[1];
			delete[0] = new CostCenterID() {
				CostCenterInteropID = InteropID,
				CompanyInteropID = CompanyInteropID
			};
			var error = _costcenter.Delete(delete, this.SharedSecret);
            if (error.Length > 0)
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
		}
	}
}
