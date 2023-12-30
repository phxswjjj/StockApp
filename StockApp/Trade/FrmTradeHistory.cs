using Newtonsoft.Json.Linq;
using Serilog;
using StockApp.Data;
using StockApp.Group;
using StockApp.UI;
using StockApp.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace StockApp.Trade
{
    public partial class FrmTradeHistory : Form
    {
        private readonly ILogger LogService;
        /// <summary>
        /// 介面上呈現的交易記錄，用來區分哪些記錄要刪除
        /// </summary>
        private List<TradeInfo> ListTrades;

        internal DisplayModel RefData { get; }

        public FrmTradeHistory()
        {
            InitializeComponent();

            var container = UnityHelper.Create();
            this.LogService = container.Resolve<ILogger>()
                .ForContext("class", this.GetType());

            InitializeGridView(dataGridView1);
        }

        private void InitializeGridView(DataGridView dataGridView)
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;

            DataGridViewNumericColumn CreateNumColumn(string colName, string headerText)
            {
                var col = new DataGridViewNumericColumn
                {
                    Name = colName,
                    HeaderText = headerText,
                    SortMode = DataGridViewColumnSortMode.Automatic,
                };
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col.DecimalPlaces = 2;
                col.Minimum = 0;
                col.Maximum = 2000;
                return col;
            }

            var tradeDataCol = new DataGridViewDateColumn()
            {
                Name = nameof(TradeInfo.TradeDate),
                HeaderText = "交易日期",
                SortMode = DataGridViewColumnSortMode.Automatic,
                Width = 140,
            };

            var tradeVolumeCol = CreateNumColumn(nameof(TradeInfo.TradeVolume), "交易股數");
            tradeVolumeCol.DefaultValue = 0;
            tradeVolumeCol.Increment = 1000;
            tradeVolumeCol.Minimum = -1000_000;
            tradeVolumeCol.Maximum = 1000_000;

            var stockCenterCol = new DataGridViewComboBoxColumn()
            {
                Name = nameof(TradeInfo.StockCenter),
                HeaderText = "證卷商",
                SortMode = DataGridViewColumnSortMode.Automatic,
            };
            stockCenterCol.Items.AddRange(Enum.GetNames(typeof(StockCenterType)));

            var currentValueCol = CreateNumColumn(nameof(TradeInfo.CurrentValue), "價值");
            currentValueCol.ReadOnly = true;

            dataGridView.Columns.Add(tradeDataCol);
            dataGridView.Columns.Add(CreateNumColumn(nameof(TradeInfo.TradePrice), "交易價格"));
            dataGridView.Columns.Add(tradeVolumeCol);
            dataGridView.Columns.Add(stockCenterCol);
            dataGridView.Columns.Add(currentValueCol);
            dataGridView.Columns.Add(nameof(TradeInfo.Memo), "備註");

        }

        internal FrmTradeHistory(DisplayModel data) : this()
        {
            this.Text = $"{data.ComCode} {data.ComName}";

            this.RefData = data;
        }

        private void FrmTradeHistory_Load(object sender, EventArgs e)
        {
            var data = this.RefData;
            var trades = data.Trades;

            dataGridView1.Rows.Clear();
            foreach (var trade in trades)
                BindData(dataGridView1, trade);

            this.ListTrades = trades.ToList();

            //預設新增資料
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.NewRowIndex].Cells[0];
        }

        private DataGridViewRow BindData(DataGridView dataGridView, TradeInfo trade)
        {
            var grow = (DataGridViewRow)dataGridView.RowTemplate.Clone();
            grow.Tag = trade;
            grow.CreateCells(dataGridView);

            FindGridViewCell(dataGridView, grow, nameof(TradeInfo.TradeDate)).Value = trade.TradeDate;
            FindGridViewCell(dataGridView, grow, nameof(TradeInfo.TradePrice)).Value = trade.TradePrice;
            FindGridViewCell(dataGridView, grow, nameof(TradeInfo.TradeVolume)).Value = trade.TradeVolume;
            FindGridViewCell(dataGridView, grow, nameof(TradeInfo.StockCenter)).Value = trade.StockCenter.ToString();
            FindGridViewCell(dataGridView, grow, nameof(TradeInfo.CurrentValue)).Value = trade.CurrentValue;
            FindGridViewCell(dataGridView, grow, nameof(TradeInfo.Memo)).Value = trade.Memo;

            dataGridView.Rows.Add(grow);
            return grow;
        }
        private static DataGridViewCell FindGridViewCell(DataGridView dataGridView, DataGridViewRow grow, string columnName)
        {
            var colIndex = dataGridView.Columns[columnName].Index;
            return grow.Cells[colIndex];
        }

        #region DataGridView Event

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var dataGridView = (DataGridView)sender;

            if (dataGridView.EditingControl is DataGridViewComboBoxEditingControl gComboxEdit)
                gComboxEdit.DroppedDown = true;
        }

        #endregion

        #region menu

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dataGridView = dataGridView1;

            if (dataGridView.SelectedRows.Count == 0)
                return;

            foreach (DataGridViewRow grow in dataGridView.SelectedRows)
            {
                if (grow.Index == dataGridView1.NewRowIndex)
                    continue;
                dataGridView.Rows.Remove(grow);
            }
        }

        #endregion

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var dataGridView = dataGridView1;
            var data = this.RefData;

            var trades = this.ListTrades;
            var updateTrades = new List<TradeInfo>();

            var logger = this.LogService.ForContext("event", nameof(BtnSave_Click))
                .ForContext("data", data);

            #region 取得異動資料
            foreach (DataGridViewRow grow in dataGridView.Rows)
            {
                if (grow.IsNewRow)
                    continue;

                var trade = grow.Tag as TradeInfo;

                if (trade == null)
                    trade = new TradeInfo(this.RefData);

                T GetGridViewCellValue<T>(string columnName)
                {
                    var colIndex = dataGridView.Columns[columnName].Index;
                    var cell = grow.Cells[colIndex];
                    if (cell.Value == null)
                        return default(T);
                    else
                    {
                        return (T)cell.Value;
                    }
                }

                trade.TradeDate = GetGridViewCellValue<DateTime?>(nameof(TradeInfo.TradeDate));
                trade.TradePrice = GetGridViewCellValue<decimal>(nameof(TradeInfo.TradePrice));
                trade.TradeVolume = (int)GetGridViewCellValue<decimal>(nameof(TradeInfo.TradeVolume));
                if (Enum.TryParse<StockCenterType>(GetGridViewCellValue<string>(nameof(TradeInfo.StockCenter)), out var stockCenterType))
                    trade.StockCenter = stockCenterType;
                else
                {
                    MessageBox.Show($"{dataGridView.Columns[nameof(TradeInfo.StockCenter)].HeaderText} 驗證失敗");
                    return;
                }
                trade.Memo = GetGridViewCellValue<string>(nameof(TradeInfo.Memo));
                updateTrades.Add(trade);
            }

            var deleteTrades = trades.Where(t => !updateTrades.Any(u => u.SysId == t.SysId)).ToList();
            #endregion

            var container = UnityHelper.Create();
            try
            {
                using(var conn = LocalDb.CreateSqlLite())
                {
                    container.RegisterInstance(conn);
                    var tradeRepo = container.Resolve<TradeRepository>();
                    using(var trans = conn.BeginTransaction())
                    {
                        try
                        {
                            if (updateTrades.Count > 0)
                                tradeRepo.Updates(updateTrades);
                            if (deleteTrades.Count > 0)
                                tradeRepo.Deletes(deleteTrades);
                            trans.Commit();
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                MessageBox.Show(ex.Message);
                return;
            }

            data.ResetTrades(updateTrades);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #region Short Command
        private void btnYuanTa_Click(object sender, EventArgs e)
        {
            var gv = this.dataGridView1;

            var selectedRow = gv.CurrentRow;
            if (selectedRow == null)
                selectedRow = gv.Rows[gv.NewRowIndex];
            FindGridViewCell(gv, selectedRow, nameof(TradeInfo.StockCenter)).Value = nameof(StockCenterType.元大);

            DefaultTradeDate(gv, selectedRow);
        }

        private static void DefaultTradeDate(DataGridView gv, DataGridViewRow selectedRow)
        {
            var dateCell = FindGridViewCell(gv, selectedRow, nameof(TradeInfo.TradeDate));
            if (dateCell.Value == null)
                dateCell.Value = DateTime.Today;
        }

        private void btnSinoPac_Click(object sender, EventArgs e)
        {
            var gv = this.dataGridView1;

            var selectedRow = gv.CurrentRow;
            if (selectedRow == null)
                selectedRow = gv.Rows[gv.NewRowIndex];
            FindGridViewCell(gv, selectedRow, nameof(TradeInfo.StockCenter)).Value = nameof(StockCenterType.永豐);

            DefaultTradeDate(gv, selectedRow);
        }

        private void btnSell100_Click(object sender, EventArgs e)
        {
            var gv = this.dataGridView1;

            var selectedRow = gv.CurrentRow;
            if (selectedRow == null)
                selectedRow = gv.Rows[gv.NewRowIndex];
            var cell = FindGridViewCell(gv, selectedRow, nameof(TradeInfo.TradeVolume));
            var curVolume = cell.Value as decimal?;
            if (!curVolume.HasValue)
                curVolume = 0;
            curVolume -= 100;
            cell.Value = curVolume;
        }

        private void btnSell1000_Click(object sender, EventArgs e)
        {
            var gv = this.dataGridView1;

            var selectedRow = gv.CurrentRow;
            if (selectedRow == null)
                selectedRow = gv.Rows[gv.NewRowIndex];
            var cell = FindGridViewCell(gv, selectedRow, nameof(TradeInfo.TradeVolume));
            var curVolume = cell.Value as decimal?;
            if (!curVolume.HasValue)
                curVolume = 0;
            curVolume -= 1000;
            cell.Value = curVolume;
        }

        private void btnVolume0_Click(object sender, EventArgs e)
        {
            var gv = this.dataGridView1;

            var selectedRow = gv.CurrentRow;
            if (selectedRow == null)
                selectedRow = gv.Rows[gv.NewRowIndex];
            FindGridViewCell(gv, selectedRow, nameof(TradeInfo.TradeVolume)).Value = 0;
        }

        private void btnBuy100_Click(object sender, EventArgs e)
        {
            var gv = this.dataGridView1;

            var selectedRow = gv.CurrentRow;
            if (selectedRow == null)
                selectedRow = gv.Rows[gv.NewRowIndex];
            var cell = FindGridViewCell(gv, selectedRow, nameof(TradeInfo.TradeVolume));
            var curVolume = cell.Value as decimal?;
            if (!curVolume.HasValue)
                curVolume = 0;
            curVolume += 100;
            cell.Value = curVolume;
        }

        private void btnBuy1000_Click(object sender, EventArgs e)
        {
            var gv = this.dataGridView1;

            var selectedRow = gv.CurrentRow;
            if (selectedRow == null)
                selectedRow = gv.Rows[gv.NewRowIndex];
            var cell = FindGridViewCell(gv, selectedRow, nameof(TradeInfo.TradeVolume));
            var curVolume = cell.Value as decimal?;
            if (!curVolume.HasValue)
                curVolume = 0;
            curVolume += 1000;
            cell.Value = curVolume;
        }
        #endregion
    }
}
