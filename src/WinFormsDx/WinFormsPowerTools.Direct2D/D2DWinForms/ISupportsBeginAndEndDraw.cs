using System.Drawing;

namespace System.Windows.Forms.Direct2D
{
    public interface ISupportsBeginAndEndDraw
    {
        void BeginDraw();
        void Clear(Color color);
        void EndDraw();
    }
}
