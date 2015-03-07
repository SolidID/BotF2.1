using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Color = System.Drawing.Color;

namespace TerrainGenerator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bitmap _bmpData;

        public MainWindow()
        {
            InitializeComponent();
        }

        // at class level
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            int size;
            if (!Int32.TryParse(SizeInupt.Text, out size))
                size = 1024;

            DiamondSquareInitType iType;
            if (Enum.TryParse(InitTypeBox.SelectedValue.ToString(), out iType))
                _bmpData =
                    HorizontalTransientDiamondSquare.Create(new HorizontalTransientDiamondSquare.Dto()
                    {
                        Roughness = RoughnessSlider.Value,
                        Seed = (int)SeedSlider.Value,
                        Height = size,
                        Width = size - 1,
                        StartType = iType
                    });
            RefreshImage();
        }

        private void RefreshImage()
        {
            IntPtr hBmp = _bmpData.GetHbitmap();

            ImageHolder.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBmp, IntPtr.Zero,
                new Int32Rect(0, 0, _bmpData.Width, _bmpData.Height), BitmapSizeOptions.FromWidthAndHeight(_bmpData.Width, _bmpData.Height));

            DeleteObject(hBmp);
        }

        private void TextureItClick(object sender, RoutedEventArgs e)
        {
            for (int x = 0; x < _bmpData.Width; x++)
            {
                for (int y = 0; y < _bmpData.Height; y++)
                {
                    Color col = _bmpData.GetPixel(x, y);
                    var colorIndex = col.R;
                    if (colorIndex <= 255 * 0.35f)
                    {
                        col = Color.DarkBlue;
                        col = GetColorForHeight(colorIndex, Color.DarkBlue, (int)(255 * .35f));
                    }
                    else if (colorIndex <= 255 * .4f)
                    {
                        col = GetColorForHeight(colorIndex, Color.BurlyWood, (int)(255 * .4f));
                    }
                    else if (colorIndex <= 255 * .75)
                    {
                        col = Color.ForestGreen;
                        col = GetColorForHeight(colorIndex, Color.ForestGreen, (int)(255 * .66f));
                    }
                    else
                    {
                        col = Color.DimGray;
                        col = GetColorForHeight(colorIndex, Color.DimGray, (int)(255));
                    }

                    _bmpData.SetPixel(x, y, col);

                }
            }

            RefreshImage();
        }

        private Color GetColorForHeight(byte brightness, Color baseColor, int maxValue)
        {
            return Color.FromArgb(
                (int)((baseColor.R / (float)maxValue) * brightness),
                (int)((baseColor.G / (float)maxValue) * brightness),
                (int)((baseColor.B / (float)maxValue) * brightness)
            );
        }

        private void DoAll_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonBase_OnClick(sender, e);
            TextureItClick(sender, e);
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            bool result = (bool)dialog.ShowDialog(this);
            if (result)
            {
                _bmpData.Save(dialog.FileName, ImageFormat.Png);
            }
        }
    }
}
