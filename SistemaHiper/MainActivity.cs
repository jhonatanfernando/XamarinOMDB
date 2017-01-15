using Android.App;
using Android.OS;
using Android.Views;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Widget;
using Android.Support.V7.App;
using Acr.UserDialogs;
using System;

namespace SistemaHiper
{
	[Activity(Label = "Sistema Hiper", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : AppCompatActivity
	{
		private string filmePesquisado;
		private int paginaInicial = 1;

		private Android.Support.V7.Widget.SearchView _searchView;
		private OmdbApiResult<Movies> resultData = new OmdbApiResult<Movies>();
		private ListView listView;
		private View footerView;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Main);

			UserDialogs.Init(this);

			listView = FindViewById<ListView>(Resource.Id.listView);
			footerView = LayoutInflater.Inflate(Resource.Layout.Footer, null, false);
			listView.AddFooterView(footerView);
			footerView.Click += FooterView_Click;

			listView.ItemClick += ListView_ItemClick;

			//listView.Scroll += ListView_Scroll;
		}

		void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			if (e.Position > -1)
			{
				Movies movie = resultData.Results[e.Position];

				UserDialogs.Instance.Toast(movie.Title, TimeSpan.FromSeconds(2));
			}
		}


		void FooterView_Click(object sender, System.EventArgs e)
		{
			CarregaFilmes(filmePesquisado, (++paginaInicial).ToString());
		}


		void ListView_Scroll(object sender, AbsListView.ScrollEventArgs e)
		{
			int lastInScreen = e.FirstVisibleItem + e.VisibleItemCount;
			if (e.TotalItemCount > 8 && (lastInScreen == e.TotalItemCount))
			{
				try
				{
					//Aqui carrega os filmes de forma automática quando chega no fim do List
					CarregaFilmes(filmePesquisado, (++paginaInicial).ToString());
				}
				catch { }
			}
		}


		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.search_menu, menu);

			IMenuItem item = menu.FindItem(Resource.Id.action_search);

			_searchView = item.ActionView.JavaCast<Android.Support.V7.Widget.SearchView>();



			_searchView.QueryTextSubmit +=  (s, e) =>
			{
				try
				{
					 CarregaFilmes(e.Query, "1");
				}
				catch (System.Exception ef)
				{
				}
				e.Handled = true;
			};

			return true;
		}

		private async void CarregaFilmes(string filter, string pagina)
		{
			var pg = UserDialogs.Instance.Loading("Carregando filmes ...", null, null, true, MaskType.Black);

			filmePesquisado = filter;
			OmdbApiService service = new OmdbApiService();
			resultData = await service.GetMovies(filter, pagina);
			listView.Adapter = new MovieAdapter(this, resultData.Results);

			if(resultData.Results.Count >= 10 && resultData.Total > 10)
			   footerView.Visibility = ViewStates.Visible;
			   else
				footerView.Visibility = ViewStates.Invisible;

			pg.Hide();

		}
	}
}

