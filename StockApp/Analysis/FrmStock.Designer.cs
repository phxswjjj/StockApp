namespace StockApp.Analysis
{
    partial class FrmStock
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
            this.lblLastYearQuarter = new System.Windows.Forms.Label();
            this.numLastYearQuarter = new System.Windows.Forms.NumericUpDown();
            this.lblPreviousYearQuarter = new System.Windows.Forms.Label();
            this.numPreviousYearQuarter = new System.Windows.Forms.NumericUpDown();
            this.lblLastYearQuarterTip = new System.Windows.Forms.Label();
            this.lblPreviousYearQuarterTip = new System.Windows.Forms.Label();
            this.lblLastDividend = new System.Windows.Forms.Label();
            this.numLastDividend = new System.Windows.Forms.NumericUpDown();
            this.lblLastDividendTip = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numLastYearQuarter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPreviousYearQuarter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLastDividend)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLastYearQuarter
            // 
            this.lblLastYearQuarter.Location = new System.Drawing.Point(12, 9);
            this.lblLastYearQuarter.Name = "lblLastYearQuarter";
            this.lblLastYearQuarter.Size = new System.Drawing.Size(166, 25);
            this.lblLastYearQuarter.TabIndex = 0;
            this.lblLastYearQuarter.Text = "Last Quarter EPS";
            this.lblLastYearQuarter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numLastYearQuarter
            // 
            this.numLastYearQuarter.DecimalPlaces = 2;
            this.numLastYearQuarter.Location = new System.Drawing.Point(184, 7);
            this.numLastYearQuarter.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numLastYearQuarter.Name = "numLastYearQuarter";
            this.numLastYearQuarter.Size = new System.Drawing.Size(90, 27);
            this.numLastYearQuarter.TabIndex = 1;
            this.numLastYearQuarter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblPreviousYearQuarter
            // 
            this.lblPreviousYearQuarter.Location = new System.Drawing.Point(12, 42);
            this.lblPreviousYearQuarter.Name = "lblPreviousYearQuarter";
            this.lblPreviousYearQuarter.Size = new System.Drawing.Size(166, 25);
            this.lblPreviousYearQuarter.TabIndex = 0;
            this.lblPreviousYearQuarter.Text = "Previous Quarter EPS";
            this.lblPreviousYearQuarter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numPreviousYearQuarter
            // 
            this.numPreviousYearQuarter.DecimalPlaces = 2;
            this.numPreviousYearQuarter.Location = new System.Drawing.Point(184, 40);
            this.numPreviousYearQuarter.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numPreviousYearQuarter.Name = "numPreviousYearQuarter";
            this.numPreviousYearQuarter.Size = new System.Drawing.Size(90, 27);
            this.numPreviousYearQuarter.TabIndex = 1;
            this.numPreviousYearQuarter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblLastYearQuarterTip
            // 
            this.lblLastYearQuarterTip.AutoSize = true;
            this.lblLastYearQuarterTip.Location = new System.Drawing.Point(280, 13);
            this.lblLastYearQuarterTip.Name = "lblLastYearQuarterTip";
            this.lblLastYearQuarterTip.Size = new System.Drawing.Size(147, 16);
            this.lblLastYearQuarterTip.TabIndex = 2;
            this.lblLastYearQuarterTip.Text = "lblLastYearQuarterTip";
            this.lblLastYearQuarterTip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPreviousYearQuarterTip
            // 
            this.lblPreviousYearQuarterTip.AutoSize = true;
            this.lblPreviousYearQuarterTip.Location = new System.Drawing.Point(280, 46);
            this.lblPreviousYearQuarterTip.Name = "lblPreviousYearQuarterTip";
            this.lblPreviousYearQuarterTip.Size = new System.Drawing.Size(175, 16);
            this.lblPreviousYearQuarterTip.TabIndex = 2;
            this.lblPreviousYearQuarterTip.Text = "lblPreviousYearQuarterTip";
            this.lblPreviousYearQuarterTip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLastDividend
            // 
            this.lblLastDividend.Location = new System.Drawing.Point(12, 93);
            this.lblLastDividend.Name = "lblLastDividend";
            this.lblLastDividend.Size = new System.Drawing.Size(166, 25);
            this.lblLastDividend.TabIndex = 0;
            this.lblLastDividend.Text = "Last Dividend";
            this.lblLastDividend.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numLastDividend
            // 
            this.numLastDividend.DecimalPlaces = 2;
            this.numLastDividend.Location = new System.Drawing.Point(184, 91);
            this.numLastDividend.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numLastDividend.Name = "numLastDividend";
            this.numLastDividend.Size = new System.Drawing.Size(90, 27);
            this.numLastDividend.TabIndex = 1;
            this.numLastDividend.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblLastDividendTip
            // 
            this.lblLastDividendTip.AutoSize = true;
            this.lblLastDividendTip.Location = new System.Drawing.Point(280, 97);
            this.lblLastDividendTip.Name = "lblLastDividendTip";
            this.lblLastDividendTip.Size = new System.Drawing.Size(128, 16);
            this.lblLastDividendTip.TabIndex = 2;
            this.lblLastDividendTip.Text = "lblLastDividendTip";
            this.lblLastDividendTip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrmStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 208);
            this.Controls.Add(this.lblPreviousYearQuarterTip);
            this.Controls.Add(this.lblLastDividendTip);
            this.Controls.Add(this.lblLastYearQuarterTip);
            this.Controls.Add(this.numPreviousYearQuarter);
            this.Controls.Add(this.numLastDividend);
            this.Controls.Add(this.lblPreviousYearQuarter);
            this.Controls.Add(this.lblLastDividend);
            this.Controls.Add(this.numLastYearQuarter);
            this.Controls.Add(this.lblLastYearQuarter);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmStock";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmStock";
            ((System.ComponentModel.ISupportInitialize)(this.numLastYearQuarter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPreviousYearQuarter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLastDividend)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLastYearQuarter;
        private System.Windows.Forms.NumericUpDown numLastYearQuarter;
        private System.Windows.Forms.Label lblPreviousYearQuarter;
        private System.Windows.Forms.NumericUpDown numPreviousYearQuarter;
        private System.Windows.Forms.Label lblLastYearQuarterTip;
        private System.Windows.Forms.Label lblPreviousYearQuarterTip;
        private System.Windows.Forms.Label lblLastDividend;
        private System.Windows.Forms.NumericUpDown numLastDividend;
        private System.Windows.Forms.Label lblLastDividendTip;
    }
}