namespace System.Windows.Forms.Direct2D
{
    public static class Direct2DExtension
    {
        public static IGraphics GetNewDirect2Graphics(this Control control)
        {
            return new Direct2DGraphics(control);
        }
    }
}
