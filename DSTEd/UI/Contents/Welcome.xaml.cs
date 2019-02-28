using System;
using System.Collections.Generic;
using System.Windows.Controls;
using DSTEd.Core;
using DSTEd.UI.Components;

namespace DSTEd.UI.Contents {
    public partial class Welcome : UserControl {
        private Document document;

        public Welcome(Document document) {
            InitializeComponent();
            this.document = document;
            this.news.Children.Clear();

            Boot.Core().GetSteam().GetNews(delegate (List<SteamKit2.KeyValue> news) {
                foreach (SteamKit2.KeyValue entry in news) {
                    this.AddNewsEntry(entry);
                }
            });
        }

        private void AddNewsEntry(SteamKit2.KeyValue news) {
            News entry = new News();
            entry.title.Text = news["title"].AsString();

            DateTime reference = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime time = reference.AddSeconds(news["date"].AsInteger());

            entry.date.Content = time.ToString("yyyy") == DateTime.Now.Year.ToString() ? time.ToString("dd. MMM") : time.ToString("dd. MMM yyyy");

            entry.AddLink(news["url"].AsString());
            entry.content.Children.Add(new BBCodePanel(news["contents"].AsString()));
            this.news.Children.Add(entry);
        }
    }
}
