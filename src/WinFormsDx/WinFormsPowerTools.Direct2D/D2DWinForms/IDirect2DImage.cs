using System.Drawing;

namespace System.Windows.Forms.Direct2D
{
    public interface IDirect2DImage : IDisposable
    {
        void SetPixel(float x, float y, Color color);
        Color GetPixel(float x, float y);
        int Width { get; }
        int Height { get; }
        Span<byte> BitmapBytes { get; }
        void ApplyBitmapBytes();
    }
}
