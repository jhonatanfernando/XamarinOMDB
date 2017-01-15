using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SistemaHiper
{
	public class OmdbApiResult<TResult>
	{
		public OmdbApiResult()
		{
			Results = new List<TResult>();
		}

		[JsonProperty("Search")]
		public List<TResult> Results { get; set; }

		[JsonProperty("totalResults")]
		public int Total { get; set; }

	}



}
