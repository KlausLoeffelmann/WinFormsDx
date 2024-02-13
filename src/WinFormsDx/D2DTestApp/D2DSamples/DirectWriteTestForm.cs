using System.Windows.Forms.Direct2D;

namespace D2DTestApp;

public partial class DirectWriteTestForm : Form
{
    public DirectWriteTestForm()
    {
        InitializeComponent();
    }

    private void direct2dPanel1_PaintIGraphics(object sender, GraphicsPaintEventArgs e)
    {
        e.Graphics.Clear(BackColor);
        e.Graphics.DrawString("TestString", Font, Brushes.Black, 50, 50);
        e.Graphics.DrawString("TestString 2", Font, Brushes.Black, 50, 70);

        var size = e.Graphics.MeasureString("TestString", Font, ClientSize);
    }
}
