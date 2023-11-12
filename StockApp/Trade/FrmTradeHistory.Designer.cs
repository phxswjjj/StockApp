namespace StockApp.Trade
{
    partial class FrmTradeHistory
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
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnYuanTa = new System.Windows.Forms.Button();
            this.btnSinoPac = new System.Windows.Forms.Button();
            this.btnSell100 = new System.Windows.Forms.Button();
            this.btnSell1000 = new System.Windows.Forms.Button();
            this.btnBuy100 = new System.Windows.Forms.Button();
            this.btnBuy1000 = new System.Windows.Forms.Button();
            this.btnVolume0 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(12, 54);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(893, 488);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(112, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(461, 551);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 37);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Location = new System.Drawing.Point(366, 551);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(89, 37);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnYuanTa
            // 
            this.btnYuanTa.Location = new System.Drawing.Point(12, 12);
            this.btnYuanTa.Name = "btnYuanTa";
            this.btnYuanTa.Size = new System.Drawing.Size(82, 36);
            this.btnYuanTa.TabIndex = 14;
            this.btnYuanTa.Text = "元大";
            this.btnYuanTa.UseVisualStyleBackColor = true;
            this.btnYuanTa.Click += new System.EventHandler(this.btnYuanTa_Click);
            // 
            // btnSinoPac
            // 
            this.btnSinoPac.Location = new System.Drawing.Point(100, 12);
            this.btnSinoPac.Name = "btnSinoPac";
            this.btnSinoPac.Size = new System.Drawing.Size(82, 36);
            this.btnSinoPac.TabIndex = 15;
            this.btnSinoPac.Text = "永豐";
            this.btnSinoPac.UseVisualStyleBackColor = true;
            this.btnSinoPac.Click += new System.EventHandler(this.btnSinoPac_Click);
            // 
            // btnSell100
            // 
            this.btnSell100.Location = new System.Drawing.Point(215, 12);
            this.btnSell100.Name = "btnSell100";
            this.btnSell100.Size = new System.Drawing.Size(82, 36);
            this.btnSell100.TabIndex = 14;
            this.btnSell100.Text = "-100";
            this.btnSell100.UseVisualStyleBackColor = true;
            this.btnSell100.Click += new System.EventHandler(this.btnSell100_Click);
            // 
            // btnSell1000
            // 
            this.btnSell1000.Location = new System.Drawing.Point(303, 12);
            this.btnSell1000.Name = "btnSell1000";
            this.btnSell1000.Size = new System.Drawing.Size(82, 36);
            this.btnSell1000.TabIndex = 14;
            this.btnSell1000.Text = "-1000";
            this.btnSell1000.UseVisualStyleBackColor = true;
            this.btnSell1000.Click += new System.EventHandler(this.btnSell1000_Click);
            // 
            // btnBuy100
            // 
            this.btnBuy100.Location = new System.Drawing.Point(479, 12);
            this.btnBuy100.Name = "btnBuy100";
            this.btnBuy100.Size = new System.Drawing.Size(82, 36);
            this.btnBuy100.TabIndex = 14;
            this.btnBuy100.Text = "+100";
            this.btnBuy100.UseVisualStyleBackColor = true;
            this.btnBuy100.Click += new System.EventHandler(this.btnBuy100_Click);
            // 
            // btnBuy1000
            // 
            this.btnBuy1000.Location = new System.Drawing.Point(567, 12);
            this.btnBuy1000.Name = "btnBuy1000";
            this.btnBuy1000.Size = new System.Drawing.Size(82, 36);
            this.btnBuy1000.TabIndex = 14;
            this.btnBuy1000.Text = "+1000";
            this.btnBuy1000.UseVisualStyleBackColor = true;
            this.btnBuy1000.Click += new System.EventHandler(this.btnBuy1000_Click);
            // 
            // btnVolume0
            // 
            this.btnVolume0.Location = new System.Drawing.Point(391, 12);
            this.btnVolume0.Name = "btnVolume0";
            this.btnVolume0.Size = new System.Drawing.Size(82, 36);
            this.btnVolume0.TabIndex = 14;
            this.btnVolume0.Text = "0";
            this.btnVolume0.UseVisualStyleBackColor = true;
            this.btnVolume0.Click += new System.EventHandler(this.btnVolume0_Click);
            // 
            // FrmTradeHistory
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(917, 600);
            this.Controls.Add(this.btnSinoPac);
            this.Controls.Add(this.btnBuy1000);
            this.Controls.Add(this.btnBuy100);
            this.Controls.Add(this.btnVolume0);
            this.Controls.Add(this.btnSell1000);
            this.Controls.Add(this.btnSell100);
            this.Controls.Add(this.btnYuanTa);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmTradeHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmTradeList";
            this.Load += new System.EventHandler(this.FrmTradeHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.Button btnYuanTa;
        private System.Windows.Forms.Button btnSinoPac;
        private System.Windows.Forms.Button btnSell100;
        private System.Windows.Forms.Button btnSell1000;
        private System.Windows.Forms.Button btnBuy100;
        private System.Windows.Forms.Button btnBuy1000;
        private System.Windows.Forms.Button btnVolume0;
    }
}