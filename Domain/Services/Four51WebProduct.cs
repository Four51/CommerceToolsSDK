using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Four51.APISDK.Four51Product;
using Four51.APISDK.Common;

namespace Four51.APISDK.Services
{

	//public class Four51WebProductList : List<Four51WebProduct>
	//{
	//	private List<Four51WebProduct> _list { get; set; }
	//	private Product _product { get; set; }
	//	private string _sharedsecret { get; set; }
	//	private string _serviceid { get; set; }

	//	public Four51WebProductList(string SharedSecret, string ServiceId)
	//	{
	//		_list = new List<Four51WebProduct>();
	//		_product = new Product()
	//		{
	//			Url = string.Format("http://www.four51.com/services/Product.asmx?id={0}", ServiceId)
	//		};
	//		_sharedsecret = SharedSecret;
	//		_serviceid = ServiceId;
	//		this.Load();
	//	}

	//	private void Load()
	//	{
	//		var products = _product.ListProducts(_sharedsecret);
	//		foreach (var p in products)
	//		{
	//			_list.Add(new Four51WebProduct(_sharedsecret, _serviceid) {
	//				InteropID = p.InteropID
	//			});
	//		}
	//	}

	//	#region implements
	//	public int IndexOf(Four51WebProduct item)
	//	{
	//		return _list.IndexOf(item);
	//	}

	//	public void Insert(int index, Four51WebProduct item)
	//	{
	//		_list.Insert(index, item);
	//	}

	//	public void RemoveAt(int index)
	//	{
	//		_list.RemoveAt(index);
	//	}

	//	public Four51WebProduct this[int index]
	//	{
	//		get
	//		{
	//			return _list[index];
	//		}
	//		set
	//		{
	//			_list[index] = value;
	//		}
	//	}

	//	public void Add(Four51WebProduct item)
	//	{
	//		_list.Add(item);
	//	}

	//	public void Clear()
	//	{
	//		_list.Clear();
	//	}

	//	public bool Contains(Four51WebProduct item)
	//	{
	//		return _list.Contains(item);
	//	}

	//	public void CopyTo(Four51WebProduct[] array, int arrayIndex)
	//	{
	//		_list.CopyTo(array, arrayIndex);
	//	}

	//	public int Count
	//	{
	//		get { return _list.Count; }
	//	}

	//	public bool IsReadOnly
	//	{
	//		get { return false; }
	//	}

	//	public bool Remove(Four51WebProduct item)
	//	{
	//		return _list.Remove(item);
	//	}

	//	public IEnumerator<Four51WebProduct> GetEnumerator()
	//	{
	//		return _list.GetEnumerator();
	//	}

	//	#endregion
	//}

	public class Four51WebProduct : BaseService {
		private Product _product;

		public Four51WebProduct(string SharedSecret, string ServiceId)
		{
			_product = new Product()
			{
				Url = string.Format("http://www.four51.com/services/Product.asmx?id={0}", ServiceId)
			};
			this.ServiceId = ServiceId;
			this.SharedSecret = SharedSecret;
		}

		#region Properties
		public string InteropID { get; set; }
		public string NewInteropID { get; set; }
		public string ID { get; set; }
		public string Name { get; set; }
		public string UNSPSC { get; set; }
		public string UnitOfMeasure { get; set; }
		public string Description { get; set; }
		public Four51Product.ProductType Type { get; set; }
		public bool StandardOrders { get; set; }
		public bool ReplenishmentOrders { get; set; }
		public bool PriceRequests { get; set; }
		public int QuantityMultiplier { get; set; }
		public decimal ShipWeight { get; set; }
		public bool TrackInventory { get; set; }
		public bool AllowOrdersToExceedInventory { get; set; }
		public bool UseVariantLevelInventoryTracking { get; set; }
		public int InventoryNotificationPoint { get; set; }
		public bool DisplayInventoryToBuyer { get; set; }
		public Four51WebProductImage SmallImage { get; set; }
		public Four51WebProductImage LargeImage { get; set; }
		public bool Active { get; set; }

		public List<Four51WebVariant> Variants = new List<Four51WebVariant>();
		public List<Four51WebPriceSchedule> PriceSchedules = new List<Four51WebPriceSchedule>();
		public List<Four51WebPriceScheduleAssignment> PriceScheduleAssignments = new List<Four51WebPriceScheduleAssignment>();

		public int AvailableQuantity { get; set; }
		public bool DecrementByReservedQuantity { get; set; }
		#endregion

