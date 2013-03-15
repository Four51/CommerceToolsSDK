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
		
		public Four51WebSalesOrderList(string SharedSecret, string ServiceId)
		{
			this.Orders = new List<OrderList>();
			var url = String.Format("http://www.four51.com/services/{0}/{1}/orders/", ServiceId, SharedSecret);
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

	public struct OrderList
	{
		public string Url;
		public string ID;
		public string DownloadStatus;
	}
}
