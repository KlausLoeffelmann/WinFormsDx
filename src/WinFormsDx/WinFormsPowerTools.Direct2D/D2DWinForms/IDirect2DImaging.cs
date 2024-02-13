using System.Drawing;

namespace System.Windows.Forms.Direct2D
{
    public interface IDirect2DImaging
    {
        IDirect2DImage FromImage(Image image);
        void DrawImage(IDirect2DImage image, float x, float y, float width, float height);
    }
}
