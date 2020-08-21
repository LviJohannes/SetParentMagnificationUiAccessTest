namespace SetParentMagnificationUiAccessTest
{
    partial class Form1
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.DockedWindowPanel = new System.Windows.Forms.Panel();
            this.MagnificationPanel = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.DockedWindowPanel);
            this.flowLayoutPanel1.Controls.Add(this.MagnificationPanel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // DockedWindowPanel
            // 
            this.DockedWindowPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.DockedWindowPanel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.DockedWindowPanel.Location = new System.Drawing.Point(0, 0);
            this.DockedWindowPanel.Margin = new System.Windows.Forms.Padding(0);
            this.DockedWindowPanel.Name = "DockedWindowPanel";
            this.DockedWindowPanel.Size = new System.Drawing.Size(200, 100);
            this.DockedWindowPanel.TabIndex = 0;
            // 
            // MagnificationPanel
            // 
            this.MagnificationPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MagnificationPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.MagnificationPanel.Location = new System.Drawing.Point(200, 0);
            this.MagnificationPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MagnificationPanel.Name = "MagnificationPanel";
            this.MagnificationPanel.Size = new System.Drawing.Size(200, 100);
            this.MagnificationPanel.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel DockedWindowPanel;
        private System.Windows.Forms.Panel MagnificationPanel;
    }
}

