using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Direct2D;
using Windows.Win32.Graphics.Direct2D.Common;
using Windows.Win32.Graphics.DirectWrite;

using Color = System.Drawing.Color;

namespace System.Windows.Forms.Direct2D
{
    internal class Direct2DLayer : IDisposable
    {
        private const int DefaultFontSize = 12;

        [AllowNull]
        private ID2D1HwndRenderTarget _renderTarget;
        private readonly Control? _window;

        private bool disposedValue;

        public Direct2DLayer(Control? window)
        {
            _window = window;
            DirectWriteFactory = CreateDirectWriteFactory();
        }

        public ID2D1RenderTarget RenderTarget => _renderTarget ??= CreateHwndRenderTarget(_window);
        public ID2D1Factory? Direct2DFactory { get; } = CreateFactory();

        [AllowNull]
        public IDWriteFactory DirectWriteFactory { get; } 
        public Control? Window => _window;
        internal float StrokeSize { get; set; }

        private static unsafe IDWriteFactory CreateDirectWriteFactory(
            DWRITE_FACTORY_TYPE factoryType = DWRITE_FACTORY_TYPE.DWRITE_FACTORY_TYPE_ISOLATED)
        {
            PInvoke.DWriteCreateFactory(factoryType, new Guid(Direct2DGuids.IID_IDWriteFactory), out var dWriteFactory);

            if (dWriteFactory is not IDWriteFactory factory)
            {
                throw new InvalidOperationException("Couldn't create a IDWriteFactory.");
            }

            return factory;
        }

        private unsafe static ID2D1Factory? CreateFactory(
            D2D1_FACTORY_TYPE factoryType = D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_SINGLE_THREADED,
            D2D1_DEBUG_LEVEL debugLevel = D2D1_DEBUG_LEVEL.D2D1_DEBUG_LEVEL_NONE)
        {
            D2D1_FACTORY_OPTIONS options = new()
            {
                debugLevel = debugLevel
            };

            var result = PInvoke.D2D1CreateFactory(
                D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_SINGLE_THREADED,
                typeof(ID2D1Factory).GUID,
                new D2D1_FACTORY_OPTIONS(),
                out var pFactory);

            // TODO: Check result for errors.
            var factory = pFactory as ID2D1Factory; // was: Marshal.GetObjectForIUnknown(new IntPtr((int)pFactory)) 
            return factory;
        }

