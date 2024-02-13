namespace D2DTestApp;

partial class Direct2DTestForm
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
        this._btnWritePixels = new System.Windows.Forms.Button();
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.convertHEXFontFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this._direct2dPanel = new System.Windows.Forms.Direct2D.Direct2DPanel();
        this.menuStrip1.SuspendLayout();
        this.SuspendLayout();
        // 
        // _btnWritePixels
        // 
        this._btnWritePixels.Location = new System.Drawing.Point(478, 635);
        this._btnWritePixels.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
        this._btnWritePixels.Name = "_btnWritePixels";
        this._btnWritePixels.Size = new System.Drawing.Size(222, 58);
        this._btnWritePixels.TabIndex = 1;
        this._btnWritePixels.Text = "Write Pixels";
        this._btnWritePixels.UseVisualStyleBackColor = true;
        this._btnWritePixels.Click += new System.EventHandler(this._btnWritePixels_Click);
        // 
        // menuStrip1
        // 
        this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.fileToolStripMenuItem});
        this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 3, 0, 3);
        this.menuStrip1.Size = new System.Drawing.Size(1180, 42);
        this.menuStrip1.TabIndex = 2;
        this.menuStrip1.Text = "menuStrip1";
        // 
        // fileToolStripMenuItem
        // 
        this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.convertHEXFontFileToolStripMenuItem});
        this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        this.fileToolStripMenuItem.Size = new System.Drawing.Size(71, 36);
        this.fileToolStripMenuItem.Text = "&File";
        // 
        // convertHEXFontFileToolStripMenuItem
        // 
        this.convertHEXFontFileToolStripMenuItem.Name = "convertHEXFontFileToolStripMenuItem";
        this.convertHEXFontFileToolStripMenuItem.Size = new System.Drawing.Size(394, 44);
        this.convertHEXFontFileToolStripMenuItem.Text = "&Convert HEX-Font file...";
        // 
        // direct2dPanel1
        // 
        this._direct2dPanel.Location = new System.Drawing.Point(38, 63);
        this._direct2dPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
        this._direct2dPanel.Name = "direct2dPanel1";
        this._direct2dPanel.Size = new System.Drawing.Size(1092, 536);
        this._direct2dPanel.TabIndex = 3;
        this._direct2dPanel.Text = "direct2dPanel1";
        this._direct2dPanel.PaintIGraphics += new System.EventHandler<System.Windows.Forms.Direct2D.GraphicsPaintEventArgs>(this.D2dPanel_PaintIGraphics);
        // 
        // Direct2DTestForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1180, 723);
        this.Controls.Add(this._direct2dPanel);
        this.Controls.Add(this._btnWritePixels);
        this.Controls.Add(this.menuStrip1);
        this.MainMenuStrip = this.menuStrip1;
        this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
        this.Name = "Direct2DTestForm";
        this.Text = "D2DTestForm";
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.Button _btnWritePixels;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem convertHEXFontFileToolStripMenuItem;
    private System.Windows.Forms.Direct2D.Direct2DPanel _direct2dPanel;
}