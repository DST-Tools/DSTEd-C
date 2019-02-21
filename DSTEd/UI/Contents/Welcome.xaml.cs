using System;
using System.Collections.Generic;
using System.Windows;
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

            this.document.GetCore().GetSteam().GetNews(delegate(List<SteamKit2.KeyValue> news) {
                foreach (SteamKit2.KeyValue entry in news) {
                    this.AddNewsEntry(entry);
                }
            });
        }

        private void AddNewsEntry(SteamKit2.KeyValue news) {
            News entry = new News();
            entry.title.Text = news["title"].AsString();

            TimeSpan time = TimeSpan.FromSeconds(news["date"].AsInteger());
            entry.date.Content = string.Format("{0} hours and {1} seconds ago",
                time.Hours,
                time.Minutes);

            entry.AddLink(news["url"].AsString());
            entry.content.Children.Add(new BBCodePanel(news["contents"].AsString()));
            this.news.Children.Add(entry);
        }
    }
}
