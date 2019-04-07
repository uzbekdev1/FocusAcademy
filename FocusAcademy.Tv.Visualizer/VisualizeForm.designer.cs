namespace FocusAcademy.Tv.Visualizer
{
    partial class VisualizeForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.pictureBoxTop = new System.Windows.Forms.PictureBox();
            this.propertyGridTop = new System.Windows.Forms.PropertyGrid();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.pictureBoxBottom = new System.Windows.Forms.PictureBox();
            this.propertyGridBottom = new System.Windows.Forms.PropertyGrid();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBottom)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(660, 362);
            this.splitContainer1.SplitterDistance = 181;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.pictureBoxTop);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.propertyGridTop);
            this.splitContainer2.Size = new System.Drawing.Size(660, 181);
            this.splitContainer2.SplitterDistance = 466;
            this.splitContainer2.TabIndex = 0;
            // 
            // pictureBoxTop
            // 
            this.pictureBoxTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxTop.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxTop.Name = "pictureBoxTop";
            this.pictureBoxTop.Size = new System.Drawing.Size(466, 181);
            this.pictureBoxTop.TabIndex = 0;
            this.pictureBoxTop.TabStop = false;
            // 
            // propertyGridTop
            // 
            this.propertyGridTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridTop.HelpVisible = false;
            this.propertyGridTop.Location = new System.Drawing.Point(0, 0);
            this.propertyGridTop.Name = "propertyGridTop";
            this.propertyGridTop.Size = new System.Drawing.Size(190, 181);
            this.propertyGridTop.TabIndex = 0;
            this.propertyGridTop.ToolbarVisible = false;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.pictureBoxBottom);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.propertyGridBottom);
            this.splitContainer3.Size = new System.Drawing.Size(660, 177);
            this.splitContainer3.SplitterDistance = 466;
            this.splitContainer3.TabIndex = 0;
            // 
            // pictureBoxBottom
            // 
            this.pictureBoxBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxBottom.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxBottom.Name = "pictureBoxBottom";
            this.pictureBoxBottom.Size = new System.Drawing.Size(466, 177);
            this.pictureBoxBottom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxBottom.TabIndex = 0;
            this.pictureBoxBottom.TabStop = false;
            // 
            // propertyGridBottom
            // 
            this.propertyGridBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridBottom.HelpVisible = false;
            this.propertyGridBottom.Location = new System.Drawing.Point(0, 0);
            this.propertyGridBottom.Name = "propertyGridBottom";
            this.propertyGridBottom.Size = new System.Drawing.Size(190, 177);
            this.propertyGridBottom.TabIndex = 0;
            this.propertyGridBottom.ToolbarVisible = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 40;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 383);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(684, 78);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pitch-shifter";
            // 
            // VisualizeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.splitContainer1);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "VisualizeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Visualizer";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTop)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBottom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PictureBox pictureBoxTop;
        private System.Windows.Forms.PropertyGrid propertyGridTop;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.PictureBox pictureBoxBottom;
        private System.Windows.Forms.PropertyGrid propertyGridBottom;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

