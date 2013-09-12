using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;
using Four51.APISDK.Services;
using Four51.APISDK.Enums;
using Four51.APISDK.Four51Product;
using Four51.APISDK.Four51OrderFields;
using Four51.APISDK.Common;
using RestSharp;

namespace SDK_Examples
{
	public partial class InputForm : Form
	{
		private Utility _util;

		public InputForm()
		{
			InitializeComponent();
			_util = new Utility();
		}

		private void btn_save_Click(object sender, EventArgs e)
		{
			var address = new Four51WebAddress(txt_sharedsecret.Text, txt_serviceid.Text)
			{
				InteropID = txt_address_interopid.Text == "" ? Guid.NewGuid().ToString() : txt_address_interopid.Text,
				CompanyInteropID = txt_address_companyinteropid.Text,
				Name = txt_address_name.Text,
				FirstName = txt_address_firstname.Text,
				LastName = txt_address_lastname.Text,
				CompanyName = txt_address_companyname.Text,
				AddressLineOne = txt_address_addresslineone.Text,
				AddressLineTwo = txt_address_addresslinetwo.Text,
				City = txt_address_city.Text,
				State = txt_address_state.Text,
				Zip = txt_address_zip.Text,
				Country = txt_address_country.Text,
				PhoneNumber = txt_address_phonenumber.Text
			};
			try
			{
				address.Save();
               MessageBox.Show("Saved: " + address.Name);
			}
			catch (Exception ex)
			{
                MessageBox.Show(ex.Message);
			}
		}

		private void btn_address_delete_Click(object sender, EventArgs e)
		{
			var address = new Four51WebAddress(txt_sharedsecret.Text, txt_serviceid.Text)
			{
				InteropID = txt_address_interopid.Text,
				CompanyInteropID = txt_address_companyinteropid.Text
			};
			try
			{
				address.Delete(false);
                MessageBox.Show("Deleted: " + address.InteropID);
            }
			catch (Exception ex)
			{
                MessageBox.Show(ex.Message);
			}
		}

		private void btn_address_random_Click(object sender, EventArgs e)
		{
			txt_address_name.Text = "Name - " + _util.RandomString();
			txt_address_firstname.Text = "FirstName - " + _util.RandomString();
			txt_address_lastname.Text = "LastName - " + _util.RandomString();
			txt_address_companyname.Text = "CompanyName - " + _util.RandomString();
			txt_address_addresslineone.Text = "AddressLineOne - " + _util.RandomString();
			txt_address_addresslinetwo.Text = "AddressLineTwo - " + _util.RandomString();
			txt_address_city.Text = "City - " + _util.RandomString();
			txt_address_state.Text = "MN";
			txt_address_zip.Text = _util.RandomNumber();
			txt_address_country.Text = "US";
			txt_address_phonenumber.Text = string.Format("{0}-{1}-{2}", _util.RandomNumber(3), _util.RandomNumber(3), _util.RandomNumber(4));
		}

		private void btn_costcenter_random_Click(object sender, EventArgs e)
		{
			txt_costcenter_name.Text = "Name - " + _util.RandomString();
            txt_costcenter_description.Text = "Description - " + _util.RandomString(30);
		}

