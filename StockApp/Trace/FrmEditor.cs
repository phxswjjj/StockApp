using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockApp.Trace
{
    public partial class FrmEditor : Form
    {
        private readonly DisplayModel RefData;

        internal FrmEditor(DisplayModel data)
        {
            this.RefData = data;

            InitializeComponent();
        }

        private void FrmEditor_Load(object sender, EventArgs e)
        {
            dpLimitDate.Value = Utility.TWSEDate.Today.AddMonths(1);

            var rdata = this.RefData;
            var tdata = rdata.TraceData;
            if (tdata != null)
            {
                txtTraceValue.Text = tdata.Value.ToString();
                if (tdata.LimitDate.HasValue)
                    dpLimitDate.Value = tdata.LimitDate.Value;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var rdata = this.RefData;
            var tdata = rdata.TraceData;
            if (tdata == null)
                tdata = new StockDetail(rdata);

            var sStockValue = txtTraceValue.Text.Trim();
            if (dpLimitDate.Value < DateTime.Today)
            {
                MessageBox.Show($"{lblLimitDate.Text} 不可設定過去的日期");
                return;
            }

            decimal iStockValue;
            if (!decimal.TryParse(sStockValue, out iStockValue))
            {
                MessageBox.Show($"非數值: {sStockValue}");
                txtTraceValue.Focus();
                return;
            }
            else
                tdata.Value = iStockValue;

            tdata.LimitDate = dpLimitDate.Value;
            tdata.Update();

            rdata.SetExtra(tdata);
            this.DialogResult = DialogResult.OK;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            var rdata = this.RefData;
            var tdata = rdata.TraceData;
            if (tdata != null)
                tdata.Remove();

            rdata.SetExtra((StockDetail)null);
            this.DialogResult = DialogResult.OK;
        }
    }
}
