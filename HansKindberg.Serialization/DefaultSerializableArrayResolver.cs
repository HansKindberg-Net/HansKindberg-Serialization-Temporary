//using System;
//using System.Globalization;
//using System.Runtime.Serialization;
//using Castle.DynamicProxy;
//using HansKindberg.Serialization.Extensions;

//namespace HansKindberg.Serialization
//{
//	public class DefaultSerializableArrayResolver : DefaultSerializableResolver, ISerializableArrayResolver
//	{
//		#region Fields

//		private const string _arraySerializationInformationName = "Array";
//		private const string _lengthSerializationInformationName = "Length";

//		#endregion

//		#region Constructors

//		public DefaultSerializableArrayResolver(IProxyBuilder proxyBuilder) : base(proxyBuilder) {}

//		#endregion

//		#region Properties

//		protected internal virtual string ArraySerializationInformationName
//		{
//			get { return _arraySerializationInformationName; }
//		}

//		protected internal virtual string LengthSerializationInformationName
//		{
//			get { return _lengthSerializationInformationName; }
//		}

//		#endregion

//		#region Methods

//		protected internal virtual object GetArray(SerializationInfo serializationInformation, StreamingContext streamingContext, string index)
//		{
//			if(serializationInformation == null)
//				throw new ArgumentNullException("serializationInformation");

//			try
//			{
//				if(serializationInformation.TryGetValue(this.IsSerializableSerializationInformationName + index, false))
//					return serializationInformation.GetValue(this.ArraySerializationInformationName + index, typeof(object));

//				Type type = serializationInformation.TryGetValue<Type>(this.TypeSerializationInformationName + index, null);
//				int length = serializationInformation.TryGetValue(this.LengthSerializationInformationName + index, 0);
//				object array = Activator.CreateInstance(type, new object[] {length});

//				Array arrayInternal = (Array) array;

//				for(int i = 0; i < length; i++)
//				{
//					index = index + this.IndexString(i);
//					object item = this.GetInstance(serializationInformation, streamingContext, index);

//					if(item != null)
//						arrayInternal.SetValue(item, i);
//				}

//				return array;
//			}
//			catch(Exception exception)
//			{
//				throw new ArgumentException("The array could not be retrieved from the serialization-information.", "serializationInformation", exception);
//			}
//		}

//		public virtual T GetArray<T>(SerializationInfo serializationInformation, StreamingContext streamingContext, string index)
//		{
//			return (T) this.GetArray(serializationInformation, streamingContext, index);
//		}

//		protected internal virtual string IndexString(int index)
//		{
//			if(index < 0)
//				throw new ArgumentException("The index can not be less than zero.", "index");

//			return ":" + index.ToString(CultureInfo.InvariantCulture);
//		}

//		protected internal virtual void SetArray(object array, SerializationInfo serializationInformation, StreamingContext streamingContext, string index)
//		{
//			if(array == null)
//				throw new ArgumentNullException("array");

//			Type type = array.GetType();

//			if(!type.IsArray)
//				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The type \"{0}\" must be an array.", type.FullName), "array");

//			if(serializationInformation == null)
//				throw new ArgumentNullException("serializationInformation");

//			if(this.IsSerializable(array.GetType()))
//			{
//				serializationInformation.AddValue(this.ArraySerializationInformationName + index, array);
//				serializationInformation.AddValue(this.IsSerializableSerializationInformationName + index, true);
//				return;
//			}

//			Array arrayInternal = (Array) array;

//			// ReSharper disable PossibleNullReferenceException
//			serializationInformation.AddValue(this.LengthSerializationInformationName + index, arrayInternal.Length);
//			// ReSharper restore PossibleNullReferenceException
//			serializationInformation.AddValue(this.IsSerializableSerializationInformationName + index, false);
//			serializationInformation.AddValue(this.TypeSerializationInformationName + index, array.GetType());

//			for(int i = 0; i < arrayInternal.Length; i++)
//			{
//				object item = arrayInternal.GetValue(i);
//				index = index + this.IndexString(i);

//				if(item != null)
//				{
//					this.SetInstance(item, serializationInformation, streamingContext, index);
//					continue;
//				}

//				serializationInformation.AddValue(this.InstanceSerializationInformationName + index, null);
//				serializationInformation.AddValue(this.IsSerializableSerializationInformationName + index, true);
//			}
//		}

//		public virtual void SetArray<T>(T array, SerializationInfo serializationInformation, StreamingContext streamingContext, string index)
//		{
//			this.SetArray((object) array, serializationInformation, streamingContext, index);
//		}

//		#endregion
//	}
//}