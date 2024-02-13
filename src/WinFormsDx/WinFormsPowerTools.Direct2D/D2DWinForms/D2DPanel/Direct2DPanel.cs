using System.ComponentModel;

namespace System.Windows.Forms.Direct2D
{
    [Designer(typeof(Direct2DPanelDesigner))]
    public partial class Direct2DPanel : Control, IGraphicsProvider
    {
        public event EventHandler<GraphicsPaintEventArgs>? PaintIGraphics;
        private IGraphics _graphics;
        private bool? _cachedIsAncestorSiteInDesignMode;

        public IGraphics Graphics => _graphics;

        public Direct2DPanel()
        {
            ResizeRedraw = true;

            // We can get the IGraphics (which covers the D2D Graphics in this case)
            // easily by the Extension method.
            _graphics = this.GetNewDirect2Graphics();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (CachedIsAncestorSiteInDesignMode)
            {
                base.OnPaintBackground(pevent);
            }
        }

        unsafe protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!IsHandleCreated || CachedIsAncestorSiteInDesignMode)

            {
                return;
            }

            ((ISupportsBeginAndEndDraw)_graphics).BeginDraw();
            OnPaintIGraphics(_graphics);
            ((ISupportsBeginAndEndDraw)_graphics).EndDraw();
        }

        protected virtual void OnPaintIGraphics(IGraphics graphics) 
            => PaintIGraphics?.Invoke(this, new GraphicsPaintEventArgs(graphics));

        protected bool CachedIsAncestorSiteInDesignMode
            => _cachedIsAncestorSiteInDesignMode ??= IsAncestorSiteInDesignMode;
    }
}
