using LiteDB;
using StockApp.Data;
using StockApp.Group;
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
            dpLimitDate.Value = Utility.TWSEDate.Today.AddDays(Properties.Settings.Default.TraceDays);

            var rdata = this.RefData;
            numTraceValue.Value = rdata.CurrentPrice;

            var tdata = rdata.TraceData;
            if (tdata != null)
            {
                numTraceValue.Value = tdata.Value;
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

            tdata.Value = numTraceValue.Value;

            tdata.LimitDate = dpLimitDate.Value;

            var container = UnityHelper.Create();
            using (ILiteDatabase db = LocalDb.Create())
            {
                container.RegisterInstance(db);

                var traceStockRepo = container.Resolve<TraceStockRepository>();
                traceStockRepo.Update(tdata);

                var custGroupRepo = container.Resolve<CustomGroupRepository>();
                custGroupRepo.AddTraceStock(tdata);
            }

            rdata.SetExtra(tdata);
            this.DialogResult = DialogResult.OK;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            var rdata = this.RefData;
            var tdata = rdata.TraceData;
            if (tdata != null)
            {
                var container = UnityHelper.Create();
                using (ILiteDatabase db = LocalDb.Create())
                {
                    container.RegisterInstance(db);

                    var traceStockRepo = container.Resolve<TraceStockRepository>();
                    traceStockRepo.Delete(tdata);

                    var custGroupRepo = container.Resolve<CustomGroupRepository>();
                    custGroupRepo.DeleteTraceStock(tdata);
                }
            }

            rdata.SetExtra((StockDetail)null);
            this.DialogResult = DialogResult.OK;
        }
    }
}
