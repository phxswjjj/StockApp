
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
            this.lblPrice = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dpLimitDate = new System.Windows.Forms.DateTimePicker();
            this.lblLimitDate = new System.Windows.Forms.Label();
            this.numTraceValue = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numTraceValue)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPrice
            // 
            this.lblPrice.Location = new System.Drawing.Point(61, 24);
            this.lblPrice.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(76, 20);
            this.lblPrice.TabIndex = 10;
            this.lblPrice.Text = "價格";
            this.lblPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClear.Location = new System.Drawing.Point(202, 119);
            this.btnClear.Margin = new System.Windows.Forms.Padding(5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(121, 46);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear && Save";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Location = new System.Drawing.Point(61, 119);
            this.btnSave.Margin = new System.Windows.Forms.Padding(5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(121, 46);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dpLimitDate
            // 
            this.dpLimitDate.Location = new System.Drawing.Point(147, 67);
            this.dpLimitDate.Margin = new System.Windows.Forms.Padding(5);
            this.dpLimitDate.Name = "dpLimitDate";
            this.dpLimitDate.Size = new System.Drawing.Size(164, 29);
            this.dpLimitDate.TabIndex = 11;
            // 
            // lblLimitDate
            // 
            this.lblLimitDate.Location = new System.Drawing.Point(49, 73);
            this.lblLimitDate.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblLimitDate.Name = "lblLimitDate";
            this.lblLimitDate.Size = new System.Drawing.Size(88, 20);
            this.lblLimitDate.TabIndex = 10;
            this.lblLimitDate.Text = "有效期限";
            this.lblLimitDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTraceValue
            // 
            this.numTraceValue.DecimalPlaces = 2;
            this.numTraceValue.Location = new System.Drawing.Point(147, 22);
            this.numTraceValue.Margin = new System.Windows.Forms.Padding(5);
            this.numTraceValue.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numTraceValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numTraceValue.Name = "numTraceValue";
            this.numTraceValue.Size = new System.Drawing.Size(166, 29);
            this.numTraceValue.TabIndex = 12;
            this.numTraceValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numTraceValue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // FrmEditor
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 179);
            this.Controls.Add(this.numTraceValue);
            this.Controls.Add(this.dpLimitDate);
            this.Controls.Add(this.lblLimitDate);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FrmEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmEditor";
            this.Load += new System.EventHandler(this.FrmEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numTraceValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DateTimePicker dpLimitDate;
        private System.Windows.Forms.Label lblLimitDate;
        private System.Windows.Forms.NumericUpDown numTraceValue;
    }
}