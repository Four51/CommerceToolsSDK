using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using Four51.APISDK;
using Four51.APISDK.Enums;
using Four51.APISDK.Common;
using Four51.APISDK.Four51Order;

namespace Four51.APISDK.Services
{
	[DataContract(Name = "SalesOrder")]
	[KnownType(typeof(SalesOrderLineItem))]
	public class Four51WebSalesOrder : BaseService, ISalesOrder {
		private Order _order;
		public Four51WebSalesOrder(string SharedSecret, string ServiceId) { 
			_order = new Order() {
				Url = string.Format("http://www.four51.com/services/Order.asmx?id={0}", ServiceId)
			};
			this.ServiceId = ServiceId;
			this.SharedSecret = SharedSecret;
		}

		public void Map(string url)
		{
			var doc = XElement.Load(url);
			this.Parse(doc);
		}

		public void Map(Stream stream)
		{
			var doc = XElement.Load(stream);
			this.Parse(doc);
		}

		private void Parse(XElement doc)
		{
			this.PayLoadID = doc.Attribute("payloadID").Value;
			this.Version = doc.Attribute("version").Value;
			this.TimeStamp = DateTime.ParseExact(doc.Attribute("timestamp").Value.Replace("T", " "), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
			this.Header = new SalesOrderHeader(doc.Elements("Header"));
			this.Request = new SalesOrderRequest(doc.Descendants("OrderRequestHeader"));
			this.LineItems = new SalesOrderLineItems(doc.Descendants("ItemOut"), this);
		}

		public string ForwardLineItemstoPO(List<SalesOrderLineItem> lineitems, string OutgoingOrderID, string SupplierCompanyDUNS, string Comments)
		{
			var ids = lineitems.Select(l => l.LineNumber).ToArray();
			var result = _order.ForwardLineItems(this.PayLoadID, OutgoingOrderID, string.Empty, SupplierCompanyDUNS, Comments, ids);
			return result;
		}

		#region ISalesOrder Members
		private SalesOrderHeader _header;
		[DataMember]
		public SalesOrderHeader Header {
			get {
				return _header;
			}
			set {
				_header = value;
			}
		}
		private string _payloadid;
		[DataMember]
		public string PayLoadID {
			get {
				return _payloadid;
			}
			set {
				_payloadid = value;
			}
		}
		private DateTime _timestamp;
		[DataMember]
		public DateTime TimeStamp {
			get {
				return _timestamp;
			}
			set {
				_timestamp = value;
			}
		}
		private string _version;
		[DataMember]
		public string Version {
			get {
				return _version;
			}
			set {
				_version = value;
			}
		}
		private SalesOrderRequest _request;
		[DataMember]
		public SalesOrderRequest Request {
			get { return _request; }
			set { _request = value; }
		}
		private IDictionary<int, SalesOrderLineItem> _lineitems;
		[DataMember]
		public IDictionary<int, SalesOrderLineItem> LineItems {
			get { return _lineitems; }
			set { _lineitems = value; }
		}
		#endregion
	}

	[KnownType(typeof(SalesOrderLineItemTax))]
	public class SalesOrderLineItem : ISalesOrderLineItem {
		public SalesOrderLineItem() { }
		private Four51WebSalesOrder _order { get; set; }

		public SalesOrderLineItem(XElement lineitem, Four51WebSalesOrder order) {
			this._linenumber = Utils.Int32ParseCatch(lineitem.Attributes("lineNumber").First().Value);
			this._quantity = Utils.Int32ParseCatch(lineitem.Attributes("quantity").First().Value);
			this._requesteddeliverydate = Utils.DateTimeParseCatch(lineitem.Attributes("requestedDeliveryDate").First().Value.Replace("T", " "));
			this._itemid = new SalesOrderLineItemItemID(lineitem.Elements("ItemID"));
			this._detail = new SalesOrderLineItemItemDetail(lineitem.Elements("ItemDetail"));
			this._shipto = new SalesOrderLineItemShipTo(lineitem.Elements("ShipTo"));
			this._taxes = new SalesOrderLineItemTaxes(lineitem.Elements("Tax"));
			this._custom = new Dictionary<object,object>();
			_order = order;
		}

		public void ForwardToSupplier(string OutgoingOrderID, string SupplierCompanyDUNS, string Comments)
		{
			var po = new Order();
			po.ForwardLineItems(_order.PayLoadID, OutgoingOrderID, null, SupplierCompanyDUNS, Comments, new int[] { this._linenumber });
		}
		#region ISalesOrderLineItem Members
		private int _linenumber;
		[DataMember]
		public int LineNumber {
			get {
				return _linenumber;
			}
			set {
				_linenumber = value;
			}
		}
		private int _quantity;
		[DataMember]
		public int Quantity {
			get {
				return _quantity;
			}
			set {
				_quantity = value;
			}
		}
		private DateTime _requesteddeliverydate;
		[DataMember]
		public DateTime RequestedDeliveryDate {
			get {
				return _requesteddeliverydate;
			}
			set {
				_requesteddeliverydate = value;
			}
		}
		private SalesOrderLineItemItemID _itemid;
		[DataMember]
		public SalesOrderLineItemItemID ItemID {
			get {
				return _itemid;
			}
			set {
				_itemid = value;
			}
		}
		private SalesOrderLineItemItemDetail _detail;
		[DataMember]
		public SalesOrderLineItemItemDetail Detail {
			get {
				return _detail;
			}
			set {
				_detail = value;
			}
		}
		private SalesOrderLineItemShipTo _shipto;
		[DataMember]
		public SalesOrderLineItemShipTo ShipTo {
			get {
				return _shipto;
			}
			set {
				_shipto = value;
			}
		}
		private IList<SalesOrderLineItemTax> _taxes;
		[DataMember]
		public IList<SalesOrderLineItemTax> Taxes {
			get {
				return _taxes;
			}
			set {
				_taxes = value;
			}
		}
		private Dictionary<object, object> _custom;
		[DataMember]
		public Dictionary<object, object> Custom {
			get {
				return _custom;
			}
			set {
				_custom = value;
			}
		}

