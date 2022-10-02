
namespace StockApp
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editTraceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showYearROEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tradeHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.觀察清單ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.庫存清單ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.將除息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.預設檢視ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbsTotalCost = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbsTotalValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbsBenefit = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(12, 27);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1246, 429);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.DataGridView_CellContextMenuStripNeeded);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellEndEdit);
            this.dataGridView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DataGridView_CellPainting);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DataGridView_DataBindingComplete);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.editTraceToolStripMenuItem,
            this.simulatorToolStripMenuItem,
            this.showYearROEToolStripMenuItem,
            this.editGroupToolStripMenuItem,
            this.tradeHistoryToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(165, 136);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem1});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.openToolStripMenuItem.Text = "Open GoodInfo";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenGoodInfo_ToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(157, 22);
            this.openToolStripMenuItem1.Text = "Open CMoney";
            this.openToolStripMenuItem1.Click += new System.EventHandler(this.OpenCMoney_ToolStripMenuItem_Click);
            // 
            // editTraceToolStripMenuItem
            // 
            this.editTraceToolStripMenuItem.Name = "editTraceToolStripMenuItem";
            this.editTraceToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.editTraceToolStripMenuItem.Text = "Edit Trace";
            this.editTraceToolStripMenuItem.Click += new System.EventHandler(this.ShowEditTraceToolStripMenuItem_Click);
            // 
            // simulatorToolStripMenuItem
            // 
            this.simulatorToolStripMenuItem.Name = "simulatorToolStripMenuItem";
            this.simulatorToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.simulatorToolStripMenuItem.Text = "Simulator";
            this.simulatorToolStripMenuItem.Click += new System.EventHandler(this.ShowSimulatorToolStripMenuItem_Click);
            // 
            // showYearROEToolStripMenuItem
            // 
            this.showYearROEToolStripMenuItem.Name = "showYearROEToolStripMenuItem";
            this.showYearROEToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.showYearROEToolStripMenuItem.Text = "Show Year ROE";
            this.showYearROEToolStripMenuItem.Click += new System.EventHandler(this.ShowYearInfoToolStripMenuItem_Click);
            // 
            // editGroupToolStripMenuItem
            // 
            this.editGroupToolStripMenuItem.Name = "editGroupToolStripMenuItem";
            this.editGroupToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.editGroupToolStripMenuItem.Text = "Edit Group";
            this.editGroupToolStripMenuItem.Click += new System.EventHandler(this.ShowEditGroupToolStripMenuItem_Click);
            // 
            // tradeHistoryToolStripMenuItem
            // 
            this.tradeHistoryToolStripMenuItem.Name = "tradeHistoryToolStripMenuItem";
            this.tradeHistoryToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.tradeHistoryToolStripMenuItem.Text = "Trade History";
            this.tradeHistoryToolStripMenuItem.Click += new System.EventHandler(this.tradeHistoryToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.觀察清單ToolStripMenuItem,
            this.庫存清單ToolStripMenuItem,
            this.將除息ToolStripMenuItem,
            this.預設檢視ToolStripMenuItem,
            this.設定ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1270, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 觀察清單ToolStripMenuItem
            // 
            this.觀察清單ToolStripMenuItem.Name = "觀察清單ToolStripMenuItem";
            this.觀察清單ToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.觀察清單ToolStripMenuItem.Text = "觀察清單▼";
            // 
            // 庫存清單ToolStripMenuItem
            // 
            this.庫存清單ToolStripMenuItem.Name = "庫存清單ToolStripMenuItem";
            this.庫存清單ToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.庫存清單ToolStripMenuItem.Text = "庫存清單";
            this.庫存清單ToolStripMenuItem.Click += new System.EventHandler(this.庫存清單ToolStripMenuItem_Click);
            // 
            // 將除息ToolStripMenuItem
            // 
            this.將除息ToolStripMenuItem.Name = "將除息ToolStripMenuItem";
            this.將除息ToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.將除息ToolStripMenuItem.Text = "將除息";
            this.將除息ToolStripMenuItem.Click += new System.EventHandler(this.將除息ToolStripMenuItem_Click);
            // 
            // 預設檢視ToolStripMenuItem
            // 
            this.預設檢視ToolStripMenuItem.Name = "預設檢視ToolStripMenuItem";
            this.預設檢視ToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.預設檢視ToolStripMenuItem.Text = "預設檢視";
            this.預設檢視ToolStripMenuItem.Click += new System.EventHandler(this.預設檢視ToolStripMenuItem_Click);
            // 
            // 設定ToolStripMenuItem
            // 
            this.設定ToolStripMenuItem.Name = "設定ToolStripMenuItem";
            this.設定ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.設定ToolStripMenuItem.Text = "設定";
            this.設定ToolStripMenuItem.Click += new System.EventHandler(this.設定ToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbsTotalCost,
            this.toolStripStatusLabel1,
            this.lbsTotalValue,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.lbsBenefit});
            this.statusStrip1.Location = new System.Drawing.Point(0, 459);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1270, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbsTotalCost
            // 
            this.lbsTotalCost.Name = "lbsTotalCost";
            this.lbsTotalCost.Size = new System.Drawing.Size(77, 17);
            this.lbsTotalCost.Text = "lbsTotalCost";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel1.Text = " | ";
            // 
            // lbsTotalValue
            // 
            this.lbsTotalValue.Name = "lbsTotalValue";
            this.lbsTotalValue.Size = new System.Drawing.Size(84, 17);
            this.lbsTotalValue.Text = "lbsTotalValue";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel3.Text = " | ";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(40, 17);
            this.toolStripStatusLabel4.Text = "損益=";
            // 
            // lbsBenfit
            // 
            this.lbsBenefit.Name = "lbsBenfit";
            this.lbsBenefit.Size = new System.Drawing.Size(55, 17);
            this.lbsBenefit.Text = "lbsBenfit";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1270, 481);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 觀察清單ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 預設檢視ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 設定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 庫存清單ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 將除息ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbsTotalCost;
        private System.Windows.Forms.ToolStripMenuItem simulatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem showYearROEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editTraceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tradeHistoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lbsTotalValue;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel lbsBenefit;
    }
}

