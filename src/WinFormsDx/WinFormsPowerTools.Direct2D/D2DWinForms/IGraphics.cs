using System.Drawing;

namespace System.Windows.Forms.Direct2D
{
    public interface IGraphics : IDisposable
    {
        /// <summary>
        /// Gets or sets a Region that limits the drawing region of this Graphics.
        /// </summary>
        Region Clip { get; set; }

        /// <summary>
        /// Gets a RectangleF structure that bounds the clipping region of this Graphics.
        /// </summary>
        RectangleF ClipBounds { get; }

        /// <summary>
        /// Clears the entire drawing surface and fills it with the specified background color.
        /// </summary>
        /// <param name="color"></param>
        void Clear(Color color);

        /// <summary>
        /// Draws an ellipse defined by a bounding rectangle specified by a pair of coordinates, a height, and a width.
        /// </summary>
        /// <param name="pen">Pen that determines the color, width, and style of the ellipse.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
        /// <param name="width">Width of the bounding rectangle that defines the ellipse.</param>
        /// <param name="height">Height of the bounding rectangle that defines the ellipse.</param>
        public void DrawEllipse(System.Drawing.Pen pen, float x, float y, float width, float height);

        /// <summary>
        /// Draws the specified Image at the specified location and with the specified size.
        /// </summary>
        /// <param name="image">Image to draw.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the drawn image.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the drawn image.</param>
        /// <param name="width">Width of the drawn image.</param>
        /// <param name="height">Height of the drawn image.</param>
        public void DrawImage(System.Drawing.Image image, float x, float y, float width, float height);

        /// <summary>
        /// Draws a line connecting the two points specified by the coordinate pairs.
        /// </summary>
        /// <param name="pen">Pen that determines the color, width, and style of the line.</param>
        /// <param name="x1">The x-coordinate of the first point.</param>
        /// <param name="y1">The y-coordinate of the first point.</param>
        /// <param name="x2">The x-coordinate of the second point.</param>
        /// <param name="y2">The y-coordinate of the second point.</param>
        void DrawLine(Pen pen, float x1, float y1, float x2, float y2);

        /// <summary>
        /// Draws a rectangle specified by a coordinate pair, a width, and a height.
        /// </summary>
        /// <param name="pen">A Pen that determines the color, width, and style of the rectangle.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="width">The width of the rectangle to draw.</param>
        /// <param name="height">The height of the rectangle to draw.</param>
        public void DrawRectangle(System.Drawing.Pen pen, float x, float y, float width, float height);

        /// <summary>
        /// Draws the specified text string at the specified location with the specified Brush and Font objects.
        /// </summary>
        /// <param name="s">String to draw.</param>
        /// <param name="font">Font that defines the text format of the string.</param>
        /// <param name="brush">Brush that determines the color and texture of the drawn text.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the drawn text.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the drawn text.</param>
        public void DrawString(string? s, System.Drawing.Font font, System.Drawing.Brush brush, float x, float y);

        public void DrawString(string? s, System.Drawing.Font font, System.Drawing.Brush brush, float x, float y, StringFormat stringFormat);

        public void DrawString(string? s, System.Drawing.Font font, System.Drawing.Brush brush, RectangleF layoutRectangle);

        public void DrawString(string? s, System.Drawing.Font font, System.Drawing.Brush brush, RectangleF layoutRectangle, StringFormat stringFormat);

        public SizeF MeasureString(string? text, System.Drawing.Font font, SizeF layoutArea);

        public SizeF MeasureString(string? text, System.Drawing.Font font, SizeF layoutArea, StringFormat stringFormat);

        /// <summary>
        /// Fills the interior of an ellipse defined by a bounding rectangle specified by a pair of coordinates, a width, and a height.
        /// </summary>
        /// <param name="brush">Brush that determines the characteristics of the fill.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
        /// <param name="width">Width of the bounding rectangle that defines the ellipse.</param>
        /// <param name="height">Height of the bounding rectangle that defines the ellipse.</param>
        public void FillEllipse(System.Drawing.Brush brush, float x, float y, float width, float height);

        /// <summary>
        /// Fills the interior of a rectangle specified by a pair of coordinates, a width, and a height.
        /// </summary>
        /// <param name="brush">Brush that determines the characteristics of the fill.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="width">Width of the rectangle to fill.</param>
        /// <param name="height">Height of the rectangle to fill.</param>
        public void FillRectangle(System.Drawing.Brush brush, float x, float y, float width, float height);
    }
}
