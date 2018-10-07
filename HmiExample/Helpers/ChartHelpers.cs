using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HmiExample.Helpers
{
    public class ChartHelpers
    {
        public static void SaveToPng(FrameworkElement visual, string fileName)
        {
            var encoder = new PngBitmapEncoder();
            EncodeVisual(visual, fileName, encoder);
        }

        public static void EncodeVisual(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        {
            var bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            var frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);
            using (var stream = File.Create(fileName)) encoder.Save(stream);
        }

        public static Bitmap ControlToImage(Visual target, double dpiX, double dpiY)
        {
            if (target == null)
            {
                return null;
            }
            // render control content
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)(bounds.Width * dpiX / 96.0),
                                                            (int)(bounds.Height * dpiY / 96.0),
                                                            dpiX,
                                                            dpiY,
                                                            PixelFormats.Pbgra32);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext ctx = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(target);
                ctx.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), bounds.Size));
            }
            rtb.Render(dv);

            //convert image format
            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(stream);

            return new Bitmap(stream);
        }

        public static Bitmap ChartToImage(FrameworkElement chart, double dpiX = 96, double dpiY = 96)
        {
            if (chart == null)
            {
                return null;
            }

            var rtb = new RenderTargetBitmap((int)chart.ActualWidth, (int)chart.ActualHeight, dpiX, dpiY, PixelFormats.Pbgra32);
            //RenderTargetBitmap rtb = new RenderTargetBitmap((int)(chart.ActualWidth * dpiX / 96.0),
            //                                                (int)(chart.ActualHeight * dpiY / 96.0),
            //                                                dpiX,
            //                                                dpiY,
            //                                                PixelFormats.Pbgra32);
            rtb.Render(chart);

            //convert image format
            MemoryStream stream = new MemoryStream(); // empty
            var encoder = new PngBitmapEncoder();
            var frame = BitmapFrame.Create(rtb);
            encoder.Frames.Add(frame);
            encoder.Save(stream);  // save data to empty stream

            return new Bitmap(stream);
        }
    }
}