		#endregion
	}

	[DataContract(Name = "Detail")]
	public class SalesOrderLineItemTaxDetail : ISalesOrderLineItemTaxDetail {
		public SalesOrderLineItemTaxDetail() { }
		public SalesOrderLineItemTaxDetail(IEnumerable<XElement> taxdetail) {
			this._category = (string)taxdetail.Attributes("category").First().Value;
			this._percentagerate = Utils.DoubleParseCatch(taxdetail.Attributes("percentageRate").First().Value);
			this._amount = Utils.DoubleParseCatch(taxdetail.FirstElement("TaxAmount"));
			this._location = taxdetail.FirstElement("TaxLocation");
			this._description = taxdetail.FirstElement("Description");
		}
		#region ISalesOrderLineItemTaxDetail Members
		private string _category;
		[DataMember]
		public string Category {
			get {
				return _category;
			}
			set {
				_category = value;
			}
		}
		private double _percentagerate;
		[DataMember]
		public double PercentageRate {
			get {
				return _percentagerate;
			}
			set {
				_percentagerate = value;
			}
		}
		private double _amount;
		[DataMember]
		public double Amount {
			get {
				return _amount;
			}
			set {
				_amount = value;
			}
		}
		private string _location;
		[DataMember]
		public string Location {
			get {
				return _location;
			}
			set {
				_location = value;
			}
		}
		private string _description;
		[DataMember]
		public string Description {
			get {
				return _description;
			}
			set {
				_description = value;
			}
		}

		#endregion
	}

	[DataContract(Name = "Tax")]
	public class SalesOrderLineItemTax : ISalesOrderLineItemTax {
		public SalesOrderLineItemTax() { }
		public SalesOrderLineItemTax(XElement tax) {
			this._type = (TaxType)Enum.Parse(typeof(TaxType), tax.Element("Description").Value.Replace(" Tax", ""));
			this._money = Utils.DoubleParseCatch(tax.Element("Money").Value);
			this._detail = new SalesOrderLineItemTaxDetail(tax.Elements("TaxDetail"));
			this._description = tax.Element("Description").Value;
		}
		#region ISalesOrderLineItemTax Members
		private TaxType _type;
		[DataMember]
		public TaxType Type {
			get {
				return _type;
			}
			set {
				_type = value;
			}
		}
		private double _money;
		[DataMember]
		public double Money {
			get {
				return _money;
			}
			set {
				_money = value;
			}
		}
		private SalesOrderLineItemTaxDetail _detail;
		[DataMember]
		public SalesOrderLineItemTaxDetail Detail {
			get {
				return _detail;
			}
			set {
				_detail = value;
			}
		}
		private string _description;
		[DataMember]
		public string Description {
			get {
				return _description;
			}
			set {
				_description = value;
			}
		}

		#endregion
	}

	[CollectionDataContract(Name = "Taxes")]
	public class SalesOrderLineItemTaxes : IList<SalesOrderLineItemTax> {
		private List<SalesOrderLineItemTax> _taxes = new List<SalesOrderLineItemTax>();
		public SalesOrderLineItemTaxes() { }
		public SalesOrderLineItemTaxes(IEnumerable<XElement> taxes) {
			foreach (XElement el in taxes) {
				this.Add(new SalesOrderLineItemTax(el));
			}
		}
		#region IList<ISalesOrderLineItemTax> Members

		public int IndexOf(SalesOrderLineItemTax item) {
			return _taxes.IndexOf(item);
		}

		public void Insert(int index, SalesOrderLineItemTax item) {
			_taxes.Insert(index, item);
		}

		public void RemoveAt(int index) {
			_taxes.RemoveAt(index);
		}

		public SalesOrderLineItemTax this[int index] {
			get {
				return _taxes[index];
			}
			set {
				_taxes[index] = value;
			}
		}

		#endregion

		#region ICollection<ISalesOrderLineItemTax> Members

		public void Add(SalesOrderLineItemTax item) {
			_taxes.Add(item);
		}

		public void Clear() {
			_taxes.Clear();
		}

		public bool Contains(SalesOrderLineItemTax item) {
			return _taxes.Contains(item);
		}

		public void CopyTo(SalesOrderLineItemTax[] array, int arrayIndex) {
			throw new NotImplementedException();
		}

		public int Count {
			get { return _taxes.Count; }
		}

		public bool IsReadOnly {
			get { throw new NotImplementedException(); }
		}

		public bool Remove(SalesOrderLineItemTax item) {
			return _taxes.Remove(item);
		}

		#endregion

		#region IEnumerable<ISalesOrderLineItemTax> Members

		public IEnumerator<SalesOrderLineItemTax> GetEnumerator() {
			return _taxes.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return _taxes.GetEnumerator();
		}

		#endregion
	}

	[DataContract(Name = "ShipTo")]
	public class SalesOrderLineItemShipTo : ISalesOrderLineItemShipTo {
		public SalesOrderLineItemShipTo() { }
		public SalesOrderLineItemShipTo(IEnumerable<XElement> shipto) {
			this._address = new SalesOrderAddress(shipto.Elements("Address"));
		}
		#region ISalesOrderLineItemShipTo Members
		private SalesOrderAddress _address;
		[DataMember]
		public SalesOrderAddress Address {
			get {
				return _address;
			}
			set {
				_address = value;
			}
		}

		#endregion
	}

	[CollectionDataContract(Name = "LineItems")]
	public class SalesOrderLineItems : IDictionary<int, SalesOrderLineItem> {
		private Dictionary<int, SalesOrderLineItem> _list = new Dictionary<int, SalesOrderLineItem>();
		public SalesOrderLineItems() { }
		public SalesOrderLineItems(IEnumerable<XElement> lineitems, Four51WebSalesOrder order) {
			foreach (XElement lineitem in lineitems) {
				SalesOrderLineItem item = new SalesOrderLineItem(lineitem, order);
				this.Add(item.LineNumber, item);
			}
		}

