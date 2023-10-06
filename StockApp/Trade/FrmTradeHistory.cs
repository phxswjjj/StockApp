using LiteDB;
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
        internal DisplayModel RefData { get; }

        public FrmTradeHistory()
        {
            InitializeComponent();

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
            tradeVolumeCol.DefaultValue = 1000;
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
        }

        private DataGridViewRow BindData(DataGridView dataGridView, TradeInfo trade)
        {
            var grow = (DataGridViewRow)dataGridView.RowTemplate.Clone();
            DataGridViewCell FindGridViewCell(string columnName)
            {
                var colIndex = dataGridView.Columns[columnName].Index;
                return grow.Cells[colIndex];
            }
            grow.Tag = trade;
            grow.CreateCells(dataGridView);

            FindGridViewCell(nameof(TradeInfo.TradeDate)).Value = trade.TradeDate;
            FindGridViewCell(nameof(TradeInfo.TradePrice)).Value = trade.TradePrice;
            FindGridViewCell(nameof(TradeInfo.TradeVolume)).Value = trade.TradeVolume;
            FindGridViewCell(nameof(TradeInfo.StockCenter)).Value = trade.StockCenter.ToString();
            FindGridViewCell(nameof(TradeInfo.CurrentValue)).Value = trade.CurrentValue;
            FindGridViewCell(nameof(TradeInfo.Memo)).Value = trade.Memo;

            dataGridView.Rows.Add(grow);
            return grow;
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
                dataGridView.Rows.Remove(grow);
            }
        }

        #endregion

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var dataGridView = dataGridView1;
            var data = this.RefData;

            var editTrades = new List<TradeInfo>();
            foreach (DataGridViewRow grow in dataGridView.Rows)
            {
                if (grow.IsNewRow)
                    continue;

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
                var trade = new TradeInfo(this.RefData)
                {
                    TradeDate = GetGridViewCellValue<DateTime?>(nameof(TradeInfo.TradeDate)),
                    TradePrice = GetGridViewCellValue<decimal>(nameof(TradeInfo.TradePrice)),
                    TradeVolume = (int)GetGridViewCellValue<decimal>(nameof(TradeInfo.TradeVolume)),
                    StockCenter = (StockCenterType)Enum.Parse(typeof(StockCenterType), GetGridViewCellValue<string>(nameof(TradeInfo.StockCenter))),
                    Memo = GetGridViewCellValue<string>(nameof(TradeInfo.Memo)),
                };
                editTrades.Add(trade);
            }
            //依交易日期遞增排序
            editTrades.Sort((x, y) => x.TradeDate.Value.CompareTo(y.TradeDate));

            var container = UnityHelper.Create();
            lock (LocalDb.DbLocker)
            {
                using (ILiteDatabase db = LocalDb.Create())
                {
                    container.RegisterInstance(db);
                    var tradeRepo = container.Resolve<TradeRepository>();
                    tradeRepo.Updates(data.ComCode, editTrades);
                }
            }

            data.ResetTrades(editTrades);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