        private static ID2D1HwndRenderTarget CreateHwndRenderTarget(Control? control)
        {
            if (control is null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            var factory = CreateFactory();
            if (factory is not null)
            {
                var renderTargetProperties = new D2D1_HWND_RENDER_TARGET_PROPERTIES
                {
                    hwnd = new HWND(control.Handle)
                };

                var size = new D2D_SIZE_U
                {
                    width = (uint)control.Width,
                    height = (uint)control.Height
                };

                renderTargetProperties.pixelSize = size;

                factory.CreateHwndRenderTarget(default, renderTargetProperties, out var dcHwndRenderTarget);

                return dcHwndRenderTarget;
            }
            else
            {
                throw new InvalidOperationException("Couldn't create a D2D render target for the given control.");
            }
        }

        private static void ResizeRenderTarget(ID2D1HwndRenderTarget renderTarget, System.Drawing.Size size)
        {
            D2D_SIZE_U newSize;
            newSize.width = (uint)size.Width;
            newSize.height = (uint)size.Height;
            renderTarget.Resize(newSize);
        }

        public void BeginDraw()
        {
            RenderTarget.BeginDraw();
        }

        unsafe public void EndDraw()
        {
            ulong* tag1 = null;
            ulong* tag2 = null;

            RenderTarget.EndDraw(tag1, tag2);
        }

        public void Clear(System.Drawing.Color color)
        {
            D2D1_COLOR_F d2dColor;
            d2dColor.r = color.R;
            d2dColor.g = color.G;
            d2dColor.b = color.B;
            d2dColor.a = color.A;

            RenderTarget!.Clear(d2dColor);
        }

        public void Resize(System.Drawing.Size size)
        {
            if (_renderTarget is null)
            {
                return;
            }

            ResizeRenderTarget(_renderTarget, size);
        }

        unsafe public void Flush()
        {
            ulong* tag1 = null;
            ulong* tag2 = null;

            RenderTarget.Flush(tag1, tag2);
        }

        internal void DrawLine(float x1, float y1, float x2, float y2, 
            ID2D1SolidColorBrush strokeColorBrush, float strokeSize, ID2D1StrokeStyle? strokeStyle)
        {
            D2D_POINT_2F startPoint = new() { x = x1, y = x1 };
            D2D_POINT_2F endPoint = new() { x = x2, y = y2 };

            RenderTarget.DrawLine(startPoint, endPoint, strokeColorBrush, strokeSize, strokeStyle);
        }

        internal unsafe void DrawImage(Image image, float x, float y, float width, float height)
        {
            using var d2dBitmap = Direct2DImage.FromImage(image, RenderTarget);
            DrawImage(d2dBitmap, x, y, width, height);
        }

        internal unsafe void DrawImage(IDirect2DImage image, float x, float y, float width, float height)
        {
            D2D_RECT_F rectangle;
            rectangle = new() { left = x, top = y, right = x + width, bottom = y + height };

            RenderTarget.DrawBitmap(
                ((Direct2DImage) image).NativeImage,
                rectangle,
                opacity: 1,
                D2D1_BITMAP_INTERPOLATION_MODE.D2D1_BITMAP_INTERPOLATION_MODE_NEAREST_NEIGHBOR,
                sourceRectangle: null);
        }

        internal IDirect2DImage FromImage(Image image)
            => Direct2DImage.FromImage(image, RenderTarget!);

        internal void DrawRectangle(float x, float y, float width, float height,
            ID2D1SolidColorBrush strokeColorBrush, float strokeSize, ID2D1StrokeStyle? strokeStyle)
        {
            D2D_RECT_F rectangle;
            rectangle = new() { left = x, top = y, right = x + width, bottom = y + height };
            RenderTarget.DrawRectangle(rectangle, strokeColorBrush, strokeSize, strokeStyle);
        }

        internal void FillRectangle(float x, float y, float width, float height, ID2D1SolidColorBrush fillColorBrush)
        {
            D2D_RECT_F rectangle;
            rectangle = new() { left = x, top = y, right = x + width, bottom = y + height };
            RenderTarget.FillRectangle(rectangle, fillColorBrush);
        }

        internal void DrawRoundedRectangle(float x, float y, float width, float height, float cornerRadius,
            ID2D1SolidColorBrush strokeColorBrush, float strokeSize, ID2D1StrokeStyle? strokeStyle)
        {
            D2D1_ROUNDED_RECT rectangle;
            rectangle.rect = new() { left = x, top = y, right = x + width, bottom = y + height };
            rectangle.radiusX = cornerRadius;
            rectangle.radiusY = cornerRadius;

            RenderTarget.DrawRoundedRectangle(rectangle, strokeColorBrush, strokeSize, strokeStyle);
        }

        internal void FillRoundedRectangle(float x, float y, float width, float height, 
            float cornerRadius, ID2D1SolidColorBrush fillColorBrush)
        {
            D2D1_ROUNDED_RECT rectangle;
            rectangle.rect = new() { left = x, top = y, right = x + width, bottom = y + height };
            rectangle.radiusX = cornerRadius;
            rectangle.radiusY = cornerRadius;

            RenderTarget.FillRoundedRectangle(rectangle, fillColorBrush);
        }

        internal void DrawArc(float x1, float y1, float x2, float y2, float startAngle, float endAngle, bool clockwise, bool closed,
                        ID2D1SolidColorBrush strokeColorBrush, float strokeSize, ID2D1StrokeStyle? strokeStyle)
        {
            // We need to create a Path with a D2D1_ARC_SEGMENT, and then draw that path.
        }

        internal void DrawEllipse(float x, float y, float width, float height,
            ID2D1SolidColorBrush strokeColorBrush, float strokeSize, ID2D1StrokeStyle? strokeStyle)

        {
            D2D1_ELLIPSE ellipse;
            ellipse.point = new() { x = x, y = y };
            ellipse.radiusX = width;
            ellipse.radiusY = height;

            RenderTarget.DrawEllipse(ellipse, strokeColorBrush, strokeSize, strokeStyle);
        }

        internal void FillEllipse(float x, float y, float width, float height, ID2D1SolidColorBrush fillColorBrush)
        {
            D2D1_ELLIPSE ellipse;
            ellipse.point = new() { x = x, y = y };
            ellipse.radiusX = width;
            ellipse.radiusY = height;

            RenderTarget.FillEllipse(ellipse, fillColorBrush);
        }

        internal void DrawText(string? s, Direct2DBrush d2dBrush, Direct2DFormat d2dFormat, float x, float y)
        {
            D2D_RECT_F layoutRect = new()
            {
                left = x,
                top = y,
                right = x + 1,
                bottom = y + 1
            };

            RenderTarget.DrawText(
                s ?? string.Empty,
                (uint) (s is null ? 0 : s.Length),
                d2dFormat.GetInstance(),
                layoutRect,
                d2dBrush.Brush,
                D2D1_DRAW_TEXT_OPTIONS.D2D1_DRAW_TEXT_OPTIONS_NONE,
                DWRITE_MEASURING_MODE.DWRITE_MEASURING_MODE_GDI_CLASSIC);
        }

        internal void DrawText(string? s, Direct2DBrush d2dBrush, Direct2DFormat d2dFormat, RectangleF layoutRectangle)
        {
            D2D_RECT_F layoutRect = new()
            {
                left = layoutRectangle.Left,
                top = layoutRectangle.Top,
                bottom = layoutRectangle.Bottom,
                right = layoutRectangle.Right
            };

            RenderTarget.DrawText(
                s ?? string.Empty,
                (uint)(s is null ? 0 : s.Length),
                d2dFormat.GetInstance(),
                layoutRect,
                d2dBrush.Brush,
                D2D1_DRAW_TEXT_OPTIONS.D2D1_DRAW_TEXT_OPTIONS_NONE,
                DWRITE_MEASURING_MODE.DWRITE_MEASURING_MODE_GDI_CLASSIC);
        }

        internal IDWriteTextLayout? TextLayout(string? s, Direct2DBrush d2dBrush, Direct2DFormat d2dFormat, 
            float maxWidth, float maxHeight)
        {
            DirectWriteFactory.CreateTextLayout(
                s ?? string.Empty,
                (uint)(s is null ? 0 : s.Length),
                d2dFormat.GetInstance(),
                maxWidth,
                maxHeight,
                out IDWriteTextLayout? textLayout);

            return textLayout;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                if (_renderTarget is not null)
                {
                    Marshal.FinalReleaseComObject(RenderTarget);
                    _renderTarget = null;
                }

                disposedValue = true;
            }
        }

        ~Direct2DLayer()
        {
            // do not change this code. put cleanup code in 'dispose(bool disposing)' method
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
