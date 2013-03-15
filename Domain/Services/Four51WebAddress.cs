using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Four51.APISDK.Four51Address;

namespace Four51.APISDK.Services
{
	public class Four51WebAddress : BaseService {
		private Address _address;

		public Four51WebAddress(string SharedSecret, string ServiceId) {
			_address = new Address() {
				Url = String.Format("http://www.four51.com/services/Address.asmx?id={0}", ServiceId) 
			};
			this.ServiceId = ServiceId;
			this.SharedSecret = SharedSecret;
		}

		#region Properties
		public string InteropID { get; set; }
		public string CompanyInteropID { get; set; }
		public string Name { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string CompanyName { get; set; }
		public string AddressLineOne { get; set; }
		public string AddressLineTwo { get; set; }
		public string City  { get; set; }
		public string State  { get; set; }
		public string Zip  { get; set; }
		public string Country { get; set; }
		public string PhoneNumber { get; set; }
		public bool OverrideOrderReferenceConflict { get; set; }
		public bool UseForBilling { get; set; }
		public bool UseForShipping { get; set; }
		#endregion

		public void Save() {
			AddressProperties[] props = new AddressProperties[1];
			props[0] = new AddressProperties() {
				InteropID = InteropID,
				CompanyInteropID = CompanyInteropID,
				AddressName = Name,
				FirstName = FirstName,
				LastName = LastName,
				CompanyName = CompanyName,
				AddressLine1 = AddressLineOne,
				AddressLine2 = AddressLineTwo,
				City = City,
				State = State,
				Zip = Zip,
				Country = Country,
				Phone = PhoneNumber
			};
			var error = _address.Save(props, SharedSecret);
			if (error != null)
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
		}

		public void Delete(bool OverrideOrderReferenceConflict) {
			AddressDelete[] delete = new AddressDelete[1];
			delete[0] = new AddressDelete() {
				AddressInteropID = InteropID,
				CompanyInteropID = CompanyInteropID,
				OverrideOrderReferenceConflict = OverrideOrderReferenceConflict
			};
			
			var error = _address.Delete(delete, SharedSecret);
			if (error != null)
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
		}
	}
}