		#region IDictionary<string,ISalesOrderLineItem> Members

		public void Add(int key, SalesOrderLineItem value) {
			_list.Add(key, value);
		}

		public bool ContainsKey(int key) {
			return _list.ContainsKey(key);
		}

		public ICollection<int> Keys {
			get { return _list.Keys; }
		}

		public bool Remove(int key) {
			return _list.Remove(key);
		}

		public bool TryGetValue(int key, out SalesOrderLineItem value) {
			return _list.TryGetValue(key, out value);
		}

		public ICollection<SalesOrderLineItem> Values {
			get { return _list.Values; }
		}

		public SalesOrderLineItem this[int key] {
			get {
				return _list[key];
			}
			set {
				_list[key] = value;
			}
		}

		#endregion

		#region ICollection<KeyValuePair<string,ISalesOrderLineItem>> Members

		public void Add(KeyValuePair<int, SalesOrderLineItem> item) {
			_list.Add(item.Key, item.Value);
		}

		public void Clear() {
			_list.Clear();
		}

		public bool Contains(KeyValuePair<int, SalesOrderLineItem> item) {
			return _list.Contains(item);
		}

		public void CopyTo(KeyValuePair<int, SalesOrderLineItem>[] array, int arrayIndex) {
			throw new NotImplementedException();
		}

		public int Count {
			get { return _list.Count; }
		}

		public bool IsReadOnly {
			get { throw new NotImplementedException(); }
		}

		public bool Remove(KeyValuePair<int, SalesOrderLineItem> item) {
			return _list.Remove(item.Key);
		}

		#endregion

		#region IEnumerable<KeyValuePair<string,ISalesOrderLineItem>> Members

		public IEnumerator<KeyValuePair<int, SalesOrderLineItem>> GetEnumerator() {
			return _list.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return _list.GetEnumerator();
		}

		#endregion
	}

	[DataContract(Name = "Detail")]
	public class SalesOrderLineItemItemDetail : ISalesOrderLineItemItemDetail {
		public SalesOrderLineItemItemDetail() { }
		public SalesOrderLineItemItemDetail(IEnumerable<XElement> detail) {
			this._unitprice = new SalesOrderLineItemItemDetailUnitPrice(detail.Elements("UnitPrice"));
			this._description = detail.FirstElement("Description");
			this._unitofmeasure = detail.FirstElement("UnitOfMeasure");
			this._classification = detail.FirstElement("Classification");
			this._url = detail.FirstElement("URL");
			this._lineitemid = detail.TryExtrinsicValue("lineItemID");
			this._quantitymultiplier = Utils.Int32ParseCatch(detail.TryExtrinsicValue("quantityMultiplier"));
			this._costcenter = detail.TryExtrinsicValue("costCenter");
			this._costcenterinteropid = detail.TryExtrinsicValue("costCenterInteropID");
			this._productspecs = detail.ProductSpecs();
			this._productinteropid = detail.TryExtrinsicValue("productInteropID");
			this._variantinteropid = detail.TryExtrinsicValue("variantInteropID");
			this._shippingaddressinteropid = detail.TryExtrinsicValue("shippingAddressInteropID");
			this._taxcode = detail.TryExtrinsicValue("taxCode");
            this._taxableitemamount = Utils.DoubleParseCatch(detail.TryExtrinsicValue("taxableItemAmount"));
			this._taxableshippingamount = Utils.DoubleParseCatch(detail.TryExtrinsicValue("taxableShippingAmount"));
			this._shipweight = Utils.DoubleParseCatch(detail.TryExtrinsicValue("shipWeight"));
			this._requestedshipper = detail.TryExtrinsicValue("requestedShipper");
			//TODO:  deal with missing xml elements
			try
			{
				this._producttype = (RequestProductType)Enum.Parse(typeof(RequestProductType), detail.TryExtrinsicValue("productType"));
			}
			catch { }
			try {
				this._requstedshippingaccount = detail.TryExtrinsicValue("requestedShippingAccount");
			}
			catch { }
			this._four51username = detail.TryExtrinsicValue("Four51UserName");
		}

