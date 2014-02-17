using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Four51.APISDK.Common;

namespace Four51.APISDK.Services
{
	public class Four51WebSalesOrderList {
		
		public Four51WebSalesOrderList(string SharedSecret, string ServiceId, string Status = "new")
		{
			this.Orders = new List<OrderList>();
			var url = String.Format("http://test.four51.com/services/{0}/{1}/orders/?downloadstatus={2}", ServiceId, SharedSecret, Status);
			var list = XElement.Load(url);
			foreach (XElement order in list.Descendants("Order"))
			{
				var o = new OrderList()
				{
					ID = order.Attributes("ID").First().Value.To<string>(),
					DownloadStatus = order.Attributes("DownloadStatus").First().Value.To<string>(),
					Url = order.Value.To<string>()
				};
				this.Orders.Add(o);
			}
		}

		#region Properties
		public List<OrderList> Orders { get; set; }
		#endregion
	}

	public class OrderList
	{
		public string Url { get; set; }
		public string ID { get; set; }
		public string DownloadStatus { get; set; }
		public void Update(string SharedSecret, string ServiceId)
		{
			var url = String.Format("{0}?setdownloadstatus={1}", this.Url, this.DownloadStatus);
			var response = XElement.Load(url);
			var value = response.Value;
			if (value != DownloadStatus)
				throw new Exception("Status update failed");
		}
	}
}
