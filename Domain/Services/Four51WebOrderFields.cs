using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Four51.APISDK.Four51OrderFields;

namespace Four51.APISDK.Services
{
	public class Four51WebOrderFields : BaseService {

		private OrderField _field;

		public Four51WebOrderFields(string SharedSecret, string ServiceId)
		{
			_field = new OrderField()
			{
				Url = String.Format("http://www.four51.com/services/OrderField.asmx?id={0}", ServiceId)
			};
			this.ServiceId = ServiceId;
			this.SharedSecret = SharedSecret;
			this.Options = new List<Four51WebOrderFieldOptions>();
		}

		#region Properties
		public string Name { get; set; }
		public string Label { get; set; }
		public OrderFieldType Type { get; set; }
		public bool Required { get; set; }
		public string DefaultValue { get; set; }
		public bool ExplicitOptionsAssignment { get; set; }
		public int Lines { get; set; }
		public int Width { get; set; }
		public int MaxLength { get; set; }
		public List<Four51WebOrderFieldOptions> Options { get; set; }
		#endregion

		public void Save() 
		{
			var fields = new OrderFieldProperties[1];
			fields[0] = new OrderFieldProperties()
			{
				Name = Name,
				Label = Label,
				Type = Type,
				Required = Required,
				DefaultValue = DefaultValue,
				ExplicitOptionAssignment = ExplicitOptionsAssignment,
				Lines = Lines,
				Width = Width,
				MaxLength = MaxLength,
				Options = Options.Select(o => o.ToOption()).ToArray()
			};
			var error = _field.Save(fields, this.SharedSecret);
			if (error.Length > 0)
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
		}

		public void Delete()
		{
			var error = _field.Delete(new string[] { Name }, this.SharedSecret);
			if (error.Length > 0)
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
		}

		public void DeleteOptions()
		{
			var error = _field.DeleteOptions(this.Options.Select(o => o.ToUpdate(this.Name)).ToArray(), this.SharedSecret);
			if (error.Length > 0)
				throw new Exception(error[0].InteropID + " failed: " + error[0].ErrorMessage);
		}
	}

	public class Four51WebOrderFieldOptions
	{
		private OrderFieldOptionProperties _options;

		public Four51WebOrderFieldOptions()
		{
			_options = new OrderFieldOptionProperties();
		}

		#region Properties
		public string InteropID { get; set; }
		public string Value { get; set; }
		#endregion

		public OrderFieldOptionProperties ToOption()
		{
			_options.InteropID = InteropID;
			_options.Value = Value;
			return _options;
		}

		public OrderFieldOptionUpadte ToUpdate(string name)
		{
			var delete = new OrderFieldOptionUpadte()
			{
				OrderFieldName = name,
				Value = this.Value,
				InteropID = this.InteropID
			};
			return delete;
		}
	}
}