		#region ISalesOrderLineItemItemDetail Members
		private SalesOrderLineItemItemDetailUnitPrice _unitprice;
		[DataMember]
		public SalesOrderLineItemItemDetailUnitPrice UnitPrice {
			get {
				return _unitprice;
			}
			set {
				_unitprice = value;
			}
		}
		private string _description;
		[DataMember]
		public string Description {
			get {
				return _description;
			}
			set {
				_description = value;
			}
		}
		private string _unitofmeasure;
		[DataMember]
		public string UnitOfMeasure {
			get {
				return _unitofmeasure;
			}
			set {
				_unitofmeasure = value;
			}
		}
		private string _classification;
		[DataMember]
		public string Classification {
			get {
				return _classification;
			}
			set {
				_classification = value;
			}
		}
		private string _lineitemid;
		[DataMember]
		public string LineItemID {
			get {
				return _lineitemid;
			}
			set {
				_lineitemid = value; 
			}
		}
		private RequestProductType _producttype;
		[DataMember]
		public RequestProductType ProductType {
			get {
				return _producttype;
			}
			set {
				_producttype = value;
			}
		}
		private int _quantitymultiplier;
		[DataMember]
		public int QuantityMultiplier {
			get {
				return _quantitymultiplier;
			}
			set {
				_quantitymultiplier = value;
			}
		}
		private string _costcenter;
		[DataMember]
		public string CostCenter {
			get {
				return _costcenter;
			}
			set {
				_costcenter = value;
			}
		}
		private string _costcenterinteropid;
		[DataMember]
		public string CostCenterInteropID {
			get {
				return _costcenterinteropid;
			}
			set {
				_costcenterinteropid = value;
			}
		}
		private IDictionary<string, string> _productspecs;
		[DataMember]
		public IDictionary<string, string> ProductSpecs {
			get {
				return _productspecs;
			}
			set {
				_productspecs = value;
			}
		}
		private string _productinteropid;
		[DataMember]
		public string ProductInteropID {
			get {
				return _productinteropid;
			}
			set {
				_productinteropid = value;
			}
		}
		private string _variantinteropid;
		[DataMember]
		public string VariantInteropID {
			get {
				return _variantinteropid;
			}
			set {
				_variantinteropid = value;
			}
		}
		private string _shippingaddressinteropid;
		[DataMember]
		public string ShippingAddressInteropID {
			get {
				return _shippingaddressinteropid;
			}
			set {
				_shippingaddressinteropid = value;
			}
		}
		private string _taxcode;
		[DataMember]
		public string TaxCode {
			get {
				return _taxcode;
			}
			set {
				_taxcode = value;
			}
		}
		private double _taxableshippingamount;
		[DataMember]
		public double TaxableShippingAmount {
			get {
				return _taxableshippingamount;
			}
			set {
				_taxableshippingamount = value;
			}
		}
		private double _taxableitemamount;
		[DataMember]
		public double TaxableItemAmount {
			get {
				return _taxableitemamount;
			}
			set {
				_taxableitemamount = value;
			}
		}
		private double _shipweight;
		[DataMember]
		public double ShipWeight {
			get {
				return _shipweight;
			}
			set {
				_shipweight = value;
			}
		}
		private string _four51username;
		[DataMember]
		public string Four51UserName {
			get {
				return _four51username;
			}
			set {
				_four51username = value;
			}
		}
		private string _requestedshipper;
		[DataMember]
		public string RequestedShipper {
			get {
				return _requestedshipper;
			}
			set {
				_requestedshipper = value;
			}
		}
		private string _requstedshippingaccount;
		[DataMember]
		public string RequestedShippingAccount {
			get {
				return _requstedshippingaccount;
			}
			set {
				_requstedshippingaccount = value;
			}
		}
		private string _url;
		[DataMember]
		public string URL {
			get {
				return _url;
			}
			set {
				_url = value;
			}
		}

		#endregion
	}
	
	[DataContract(Name = "UnitPrice")]
	public class SalesOrderLineItemItemDetailUnitPrice : ISalesOrderLineItemItemDetailUnitPrice {
		public SalesOrderLineItemItemDetailUnitPrice() { }
		public SalesOrderLineItemItemDetailUnitPrice(IEnumerable<XElement> unitprice) {
			this._money = Utils.DoubleParseCatch(unitprice.FirstElement("Money"));
		}
		#region ISalesOrderLineItemItemDetailUnitPrice Members
		private double _money;
		[DataMember]
		public double Money {
			get {
				return _money;
			}
			set {
				_money = value;
			}
		}

		#endregion
	}
	
	[DataContract(Name = "LineItemID")]
	public class SalesOrderLineItemItemID : ISalesOrderLineItemItemID {
		public SalesOrderLineItemItemID() { }
		public SalesOrderLineItemItemID(IEnumerable<XElement> itemid) {
			this._supplierpartauxiliaryid = itemid.FirstElement("SupplierPartAuxiliaryID");
			this._supplierpartid = itemid.FirstElement("SupplierPartID");
		}
		#region ISalesOrderLineItemItemID Members
		private string _supplierpartid;
		[DataMember]
		public string SupplierPartID {
			get {
				return _supplierpartid;
			}
			set {
				_supplierpartid = value;
			}
		}
		private string _supplierpartauxiliaryid;
		[DataMember]
		public string SupplierPartAuxiliaryID {
			get {
				return _supplierpartauxiliaryid;
			}
			set {
				_supplierpartauxiliaryid = value;
			}
		}

		#endregion
	}
	
	[DataContract(Name = "Header")]
	public class SalesOrderHeader : ISalesOrderHeader {
		public SalesOrderHeader() { }
		public SalesOrderHeader(IEnumerable<XElement> header) {
			this._sender = new SalesOrderHeaderSender(header.Elements("Sender"));
			this._from = new SalesOrderHeaderFrom(header.Elements("From"));
			this._to = new SalesOrderHeaderTo(header.Elements("To"));
			this._custom = new Dictionary<object, object>();
		}
		#region SalesOrderHeader Members
		private SalesOrderHeaderFrom _from;
		[DataMember]
		public SalesOrderHeaderFrom From {
			get {
				return _from;
			}
			set {
				_from = value;
			}
		}
		private SalesOrderHeaderTo _to;
		[DataMember]
		public SalesOrderHeaderTo To {
			get {
				return _to;
			}
			set {
				_to = value;
			}
		}
		private SalesOrderHeaderSender _sender;
		[DataMember]
		public SalesOrderHeaderSender Sender {
			get {
				return _sender;
			}
			set {
				_sender = value;
			}
		}
		private Dictionary<object, object> _custom;
		[DataMember]
		public Dictionary<object, object> Custom {
			get { return _custom; }
			set { _custom = value; }
		}

		#endregion
	}
	
