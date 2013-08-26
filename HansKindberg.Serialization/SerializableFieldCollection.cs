using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HansKindberg.Serialization
{
	[Serializable]
	public class SerializableFieldCollection : Serializable
	{
		public SerializableFieldCollection(IEnumerable<SerializableField> instance) : base(instance) {}
	}
}
