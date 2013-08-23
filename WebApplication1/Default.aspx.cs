using System;
using System.Web;
using HansKindberg.Serialization;
using WebApplication1.Helpers.Extensions;

namespace WebApplication1
{
	public partial class Default : System.Web.UI.Page
	{
		#region Methods

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Serializable<HttpContext> serializableHttpContext = new Serializable<HttpContext>(this.Context);
			string binarySerializedSerializableHttpContext = serializableHttpContext.SerializeBinary();

			Serializable<HttpContext> deserializedSerializableHttpContext = (Serializable<HttpContext>) ObjectExtension.DeserializeBinary(binarySerializedSerializableHttpContext);

			//Serializable<HttpRequest> serializableHttpRequest = new Serializable<HttpRequest>(this.Request);
			//string binarySerializedSerializableHttpRequest = serializableHttpRequest.SerializeBinary();
		}

		#endregion
	}
}