	[DataContract(Name = "Request")]
	public class SalesOrderRequest : ISalesOrderRequest {
		public SalesOrderRequest() { }
		public SalesOrderRequest(IEnumerable<XElement> request) {
			this._orderID = request.Attributes("orderID").First().Value;
			this._orderdatetime = DateTime.ParseExact(request.Attributes("orderDate").First().Value.Replace("T", " "), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
			this._comments = request.FirstElement("Comments");
			this._four51username = request.TryExtrinsicValue("Four51UserName");
			this._four51userinteropid = request.TryExtrinsicValue("Four51UserInteropID");
			this._four51userfirstname = request.TryExtrinsicValue("Four51UserFirstName");
			this._fromUserIP = request.TryExtrinsicValue("FromUserIP");
			this._four51userlastname = request.TryExtrinsicValue("Four51UserLastName");
			this._four51userphone = request.TryExtrinsicValue("Four51UserPhone");
			this._paymenttype = (PaymentType)Enum.Parse(typeof(PaymentType), request.TryExtrinsicValue("PaymentType"), true);
			this._paymentsource = new SalesOrderPaymentSource(request.Elements("Extrinsic").Where(x => x.Attribute("name").Value == "PaymentSource"));
			this._payment = new SalesOrderPayment(request.Elements("Payment"));
			this._buyerinteropid = request.TryExtrinsicValue("BuyerInteropID");
			this._billingaddressinteropid = request.TryExtrinsicValue("BillingAddressInteropID");
			this._ordertype = (OrderType)Enum.Parse(typeof(OrderType), request.TryExtrinsicValue("OrderType"), true);
			this._orderfields = request.OrderFields();
			this._tax = new SalesOrderTax(request.Elements("Tax"));
			this._total = new SalesOrderTotal(request.Elements("Total"));
			this._shipping = new SalesOrderShipping(request.Elements("Shipping"));
			this._billto = new SalesOrderBillTo(request.Elements("BillTo"));
			this._coupon = new SalesOrderCoupon(request.Elements("Extrinsic").Where(x => x.Attribute("name").Value == "Coupon"));
		}

		#region ISalesOrderRequest Members
		private string _orderID;
		[DataMember]
		public string  OrderID
		{
			  get { return _orderID; }
			  set { _orderID = value; }
		}
		private DateTime _orderdatetime;
		[DataMember]
		public DateTime  OrderDateTime
		{
			  get { return _orderdatetime; }
			  set { _orderdatetime = value; }
		}
		private SalesOrderTotal _total;
		[DataMember]
		public SalesOrderTotal  Total
		{
			  get { return _total; }
			  set { _total = value; }
		}
		private SalesOrderBillTo _billto;
		[DataMember]
		public SalesOrderBillTo  BillTo
		{
			  get { return _billto; }
			  set { _billto = value; }
		}
		private SalesOrderShipping _shipping;
		[DataMember]
		public SalesOrderShipping  Shipping
		{
			  get { return _shipping; }
			  set { _shipping = value; }
		}
		private SalesOrderTax _tax;
		[DataMember]
		public SalesOrderTax Tax
		{
			  get { return _tax; }
			  set { _tax = value; }
		}
		private SalesOrderCoupon _coupon;
		[DataMember]
		public SalesOrderCoupon Coupon
		{
			get { return _coupon; }
			set { _coupon = value; }
		}
		private string _comments;
		[DataMember]
		public string  Comments
		{
			  get { return _comments; }
			  set { _comments = value; }
		}
		private string _four51username;
		[DataMember]
		public string  Four51UserName
		{
			  get { return _four51username; }
			  set { _four51username = value; }
		}
		private string _four51userinteropid;
		[DataMember]
		public string  Four51UserInteropId
		{
			  get { return _four51userinteropid; }
			  set { _four51userinteropid = value; }
		}
		private string _four51userfirstname;
		public string  Four51UserFirstName
		{
			  get { return _four51userfirstname; }
			  set { _four51userfirstname = value; }
		}
		private string _fromUserIP;
		public string FromUserIP
		{
			get { return _fromUserIP; }
			set { _fromUserIP = value; }
		}
		private string _four51userlastname;
		[DataMember]
		public string  Four51UserLastName
		{
			  get { return _four51userlastname; }
			  set { _four51userlastname = value; }
		}
		private string _four51userphone;
		[DataMember]
		public string  Four51UserPhone
		{
			  get { return _four51userphone; }
			  set { _four51userphone = value; }
		}
		private PaymentType _paymenttype;
		[DataMember]
		public PaymentType PaymentType {
			get { return _paymenttype; }
			set { _paymenttype = value; }
		}
		private SalesOrderPayment _payment;
		[DataMember]
		public SalesOrderPayment Payment
		{
			get { return _payment; }
			set { _payment = value; }
		}
		private SalesOrderPaymentSource _paymentsource;
		[DataMember]
		public SalesOrderPaymentSource  PaymentSource
		{
			  get { return _paymentsource; }
			  set { _paymentsource = value; }
		}
		private string _buyerinteropid;
		[DataMember]
		public string  BuyerInteropId
		{
			  get { return _buyerinteropid; }
			  set { _buyerinteropid = value; }
		}
		private string _billingaddressinteropid;
		[DataMember]
		public string  BillingAddressInteropId
		{
			  get { return _billingaddressinteropid; }
			  set { _billingaddressinteropid = value; }
		}
		private OrderType _ordertype;
		[DataMember]
		public OrderType  OrderType
		{
			  get { return _ordertype; }
			  set { _ordertype = value; }
		}
		private Dictionary<string,string> _orderfields;
		[DataMember]
		public Dictionary<string,string>  OrderFields
		{
			  get { return _orderfields; }
			  set { _orderfields = value; }
		}
		#endregion
	}
	
	[DataContract(Name = "From")]
	public class SalesOrderHeaderFrom : ISalesOrderHeaderFrom {
		public SalesOrderHeaderFrom() { }
		public SalesOrderHeaderFrom(IEnumerable<XElement> headerfrom) {
			this._duns = headerfrom.FirstElement("Identity");
			this._companyname = (string)(headerfrom.Descendants("Identity").ElementAt(1));
		}
		#region ISalesOrderHeaderFrom Members
		private string _duns;
		[DataMember]
		public string DUNS {
			get {
				return _duns;
			}
			set {
				_duns = value;
			}
		}
		private string _companyname;
		[DataMember]
		public string CompanyName {
			get {
				return _companyname;
			}
			set {
				_companyname = value;
			}
		}

