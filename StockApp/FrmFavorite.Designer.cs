
namespace StockApp
{
    partial class FrmFavorite
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
            this.txtFavorite = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtFavorite
            // 
            this.txtFavorite.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFavorite.Location = new System.Drawing.Point(12, 12);
            this.txtFavorite.Multiline = true;
            this.txtFavorite.Name = "txtFavorite";
            this.txtFavorite.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFavorite.Size = new System.Drawing.Size(247, 341);
            this.txtFavorite.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Location = new System.Drawing.Point(17, 359);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(105, 47);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Apply && Close";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnView
            // 
            this.btnView.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnView.Location = new System.Drawing.Point(148, 359);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(105, 47);
            this.btnView.TabIndex = 2;
            this.btnView.Text = "Apply && View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // FrmFavorite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 417);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtFavorite);
            this.Name = "FrmFavorite";
            this.Text = "FrmFavorite";
            this.Load += new System.EventHandler(this.FrmFavorite_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFavorite;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnView;
    }
}