		public void Save() {
			var product = new ProductProperties[1];
			product[0] = new ProductProperties()
			{
				InteropID = InteropID,
				NewInteropID = NewInteropID,
				ProductID = ID,
				Name = Name,
				UNSPSC = UNSPSC,
				UnitOfMeasure = UnitOfMeasure,
				Description = Description,
				Type = Type,
				StandardOrders = StandardOrders,
				ReplenishmentOrders = ReplenishmentOrders,
				PriceRequests = PriceRequests,
				QuantityMultiplier = QuantityMultiplier,
				ShipWeight = ShipWeight,
				TrackInventory = TrackInventory,
				AllowOrdersToExceedInventory = AllowOrdersToExceedInventory,
				UseVariantLevelInventoryTracking = UseVariantLevelInventoryTracking,
				InventoryNotificationPoint = InventoryNotificationPoint,
				DisplayInventoryToBuyer = DisplayInventoryToBuyer,
				SmallImage = SmallImage == null ? null : SmallImage.ToFile(),
				LargeImage = LargeImage == null ? null : LargeImage.ToFile(),
				Active = Active
			};
			var error = _product.SaveProducts(product, this.SharedSecret);
			if (error.Length > 0)
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
			Variants.ForEach(v => v.Save(product[0]));
			PriceSchedules.ForEach(p => p.Save(product[0]));
			if (TrackInventory) UpdateInventory();
			PriceScheduleAssignments.ForEach(p => p.Save(this.InteropID));
		}

		public void Delete() {
			var error = _product.DeleteProduct(new string[] { InteropID }, this.SharedSecret);
			if (error.Length > 0)
					throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
		}

		public void UpdateInventory() {
			var inv = new Inventory[1];
			inv[0] = new Inventory()
			{
				AvailableQuantity = AvailableQuantity,
				DecrementByReservedQuantity = DecrementByReservedQuantity,
				ProductInteropID = NewInteropID.IsNullOrWhiteSpace() ? InteropID : NewInteropID
			};

			var error = _product.UpdateProductInventory(inv, this.SharedSecret);
			if (error.Length > 0)
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
		}
	}

	public class Four51WebPriceScheduleAssignment : BaseService {
		private PriceScheduleAssignment[] _assignment;
		private Product _product;

		public Four51WebPriceScheduleAssignment(string SharedSecret, string ServiceId)
		{
			_product = new Product() {
				Url = String.Format("http://www.four51.com/services/Product.asmx?id={0}", ServiceId)
			};
			_assignment = new PriceScheduleAssignment[0];
			this.ServiceId = ServiceId;
			this.SharedSecret = SharedSecret;
		}

		#region Properties
		public PartyType AssignmentLevel { get; set; }
		public string BuyerCompanyInteropID { get; set; }
		public string ProductInteropID { get; set; }
		public string VariantInteropID { get; set; }
		public string PartyInteropID { get; set; }
		public string StandardOrderScheduleName { get; set; }
		public string ReplenishmentOrderScheduleName { get; set; }
		public string PriceRequestOrderScheduleName { get; set; }
		#endregion

		public void Save(string productid = null, string variantid = null)
		{
			var assignment = new PriceScheduleAssignment[1];
			assignment[0] = new PriceScheduleAssignment()
			{
				ProductInteropID = productid ?? ProductInteropID,
				VariantInteropID = variantid ?? VariantInteropID,
				BuyerCompanyInteropID = BuyerCompanyInteropID,
				AssignmentLevel = AssignmentLevel,
				PartyInteropID = PartyInteropID,
				StandardOrderScheduleName = StandardOrderScheduleName ?? "",
				ReplenishmentOrderScheduleName = ReplenishmentOrderScheduleName ?? "",
				PriceRequestOrderScheduleName = PriceRequestOrderScheduleName ?? ""
			};
			var error = _product.SavePriceScheduleAssignments(assignment, this.SharedSecret);
			if (error.Length > 0)
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
		}
	}

	public class Four51WebVariant : BaseService
	{
		private VariantProperties[] _variant;
		private Product _product;

		public Four51WebVariant(string SharedSecret, string ServiceId)
		{
			_product = new Product()
			{
				Url = String.Format("http://www.four51.com/services/Product.asmx?id={0}", ServiceId)
			};
			_variant = new VariantProperties[0];
			this.ServiceId = ServiceId;
			this.SharedSecret = SharedSecret;
		}

		#region Properties
		public string InteropID { get; set; }
		public string ID { get; set; }
		public string ProductInteropID { get; set; }
		public string NewInteropID { get; set; }
		public string Description { get; set; }
		public Four51WebProductImage Image { get; set; }
		public bool Active { get; set; }
		public int AvailableQuantity { get; set; }
		public bool DecrementByReservedQuantity { get; set; }
		public List<Four51WebPriceScheduleAssignment> PriceScheduleAssignments = new List<Four51WebPriceScheduleAssignment>();
		#endregion

		public VariantProperties[] ToVariant(string productinteropid = null) {
			var variant = new VariantProperties[1];
			variant[0] = new VariantProperties()
			{
				InteropID = InteropID,
				VariantID = ID,
				ProductInteropID = productinteropid ?? ProductInteropID,
				NewInteropID = NewInteropID ?? InteropID,
				Description = Description,
				Image = Image == null ? null : Image.ToFile(),
				Active = Active
			};
			return variant;
		}

