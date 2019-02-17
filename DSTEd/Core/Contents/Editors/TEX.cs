using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DSTEd.Core.Klei.KTEX;
using DSTEd.UI.Components;

namespace DSTEd.Core.Contents.Editors {
    public class TEX : Container {
        private Document document = null;
        private Canvas canvas = null;

        public TEX() {

        }

        public TEX(Document document) {
            this.document = document;
            this.canvas = new Canvas();
            this.Background = this.CreateBackground();
            this.HorizontalContentAlignment = HorizontalAlignment.Center;
            this.Load();
        }

        private ImageBrush CreateBackground() {
            ImageBrush brush    = new ImageBrush();
            brush.Stretch       = Stretch.None;
            brush.Viewport      = new Rect(0, 0, 120, 120);
            brush.TileMode      = TileMode.FlipXY;
            brush.ViewportUnits = BrushMappingMode.Absolute;
            System.Windows.Controls.Image image         = new System.Windows.Controls.Image();
            image.Source        = new BitmapImage(new Uri("pack://application:,,,/DSTEd;component/Assets/Raster.png"));
            brush.ImageSource = image.Source;
            return brush;
        }

        private void CreateLabel(string text) {
            TextBlock t = new TextBlock();
            t.Text = text;
            this.Children.Add(t);
        }

        public BitmapImage ToBitmapImage(Bitmap bitmap) {
            using (var memory = new MemoryStream()) {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public void Load() {
            try {
                KTEX texture = new KTEX(File.OpenRead(this.document.GetFile()));
                if (!texture.IsValid()) {
                    this.CreateLabel("Texture is Invalid.");
                } else {

                    Bitmap img = texture.GenerateBitMap();
                    System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                    image.Source = this.ToBitmapImage(img);
                    this.Children.Add(image);
                    //var mipmap = CurrentFile.GetMainMipmap();

                    // EArgs.Size = mipmap.Width + "x" + mipmap.Height;
                }
            } catch(Exception e) {
                this.CreateLabel("Texture can't load: " + e.Message);
            }
            
        }
    }
}
