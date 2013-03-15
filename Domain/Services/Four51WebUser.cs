using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Four51.APISDK.Four51User;

namespace Four51.APISDK.Services
{
	public class Four51WebUser : BaseService {

		private User _user;

		public Four51WebUser(string SharedSecret, string ServiceId)
		{
			_user = new User()
			{
				Url = String.Format("http://www.four51.com/services/User.asmx?id={0}", ServiceId)
			};
			this.ServiceId = ServiceId;
			this.SharedSecret = SharedSecret;
			this.Groups = new List<Four51WebGroupAssignment>();
			this.Addresses = new List<Four51WebAddressAssignment>();
			this.CostCenters = new List<Four51WebCostCenterAssignment>();
			this.OrderFields = new List<Four51WebOrderFieldAssignment>();
		}

		#region Properties
		public string CompanyInteropID { get; set; }
		public string InteropID { get; set; }
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public bool Active { get; set; }
		public bool TermsAccepted { get; set; }

		public List<Four51WebGroupAssignment> Groups { get; set; }
		public List<Four51WebAddressAssignment> Addresses { get; set; }
		public List<Four51WebCostCenterAssignment> CostCenters { get; set; }
		public List<Four51WebOrderFieldAssignment> OrderFields { get; set; }
		#endregion

		public void Save() {
			var props = new UserProperties[1];
			props[0] = new UserProperties() {
				CompanyInteropID = CompanyInteropID,
				UserInteropID = InteropID,
				UserName = UserName,
				FirstName = FirstName,
				LastName = LastName,
				PhoneNumber = PhoneNumber,
				Email = Email,
				Active = Active,
				TermsAccepted = TermsAccepted
			};

			if (Password != null)
				props[0].Password = Password;
			
			var error = _user.Save(props, this.SharedSecret);
			if (error.Length > 0)
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
			this.Sync();
		}

		public void AssignToAddress(Four51WebAddressAssignment address)
		{
			var assign = new AddressAssignment() {
				AddressInteropID = address.AddressInteropID,
				UseForBilling = address.UseForBilling,
				UseForShipping = address.UseForShipping,
			};
			_user.AssignAddress(InteropID, assign, this.SharedSecret);
		}

		public void AssignToCostCenter(Four51WebCostCenterAssignment costcenter)
		{
			_user.AssignCostCenter(this.InteropID, costcenter.CostCenterInteropID, this.SharedSecret);
		}

		public void AssignToGroup(Four51WebGroupAssignment group)
		{
			if (!group.Remove)
				_user.AssignToGroup(this.InteropID, group.GroupInteropID, this.SharedSecret);
			else
				_user.RemoveFromGroup(this.InteropID, group.GroupInteropID, this.SharedSecret);
		}

		public void Delete() {
			var error = _user.Delete(new string[] { InteropID }, this.SharedSecret);
			if (error.Equals(null))
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
		}

		public void Sync() {
			var assoc = new UserAssociation[1];
			assoc[0] = new UserAssociation()
			{
				UserInteropID = this.InteropID,
				CostCenterInteropIDs = this.CostCenters.Select(c => c.CostCenterInteropID).ToArray(),
				Addresses = this.Addresses.Select(a => new AddressAssignment() {
					AddressInteropID = a.AddressInteropID,
					UseForBilling = a.UseForBilling,
					UseForShipping = a.UseForShipping
				}).ToArray(),
				GroupInteropIDs = this.Groups.Where(g => !g.Remove).Select(g => g.GroupInteropID).ToArray(),
				OrderFields = this.OrderFields.Select(o => new OrderFieldAssignment() {
					Name = o.Name,
					OptionInteropIDs = new string[] { o.OptionInteropID }
				}).ToArray()
			};

			var error = _user.SyncAssociations(assoc, this.SharedSecret);
			if (error.Equals(null))
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
		}
	}
}
