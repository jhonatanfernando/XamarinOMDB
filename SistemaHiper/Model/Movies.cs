using System;
using Newtonsoft.Json;

namespace SistemaHiper
{
	public class Movies
	{
		public Movies()
		{
		}

		[JsonProperty("Title")]
		public string Title
		{
			get;
			set;
		}

		[JsonProperty("Year")]
		public string Year
		{
			get;
			set;
		}

		[JsonProperty("Poster")]
		public string Poster
		{
			get;
			set;
		}
	}
}
