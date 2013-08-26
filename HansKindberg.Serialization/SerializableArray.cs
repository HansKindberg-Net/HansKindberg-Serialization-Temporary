using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HansKindberg.Serialization
{
	[Serializable]
	public class SerializableArray : Serializable
	{
		public SerializableArray(Array array) : base(array) {}
	}
}
