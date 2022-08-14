
namespace StockApp.Trace
{
    partial class FrmEditor
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
            this.txtTraceValue = new System.Windows.Forms.TextBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dpLimitDate = new System.Windows.Forms.DateTimePicker();
            this.lblLimitDate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtTraceValue
            // 
            this.txtTraceValue.Location = new System.Drawing.Point(88, 12);
            this.txtTraceValue.Name = "txtTraceValue";
            this.txtTraceValue.Size = new System.Drawing.Size(100, 22);
            this.txtTraceValue.TabIndex = 7;
            this.txtTraceValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(49, 15);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(29, 12);
            this.lblPrice.TabIndex = 10;
            this.lblPrice.Text = "價格";
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClear.Location = new System.Drawing.Point(133, 78);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(89, 37);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear && Save";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Location = new System.Drawing.Point(38, 78);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(89, 37);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dpLimitDate
            // 
            this.dpLimitDate.Location = new System.Drawing.Point(88, 40);
            this.dpLimitDate.Name = "dpLimitDate";
            this.dpLimitDate.Size = new System.Drawing.Size(125, 22);
            this.dpLimitDate.TabIndex = 11;
            // 
            // lblLimitDate
            // 
            this.lblLimitDate.AutoSize = true;
            this.lblLimitDate.Location = new System.Drawing.Point(25, 47);
            this.lblLimitDate.Name = "lblLimitDate";
            this.lblLimitDate.Size = new System.Drawing.Size(53, 12);
            this.lblLimitDate.TabIndex = 10;
            this.lblLimitDate.Text = "有效期限";
            // 
            // FrmEditor
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 127);
            this.Controls.Add(this.dpLimitDate);
            this.Controls.Add(this.txtTraceValue);
            this.Controls.Add(this.lblLimitDate);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSave);
            this.Name = "FrmEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmEditor";
            this.Load += new System.EventHandler(this.FrmEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTraceValue;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DateTimePicker dpLimitDate;
        private System.Windows.Forms.Label lblLimitDate;
    }
}