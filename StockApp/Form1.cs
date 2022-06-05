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

            var textCellStyle = new DataGridViewCellStyle();
            textCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            var numCellStyle = new DataGridViewCellStyle();
            numCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            numCellStyle.Format = ".00";

            var bigNumCellStyle2 = new DataGridViewCellStyle(numCellStyle);
            bigNumCellStyle2.Format = "#,###";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                switch (col.Name)
                {
                    case nameof(DisplayModel.ComCode):
                    case nameof(DisplayModel.ComName):
                        col.DefaultCellStyle = textCellStyle;
                        break;
                    case nameof(DisplayModel.LastDayVolume):
                    case nameof(DisplayModel.ExDividendDateT):
                    case nameof(DisplayModel.ContBonusTimes):
                    case nameof(DisplayModel.HoldStock):
                        col.DefaultCellStyle = bigNumCellStyle2;
                        break;
                    default:
                        col.DefaultCellStyle = numCellStyle;
                        break;
                }
            }
        }

        private void LoadData(string[] assignCodes = null)
        {
            var taskContBonus = Task.Factory.StartNew(() =>
            {
                return CompanyContBonus.GetAll();
            });
            var taskExDividend = Task.Factory.StartNew(() =>
            {
                return CompanyExDividend.GetAll();
            });
            var taskDayVolume = Task.Factory.StartNew(() =>
            {
                return CompanyDayVolume.GetAll();
            });

            var taskAvgBonus = Task.Factory.StartNew(() =>
            {
                return CompanyAvgBonus.GetAll().ConvertAll(d => new DisplayModel(d));
            });

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

            var memoList = MemoContent.Load();

            var list = taskAvgBonus.Result;

            var list2 = list.ToList();

            var contBonusList = taskContBonus.Result;
            list2.ForEach(l =>
            {
                var find = contBonusList.FirstOrDefault(b => b.ComCode == l.ComCode);
                if (find != null)
                    l.SetExtra(find);
            });

            if (assignCodes == null)
            {
                list2.RemoveAll(l => hateComCodes.Contains(l.ComCode));
                list2.Sort(new DisplayModel.Expect7DiffComparer());
                list2 = list2
                    .Where(l => l.CurrentPrice < BasicSetting.Instance.PriceLimit)
                    .Where(l => l.ContBonusTimes >= BasicSetting.Instance.ContBonusTimesLimit)
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
                foreach (var code in memoList.Select(m => m.ComCode))
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

            var exDividendList = taskExDividend.Result;
            list2.ForEach(l =>
            {
                var find = exDividendList.FirstOrDefault(b => b.ComCode == l.ComCode);
                if (find != null)
                    l.SetExtra(find);
            });

            var dayVolumeList = taskDayVolume.Result;
            list2.ForEach(l =>
            {
                var find = dayVolumeList.FirstOrDefault(b => b.ComCode == l.ComCode);
                if (find != null)
                    l.SetExtra(find);
            });

            list2.ForEach(l =>
            {
                var find = memoList.FirstOrDefault(b => b.ComCode == l.ComCode);
                if (find != null)
                    l.SetExtra(find);
            });

            list2.Sort(new DisplayModel.Expect7DiffComparer());

            var binding = new BindingList<DisplayModel>(list2);
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
            var data = (DisplayModel)grow.DataBoundItem;

            if (FavoriteComCodes.Contains(data.ComCode))
                grow.DefaultCellStyle.BackColor = Color.LightYellow;
            else if (HateComCodes.Contains(data.ComCode))
                grow.DefaultCellStyle.BackColor = Color.LightGray;
            else
                grow.DefaultCellStyle.BackColor = Color.White;

            var defaultForeColor = grow.Cells[nameof(DisplayModel.ComCode)].Style.ForeColor;

            grow.Cells[nameof(DisplayModel.Expect5)].Style.ForeColor = defaultForeColor;
            grow.Cells[nameof(DisplayModel.Expect7)].Style.ForeColor = defaultForeColor;
            grow.Cells[nameof(DisplayModel.Expect9)].Style.ForeColor = defaultForeColor;

            if (data.CurrentPrice < data.Expect9)
                grow.Cells[nameof(DisplayModel.Expect9)].Style.ForeColor = Color.Red;
            else if (data.CurrentPrice < data.Expect7)
                grow.Cells[nameof(DisplayModel.Expect7)].Style.ForeColor = Color.Red;
            else if (data.CurrentPrice < data.Expect5)
                grow.Cells[nameof(DisplayModel.Expect5)].Style.ForeColor = Color.Red;
        }

        #region Menu
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

        private void 設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = new FrmBasicSetting();
            if (editor.ShowDialog(this) == DialogResult.OK)
            {
                LoadData();
            }
        }
        #endregion

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
            var data = (DisplayModel)grow.DataBoundItem;
            openToolStripMenuItem.Visible = true;
            addFavoriteToolStripMenuItem.Visible = !FavoriteComCodes.Contains(data.ComCode);
            removeFavoriteToolStripMenuItem.Visible = FavoriteComCodes.Contains(data.ComCode);
            addHateToolStripMenuItem.Visible = !HateComCodes.Contains(data.ComCode);
            removeHateToolStripMenuItem.Visible = HateComCodes.Contains(data.ComCode);

            gv.ClearSelection();
            grow.Selected = true;
            contextMenuStrip1.Tag = grow;
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var gv = (DataGridView)sender;
            var colName = gv.Columns[e.ColumnIndex].Name;
            var lessYield = 0m;
            var maxYield = 0.02m;
            var color = Color.FromArgb(0x2F72EF73);

            var grow = gv.Rows[e.RowIndex];
            var data = (DisplayModel)grow.DataBoundItem;
            var curYield = data.AvgBonus / data.CurrentPrice;
            switch (colName)
            {
                case nameof(DisplayModel.Expect5):
                    lessYield = 0.05m;
                    break;
                case nameof(DisplayModel.Expect7):
                    lessYield = 0.07m;
                    break;
                case nameof(DisplayModel.Expect9):
                    lessYield = 0.09m;
                    break;
                case nameof(DisplayModel.HoldValue):
                    if (!data.HoldValue.HasValue)
                        return;
                    else if (data.CurrentPrice > data.HoldValue.Value)
                        color = Color.FromArgb(0x2FDB7A82);
                    maxYield = 0.1m;
                    curYield = Math.Abs(data.CurrentPrice - data.HoldValue.Value) / data.HoldValue.Value;
                    break;
                default:
                    return;
            }

            e.PaintBackground(e.CellBounds, true);
            var diffYield = curYield - lessYield;
            var ratio = diffYield / maxYield;
            if (ratio > 1)
                ratio = 1;
            var rect = new Rectangle(e.CellBounds.Left, e.CellBounds.Top, (int)(e.CellBounds.Width * ratio), e.CellBounds.Height);
            e.Graphics.FillRectangle(new SolidBrush(color), rect);

            e.PaintContent(e.CellBounds);
            e.Handled = true;
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
                        var data = (DisplayModel)grow.DataBoundItem;
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
            var data = (DisplayModel)grow.DataBoundItem;

            System.Diagnostics.Process.Start("https://goodinfo.tw/tw/StockDividendPolicy.asp?STOCK_ID=" + data.ComCode);
        }

        private void addFavoriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var jsonPath = FrmFavorite.FavoriteFilePath;

            var grow = (DataGridViewRow)contextMenuStrip1.Tag;
            var data = (DisplayModel)grow.DataBoundItem;

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
            var data = (DisplayModel)grow.DataBoundItem;

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
            var data = (DisplayModel)grow.DataBoundItem;

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
            var data = (DisplayModel)grow.DataBoundItem;

            var codes = JsonCache.Load<List<string>>(jsonPath);
            if (codes.Remove(data.ComCode))
            {
                HateComCodes = codes.Distinct().ToArray();
                JsonCache.Store(jsonPath, HateComCodes);

                RefreshCellStyle(grow);
            }
        }

        private void editMemoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grow = (DataGridViewRow)contextMenuStrip1.Tag;
            var data = (DisplayModel)grow.DataBoundItem;

            var editor = new FrmMemo(data);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                RefreshCellStyle(grow);
                dataGridView1.Refresh();
            }
        }

        #endregion
    }
}