		public void UpdateInventory() {
			var inv = new Inventory[1];
			inv[0] = new Inventory()
			{
				AvailableQuantity = AvailableQuantity,
				DecrementByReservedQuantity = DecrementByReservedQuantity,
				ProductInteropID = ProductInteropID,
				VariantInteropID = InteropID
			};

			var error = _product.UpdateProductInventory(inv, this.SharedSecret);
			if (error.Length > 0 && !error[0].ErrorMessage.Contains("enabled"))
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
		}

		public void Save()
		{
			this.Save(_variant[0].ProductInteropID);
		}

		public void Save(ProductProperties product)
		{
			this.Save(product.NewInteropID.IsNullOrWhiteSpace() ? product.InteropID : product.NewInteropID);
		}

		private void Save(string productid)
		{
			var error = _product.SaveVariants(this.ToVariant(), this.SharedSecret);
			if (error.Length > 0)
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
			UpdateInventory();
			this.PriceScheduleAssignments.ForEach(p => p.Save(null, this.InteropID));
		}

		public void Delete() {
			var error = _product.DeleteVariants(this.ToVariant(), this.SharedSecret);
			if (error.Length > 0)
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
		}
	}

	public class Four51WebPriceSchedule : BaseService {
		private PriceScheduleProperties[] _price;
		private Product _product;

		public Four51WebPriceSchedule(string SharedSecret, string ServiceId)
		{
			_product = new Product() {
				Url = String.Format("http://www.four51.com/services/Product.asmx?id={0}", ServiceId)
			};
			_price = new PriceScheduleProperties[0];
			this.ServiceId = ServiceId;
			this.SharedSecret = SharedSecret;
			this.PriceBreaks = new List<Four51WebPriceBreak>();
		}

		#region Properties
		public string ProductInteropID { get; set; }
		public string Name { get; set; }
		public Four51Product.OrderType OrderType { get; set; }
		public bool ApplySalesTax { get; set; }
		public bool ApplyShipping { get; set; }
		public bool RestrictedQuantitySelection { get; set; }
		public int MinimumQuantity { get; set; }
		public int MaximumQuantity { get; set; }
		public bool UseCumulativeQuantity { get; set; }
		public List<Four51WebPriceBreak> PriceBreaks = new List<Four51WebPriceBreak>();
		#endregion

		public void Save()
		{
			this.Save(_price[0].ProductInteropID);
		}

		public void Save(ProductProperties product)
		{
			this.Save(product.NewInteropID.IsNullOrWhiteSpace() ? product.InteropID : product.NewInteropID);
		}

		private void Save(string productid) {
			var price = new PriceScheduleProperties[1];
			price[0] = new PriceScheduleProperties()
			{
				ProductInteropID = productid,
				Name = Name,
				OrderType = OrderType,
				ApplySalesTax = ApplySalesTax,
				ApplyShipping = ApplyShipping,
				RestrictedQuantitySelection = RestrictedQuantitySelection,
				MinimumQuantity = MinimumQuantity,
				MaximumQuantity = MaximumQuantity,
				UseCumulativeQuantity = UseCumulativeQuantity,
				PriceBreaks = PriceBreaks.Select(p => p.ToBreak()).ToArray()
			};

			var error = _product.SavePriceSchedules(price, this.SharedSecret);
			if (error.Length > 0)
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
		}

		public void Delete() {
			var ids = new PriceScheduleID[1];
			ids[0] = new PriceScheduleID()
			{
				ProductInteropID = ProductInteropID,
				PriceScheduleName = Name
			};
			var error = new Product().DeletePriceSchedule(ids, this.SharedSecret);
			if (error.Length > 0)
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
		}
	}

	public class Four51WebPriceBreak {
		private PriceBreak _break;

		public Four51WebPriceBreak() {
			_break = new PriceBreak();
		}

		#region Properties
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		#endregion

		public PriceBreak ToBreak() {
			_break.Price = Price;
			_break.Quantity = Quantity;
			return _break;
		}
	}

	public class Four51WebProductImage {
		private ProductFile file;

		public Four51WebProductImage() {
			file = new ProductFile();
		}
		public Four51WebProductImage(string filename) {
			file = new ProductFile()
			{
				FileName = filename
			};
			var name = filename.Split('\\');
			Name = name[name.Length-1];
			this.Load(filename);
		}
		
		#region Properties
		public string Name { get; set; }
		private byte[] Data { get; set; }

		public void Load(string FilePath)
		{
			Data = File.ReadAllBytes(FilePath);
		}
		#endregion

		public ProductFile ToFile() {
			file.FileName = Name;
			file.File = Data;
			return file;
		}
	}
}
