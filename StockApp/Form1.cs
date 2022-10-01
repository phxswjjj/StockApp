using StockApp.Group;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockApp
{
    public partial class Form1 : Form
    {
        List<string> FavoriteComCodes = new List<string>();
        List<string> HateComCodes = new List<string>();
        List<CustomGroup> CustomGroups = new List<CustomGroup>();
        private readonly string FavoriteFilePath = "Favorite.json";
        private readonly string HateFilePath = "Hate.json";

        public Form1()
        {
            InitializeComponent();

            InitializeGridView(dataGridView1);
        }

        private void InitializeGridView(DataGridView gridview)
        {
            gridview.RowHeadersWidth = 55;
            gridview.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadSetting();
            PreLoadData();
            LoadData();

            SetupGridViewFitData(dataGridView1);
        }

        private void LoadSetting()
        {
            var loading = new FrmLoading();
            var taskGroups = loading.AddTask("自訂觀察清單", () => CustomGroup.GetAll());

            var taskFavorite = loading.AddTask("觀察清單", () =>
            {
                if (File.Exists(this.FavoriteFilePath))
                    return JsonCache.Load<List<string>>(this.FavoriteFilePath);
                else
                    return new List<string>();
            });
            List<string> favoriteComCodes = taskFavorite.Result;

            var taskHate = loading.AddTask("排除清單", () =>
            {
                if (File.Exists(this.HateFilePath))
                    return JsonCache.Load<List<string>>(this.HateFilePath);
                else
                    return new List<string>();
            });
            List<string> hateComCodes = taskHate.Result;

            var task0050 = loading.AddTask("0050", () =>
            {
                return new ETF.ETF0050().GetAll();
            });
            var task0056 = loading.AddTask("0056", () =>
            {
                return new ETF.ETF0056().GetAll();
            });
            var task00900 = loading.AddTask("00900", () =>
            {
                return new ETF.ETF00900().GetAll();
            });
            var task00878 = loading.AddTask("00878", () =>
            {
                return new ETF.ETF00878().GetAll();
            });
            var taskTrace = loading.AddTask("追蹤價格", () =>
            {
                var group = new TraceGroup()
                {
                    Name = "追蹤價格",
                    ComCodes = Trace.StockDetail.Load()
                        .Select(t => t.ComCode).ToList(),
                };
                return group;
            });
            if (!loading.Start())
                loading.ShowDialog(this);

            var groups = taskGroups.Result;

            var group0050 = task0050.Result;
            groups.RemoveAll(g => g.Name == group0050.Name);
            groups.Add(group0050);

            var group0056 = task0056.Result;
            groups.RemoveAll(g => g.Name == group0056.Name);
            groups.Add(group0056);

            var group00900 = task00900.Result;
            groups.RemoveAll(g => g.Name == group00900.Name);
            groups.Add(group00900);

            var group00878 = task00878.Result;
            groups.RemoveAll(g => g.Name == group00878.Name);
            groups.Add(group00878);

            var groupTrace = taskTrace.Result;
            groups.RemoveAll(g => g.Name == groupTrace.Name);
            groups.Add(groupTrace);

            var favoriteGroup = groups.FirstOrDefault(g => g.Name == "觀察清單");
            if (favoriteGroup == null)
            {
                favoriteGroup = new FavoriteGroup()
                {
                    Name = "觀察清單",
                };
                groups.Add(favoriteGroup);
            }
            var extraFavorites = favoriteComCodes.Except(favoriteGroup.ComCodes);
            favoriteGroup.ComCodes.AddRange(extraFavorites);
            this.FavoriteComCodes = favoriteGroup.ComCodes;

            var hateGroup = groups.FirstOrDefault(g => g.Name == "排除清單");
            if (hateGroup == null)
            {
                hateGroup = new HateGroup()
                {
                    Name = "排除清單",
                };
                groups.Add(hateGroup);
            }
            var extraHates = hateComCodes.Except(hateGroup.ComCodes);
            hateGroup.ComCodes.AddRange(extraHates);
            this.HateComCodes = hateGroup.ComCodes;

            groups.Sort((x, y) => x.SortIndex.CompareTo(y.SortIndex));
            this.CustomGroups = groups;

            foreach (var group in groups)
            {
                var newMainMenuItem = new ToolStripMenuItem(group.Name);
                newMainMenuItem.Click += CustomGroupMenuItem_Click; ;
                觀察清單ToolStripMenuItem.DropDownItems.Add(newMainMenuItem);
            }
        }
        private void PreLoadData()
        {
            var loading = new FrmLoading();
            var taskROE = loading.AddTask("ROE", () =>
            {
                var list = CompanyROE.GetAll();
                return list;
            });
            if (!loading.Start())
                loading.ShowDialog(this);
        }
        private void LoadData(string[] assignCodes = null, IComparer<DisplayModel> comparer = null,
            string likeComName = null)
        {
            var groups = this.CustomGroups;
            List<string> favoriteComCodes = this.FavoriteComCodes;
            List<string> hateComCodes = this.HateComCodes;

            var loading = new FrmLoading();
            var taskContBonus = loading.AddTask("連續股息", () =>
            {
                return CompanyContBonus.GetAll();
            });
            var taskExDividend = loading.AddTask("除息時間", () =>
            {
                return CompanyExDividend.GetAll();
            });
            var taskDayVolume = loading.AddTask("日交易量", () =>
            {
                return CompanyDayVolume.GetAll().ConvertAll(d => new DisplayModel(d));
            });
            var taskAvgBonus = loading.AddTask("平均股息", () =>
            {
                return CompanyAvgBonus.GetAll();
            });
            var taskTraceStock = loading.AddTask("追蹤價格", () =>
            {
                return Trace.StockDetail.Load();
            });
            var taskKDJ = loading.AddTask("KDJ", () => CompanyKDJ.GetAll());
            var taskTradeHistory = loading.AddTask("交易記錄", () =>
            {
                return Trade.TradeInfo.GetAll();
            });
            if (!loading.Start())
                loading.ShowDialog(this);

            var traceStockList = taskTraceStock.Result;

            var list = taskDayVolume.Result;

            var list2 = list.ToList();

            var contBonusList = taskContBonus.Result;
            list2.ForEach(l =>
            {
                var find = contBonusList.FirstOrDefault(b => b.ComCode == l.ComCode);
                if (find != null)
                    l.SetExtra(find);
            });

            var avgBonusList = taskAvgBonus.Result;
            list2.ForEach(l =>
            {
                var find = avgBonusList.FirstOrDefault(b => b.ComCode == l.ComCode);
                if (find != null)
                    l.SetExtra(find);
            });

            if (comparer == null)
                comparer = new DisplayModel.Expect7DiffComparer();

            if (assignCodes == null)
            {
                list2.RemoveAll(l => hateComCodes.Contains(l.ComCode));
                list2.Sort(comparer);
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
                foreach (var code in traceStockList.Select(m => m.ComCode))
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
                list2.RemoveAll(l => !assignCodes.Contains(l.ComCode)
                    && (string.IsNullOrEmpty(likeComName) || !l.ComName.Contains(likeComName)));
            }

            var exDividendList = taskExDividend.Result;
            list2.ForEach(l =>
            {
                var find = exDividendList.FirstOrDefault(b => b.ComCode == l.ComCode);
                if (find != null)
                    l.SetExtra(find);
            });

            list2.ForEach(l =>
            {
                var findTrace = traceStockList.FirstOrDefault(b => b.ComCode == l.ComCode);
                if (findTrace != null)
                    l.SetExtra(findTrace);
            });

            var kdjList = taskKDJ.Result;
            list2.ForEach(l =>
            {
                var find = kdjList.FirstOrDefault(k => k.ComCode == l.ComCode);
                if (find != null)
                    l.SetExtra(find);
            });

            var allTrades = taskTradeHistory.Result;
            list2.ForEach(l =>
            {
                var trades = allTrades.Where(t => t.ComCode == l.ComCode);
                l.ResetTrades(trades);
            });

            list2.Sort(comparer);

            var binding = new BindingList<DisplayModel>(list2);
            dataGridView1.DataSource = binding;
        }

        private void SetupGridViewFitData(DataGridView gridview)
        {
            foreach (DataGridViewColumn col in gridview.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                //default
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                switch (col.Name)
                {
                    case nameof(DisplayModel.ComType):
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                    case nameof(DisplayModel.ComName):
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                        col.Width = 100;
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        break;
                    case nameof(DisplayModel.HoldStock):
                    case nameof(DisplayModel.LastDayVolume):
                        col.DefaultCellStyle.Format = "N0";
                        break;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            JsonCache.Store(this.FavoriteFilePath, this.FavoriteComCodes.Distinct());
            JsonCache.Store(this.HateFilePath, this.HateComCodes.Distinct());

            var groups = this.CustomGroups;
            groups.ForEach(g => g.Distinct());
            groups.RemoveAll(g => g.ComCodes.Count == 0);
            CustomGroup.Store(groups);
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
                    LoadData(new string[] { findText }, likeComName: findText);
                    //MessageBox.Show($"找不到 {findText}");
                }
            }
        }

        #region DataGridView
        private void DataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            int totalVolume = 0;
            decimal totalValue = 0m;    //volume * current price
            decimal totalCost = 0m;     //volume * cost
            foreach (DataGridViewRow grow in dataGridView1.Rows)
            {
                grow.HeaderCell.Value = (grow.Index + 1).ToString();
                RefreshCellStyle(grow);

                var volume = grow.Cells[nameof(DisplayModel.HoldStock)].Value as int?;
                var price = grow.Cells[nameof(DisplayModel.CurrentPrice)].Value as decimal?;
                var cost = grow.Cells[nameof(DisplayModel.HoldValue)].Value as decimal?;

                if (!volume.HasValue || !price.HasValue || !cost.HasValue)
                    continue;

                totalVolume += volume.Value;
                totalValue += volume.Value * price.Value;
                totalCost += volume.Value * cost.Value;
            }

            var benfit = totalValue - totalCost;
            var benfitPercent = 0m;
            if (totalCost != 0)
                benfitPercent = Math.Abs(benfit / totalCost);
            lbsTotalCost.Text = $"總成本={totalCost:N0}";
            lbsTotalValue.Text = $"總價值={totalValue:N0}";
            lbsBenfit.Text = $"{benfit:+#,###;-#,###;0}({benfitPercent:P1})";
            if (benfit > 0)
                lbsBenfit.ForeColor = Color.Red;
            else if (benfit < 0)
                lbsBenfit.ForeColor = Color.Green;
        }

        private void DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var gv = (DataGridView)sender;
            var grow = gv.Rows[e.RowIndex];

            RefreshCellStyle(grow);

            dataGridView1.Refresh();
        }

        private void DataGridView_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            contextMenuStrip1.Tag = null;

            var gv = (DataGridView)sender;
            if (e.RowIndex == -1)
            {
                foreach (ToolStripItem item in contextMenuStrip1.Items)
                    item.Visible = false;
                return;
            }

            if (gv.SelectedRows.Count <= 1)
            {
                foreach (ToolStripItem item in contextMenuStrip1.Items)
                    item.Visible = true;

                var grow = gv.Rows[e.RowIndex];

                gv.ClearSelection();
                grow.Selected = true;
                contextMenuStrip1.Tag = grow;
            }
            else
            {
                foreach (ToolStripItem item in contextMenuStrip1.Items)
                    item.Visible = false;

                showYearROEToolStripMenuItem.Visible = true;
            }
        }

        private void DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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
                case nameof(DisplayModel.TraceValue):
                    if (!data.TraceValue.HasValue)
                        return;
                    else if (data.CurrentPrice > data.TraceValue.Value)
                        color = Color.FromArgb(0x2FDB7A82);
                    maxYield = 0.1m;
                    curYield = Math.Abs(data.CurrentPrice - data.TraceValue.Value) / data.TraceValue.Value;
                    break;
                case nameof(DisplayModel.ValueKDJ):
                    maxYield = 1m;
                    curYield = 1m;
                    color = data.KDJColor;
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
        #endregion

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

        #region Main Menu
        private void 觀察清單ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = new FrmFavorite();
            if (editor.ShowFavoriteDialog(this, this.FavoriteComCodes) == DialogResult.OK)
            {
                this.FavoriteComCodes = new List<string>(editor.ViewCodes);
                LoadData(editor.ViewCodes);
            }
        }

        private void CustomGroupMenuItem_Click(object sender, EventArgs e)
        {
            var item = (ToolStripMenuItem)sender;
            var text = item.Text;

            var groups = this.CustomGroups;
            var group = groups.FirstOrDefault(g => g.Name == text);
            if (group == null)
            {
                MessageBox.Show($"{text} Group not exists");
                return;
            }
            LoadData(group.ComCodes.ToArray());
        }

        private void 預設檢視ToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void 庫存清單ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var loading = new FrmLoading();
            var taskTradeHistory = loading.AddTask("交易記錄", () =>
            {
                return Trade.TradeInfo.GetAll();
            });
            if (!loading.Start())
                loading.ShowDialog(this);
            var list = taskTradeHistory.Result;
            LoadData(list.Select(l => l.ComCode).ToArray());
        }

        private void 將除息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var comparer = new DisplayModel.ExDividendDateTComparer();

            var today = Utility.TWSEDate.Today;
            var codes = CompanyExDividend.GetAll()
                .Where(l => l.ExDividendDate.HasValue && l.ExDividendDate > today)
                .Take(100)
                .Select(l => l.ComCode).ToArray();
            LoadData(codes, comparer);
        }
        #endregion

        #region Context Menu
        private void OpenGoodInfo_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grow = (DataGridViewRow)contextMenuStrip1.Tag;
            var data = (DisplayModel)grow.DataBoundItem;

            System.Diagnostics.Process.Start("https://goodinfo.tw/tw/StockDividendPolicy.asp?STOCK_ID=" + data.ComCode);
        }
        private void OpenCMoney_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grow = (DataGridViewRow)contextMenuStrip1.Tag;
            var data = (DisplayModel)grow.DataBoundItem;

            System.Diagnostics.Process.Start("https://www.cmoney.tw/forum/stock/" + data.ComCode);
        }
        private void ShowYearInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var list = new List<DisplayModel>();
            var gv = dataGridView1;

            if (contextMenuStrip1.Tag != null)
            {
                var grow = (DataGridViewRow)contextMenuStrip1.Tag;
                var data = (DisplayModel)grow.DataBoundItem;
                list.Add(data);
            }
            else
            {
                foreach (DataGridViewRow grow in gv.SelectedRows)
                {
                    var data = (DisplayModel)grow.DataBoundItem;
                    list.Add(data);
                }
            }
            if (list.Count == 0) return;

            var frm = (FrmYearROE)this.OwnedForms.FirstOrDefault(f => f is FrmYearROE);
            if (frm == null)
                frm = new FrmYearROE();
            foreach (var data in list)
                frm.AddData(data);
            if (!frm.Visible)
                frm.Show(this);
        }

        private void ShowFavoriteCustomGroup_Click(object sender, EventArgs e)
        {
            var item = (ToolStripMenuItem)sender;
            var text = item.Text;

            var groups = this.CustomGroups;
            var group = groups.FirstOrDefault(g => g.Name == text);
            if (group == null)
            {
                MessageBox.Show($"找不到自訂群組: {text}");
                return;
            }

            var grow = (DataGridViewRow)contextMenuStrip1.Tag;
            var data = (DisplayModel)grow.DataBoundItem;

            group.ComCodes.Add(data.ComCode);
            FavoriteComCodes.Add(data.ComCode);
            RefreshCellStyle(grow);
        }

        private void ShowEditTraceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grow = (DataGridViewRow)contextMenuStrip1.Tag;
            var data = (DisplayModel)grow.DataBoundItem;

            var editor = new Trace.FrmEditor(data);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                RefreshCellStyle(grow);
                dataGridView1.Refresh();

                var group = this.CustomGroups.FirstOrDefault(g => g.Name == "追蹤價格");
                group.ComCodes = Trace.StockDetail.Load()
                    .Select(d => d.ComCode).ToList();
            }
        }

        private void ShowSimulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grow = (DataGridViewRow)contextMenuStrip1.Tag;
            var data = (DisplayModel)grow.DataBoundItem;

            var months = Properties.Settings.Default.SimulateMaxMonth;
            var dp = CompanyDayPrice.New(data);
            var bh = CompanyBonusHistory.New(data);

            var loading = new FrmLoading();

            var today = Utility.TWSEDate.Today;
            var curMonth = new DateTime(today.Year, today.Month, 1);
            for (var i = 0; i < months; i++)
            {
                var target = curMonth.AddMonths(-i);
                var year = target.Year;
                var month = target.Month;
                var task = loading.AddTask($"每日成交記錄 {year:0000}/{month:00}", () =>
                {
                    return dp.GetMonth(year, month);
                });
            }
            loading.AddTask("除權息記錄", () =>
            {
                return bh.GetAll();
            });

            if (!loading.Start())
                loading.ShowDialog(this);

            dp.Sort();
            bh.Sort();

            var simulator = new FrmSimulator(dp, bh);
            simulator.Show(this);

        }

        private void ShowEditGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grow = (DataGridViewRow)contextMenuStrip1.Tag;
            var data = (DisplayModel)grow.DataBoundItem;

            var editor = new FrmEditGroup(data, this.CustomGroups);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                RefreshCellStyle(grow);
                dataGridView1.Refresh();
            }
        }

        private void tradeHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grow = (DataGridViewRow)contextMenuStrip1.Tag;
            var data = (DisplayModel)grow.DataBoundItem;

            var editor = new Trade.FrmTradeHistory(data);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                RefreshCellStyle(grow);
                dataGridView1.Refresh();
            }
        }
        #endregion
    }
}
