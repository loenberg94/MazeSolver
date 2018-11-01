namespace Maze
{
    partial class VisualizationForn
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
            this.visualBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.visualBox)).BeginInit();
            this.SuspendLayout();
            // 
            // visualBox
            // 
            this.visualBox.Location = new System.Drawing.Point(0, 0);
            this.visualBox.Name = "visualBox";
            this.visualBox.Size = new System.Drawing.Size(800, 800);
            this.visualBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.visualBox.TabIndex = 0;
            this.visualBox.TabStop = false;
            this.visualBox.Click += new System.EventHandler(this.visualBox_Click);
            // 
            // VisualizationForn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 814);
            this.Controls.Add(this.visualBox);
            this.MinimumSize = new System.Drawing.Size(824, 857);
            this.Name = "VisualizationForn";
            this.Text = "VisualizationForn";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.VisualizationForn_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.visualBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox visualBox;
    }
}