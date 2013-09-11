using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Four51.APISDK.Enums;

namespace Four51.APISDK.Services {
	public interface ISalesOrder {
		string PayLoadID { get; set; }
		DateTime TimeStamp { get; set; }
		string Version { get; set; }
		SalesOrderHeader Header { get; set; }
		SalesOrderRequest Request { get; set; }
		IDictionary<int, SalesOrderLineItem> LineItems { get; set; }
	}

	#region Header
	public interface ISalesOrderHeader {
		SalesOrderHeaderFrom From { get; set; }
		SalesOrderHeaderTo To { get; set; }
		SalesOrderHeaderSender Sender { get; set; }
		Dictionary<object, object> Custom { get; set; }
	}

	public interface ISalesOrderHeaderFrom {
		string DUNS { get; set; }
		string CompanyName { get; set; }
	}

	public interface ISalesOrderHeaderTo {
		string ToDomain { get; set; }
	}

	public interface ISalesOrderHeaderSender {
		string Identity { get; set; }
		string SharedSecret { get; set; }
		string UserAgent { get; set; }
	}
	#endregion

	#region Request
	public interface ISalesOrderRequest {
		string OrderID { get; set; }
		DateTime OrderDateTime { get; set; }
		SalesOrderTotal Total { get; set; }
		SalesOrderBillTo BillTo { get; set; }
		SalesOrderShipping Shipping { get; set; }
		SalesOrderTax Tax { get; set; }
		SalesOrderCoupon Coupon { get; set; }
		string Comments { get; set; }
		string Four51UserName { get; set; }
		string Four51UserInteropId { get; set; }
		string Four51UserFirstName { get; set; }
		string FromUserIP { get; set; }
		string Four51UserLastName { get; set; }
		string Four51UserPhone { get; set; }
		PaymentType PaymentType { get; set; }
		SalesOrderPaymentSource PaymentSource { get; set; }
		string BuyerInteropId { get; set; }
		string BillingAddressInteropId { get; set; }
		OrderType OrderType { get; set; }
		Dictionary<string, string> OrderFields { get; set; }
	}
	public interface ISalesOrderCoupon
	{
		string Code { get; set; }
		string InteropID { get; set; }
		double DiscountAmount { get; set; }
		CouponType Type { get; set; }
		CouponDiscountAmountType DiscountAmountType { get; set; }
		int RedeemLimit { get; set; }
		double MinimumPurchase { get; set; }
		bool ApplyToSubtotal { get; set; }
		bool ApplyToShipping { get; set; }
		bool ApplyToTax { get; set; }
		double CouponConfiguredDiscountAmount { get; set; }
		IList<SalesOrderCouponProduct> Products { get; set; }
		IList<SalesOrderCouponCategory> Categories { get; set; }
	}
	public interface ISalesOrderCouponProduct
	{
		string InteropID { get; set; }
		string ProductID { get; set; }
	}
	public interface ISalesOrderCouponCategory
	{
		string InteropID { get; set; }
		string CategoryName { get; set; }
	}
	public interface ISalesOrderPayment
	{
		SalesOrderPaymentPCard PCard { get; set; }
	}
	public interface ISalesOrderPaymentPCard
	{
		string Number { get; set; }
		DateTime Expiration { get; set; }
		string Name { get; set; }
	}
	public interface ISalesOrderPaymentSource {
		PaymentType Type { get; set; }
		double Amount { get; set; }
		string Name { get; set; }
	}
	public interface ISalesOrderTotal {
		double Money { get; set; }
	}
	public interface ISalesOrderBillTo {
		SalesOrderAddress Address { get; set; }
	}
	public interface ISalesOrderTax {
		double Money { get; set; }
		string Description { get; set; }
	}
	public interface ISalesOrderShipping {
		double Money { get; set; }
		string Description { get; set; }
	}

	#endregion

	#region LineItems
	public interface ISalesOrderLineItem {
		int LineNumber { get; set; }
		int Quantity { get; set; }
		DateTime RequestedDeliveryDate { get; set; }
		SalesOrderLineItemItemID ItemID { get; set; }
		SalesOrderLineItemItemDetail Detail { get; set; }
		SalesOrderLineItemShipTo ShipTo { get; set; }
		IList<SalesOrderLineItemTax> Taxes { get; set; }
		Dictionary<object, object> Custom { get; set; }
	}

	public interface ISalesOrderLineItemTax {
		TaxType Type { get; set; }
		Double Money { get; set; }
		string Description { get; set; }
		SalesOrderLineItemTaxDetail Detail { get; set; }
	}

	public interface ISalesOrderLineItemTaxDetail {
		string Category { get; set; }
		Double PercentageRate { get; set; }
		Double Amount { get; set; }
		string Location { get; set; }
		string Description { get; set; }
	}
	public interface ISalesOrderLineItemShipTo {
		SalesOrderAddress Address { get; set; }
	}

	public interface ISalesOrderLineItemItemDetail {
		SalesOrderLineItemItemDetailUnitPrice UnitPrice { get; set; }
		string Description { get; set; }
		string UnitOfMeasure { get; set; }
		string Classification { get; set; }
		string URL { get; set; }
		string LineItemID { get; set; }
		RequestProductType ProductType { get; set; }
		int QuantityMultiplier { get; set; }
		string CostCenter { get; set; }
		string CostCenterInteropID { get; set; }
		IDictionary<string, string> ProductSpecs { get; set; }
		string ProductInteropID { get; set; }
		string VariantInteropID { get; set; }
		string ShippingAddressInteropID { get; set; }
		string TaxCode { get; set; }
		Double TaxableShippingAmount { get; set; }
		Double TaxableItemAmount { get; set; }
		Double ShipWeight { get; set; }
		string RequestedShipper { get; set; }
		string RequestedShippingAccount { get; set; }
		string Four51UserName { get; set; }
	}


	public interface ISalesOrderLineItemItemDetailUnitPrice {
		double Money { get; set; }
	}

	public interface ISalesOrderLineItemItemID {
		string SupplierPartID { get; set; }
		string SupplierPartAuxiliaryID { get; set; }
	}
	#endregion

	public interface ISalesOrderAddress {
		string ID { get; set; }
		string Name { get; set; }
		SalesOrderPostalAddress PostalAddress { get; set; }
		string Email { get; set; }
		string Phone { get; set; }
	}

	public interface ISalesOrderPostalAddress {
		string Name { get; set; }
		string DeliverTo { get; set; }
		string StreetLineOne { get; set; }
		string StreetLineTwo { get; set; }
		string City { get; set; }
		string State { get; set; }
		string PostalCode { get; set; }
		string Country { get; set; }
	}
}
