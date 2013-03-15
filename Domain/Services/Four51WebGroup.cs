using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Four51.APISDK.Four51Group;

namespace Four51.APISDK.Services
{
	public class Four51WebGroup : BaseService {
		private Group _group;

		public Four51WebGroup(string SharedSecret, string ServiceId) {
            _group = new Group() {
                Url = String.Format("http://www.four51.com/services/Group.asmx?id={0}", ServiceId)
            };
            this.ServiceId = ServiceId;
            this.SharedSecret = SharedSecret;
			this.Address = new List<Four51WebAddressAssignment>();
		}

		#region Properties
		public string BuyerCompanyInteropID { get; set; }
		public string InteropID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsReportingGroup { get; set; }
		public List<Four51WebAddressAssignment> Address { get; set; }
		#endregion

		public void Save()
		{
			_group.Save(BuyerCompanyInteropID, InteropID, Name, Description, IsReportingGroup, SharedSecret);
			this.Address.ForEach(a => {
				_group.AssignAddress(this.BuyerCompanyInteropID, this.InteropID, new AddressAssignment()
					{
						AddressInteropID = a.AddressInteropID,
						UseForShipping = a.UseForShipping,
						UseForBilling = a.UseForBilling
					},
					this.SharedSecret);
			});
		}

		public void Delete()
		{
			_group.Delete(BuyerCompanyInteropID, InteropID, SharedSecret);
		}

		public void AssignToAddress(Four51WebAddress Address) {
			AddressAssignment assign = new AddressAssignment() {
				AddressInteropID = Address.InteropID,
				UseForBilling = Address.UseForBilling,
				UseForShipping = Address.UseForShipping
			};
			_group.AssignAddress(BuyerCompanyInteropID, InteropID, assign, this.SharedSecret);
		}
	}
}
