namespace System.Windows.Forms.Direct2D
{
    public class GraphicsPaintEventArgs : EventArgs
    {
        public GraphicsPaintEventArgs(IGraphics graphics)
            => Graphics = graphics;

        public IGraphics Graphics { get; }
    }
}
