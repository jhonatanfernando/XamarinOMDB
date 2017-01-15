using System;
using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Net;

namespace SistemaHiper
{
	public class MovieAdapter : BaseAdapter<Movies>
	{
		List<Movies> items;
		Activity context;
		public MovieAdapter(Activity context, List<Movies> items)
	   : base()
		{
			this.context = context;
			this.items = items;
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override Movies this[int position]
		{
			get { return items[position]; }
		}
		public override int Count
		{
			get { return items.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = items[position];
			View view = convertView;
			if (view == null) // no view to re-use, create new
				view = context.LayoutInflater.Inflate(Resource.Layout.CustomView, null);
			view.FindViewById<TextView>(Resource.Id.Text1).Text = item.Title;
			view.FindViewById<TextView>(Resource.Id.Text2).Text = item.Year.ToString();

			if(item.Year.StartsWith("2017", StringComparison.CurrentCultureIgnoreCase))
				view.SetBackgroundColor(Color.Red);
			else
				view.SetBackgroundColor(Color.White);

			try
			{
				var imageBitmap = GetImageBitmapFromUrl(item.Poster);
				view.FindViewById<ImageView>(Resource.Id.Image).SetImageBitmap(imageBitmap);
			}
			catch
			{
			}
			return view;
		}


		private Bitmap GetImageBitmapFromUrl(string url)
		{
			Bitmap imageBitmap = null;

			using (var webClient = new System.Net.WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
				{
					imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
				}
			}

			return imageBitmap;
		}
	}
}
