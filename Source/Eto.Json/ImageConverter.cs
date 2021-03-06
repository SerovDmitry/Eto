using System;
using Newtonsoft.Json;
using Eto.Drawing;
using Newtonsoft.Json.Linq;
using Eto.Forms;
using System.IO;

namespace Eto.Json
{
	public class ImageConverter : JsonConverter
	{
		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException ();
		}

		public override object ReadJson (Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.String) {
				var val = (string)((JValue)JValue.ReadFrom (reader)).Value;
				var converter = new Eto.Drawing.ImageConverter ();
				return converter.ConvertFrom (val);
			}
			else
				throw new JsonSerializationException("Image or Icon must be defined as a resource or file string");
		}

		public override bool CanConvert (Type objectType)
		{
			return typeof(Image).IsAssignableFrom (objectType);
		}
	}
}

