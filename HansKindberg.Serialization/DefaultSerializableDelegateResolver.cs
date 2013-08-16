//using System;
//using System.Diagnostics.CodeAnalysis;
//using System.Globalization;
//using System.Runtime.Serialization;
//using Castle.DynamicProxy;

//namespace HansKindberg.Serialization
//{
//	public class DefaultSerializableDelegateResolver : SerializableResolver, ISerializableDelegateResolver
//	{
//		#region Fields

//		private const string _delegateSerializationInformationName = "Delegate";

//		#endregion

//		#region Constructors

//		public DefaultSerializableDelegateResolver(IProxyBuilder proxyBuilder) : base(proxyBuilder) {}

//		#endregion

//		#region Properties

//		protected internal virtual string DelegateSerializationInformationName
//		{
//			get { return _delegateSerializationInformationName; }
//		}

//		#endregion

//		#region Methods

//		protected internal virtual object GetDelegate(SerializationInfo serializationInformation, StreamingContext streamingContext, string index)
//		{
//			throw new NotImplementedException();
//		}

//		public virtual T GetDelegate<T>(SerializationInfo serializationInformation, StreamingContext streamingContext, string index)
//		{
//			return (T) this.GetDelegate(serializationInformation, streamingContext, index);
//		}

//		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "delegate")]
//		protected internal virtual void SetDelegate(object @delegate, SerializationInfo serializationInformation, StreamingContext streamingContext, string index)
//		{
//			if(@delegate == null)
//				throw new ArgumentNullException("delegate");

//			Type type = @delegate.GetType();

//			if(!typeof(Delegate).IsAssignableFrom(type))
//				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The type \"{0}\" must be a delegate ({1}).", type.FullName, typeof(Delegate)), "delegate");
//		}

//		public virtual void SetDelegate<T>(T @delegate, SerializationInfo serializationInformation, StreamingContext streamingContext, string index)
//		{
//			this.SetDelegate((object) @delegate, serializationInformation, streamingContext, index);
//		}

//		#endregion
//	}
//}