using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Graphics.Direct2D;
using Windows.Win32.Graphics.Direct2D.Common;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.Graphics.Imaging;
using Windows.Win32.System.Com;

namespace System.Windows.Forms.Direct2D
{
    internal class Direct2DImage : IDirect2DImage
    {
        private const int BytesPerPixel = 4;

        private bool _disposedValue;

        private ID2D1Bitmap _d2dBitmap;
        private int _bitmapWidth;
        private int _bitmapHeight;
        private byte[]? _bitmapBytes;
        private uint _bitmapStride;

        private Direct2DImage(ID2D1Bitmap d2dBitmap)
        {
            _d2dBitmap = d2dBitmap ?? throw new ArgumentNullException(nameof(d2dBitmap));
        }

        public unsafe static Direct2DImage FromImage(Image image, ID2D1RenderTarget renderTarget)
        {
            Bitmap? targetBitmap = null;

            try
            {
                // Odd: Clone only works on a copy of the passed image instance.
                if (image.PixelFormat != PixelFormat.Format32bppPArgb)
                {
                    using Bitmap bitmapCopy = new Bitmap(image);
                    targetBitmap = bitmapCopy.Clone(
                        new Rectangle(0, 0, bitmapCopy.Width, bitmapCopy.Height),
                        PixelFormat.Format32bppPArgb);
                }
                else
                {
                    targetBitmap = typeof(Bitmap).IsAssignableFrom(image.GetType())
                        ? (Bitmap)image
                        : new Bitmap(image);
                }

                var hres = PInvoke.CoInitializeEx(null, COINIT.COINIT_APARTMENTTHREADED);

                // Important: For Win Ver>8 we need to call this with CLSID_WICImagingFactory>>2<<!
                hres = PInvoke.CoCreateInstance<IWICImagingFactory>(
                    PInvoke.CLSID_WICImagingFactory2,
                    pUnkOuter: null,
                    CLSCTX.CLSCTX_INPROC_SERVER,
                    out var imageFactory);

                var hBitmapPtr = targetBitmap.GetHbitmap();
                HBITMAP hBitmap = new(hBitmapPtr);
                HPALETTE hPalette = new();

                imageFactory.CreateBitmapFromHBITMAP(
                    hBitmap,
                    hPalette,
                    WICBitmapAlphaChannelOption.WICBitmapUsePremultipliedAlpha,
                    out var pplBitmap);

                pplBitmap.GetSize(out var bitmapWidth, out var bitmapHeight);

                uint bitmapStride = bitmapWidth * BytesPerPixel;

                WICRect bitmapRect = new()
                {
                    X = 0,
                    Y = 0,
                    Width = (int)bitmapWidth,
                    Height = (int)bitmapHeight
                };

                byte[] bitmapBytes = new byte[BytesPerPixel * bitmapWidth * bitmapHeight];
                pplBitmap.CopyPixels(in bitmapRect, bitmapStride, bitmapBytes);

                renderTarget.CreateBitmapFromWicBitmap(
                    pplBitmap,
                    null,
                    out var d2dBitmap);

                Direct2DImage d2dImage = new Direct2DImage(d2dBitmap);
                d2dImage._bitmapBytes = bitmapBytes;
                d2dImage._bitmapStride = bitmapStride;
                d2dImage._bitmapWidth = (int)bitmapWidth;
                d2dImage._bitmapHeight = (int)bitmapHeight;

                GC.KeepAlive(d2dImage);
                return d2dImage;

            }
            finally
            {
                // If image had the correct PixelFormat to begin with,
                // we did not clone it into targetBitmap, but used a reference.
                // But then it must not be disposed.
                if (!Equals(image, targetBitmap))
                {
                    targetBitmap?.Dispose();
                }
            }
        }

        public Span<byte> BitmapBytes
            => _bitmapBytes;

        public uint BitmapStride => _bitmapStride;

        public unsafe void ApplyBitmapBytes()
        {
            fixed (byte* bbytes = _bitmapBytes)
            {
                D2D_RECT_U rect;
                rect = new()
                {
                    left = 0,
                    top = 0,
                    right = (uint)_bitmapWidth,
                    bottom = (uint)_bitmapHeight
                };

                _d2dBitmap.CopyFromMemory(rect, bbytes, _bitmapStride);
            }
        }

        public ID2D1Bitmap NativeImage
        {
            get
            {
                if (_d2dBitmap is null)
                {
                    throw new NullReferenceException("Native COM Image was collected.");
                }

                return _d2dBitmap;
            }
        }

        public int Width => _bitmapWidth;

        public int Height => _bitmapHeight;

        public Color GetPixel(float x, float y)
        {
            throw new NotImplementedException();
        }

        public void SetPixel(float x, float y, Color color)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                Marshal.ReleaseComObject(_d2dBitmap);
                _d2dBitmap = null!;

                _disposedValue = true;
            }
        }

        ~Direct2DImage()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
