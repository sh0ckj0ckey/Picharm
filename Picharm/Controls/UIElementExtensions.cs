using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Picharm.Controls
{
    /// <summary>
    /// https://github.com/LanceMcCarthy/UwpProjects?tab=readme-ov-file#blurelementasync
    /// </summary>
    public static class UIElementExtensions
    {
        public static async Task<BitmapImage> BlurElementAsync(this UIElement sourceElement, float blurAmount = 2.0f)
        {
            if (sourceElement == null)
                return null;

            var rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(sourceElement);

            var buffer = await rtb.GetPixelsAsync();
            if (buffer.Length <= 0)
            {
                return null;
            }

            var array = buffer.ToArray();

            var displayInformation = DisplayInformation.GetForCurrentView();

            using (var stream = new InMemoryRandomAccessStream())
            {
                var pngEncoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);

                pngEncoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                     BitmapAlphaMode.Premultiplied,
                                     (uint)rtb.PixelWidth,
                                     (uint)rtb.PixelHeight,
                                     displayInformation.RawDpiX,
                                     displayInformation.RawDpiY,
                                     array);

                await pngEncoder.FlushAsync();
                stream.Seek(0);

                var canvasDevice = new CanvasDevice();
                var bitmap = await CanvasBitmap.LoadAsync(canvasDevice, stream);

                var renderer = new CanvasRenderTarget(canvasDevice,
                                                      bitmap.SizeInPixels.Width,
                                                      bitmap.SizeInPixels.Height,
                                                      bitmap.Dpi);

                using (var ds = renderer.CreateDrawingSession())
                {
                    var blur = new GaussianBlurEffect
                    {
                        BlurAmount = blurAmount,
                        Source = bitmap
                    };
                    ds.DrawImage(blur);
                }

                stream.Seek(0);
                await renderer.SaveAsync(stream, CanvasBitmapFileFormat.Png);

                var image = new BitmapImage();
                await image.SetSourceAsync(stream);

                return image;
            }
        }

    }

}
