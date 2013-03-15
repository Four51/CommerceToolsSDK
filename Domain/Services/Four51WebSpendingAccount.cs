using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Four51.APISDK.Four51SpendingAccount;

namespace Four51.APISDK.Services
{
	public class Four51WebSpendingAccount : BaseService {

		private SpendingAccount _account;

		public Four51WebSpendingAccount(string SharedSecret, string ServiceId)
		{
			_account = new SpendingAccount()
			{
				Url = String.Format("http://www.four51.com/services/SpendingAccount.asmx?id={0}", ServiceId)
			};
			this.ServiceId = ServiceId;
			this.SharedSecret = SharedSecret;
		}

		#region Properties
		public string CompanyInteropID { get; set; }
		public string InteropID { get; set; }
		public string Name { get; set; }
		public string Label { get; set; }
		public string SpendingAccountTypeName { get; set; }
		public string UserInteropID { get; set; }
		public string GroupInteropID { get; set; }
		public decimal Balance { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? ExpirationDate { get; set; }
		/// <summary>
		/// Used for the add to balance operation
		/// </summary>
		public decimal Value { get; set; }
		#endregion

		public void Save() {
			_account.Save(InteropID, Name, Label, SpendingAccountTypeName, CompanyInteropID, UserInteropID, GroupInteropID, Balance, StartDate.Value, ExpirationDate.Value, SharedSecret);
		}

		public void Delete()
		{
			_account.Delete(InteropID, SharedSecret);
		}

		public void AddToBalance()
		{
			_account.AddToBalance(this.InteropID, this.Value, this.SharedSecret);
		}
	}
}