        private void btn_costcenter_save_Click(object sender, EventArgs e)
        {
            var costcenter = new Four51WebCostCenter(txt_sharedsecret.Text, txt_serviceid.Text)
            {
                InteropID = txt_costcenter_interopid.Text == "" ? Guid.NewGuid().To<string>() : txt_costcenter_interopid.Text,
                CompanyInteropID = txt_costcenter_companyinteropid.Text,
                Name = txt_costcenter_name.Text,
                Description = txt_costcenter_description.Text
            };
            try
            {
                costcenter.Save();
                MessageBox.Show("Saved: " + costcenter.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_costcenter_delete_Click(object sender, EventArgs e)
        {
            var costcenter = new Four51WebCostCenter(txt_sharedsecret.Text, txt_serviceid.Text)
            {
                InteropID = txt_costcenter_interopid.Text,
                CompanyInteropID = txt_costcenter_companyinteropid.Text
            };
            try
            {
                costcenter.Delete();
                MessageBox.Show("Deleted: " + costcenter.InteropID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

		private void btn_group_random_Click(object sender, EventArgs e)
		{
			txt_group_name.Text = "Name - " + _util.RandomString();
			txt_group_description.Text = "Description - " + _util.RandomString(30);
		}

		private void btn_group_save_Click(object sender, EventArgs e)
		{
			var group = new Four51WebGroup(txt_sharedsecret.Text, txt_serviceid.Text)
			{
				InteropID = txt_group_interopid.Text == "" ? Guid.NewGuid().To<string>() : txt_group_interopid.Text,
				BuyerCompanyInteropID = txt_group_companyinteropid.Text,
				Description = txt_group_description.Text,
				Name = txt_group_name.Text
			};

			foreach (DataGridViewRow row in grid_group_address.Rows)
			{
				if (row.Cells[0].Value == null) break;
				group.Address.Add(new Four51WebAddressAssignment()
				{
					AddressInteropID = row.Cells[0].Value.To<string>(),
					UseForShipping = row.Cells[1].Value.To<bool>(),
					UseForBilling = row.Cells[2].Value.To<bool>()
				});
			}

			try
			{
				group.Save();
				MessageBox.Show("Saved: " + group.InteropID);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btn_group_delete_Click(object sender, EventArgs e)
		{
			var group = new Four51WebGroup(txt_sharedsecret.Text, txt_serviceid.Text)
			{
				InteropID = txt_group_interopid.Text,
				BuyerCompanyInteropID = txt_group_companyinteropid.Text
			};
			try
			{
				group.Delete();
				MessageBox.Show("Deleted: " + group.InteropID);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btn_user_random_Click(object sender, EventArgs e)
		{
			chk_user_active.Checked = true;
			chk_user_terms.Checked = true;
			txt_user_firstname.Text = "FirstName - " + _util.RandomString(10);
			txt_user_lastname.Text = "LastName - " + _util.RandomString(12);
			txt_user_username.Text = "user:" + _util.RandomString();
			txt_user_password.Text = "password";
			txt_user_phonenumber.Text = string.Format("{0}-{1}-{2}", _util.RandomNumber(3), _util.RandomNumber(3), _util.RandomNumber(4));
			txt_user_email.Text = txt_user_username.Text + "@four51.com";
		}

		private void btn_user_save_Click(object sender, EventArgs e)
		{
			var user = new Four51WebUser(txt_sharedsecret.Text, txt_serviceid.Text)
			{
				InteropID = txt_user_interopid.Text == "" ? Guid.NewGuid().To<string>() : txt_user_interopid.Text,
				CompanyInteropID = txt_user_companyinteropid.Text,
				Active = chk_user_active.Checked,
				Email = txt_user_email.Text,
				FirstName = txt_user_firstname.Text,
				LastName = txt_user_lastname.Text,
				Password = txt_user_password.Text.Length > 0 ? txt_user_password.Text : null,
				PhoneNumber = txt_user_phonenumber.Text,
				TermsAccepted = chk_user_terms.Checked,
				UserName = txt_user_username.Text
			};

			foreach (DataGridViewRow row in grid_user_address.Rows)
			{
				if (row.Cells[0].Value == null) break;
				user.Addresses.Add(new Four51WebAddressAssignment()
				{
					AddressInteropID = row.Cells[0].Value.To<string>(),
					UseForShipping = row.Cells[1].Value.To<bool>(),
					UseForBilling = row.Cells[2].Value.To<bool>()
				});
			}

			foreach (DataGridViewRow row in grid_user_costcenters.Rows)
			{
				if (row.Cells[0].Value == null) break;
				user.CostCenters.Add(new Four51WebCostCenterAssignment()
				{
					CostCenterInteropID = row.Cells[0].Value.To<string>()
				});
			}

			foreach (DataGridViewRow row in grid_user_groups.Rows)
			{
				if (row.Cells[0].Value == null) break;
				user.Groups.Add(new Four51WebGroupAssignment()
				{
					GroupInteropID = row.Cells[0].Value.To<string>(),
					Remove = row.Cells[1].Value.To<bool>()
				});
			}

			foreach (DataGridViewRow row in grid_user_orderfields.Rows)
			{
				if (row.Cells[0].Value == null) break;
				user.OrderFields.Add(new Four51WebOrderFieldAssignment()
				{
					Name = row.Cells[0].Value.To<string>(),
					OptionInteropID = row.Cells[1].Value.To<string>()
				});
			}

			try
			{
				if (!user.Password.IsNullOrWhiteSpace())
					user.Save();
				else
					user.Sync();
				MessageBox.Show("Saved: " + user.InteropID);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btn_user_delete_Click(object sender, EventArgs e)
		{
			var user = new Four51WebUser(txt_sharedsecret.Text, txt_serviceid.Text)
			{
				InteropID = txt_user_interopid.Text,
				CompanyInteropID = txt_user_companyinteropid.Text
			};
			try
			{
				user.Delete();
				MessageBox.Show("Deleted: " + user.InteropID);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void pnlThumbnail_Click(object sender, EventArgs e)
		{
			var result = openSmallImage.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				pnlThumbnail.BackgroundImage = Image.FromFile(openSmallImage.FileName);
			}
		}

		private void pnlImage_Click(object sender, EventArgs e)
		{
			var result = openLargeImage.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				pnlImage.BackgroundImage = Image.FromFile(openLargeImage.FileName);
			}
		}

		private void btn_product_random_Click(object sender, EventArgs e)
		{
			txt_product_productid.Text = "ID: " + _util.RandomString();
			txt_product_name.Text = "Name: " + _util.RandomString();
			txt_product_unspsc.Text = "UNSPSC: " + _util.RandomString();
			txt_product_unitofmeasure.Text = _util.RandomNumber(1);
			txt_product_description.Text = "Description: " + _util.RandomString(30);
			ddl_product_type.SelectedIndex = Math.Min(_util.RandomNumber(1).To<int>(), 2);
			chk_product_standardorders.Checked = true;
			txt_product_quantitymultiplier.Text = Math.Min(_util.RandomNumber(4).To<int>(), 1).ToString();
			txt_product_shipweight.Text = _util.RandomNumber(2);
			chk_product_active.Checked = true;

			// variant randomness add 2 rows
			grid_variants.Rows.Clear();
			for(var i = 0; i <= 1; i++) {
				grid_variants.Rows.Add();
				var row = grid_variants.Rows[i];
				row.Cells[0].Value = "Variant - " + txt_product_productid.Text + " - " + _util.RandomString(5);
				row.Cells[2].Value = _util.RandomString(10);
				row.Cells[3].Value = "Description: " + _util.RandomString(30);
				row.Cells[5].Value = true;
				row.Cells[6].Value = _util.RandomNumber(2);
				row.Cells[7].Value = false;
			}
			// price schedule randomness add 2 price braks
			grid_pricebreaks.Rows.Clear();
			for (var i = 1; i <= 6; i++)
			{
				grid_pricebreaks.Rows.Add();
				var row = grid_pricebreaks.Rows[i-1];
				row.Cells[1].Value = i * 5;
				row.Cells[0].Value = i * 10 + ".00";
			}

			grid_priceschedules.Rows.Clear();
			for (var i = 0; i <= 2; i++)
			{
				grid_priceschedules.Rows.Add();
				var row = grid_priceschedules.Rows[i];
				row.Cells[0].Value = "PriceSchedule: " + _util.RandomString(5);
				row.Cells[1].Value = _util.RandomNumber(1);
				row.Cells[2].Value = _util.RandomNumber(2);
				row.Cells[6].Value = true;
				row.Cells[3].Value = "Standard";
			}
			
		}

		private void btn_product_save_Click(object sender, EventArgs e)
		{
			var product = new Four51WebProduct(txt_sharedsecret.Text, txt_serviceid.Text)
			{
				InteropID = txt_product_interopid.Text,
				NewInteropID = txt_product_newinteropid.Text,
				ID = txt_product_productid.Text,
				Name = txt_product_name.Text,
				UNSPSC = txt_product_unspsc.Text,
				UnitOfMeasure = txt_product_unitofmeasure.Text,
				Description = txt_product_description.Text,
				Type = ddl_product_type.SelectedItem != null ? ddl_product_type.SelectedItem.To<string>().Replace(" ", "").To<ProductType>() : ProductType.Static,
				StandardOrders = chk_product_standardorders.Checked,
				ReplenishmentOrders = chk_product_replenishmentorders.Checked,
				PriceRequests = chk_product_pricerequests.Checked,
				QuantityMultiplier = txt_product_quantitymultiplier.Text.To<int>(),
				ShipWeight = txt_product_shipweight.Text.To<decimal>(),
				TrackInventory = chk_product_trackinventory.Checked,
				AllowOrdersToExceedInventory = chk_product_allowexceed.Checked,
				InventoryNotificationPoint = txt_product_inventorynotification.Text.To<int>(),
				AvailableQuantity = txt_product_quantity.Text.To<int>(),
				DisplayInventoryToBuyer = chk_product_displayinventory.Checked,
				Active = chk_product_active.Checked,
				LargeImage = openLargeImage.FileName.Length > 0 ? new Four51WebProductImage(openLargeImage.FileName) : null,
				SmallImage = openSmallImage.FileName.Length > 0 ? new Four51WebProductImage(openSmallImage.FileName) : null
			};

			foreach (DataGridViewRow row in grid_variants.Rows)
			{
				if (row.Cells[0].Value == null) break;
				product.Variants.Add(new Four51WebVariant(txt_sharedsecret.Text, txt_serviceid.Text)
				{
					InteropID = row.Cells[2].Value.To<string>(),
					NewInteropID = row.Cells[1].Value.To<string>(),
					ID = row.Cells[0].Value.To<string>(),
					Description = row.Cells[3].Value.To<string>(),
					Active = row.Cells[5].Value.To<bool>(),
					Image = row.Cells["FileName"].To<string>().IsNullOrWhiteSpace() ? null : new Four51WebProductImage(row.Cells["FileName"].Value.To<string>()),
					AvailableQuantity = row.Cells[6].Value.To<int>(),
					DecrementByReservedQuantity = row.Cells[7].Value.To<bool>(),
					ProductInteropID = txt_product_interopid.Text
				});
			}

			foreach (DataGridViewRow row in grid_priceschedules.Rows)
			{
				if (row.Cells[0].Value == null) break;
				product.PriceSchedules.Add(new Four51WebPriceSchedule(txt_sharedsecret.Text, txt_serviceid.Text)
				{
					ProductInteropID = txt_product_interopid.Text,
					Name = row.Cells[0].Value.To<string>(),
					MinimumQuantity = row.Cells[1].Value.To<int>(),
					MaximumQuantity = row.Cells[2].Value.To<int>(),
					OrderType = (Four51.APISDK.Four51Product.OrderType)Enum.Parse(typeof(Four51.APISDK.Four51Product.OrderType), row.Cells[3].Value.To<string>()),
					ApplySalesTax = row.Cells[4].Value.To<bool>(),
					ApplyShipping = row.Cells[5].Value.To<bool>(),
					RestrictedQuantitySelection = row.Cells[6].Value.To<bool>(),
					UseCumulativeQuantity = row.Cells[7].Value.To<bool>()
				});

				if (row.Cells[10].Value.To<string>().Length > 0)
				{
					product.PriceScheduleAssignments.Add(new Four51WebPriceScheduleAssignment(txt_sharedsecret.Text, txt_serviceid.Text)
					{
						ProductInteropID = row.Cells[8].Value.To<string>() == txt_product_interopid.Text ? product.InteropID : null,
						VariantInteropID = row.Cells[8].Value.To<string>() == txt_product_interopid.Text ? null :  row.Cells[8].Value.To<string>(),
						BuyerCompanyInteropID = txt_product_companyinteropid.Text,
						AssignmentLevel = row.Cells[9].Value.To<PartyType>(),
						PartyInteropID = row.Cells[10].Value.To<string>(),
						StandardOrderScheduleName = row.Cells[3].Value.To<Four51.APISDK.Four51Product.OrderType>() == Four51.APISDK.Four51Product.OrderType.Standard ? row.Cells[0].Value.To<string>() : null,
						ReplenishmentOrderScheduleName = row.Cells[3].Value.To<Four51.APISDK.Four51Product.OrderType>() == Four51.APISDK.Four51Product.OrderType.Replenishment ? row.Cells[0].Value.To<string>() : null,
						PriceRequestOrderScheduleName = row.Cells[3].Value.To<Four51.APISDK.Four51Product.OrderType>() == Four51.APISDK.Four51Product.OrderType.PriceRequest ? row.Cells[0].Value.To<string>() : null
					});
				}

				foreach (DataGridViewRow rw in grid_pricebreaks.Rows)
				{
					if (rw.Cells[0].Value == null) break;
					product.PriceSchedules.Last().PriceBreaks.Add(new Four51WebPriceBreak()
					{
						Price = rw.Cells[0].Value.To<decimal>(),
						Quantity = rw.Cells[1].Value.To<int>()
					});
				}
			};

			try
			{
				product.Save();
				MessageBox.Show("Saved: " + product.ID);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void grid_variants_CellContentClick(object sender, DataGridViewCellEventArgs e) {
			if (grid_variants.Columns[e.ColumnIndex] is DataGridViewImageColumn) {
				var result = openVariantImage.ShowDialog();
				if (result == System.Windows.Forms.DialogResult.OK) {
					grid_variants[e.ColumnIndex, e.RowIndex].Value = Image.FromFile(openVariantImage.FileName);
					grid_variants["FileName", e.RowIndex].Value = openVariantImage.FileName;
				}
			}
			if (grid_variants.Columns[e.ColumnIndex] is DataGridViewButtonColumn) {
				var variant = new Four51WebVariant(txt_sharedsecret.Text, txt_serviceid.Text) {
					InteropID = grid_variants[2, e.RowIndex].Value.To<string>(),
					ID = grid_variants[0, e.RowIndex].Value.To<string>(),
					ProductInteropID = txt_product_interopid.Text
				};
				try {
					variant.Delete();
					MessageBox.Show("Deleted: " + variant.InteropID);
				}
				catch (Exception ex) {
					MessageBox.Show(ex.Message);
				}
			}
			this.FillPriceScheduleInteropIDs();
		}

		private void btn_product_delete_Click(object sender, EventArgs e)
		{
			var product = new Four51WebProduct(txt_sharedsecret.Text, txt_serviceid.Text)
			{
				InteropID = txt_product_interopid.Text
			};
			try
			{
				product.Delete();
				MessageBox.Show("Deleted: " + product.InteropID);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btn_spendingaccount_random_Click(object sender, EventArgs e)
		{
			txt_spendingaccount_name.Text = "Name: " +_util.RandomString();
			txt_spendingaccount_label.Text = "Label: " + _util.RandomString();
			txt_spendingaccount_balance.Text = _util.RandomNumber();
		}

		private void btn_spendingaccount_save_Click(object sender, EventArgs e)
		{
			var account = new Four51WebSpendingAccount(txt_sharedsecret.Text, txt_serviceid.Text)
			{
				InteropID = txt_spendingaccount_interopid.Text,
				Name = txt_spendingaccount_name.Text,
				Label = txt_spendingaccount_label.Text,
				SpendingAccountTypeName = txt_spendingaccount_type.Text,
				CompanyInteropID = txt_spendingaccount_companyid.Text,
				GroupInteropID = txt_spendingaccount_groupid.Text,
				UserInteropID = txt_spendingaccount_userid.Text,
				Balance = txt_spendingaccount_balance.Text.To<decimal>(),
				StartDate = date_spendingaccount_start.Value,
				ExpirationDate = date_spendingaccount_expiration.Value
			};
			try
			{
				account.Save();
				MessageBox.Show("Saved Spending Account: " + account.InteropID);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btn_spendingaccount_delete_Click(object sender, EventArgs e)
		{
			var account = new Four51WebSpendingAccount(txt_sharedsecret.Text, txt_serviceid.Text)
			{
				InteropID = txt_spendingaccount_interopid.Text
			};
			try
			{
				account.Delete();
				MessageBox.Show("Deleted Spending Account: " + account.InteropID);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btn_orderfields_random_Click(object sender, EventArgs e)
		{
			txt_orderfields_name.Text = "Name: " + _util.RandomString();
			txt_orderfields_label.Text = "Label: " + _util.RandomString();
			ddl_orderfields_type.SelectedIndex = 0;
			txt_orderfields_lines.Text = 1.To<string>();
			txt_orderfields_width.Text = 80.To<string>();
			txt_orderfields_maxlength.Text = 100.To<string>();
		}


		private void btn_ship_random_Click(object sender, EventArgs e)
		{
			txt_ship_shipmentid.Text = "ID: " + _util.RandomString();
			txt_ship_noticedate.Text = DateTime.Now.ToShortDateString();
			cmb_ship_carrier.SelectedIndex = 0;
			txt_ship_identifier.Text = _util.RandomString(30);
			txt_ship_cost.Text = _util.RandomNumber(2) + "." + _util.RandomNumber(2);
		}

		private void btn_orderfields_save_Click(object sender, EventArgs e)
		{
			var fields = new Four51WebOrderFields(txt_sharedsecret.Text, txt_serviceid.Text)
			{
				Name = txt_orderfields_name.Text,
				Label = txt_orderfields_label.Text,
				Type = ddl_orderfields_type.Text.Replace(" ", string.Empty).To<OrderFieldType>(),
				Required = chk_orderfields_required.To<bool>(),
				DefaultValue = txt_orderfields_default.Text,
				ExplicitOptionsAssignment = chk_orderfields_explicit.To<bool>(),
				Lines = txt_orderfields_lines.Text.To<int>(),
				Width = txt_orderfields_width.Text.To<int>(),
				MaxLength = txt_orderfields_maxlength.Text.To<int>()
			};
			foreach (DataGridViewRow rw in grid_orderfields_options.Rows)
			{
				if (rw.Cells[0].Value == null) break;
				fields.Options.Add(new Four51WebOrderFieldOptions()
				{
					InteropID = rw.Cells[0].Value.To<string>(),
					Value = rw.Cells[1].Value.To<string>()
				});
			}

			try
			{
				fields.Save();
				MessageBox.Show("Saved Order Field: " + fields.Name);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void grid_orderfields_options_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (grid_orderfields_options.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
			{
				var fields = new Four51WebOrderFields(txt_sharedsecret.Text, txt_serviceid.Text)
				{
					Name = txt_orderfields_name.Text
				};
				
				fields.Options.Add(new Four51WebOrderFieldOptions()
				{
					InteropID = grid_orderfields_options[1, e.RowIndex].Value.To<string>(),
					Value = grid_orderfields_options[0, e.RowIndex].Value.To<string>()
				});

				try
				{
					fields.DeleteOptions();
					MessageBox.Show("Deleted options");
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void btn_orderfields_delete_Click(object sender, EventArgs e)
		{
			var fields = new Four51WebOrderFields(txt_sharedsecret.Text, txt_serviceid.Text)
			{
				Name = txt_orderfields_name.Text
			};
			try
			{
				fields.Delete();
				MessageBox.Show("Deleted Order Field: " + fields.Name);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btn_orderrequest_get_Click(object sender, EventArgs e)
		{
			var url = new StringBuilder("http://www.four51.com/services/");
			url.Append(string.Format("{0}/{1}/orders/", txt_serviceid.Text, txt_sharedsecret.Text));
			var list = new Four51WebSalesOrderList(txt_sharedsecret.Text, txt_serviceid.Text);
			grid_orders.Rows.Clear();
			foreach (var o in list.Orders)
			{
				grid_orders.Rows.Add();
				var row = grid_orders.Rows[grid_orders.Rows.GetLastRow(DataGridViewElementStates.None)-1];
				row.Cells[0].Value = o.ID;
				row.Cells[1].Value = o.DownloadStatus;
				row.Cells[2].Value = o.Url;
			}
		}
		//TODO: use as exmample to display order information
		private void grid_orders_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
		{
			if (grid_orders.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
			{
				var url = grid_orders[2, e.RowIndex].Value.To<string>();
				var order = new Four51WebSalesOrder();
				order.Map(url);
				txt_orders_result.Text = order.LineItems[1].Detail.Description.ToString();
			}
		}

		private void txt_product_interopid_TextChanged(object sender, EventArgs e)
		{
			this.FillPriceScheduleInteropIDs();
		}

		private void FillPriceScheduleInteropIDs()
		{
			// row 8 for ids
			for (var i = 0; i <= grid_priceschedules.Rows.Count-1; i++)
			{
				var ddl = (DataGridViewComboBoxCell)grid_priceschedules.Rows[i].Cells[8];
				ddl.Items.Clear();
				if (txt_product_interopid.Text.Length > 0)
					ddl.Items.Add(txt_product_interopid.Text);
				foreach (DataGridViewRow rw in grid_variants.Rows)
				{
					if (rw.Cells[2].Value == null) break;
					ddl.Items.Add(rw.Cells[2].Value);
				}
			}
		}

		private void btn_spendingaccount_update_Click(object sender, EventArgs e)
		{
			var acct = new Four51WebSpendingAccount(txt_sharedsecret.Text, txt_serviceid.Text)
			{
				InteropID = txt_spendingaccount_interopid.Text,
				Value = txt_spendingaccount_balance.Text.To<decimal>()
			};
			try
			{
				acct.AddToBalance();
				MessageBox.Show("Added to spending account balance: " + acct.InteropID);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btn_openxml_Click(object sender, EventArgs e)
		{
			DialogResult ok = openOrderXML.ShowDialog();
			if (ok == System.Windows.Forms.DialogResult.OK)
			{
				var file = openOrderXML.OpenFile();
				var order = new Four51WebSalesOrder();
				order.Map(file);
				txt_orderxml.Text = order.ToJson();// order.Request.FromUserIP;
			}
		}

		//private void btn_productlist_get_Click(object sender, EventArgs e)
		//{
		//	grid_products.Rows.Clear();
		//	try
		//	{
		//		List<Four51WebProduct> products = new Four51WebProductList(txt_sharedsecret.Text, txt_serviceid.Text);
		//		products.ForEach(p =>
		//		{
		//			grid_products.Rows.Add();
		//			var row = grid_variants.Rows[grid_products.Rows.Count - 1];
		//			row.Cells[0].Value = p.InteropID;
		//			row.Cells[1].Value = p.ID;
		//			row.Cells[2].Value = p.Name;
		//			row.Cells[3].Value = p.Type.To<string>();
		//			row.Cells[4].Value = p.TrackInventory;
		//		});
		//	}
		//	catch (Exception ex)
		//	{
		//		MessageBox.Show(ex.Message);
		//	}
		//}
	}
}
