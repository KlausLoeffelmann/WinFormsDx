using Microsoft.DotNet.DesignTools.Designers;
using System.Drawing;

namespace System.Windows.Forms.Direct2D
{
    internal partial class Direct2DPanelDesigner : ControlDesigner
    {
        protected override void OnPaintAdornments(PaintEventArgs paintEventArgs)
        {
            base.OnPaintAdornments(paintEventArgs);

            // If you want to paint custom adorner or other GDI+ based content,
            // use the paintEventArgs' Graphics methods to render it.

            // We just drawing frame around the ClientRectangle with dotted brush...
            if (!(SelectionService?.GetComponentSelected(Control) ?? false))
            {
                using Pen pen = new(Control.ForeColor);
                //...if the control is not currently selected.

                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                using SolidBrush brush = new(Control.ForeColor);

                var clientRect = Control.ClientRectangle;
                clientRect.Inflate(-1, -1);

                paintEventArgs.Graphics.DrawRectangle(pen, clientRect);
            }
        }
    }
}
