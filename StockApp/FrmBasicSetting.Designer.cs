
namespace StockApp
{
    partial class FrmBasicSetting
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtPriceLimit = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtContBonusTimes = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSimulateMonths = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbxKDJRange = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "成交限制";
            // 
            // txtPriceLimit
            // 
            this.txtPriceLimit.Location = new System.Drawing.Point(71, 12);
            this.txtPriceLimit.Name = "txtPriceLimit";
            this.txtPriceLimit.Size = new System.Drawing.Size(100, 22);
            this.txtPriceLimit.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Location = new System.Drawing.Point(53, 138);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(105, 47);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtContBonusTimes
            // 
            this.txtContBonusTimes.Location = new System.Drawing.Point(71, 40);
            this.txtContBonusTimes.Name = "txtContBonusTimes";
            this.txtContBonusTimes.Size = new System.Drawing.Size(100, 22);
            this.txtContBonusTimes.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "連續股利";
            // 
            // txtSimulateMonths
            // 
            this.txtSimulateMonths.Location = new System.Drawing.Point(71, 68);
            this.txtSimulateMonths.Name = "txtSimulateMonths";
            this.txtSimulateMonths.Size = new System.Drawing.Size(100, 22);
            this.txtSimulateMonths.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "模擬月數";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "KDJ範圍";
            // 
            // cbxKDJRange
            // 
            this.cbxKDJRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxKDJRange.FormattingEnabled = true;
            this.cbxKDJRange.Location = new System.Drawing.Point(71, 99);
            this.cbxKDJRange.Name = "cbxKDJRange";
            this.cbxKDJRange.Size = new System.Drawing.Size(100, 20);
            this.cbxKDJRange.TabIndex = 7;
            // 
            // FrmBasicSetting
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(210, 197);
            this.Controls.Add(this.cbxKDJRange);
            this.Controls.Add(this.txtSimulateMonths);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtContBonusTimes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtPriceLimit);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "FrmBasicSetting";
            this.Text = "FrmBasicSetting";
            this.Load += new System.EventHandler(this.FrmBasicSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPriceLimit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtContBonusTimes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSimulateMonths;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbxKDJRange;
    }
}