﻿
namespace StockApp
{
    partial class FrmMemo
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
            this.btnSave = new System.Windows.Forms.Button();
            this.txtHoldStock = new System.Windows.Forms.TextBox();
            this.lblHoldStock = new System.Windows.Forms.Label();
            this.txtHoldValue = new System.Windows.Forms.TextBox();
            this.lblHoldValue = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Location = new System.Drawing.Point(7, 77);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(89, 37);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtHoldStock
            // 
            this.txtHoldStock.Location = new System.Drawing.Point(75, 12);
            this.txtHoldStock.Name = "txtHoldStock";
            this.txtHoldStock.Size = new System.Drawing.Size(100, 22);
            this.txtHoldStock.TabIndex = 1;
            this.txtHoldStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblHoldStock
            // 
            this.lblHoldStock.AutoSize = true;
            this.lblHoldStock.Location = new System.Drawing.Point(16, 15);
            this.lblHoldStock.Name = "lblHoldStock";
            this.lblHoldStock.Size = new System.Drawing.Size(49, 12);
            this.lblHoldStock.TabIndex = 3;
            this.lblHoldStock.Text = "庫存(張)";
            // 
            // txtHoldValue
            // 
            this.txtHoldValue.Location = new System.Drawing.Point(75, 40);
            this.txtHoldValue.Name = "txtHoldValue";
            this.txtHoldValue.Size = new System.Drawing.Size(100, 22);
            this.txtHoldValue.TabIndex = 2;
            this.txtHoldValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblHoldValue
            // 
            this.lblHoldValue.AutoSize = true;
            this.lblHoldValue.Location = new System.Drawing.Point(36, 43);
            this.lblHoldValue.Name = "lblHoldValue";
            this.lblHoldValue.Size = new System.Drawing.Size(29, 12);
            this.lblHoldValue.TabIndex = 6;
            this.lblHoldValue.Text = "成本";
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClear.Location = new System.Drawing.Point(102, 77);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(89, 37);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear && Save";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // FrmMemo
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(198, 126);
            this.Controls.Add(this.txtHoldValue);
            this.Controls.Add(this.lblHoldValue);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtHoldStock);
            this.Controls.Add(this.lblHoldStock);
            this.MaximizeBox = false;
            this.Name = "FrmMemo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmMemo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtHoldStock;
        private System.Windows.Forms.Label lblHoldStock;
        private System.Windows.Forms.TextBox txtHoldValue;
        private System.Windows.Forms.Label lblHoldValue;
        private System.Windows.Forms.Button btnClear;
    }
}