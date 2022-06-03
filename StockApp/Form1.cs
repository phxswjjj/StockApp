using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockApp
{
    public partial class Form1 : Form
    {

        string[] FavoriteComCodes = new string[] { };
        string[] HateComCodes = new string[] { };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData(string[] assignCodes = null)
        {
            List<string> favoriteComCodes;
            if (File.Exists(FrmFavorite.FavoriteFilePath))
                favoriteComCodes = JsonCache.Load<List<string>>(FrmFavorite.FavoriteFilePath);
            else
                favoriteComCodes = new List<string>();

            List<string> hateComCodes;
            if (File.Exists(FrmFavorite.HateFilePath))
                hateComCodes = JsonCache.Load<List<string>>(FrmFavorite.HateFilePath);
            else
                hateComCodes = new List<string>();

            FavoriteComCodes = favoriteComCodes.ToArray();
            HateComCodes = hateComCodes.ToArray();

            var list = CompanyAvgBonus.GetAll();

            var list2 = list.ToList();
            if (assignCodes == null)
            {
                list2.RemoveAll(l => hateComCodes.Contains(l.ComCode));
                list2.Sort(new CompanyAvgBonus.Expect7DiffComparer());
                list2 = list2
                    .Where(l => l.CurrentPrice < 60)
                    .Take(100).ToList();

                foreach (var code in favoriteComCodes)
                {
                    if (list2.Any(l => l.ComCode == code))
                        continue;
                    var data = list.Find(l => l.ComCode == code);
                    if (data == null)
                        continue;
                    list2.Add(data);
                }
            }
            else
            {
                list2.RemoveAll(l => !assignCodes.Contains(l.ComCode));
            }

            list2.Sort(new CompanyAvgBonus.Expect7DiffComparer());

            var binding = new BindingList<CompanyAvgBonus>(list2);
            dataGridView1.DataSource = binding;
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow grow in dataGridView1.Rows)
                RefreshCellStyle(grow);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var gv = (DataGridView)sender;
            var grow = gv.Rows[e.RowIndex];

            RefreshCellStyle(grow);

            dataGridView1.Refresh();
        }

        private void RefreshCellStyle(DataGridViewRow grow)
        {
            var data = (CompanyAvgBonus)grow.DataBoundItem;

            if (FavoriteComCodes.Contains(data.ComCode))
                grow.DefaultCellStyle.BackColor = Color.LightYellow;
            else if (HateComCodes.Contains(data.ComCode))
                grow.DefaultCellStyle.BackColor = Color.LightGray;
            else
                grow.DefaultCellStyle.BackColor = Color.White;

            var defaultForeColor = grow.Cells[nameof(CompanyAvgBonus.ComCode)].Style.ForeColor;

            grow.Cells[nameof(CompanyAvgBonus.Expect5)].Style.ForeColor = defaultForeColor;
            grow.Cells[nameof(CompanyAvgBonus.Expect7)].Style.ForeColor = defaultForeColor;
            grow.Cells[nameof(CompanyAvgBonus.Expect9)].Style.ForeColor = defaultForeColor;

            if (data.CurrentPrice < data.Expect9)
                grow.Cells[nameof(CompanyAvgBonus.Expect9)].Style.ForeColor = Color.Red;
            else if (data.CurrentPrice < data.Expect7)
                grow.Cells[nameof(CompanyAvgBonus.Expect7)].Style.ForeColor = Color.Red;
            else if (data.CurrentPrice < data.Expect5)
                grow.Cells[nameof(CompanyAvgBonus.Expect5)].Style.ForeColor = Color.Red;
        }

        private void 觀察清單ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = new FrmFavorite();
            if (editor.ShowDialog(this) == DialogResult.OK)
            {
                LoadData(editor.ViewCodes);
            }
        }

        private void 排除清單ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = new FrmFavorite();
            if (editor.ShowHateDialog(this) == DialogResult.OK)
            {
                LoadData(editor.ViewCodes);
            }
        }

        private void 重新整理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dataGridView1_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            var gv = (DataGridView)sender;
            if (e.RowIndex == -1)
            {
                foreach (ToolStripItem item in contextMenuStrip1.Items)
                    item.Visible = false;
                return;
            }
            var grow = gv.Rows[e.RowIndex];
            var data = (CompanyAvgBonus)grow.DataBoundItem;
            openToolStripMenuItem.Visible = true;
            addFavoriteToolStripMenuItem.Visible = !FavoriteComCodes.Contains(data.ComCode);
            removeFavoriteToolStripMenuItem.Visible = FavoriteComCodes.Contains(data.ComCode);
            addHateToolStripMenuItem.Visible = !HateComCodes.Contains(data.ComCode);
            removeHateToolStripMenuItem.Visible = HateComCodes.Contains(data.ComCode);

            gv.ClearSelection();
            grow.Selected = true;
            contextMenuStrip1.Tag = grow;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.F)
            {
                var findText = Microsoft.VisualBasic.Interaction.InputBox("輸入完整代號", "尋找代號")
                    .Trim();
                if (!string.IsNullOrEmpty(findText))
                {
                    foreach (DataGridViewRow grow in dataGridView1.Rows)
                    {
                        var data = (CompanyAvgBonus)grow.DataBoundItem;
                        if (data.ComCode == findText)
                        {
                            dataGridView1.ClearSelection();
                            grow.Selected = true;
                            dataGridView1.FirstDisplayedScrollingRowIndex = grow.Index;
                            return;
                        }
                    }
                    MessageBox.Show($"找不到 {findText}");
                }
            }
        }

        #region Context Menu
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grow = (DataGridViewRow)contextMenuStrip1.Tag;
            var data = (CompanyAvgBonus)grow.DataBoundItem;

            System.Diagnostics.Process.Start("https://goodinfo.tw/tw/StockDividendPolicy.asp?STOCK_ID=" + data.ComCode);
        }

        private void addFavoriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var jsonPath = FrmFavorite.FavoriteFilePath;

            var grow = (DataGridViewRow)contextMenuStrip1.Tag;
            var data = (CompanyAvgBonus)grow.DataBoundItem;

            var codes = JsonCache.Load<List<string>>(jsonPath);
            codes.Add(data.ComCode);
            FavoriteComCodes = codes.Distinct().ToArray();
            JsonCache.Store(jsonPath, FavoriteComCodes);

            RefreshCellStyle(grow);
        }
        private void removeFavoriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var jsonPath = FrmFavorite.FavoriteFilePath;

            var grow = (DataGridViewRow)contextMenuStrip1.Tag;
            var data = (CompanyAvgBonus)grow.DataBoundItem;

            var codes = JsonCache.Load<List<string>>(jsonPath);
            if (codes.Remove(data.ComCode))
            {
                FavoriteComCodes = codes.Distinct().ToArray();
                JsonCache.Store(jsonPath, FavoriteComCodes);

                RefreshCellStyle(grow);
            }
        }
        private void addHateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var jsonPath = FrmFavorite.HateFilePath;

            var grow = (DataGridViewRow)contextMenuStrip1.Tag;
            var data = (CompanyAvgBonus)grow.DataBoundItem;

            var codes = JsonCache.Load<List<string>>(jsonPath);
            codes.Add(data.ComCode);
            HateComCodes = codes.Distinct().ToArray();
            JsonCache.Store(jsonPath, HateComCodes);

            RefreshCellStyle(grow);
        }

        private void removeHateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var jsonPath = FrmFavorite.HateFilePath;

            var grow = (DataGridViewRow)contextMenuStrip1.Tag;
            var data = (CompanyAvgBonus)grow.DataBoundItem;

            var codes = JsonCache.Load<List<string>>(jsonPath);
            if (codes.Remove(data.ComCode))
            {
                HateComCodes = codes.Distinct().ToArray();
                JsonCache.Store(jsonPath, HateComCodes);

                RefreshCellStyle(grow);
            }
        }

        #endregion
    }
}
