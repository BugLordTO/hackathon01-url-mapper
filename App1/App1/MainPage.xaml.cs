using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlMapper;
using Xamarin.Forms;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        SimpleStringParameterBuilder engine = new SimpleStringParameterBuilder();
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_IsMatched_Clicked(object sender, EventArgs e)
        {
            var engine2 = engine.Parse(Pattern.Text);
            await DisplayAlert("", engine2.IsMatched(Url.Text) ? "Matched" : "Not IsMatched", "OK");
        }

        private async void Button_ExtractParameters_Clicked(object sender, EventArgs e)
        {
            var engine = new SimpleStringParameterBuilder();
            var engine2 = engine.Parse(Pattern.Text);

            Result.ItemsSource = new List<Data>();

            if (engine2.IsMatched(Url.Text))
            {
                var dict = new Dictionary<string, string>();
                engine2.ExtractVariables(Url.Text, dict);
                Result.ItemsSource = dict.Select(x => new Data { Name = x.Key, Value = x.Value });
            }
            else await DisplayAlert("", "Not IsMatched", "OK");
        }
    }

    public class Data
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
