namespace D2DTestApp;

partial class DirectWriteTestForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.direct2dPanel1 = new System.Windows.Forms.Direct2D.Direct2DPanel();
        this.SuspendLayout();
        // 
        // direct2dPanel1
        // 
        this.direct2dPanel1.Location = new System.Drawing.Point(22, 27);
        this.direct2dPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
        this.direct2dPanel1.Name = "direct2dPanel1";
        this.direct2dPanel1.Size = new System.Drawing.Size(831, 507);
        this.direct2dPanel1.TabIndex = 0;
        this.direct2dPanel1.Text = "_direct2dPanel";
        this.direct2dPanel1.PaintIGraphics += new System.EventHandler<System.Windows.Forms.Direct2D.GraphicsPaintEventArgs>(this.direct2dPanel1_PaintIGraphics);
        // 
        // DirectWriteTestForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(878, 620);
        this.Controls.Add(this.direct2dPanel1);
        this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
        this.Name = "DirectWriteTestForm";
        this.Text = "DirectWriteTestForm";
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Direct2D.Direct2DPanel direct2dPanel1;
}