
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
            this.addFavoriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFavoriteTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.removeFavoriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addHateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeHateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editMemoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.觀察清單ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.排除清單ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.庫存清單ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.將除息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重新整理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbsSelectedTotal = new System.Windows.Forms.ToolStripStatusLabel();
            this.showYearROEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.dataGridView1.Size = new System.Drawing.Size(1167, 398);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.dataGridView1_CellContextMenuStripNeeded);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridView1_CellPainting);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.addFavoriteToolStripMenuItem,
            this.removeFavoriteToolStripMenuItem,
            this.addHateToolStripMenuItem,
            this.removeHateToolStripMenuItem,
            this.editMemoToolStripMenuItem,
            this.simulatorToolStripMenuItem,
            this.showYearROEToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 202);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem1});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open GoodInfo";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem1.Text = "Open CMoney";
            this.openToolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem1_Click);
            // 
            // addFavoriteToolStripMenuItem
            // 
            this.addFavoriteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFavoriteTextBox});
            this.addFavoriteToolStripMenuItem.Name = "addFavoriteToolStripMenuItem";
            this.addFavoriteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addFavoriteToolStripMenuItem.Text = "Add Favorite";
            this.addFavoriteToolStripMenuItem.Click += new System.EventHandler(this.addFavoriteToolStripMenuItem_Click);
            // 
            // addFavoriteTextBox
            // 
            this.addFavoriteTextBox.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.addFavoriteTextBox.Name = "addFavoriteTextBox";
            this.addFavoriteTextBox.Size = new System.Drawing.Size(100, 23);
            this.addFavoriteTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.addFavoriteTextBox_KeyPress);
            // 
            // removeFavoriteToolStripMenuItem
            // 
            this.removeFavoriteToolStripMenuItem.Name = "removeFavoriteToolStripMenuItem";
            this.removeFavoriteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removeFavoriteToolStripMenuItem.Text = "Remove Favorite";
            this.removeFavoriteToolStripMenuItem.Click += new System.EventHandler(this.removeFavoriteToolStripMenuItem_Click);
            // 
            // addHateToolStripMenuItem
            // 
            this.addHateToolStripMenuItem.Name = "addHateToolStripMenuItem";
            this.addHateToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addHateToolStripMenuItem.Text = "Add Hate";
            this.addHateToolStripMenuItem.Click += new System.EventHandler(this.addHateToolStripMenuItem_Click);
            // 
            // removeHateToolStripMenuItem
            // 
            this.removeHateToolStripMenuItem.Name = "removeHateToolStripMenuItem";
            this.removeHateToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removeHateToolStripMenuItem.Text = "Remove Hate";
            this.removeHateToolStripMenuItem.Click += new System.EventHandler(this.removeHateToolStripMenuItem_Click);
            // 
            // editMemoToolStripMenuItem
            // 
            this.editMemoToolStripMenuItem.Name = "editMemoToolStripMenuItem";
            this.editMemoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.editMemoToolStripMenuItem.Text = "Edit Memo";
            this.editMemoToolStripMenuItem.Click += new System.EventHandler(this.editMemoToolStripMenuItem_Click);
            // 
            // simulatorToolStripMenuItem
            // 
            this.simulatorToolStripMenuItem.Name = "simulatorToolStripMenuItem";
            this.simulatorToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.simulatorToolStripMenuItem.Text = "Simulator";
            this.simulatorToolStripMenuItem.Click += new System.EventHandler(this.simulatorToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.觀察清單ToolStripMenuItem,
            this.排除清單ToolStripMenuItem,
            this.庫存清單ToolStripMenuItem,
            this.將除息ToolStripMenuItem,
            this.重新整理ToolStripMenuItem,
            this.設定ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1191, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 觀察清單ToolStripMenuItem
            // 
            this.觀察清單ToolStripMenuItem.Name = "觀察清單ToolStripMenuItem";
            this.觀察清單ToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.觀察清單ToolStripMenuItem.Text = "觀察清單";
            this.觀察清單ToolStripMenuItem.Click += new System.EventHandler(this.觀察清單ToolStripMenuItem_Click);
            // 
            // 排除清單ToolStripMenuItem
            // 
            this.排除清單ToolStripMenuItem.Name = "排除清單ToolStripMenuItem";
            this.排除清單ToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.排除清單ToolStripMenuItem.Text = "排除清單";
            this.排除清單ToolStripMenuItem.Click += new System.EventHandler(this.排除清單ToolStripMenuItem_Click);
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
            // 重新整理ToolStripMenuItem
            // 
            this.重新整理ToolStripMenuItem.Name = "重新整理ToolStripMenuItem";
            this.重新整理ToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.重新整理ToolStripMenuItem.Text = "重新整理";
            this.重新整理ToolStripMenuItem.Click += new System.EventHandler(this.重新整理ToolStripMenuItem_Click);
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
            this.lbsSelectedTotal});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1191, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbsSelectedTotal
            // 
            this.lbsSelectedTotal.Name = "lbsSelectedTotal";
            this.lbsSelectedTotal.Size = new System.Drawing.Size(128, 17);
            this.lbsSelectedTotal.Text = "toolStripStatusLabel1";
            // 
            // showYearROEToolStripMenuItem
            // 
            this.showYearROEToolStripMenuItem.Name = "showYearROEToolStripMenuItem";
            this.showYearROEToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showYearROEToolStripMenuItem.Text = "Show Year ROE";
            this.showYearROEToolStripMenuItem.Click += new System.EventHandler(this.showYearInfoToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1191, 450);
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
        private System.Windows.Forms.ToolStripMenuItem 排除清單ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addFavoriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFavoriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addHateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重新整理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeHateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 設定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editMemoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 庫存清單ToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox addFavoriteTextBox;
        private System.Windows.Forms.ToolStripMenuItem 將除息ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbsSelectedTotal;
        private System.Windows.Forms.ToolStripMenuItem simulatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem showYearROEToolStripMenuItem;
    }
}

