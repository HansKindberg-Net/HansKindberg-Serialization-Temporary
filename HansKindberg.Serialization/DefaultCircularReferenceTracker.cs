using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace HansKindberg.Serialization
{
	public class DefaultCircularReferenceTracker : ICircularReferenceTracker
	{
		#region Fields

		private readonly IDictionary<Guid, object> _idDictionary = new Dictionary<Guid, object>();
		private readonly IDictionary<object, Guid> _instanceDictionary = new Dictionary<object, Guid>();
		private readonly IList<Guid> _references = new List<Guid>();

		#endregion

		#region Properties

		protected internal virtual IDictionary<Guid, object> IdDictionary
		{
			get { return this._idDictionary; }
		}

		protected internal virtual IDictionary<object, Guid> InstanceDictionary
		{
			get { return this._instanceDictionary; }
		}

		public virtual IEnumerable<Guid> References
		{
			get { return this.ReferencesInternal.ToArray(); }
		}

		[SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
		protected internal virtual IList<Guid> ReferencesInternal
		{
			get { return this._references; }
		}

		#endregion

		#region Methods

		public virtual void AddReference(Guid id)
		{
			if(!this.ReferencesInternal.Contains(id))
				this.ReferencesInternal.Add(id);
		}

		public virtual void Clear()
		{
			this.InstanceDictionary.Clear();
			this.IdDictionary.Clear();
			this.ReferencesInternal.Clear();
		}

		public virtual bool ContainsTrackedInstance(Guid id)
		{
			return this.GetTrackedInstance(id) != null;
		}

		public virtual bool ContainsTrackedInstance(object instance)
		{
			return this.GetTrackedInstanceId(instance).HasValue;
		}

		public virtual object GetTrackedInstance(Guid id)
		{
			if(this.IdDictionary.ContainsKey(id))
				return this.IdDictionary[id];

			return null;
		}

		public virtual Guid? GetTrackedInstanceId(object instance)
		{
			if(instance != null)
			{
				if(this.InstanceDictionary.ContainsKey(instance))
					return this.InstanceDictionary[instance];
			}

			return null;
		}

		public virtual bool RemoveTrackedInstance(Guid id)
		{
			this.ReferencesInternal.Remove(id);
			this.InstanceDictionary.Values.Remove(id);
			return this.IdDictionary.Remove(id);
		}

		public virtual bool RemoveTrackedInstance(object instance)
		{
			Guid? id = this.GetTrackedInstanceId(instance);

			if(id.HasValue)
				this.ReferencesInternal.Remove(id.Value);

			this.IdDictionary.Values.Remove(instance);
			return this.InstanceDictionary.Remove(instance);
		}

		public virtual void TrackInstance(Guid id, object instance)
		{
			if(id == Guid.Empty)
				throw new ArgumentException("The instance-id can not be empty.", "id");

			if(this.IdDictionary.ContainsKey(id))
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The instance-id \"{0}\" has already been added.", id), "id");

			if(instance == null)
				throw new ArgumentNullException("instance");

			if(this.InstanceDictionary.ContainsKey(instance))
				throw new ArgumentException("The instance has already been added.", "instance");

			this.InstanceDictionary.Add(instance, id);
			this.IdDictionary.Add(id, instance);
		}

		public virtual void TrackInstanceIfNecessary(Serializable serializable, ISerializationResolver serializationResolver)
		{
			if(serializationResolver == null)
				throw new ArgumentNullException("serializationResolver");

			if(serializable == null)
				return;

			if(this.ContainsTrackedInstance(serializable.Id))
				return;

			if(serializationResolver.IsSerializable(serializable.Instance))
				return;

			if(this.ContainsTrackedInstance(serializable.Instance))
				return;

			this.TrackInstance(serializable.Id, serializable.Instance);
		}

		#endregion
	}
}