		#endregion
	}
	
	[DataContract(Name = "To")]
	public class SalesOrderHeaderTo : ISalesOrderHeaderTo {
		public SalesOrderHeaderTo() { }
		public SalesOrderHeaderTo(IEnumerable<XElement> to) {
			this._todomain = (string)(to.Descendants("Identity")).First();
		}
		#region ISalesOrderHeaderTo Members
		private string _todomain;
		[DataMember]
		public string ToDomain {
			get {
				return _todomain;
			}
			set {
				_todomain = value;
			}
		}

		#endregion
	}
	
	[DataContract(Name = "Sender")]
	public class SalesOrderHeaderSender : ISalesOrderHeaderSender {
		public SalesOrderHeaderSender() { }
		public SalesOrderHeaderSender(IEnumerable<XElement> sender) {
			this._identity = sender.FirstElement("Identity");
			this._sharedsecret = sender.FirstElement("SharedSecret");
			this._useragent = sender.FirstElement("UserAgent");
		}
		#region ISalesOrderHeaderSender Members

		private string _identity;
		[DataMember]
		public string Identity {
			get {
				return _identity;
			}
			set {
				_identity = value;
			}
		}

		private string _sharedsecret;
		[DataMember]
		public string SharedSecret {
			get {
				return _sharedsecret;
			}
			set {
				_sharedsecret = value;
			}
		}

		private string _useragent;
		[DataMember]
		public string UserAgent {
			get {
				return _useragent;
			}
			set {
				_useragent = value;
			}
		}

		#endregion
	}

	[DataContract(Name = "Payment")]
	public class SalesOrderPayment : ISalesOrderPayment
	{
		public SalesOrderPayment() { }
		public SalesOrderPayment(IEnumerable<XElement> payment)
		{
			_pcard = new SalesOrderPaymentPCard(payment.Elements("PCard"));
		}
		
		private SalesOrderPaymentPCard _pcard;
		[DataMember]
		public SalesOrderPaymentPCard PCard
		{
			get { return _pcard; }
			set { _pcard = value; }
		}
	}

	[DataContract(Name = "PCard")]
	public class SalesOrderPaymentPCard : ISalesOrderPaymentPCard
	{
		public SalesOrderPaymentPCard() { }
		public SalesOrderPaymentPCard(IEnumerable<XElement> card)
		{
			if (card.Count() == 0) return;
			this._number = card.Attributes("number").FirstOrDefault().Value;
			this._name = card.Attributes("name").First().Value;
			this._expiration = Utils.DateTimeParseCatch(card.Attributes("expiration").First().Value);
		}
		private string _number;
		[DataMember]
		public string Number
		{
			get { return _number; }
			set { _number = value; }
		}
		private DateTime _expiration;
		[DataMember]
		public DateTime Expiration
		{
			get { return _expiration; }
			set { _expiration = value; }
		}
		private string _name;
		[DataMember]
		public string Name {
			get { return _name; }
			set { _name = value; }
		}
	}

	[DataContract(Name = "Payment")]
	public class SalesOrderPaymentSource : ISalesOrderPaymentSource
	{
		public SalesOrderPaymentSource() { }
		public SalesOrderPaymentSource(IEnumerable<XElement> payment) {
			this._type = (PaymentType)Enum.Parse(typeof(PaymentType), payment.TryExtrinsicValue("PaymentType"), true);
			this._amount = Utils.DoubleParseCatch(payment.TryExtrinsicValue("Amount"));
			this._name = payment.TryExtrinsicValue("PaymentSourceName");
		}
		#region ISalesOrderPayment Members
		private PaymentType _type;
		[DataMember]
		public PaymentType Type
		{
			  get { return _type; }
			  set { _type = value; }
		}
		private double _amount;
		[DataMember]
		public double  Amount
		{
			  get { return _amount; }
			  set { _amount = value; }
		}
		private string _name;
		[DataMember]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		#endregion
	}

	[DataContract(Name = "CouponProduct")]
	public class SalesOrderCouponProduct : ISalesOrderCouponProduct
	{
		public SalesOrderCouponProduct() { }
		public SalesOrderCouponProduct(XElement product)
		{
			this._interopid = product.Elements("Extrinsic").Where(x => x.Attribute("name").Value == "InteropID").First().Value;
			this._productid = product.Elements("Extrinsic").Where(x => x.Attribute("name").Value == "ProductID").First().Value;
		}
		private string _interopid;
		[DataMember]
		public string InteropID
		{
			get { return _interopid; }
			set { _interopid = value; }
		}
		private string _productid;
		[DataMember]
		public string ProductID
		{
			get { return _productid; }
			set { _productid = value; }
		}
	}

	[DataContract(Name = "CouponCategory")]
	public class SalesOrderCouponCategory : ISalesOrderCouponCategory
	{
		public SalesOrderCouponCategory() { }
		public SalesOrderCouponCategory(XElement category)
		{
			this._interopid = category.Elements("Extrinsic").Where(x => x.Attribute("name").Value == "InteropID").First().Value;
			this._name = category.Elements("Extrinsic").Where(x => x.Attribute("name").Value == "CategoryName").First().Value;
		}
		private string _interopid;
		[DataMember]
		public string InteropID
		{
			get { return _interopid; }
			set { _interopid = value; }
		}
		private string _name;
		[DataMember]
		public string CategoryName
		{
			get { return _name; }
			set { _name = value; }
		}
	}

	[CollectionDataContract(Name = "CouponProducts")]
	public class SalesOrderCouponProducts : IList<SalesOrderCouponProduct>
	{
		private List<SalesOrderCouponProduct> _list = new List<SalesOrderCouponProduct>();
		public SalesOrderCouponProducts(IEnumerable<XElement> product)
		{
			var products = product.Elements("Extrinsic").Where(x => x.Attribute("name").Value == "Product");
			foreach (XElement el in products)
			{
				this.Add(new SalesOrderCouponProduct(el));
			}
		}
		#region List implementation
		public int IndexOf(SalesOrderCouponProduct item)
		{
			return _list.IndexOf(item);
		}

