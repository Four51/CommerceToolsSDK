using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Four51.APISDK.Services
{
	public interface IBaseService
	{
		string SharedSecret { get; set; }
		string ServiceId { get; set; }
	}
	public class BaseService : IBaseService
	{
		public virtual string SharedSecret { get; set; }
		public virtual string ServiceId { get; set; }
	}
}
