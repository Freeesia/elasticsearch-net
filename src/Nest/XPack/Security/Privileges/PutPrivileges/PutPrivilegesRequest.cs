﻿using System;
using System.Collections.Generic;
using System.IO;
using Elasticsearch.Net;

namespace Nest
{
	[MapsApi("security.put_privileges")]
	[JsonFormatter(typeof(PutPrivilegesFormatter))]
	public partial interface IPutPrivilegesRequest : IProxyRequest
	{
		IAppPrivileges Applications { get; set; }
	}

	public partial class PutPrivilegesRequest
	{
		public IAppPrivileges Applications { get; set; }

		void IProxyRequest.WriteJson(IElasticsearchSerializer sourceSerializer, Stream stream, SerializationFormatting formatting) =>
			sourceSerializer.Serialize(Self.Applications, stream, formatting);
	}

	public partial class PutPrivilegesDescriptor
	{
		IAppPrivileges IPutPrivilegesRequest.Applications { get; set; }

		void IProxyRequest.WriteJson(IElasticsearchSerializer sourceSerializer, Stream stream, SerializationFormatting formatting) =>
			sourceSerializer.Serialize(Self.Applications, stream, formatting);

		public PutPrivilegesDescriptor Applications(Func<AppPrivilegesDescriptor, IPromise<IAppPrivileges>> selector) =>
			Assign(a => a.Applications = selector?.Invoke(new AppPrivilegesDescriptor())?.Value);
	}

	internal class PutPrivilegesFormatter : IJsonFormatter<IPutPrivilegesRequest>
	{
		public IPutPrivilegesRequest Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
		{
			var appPrivileges = formatterResolver.GetFormatter<AppPrivileges>().Deserialize(ref reader, formatterResolver);
			return new PutPrivilegesRequest { Applications = appPrivileges };
		}

		public void Serialize(ref JsonWriter writer, IPutPrivilegesRequest value, IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			var formatter = formatterResolver.GetFormatter<IDictionary<string, IPrivileges>>();
			formatter.Serialize(ref writer, value.Applications, formatterResolver);
		}
	}
}