		public void Insert(int index, SalesOrderCouponProduct item)
		{
			_list.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}

		public SalesOrderCouponProduct this[int index]
		{
			get
			{
				return _list[index];
			}
			set
			{
				_list[index] = value;
			}
		}

		public void Add(SalesOrderCouponProduct item)
		{
			_list.Add(item);
		}

		public void Clear()
		{
			_list.Clear();
		}

		public bool Contains(SalesOrderCouponProduct item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(SalesOrderCouponProduct[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(SalesOrderCouponProduct item)
		{
			return _list.Remove(item);
		}

		public IEnumerator<SalesOrderCouponProduct> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}
		#endregion
	}

	[CollectionDataContract(Name = "CouponCategories")]
	public class SalesOrderCouponCategories : IList<SalesOrderCouponCategory>
	{
		private List<SalesOrderCouponCategory> _list = new List<SalesOrderCouponCategory>();
		public SalesOrderCouponCategories(IEnumerable<XElement> category)
		{
			var categories = category.Elements("Extrinsic").Where(x => x.Attribute("name").Value == "Category");
			foreach (XElement el in categories)
			{
				this.Add(new SalesOrderCouponCategory(el));
			}
		}
		#region List implementation
		public int IndexOf(SalesOrderCouponCategory item)
		{
			return _list.IndexOf(item);
		}

		public void Insert(int index, SalesOrderCouponCategory item)
		{
			_list.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}

		public SalesOrderCouponCategory this[int index]
		{
			get
			{
				return _list[index];
			}
			set
			{
				_list[index] = value;
			}
		}

		public void Add(SalesOrderCouponCategory item)
		{
			_list.Add(item);
		}

		public void Clear()
		{
			_list.Clear();
		}

		public bool Contains(SalesOrderCouponCategory item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(SalesOrderCouponCategory[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(SalesOrderCouponCategory item)
		{
			return _list.Remove(item);
		}

		public IEnumerator<SalesOrderCouponCategory> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}
		#endregion
	}

	[DataContract(Name = "Coupon")]
	public class SalesOrderCoupon : ISalesOrderCoupon
	{
		public SalesOrderCoupon() { }
		public SalesOrderCoupon(IEnumerable<XElement> coupon)
		{
			if (coupon.Count() == 0) return;
			this._code = coupon.TryExtrinsicValue("CouponCode");
			this._interopid = coupon.TryExtrinsicValue("InteropID");
			this._amount = Utils.DoubleParseCatch(coupon.TryExtrinsicValue("DiscountAmount"));
			this._type = (CouponType)Enum.Parse(typeof(CouponType), coupon.TryExtrinsicValue("Type"), true);
			this._distype = (CouponDiscountAmountType)Enum.Parse(typeof(CouponDiscountAmountType), coupon.TryExtrinsicValue("DiscountAmountType"), true);
			this._limit = Utils.Int32ParseCatch(coupon.TryExtrinsicValue("RedeemLimit"));
			this._minimum = Utils.DoubleParseCatch(coupon.TryExtrinsicValue("MinimumPurchase"));
			this._applysubtotal = Utils.BoolParseCatch(coupon.TryExtrinsicValue("ApplyToSubtotal"));
			this._applyshipping = Utils.BoolParseCatch(coupon.TryExtrinsicValue("ApplyToShipping"));
			this._applytax = Utils.BoolParseCatch(coupon.TryExtrinsicValue("ApplyToTax"));
			this._disamount = Utils.DoubleParseCatch(coupon.TryExtrinsicValue("CouponConfiguredDiscountAmount"));
			this._products = new SalesOrderCouponProducts(coupon.Elements("Extrinsic").Where(x => x.Attribute("name").Value == "Products"));
			this._categories = new SalesOrderCouponCategories(coupon.Elements("Extrinsic").Where(x => x.Attribute("name").Value == "Categories"));
		}
		private IList<SalesOrderCouponCategory> _categories;
		[DataMember]
		public IList<SalesOrderCouponCategory> Categories
		{
			get { return _categories; }
			set { _categories = value; }
		}
		private IList<SalesOrderCouponProduct> _products;
		[DataMember]
		public IList<SalesOrderCouponProduct> Products
		{
			get { return _products; }
			set { _products = value; }
		}
		private string _code;
		[DataMember]
		public string Code
		{
			get { return _code; }
			set { _code = value; }
		}
		private string _interopid;
		[DataMember]
		public string InteropID
		{
			get { return _interopid; }
			set { _interopid = value; }
		}
		private double _amount;
		[DataMember]
		public double DiscountAmount
		{
			get { return _amount; }
			set { _amount = value; }
		}
		private CouponType _type;
		[DataMember]
		public CouponType Type
		{
			get { return _type; }
			set { _type = value; }
		}
		private CouponDiscountAmountType _distype;
		[DataMember]
		public CouponDiscountAmountType DiscountAmountType
		{
			get { return _distype; }
			set { _distype = value; }
		}
		private int _limit;
		[DataMember]
		public int RedeemLimit
		{
			get { return _limit; }
			set { _limit = value; }
		}
		private double _minimum;
		[DataMember]
		public double MinimumPurchase
		{
			get { return _minimum; }
			set { _minimum = value; }
		}
		private bool _applysubtotal;
		[DataMember]
		public bool ApplyToSubtotal
		{
			get { return _applysubtotal; }
			set { _applysubtotal = value; }
		}
		private bool _applyshipping;
		[DataMember]
		public bool ApplyToShipping
		{
			get { return _applyshipping; }
			set { _applyshipping = value; }
		}
		private bool _applytax;
		[DataMember]
		public bool ApplyToTax
		{
			get { return _applytax; }
			set { _applytax = value; }
		}
		private double _disamount;
		[DataMember]
		public double CouponConfiguredDiscountAmount
		{
			get { return _disamount; }
			set { _disamount = value; }
		}
	}

	[DataContract(Name = "Total")]
	public class SalesOrderTotal : ISalesOrderTotal {
		public SalesOrderTotal() { }
		public SalesOrderTotal(IEnumerable<XElement> total) {
			this._money = Utils.DoubleParseCatch(total.FirstElement("Money"));
		}
		#region ISalesOrderTotal Members
		private double _money;
		[DataMember]
		public double  Money
		{
			  get { return _money; }
			  set { _money = value; }
		}

		#endregion
	}
	
	[DataContract(Name = "BillTo")]
	public class SalesOrderBillTo : ISalesOrderBillTo {
		public SalesOrderBillTo() { }
		public SalesOrderBillTo(IEnumerable<XElement> billto) {
			this._address = new SalesOrderAddress(billto.Elements("Address"));
		}
		#region ISalesOrderBillTo Members
		private SalesOrderAddress _address;
		[DataMember]
		public SalesOrderAddress  Address
		{
			  get { return _address; }
			  set { _address = value; }
		}
		#endregion
	}
	
	[DataContract(Name = "Tax")]
	public class SalesOrderTax : ISalesOrderTax {
		public SalesOrderTax() { }
		public SalesOrderTax(IEnumerable<XElement> tax) {
			this._description = tax.FirstElement("Description");
			this._money = Utils.DoubleParseCatch(tax.FirstElement("Money"));
		}

		#region ISalesOrderTax Members
		private double _money;
		[DataMember]
		public double  Money
		{
			  get { return _money; }
			  set { _money = value; }
		}
		private string _description;
		[DataMember]
		public string  Description
		{
			  get { return _description; }
			  set { _description = value; }
		}

		#endregion
	}
	
	[DataContract(Name = "Shipping")]
	public class SalesOrderShipping : ISalesOrderShipping {
		public SalesOrderShipping() { }
		public SalesOrderShipping(IEnumerable<XElement> shipping) {
			this._description = shipping.FirstElement("Description");
			this._money = Utils.DoubleParseCatch(shipping.FirstElement("Money"));
		}
		#region ISalesOrderShipping Members
		private double _money;
		[DataMember]
		public double  Money
		{
			  get { return _money; }
			  set { _money = value; }
		}
		private string _description;
		[DataMember]
		public string  Description
		{
			  get { return _description; }
			  set { _description = value; }
		}

		#endregion
	}
	
	[DataContract(Name = "Address")]
	public class SalesOrderAddress : ISalesOrderAddress {
		public SalesOrderAddress() { }
		public SalesOrderAddress(IEnumerable<XElement> address) {
			this._id = address.Attributes("addressID").First().Value;
			this._name = address.FirstElement("Name");
			this._email = address.FirstElement("Email");
			var country = address.FirstElement("CountryCode");
			var area = address.FirstElement("AreaOrCityCode");
			this._phone = String.Format("{0}{1}{2}",
				country != null ? country + "." : "",
				area != null ? area + "." : "",
				address.FirstElement("Number"));
			this._postalAddress = new SalesOrderPostalAddress(address.Elements("PostalAddress"));
		}
		#region ISalesOrderAddress Members
		private string _name;
		[DataMember]
		public string  Name
		{
				get { return _name; }
				set { _name = value; }
		}

		private SalesOrderPostalAddress _postalAddress;
		[DataMember]
		public SalesOrderPostalAddress  PostalAddress
		{
				get { return _postalAddress; }
				set { _postalAddress = value; }
		}
		private string _email;
		[DataMember]
		public string  Email
		{
				get { return _email; }
				set { _email = value; }
		}
		private string _phone;
		[DataMember]
		public string  Phone
		{
				get { return _phone; }
				set { _phone = value; }
		}
		private string _id;
		[DataMember]
		public string ID {
			get {
				return _id;
			}
			set {
				_id = value;
			}
		}

		#endregion
	}
	
	[DataContract(Name = "PostalAddress")]
	public class SalesOrderPostalAddress : ISalesOrderPostalAddress {
		public SalesOrderPostalAddress() { }
		public SalesOrderPostalAddress(IEnumerable<XElement> address) {
			this._name = address.Attributes("name").First().Value;
			this._deliveryto = address.FirstElement("DeliverTo");
			this._streetlineone = (string)address.Descendants("Street").First();
			this._streelinetwo = (string)address.Descendants("Street").Last();
			this._city = address.FirstElement("City");
			this._state = address.FirstElement("State");
			this._postalcode = address.FirstElement("PostalCode");
			this._country = address.FirstElement("Country");
		}
		#region ISalesOrderPostalAddress Members
		private string _deliveryto;
		[DataMember]
		public string  DeliverTo
		{
			get { return _deliveryto; }
			set { _deliveryto = value; }
		}
		private string _streetlineone;
		[DataMember]
		public string  StreetLineOne
		{
			get { return _streetlineone; }
			set { _streetlineone = value; }
		}
		private string _streelinetwo;
		[DataMember]
		public string  StreetLineTwo
		{
			get { return _streelinetwo; }
			set { _streelinetwo = value; }
		}
		private string _city;
		[DataMember]
		public string  City
		{
			get { return _city; }
			set { _city = value; }
		}
		private string _state;
		[DataMember]
		public string  State
		{
			get { return _state; }
			set { _state = value; }
		}
		private string _postalcode;
		[DataMember]
		public string  PostalCode
		{
			get { return _postalcode; }
			set { _postalcode = value; }
		}
		private string _country;
		[DataMember]
		public string  Country
		{
			get { return _country; }
			set { _country = value; }
		}
		private string _name;
		[DataMember]
		public string Name {
			get {
				return _name;
			}
			set {
				_name = value;
			}
		}

		#endregion
	}
}
