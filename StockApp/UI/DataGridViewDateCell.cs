using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockApp.UI
{
    internal class DataGridViewDateCell : DataGridViewTextBoxCell
    {
        public DataGridViewDateCell() : base()
        {
            this.Style.Format = "d";
        }

        protected override object GetValue(int rowIndex)
        {
            var v = base.GetValue(rowIndex);
            if (v is string s)
            {
                try
                {
                    return DateTime.Parse(s);
                }
                catch (Exception)
                {
                    if (string.IsNullOrEmpty(s))
                        return this.DefaultNewRowValue;
                    else
                        throw;
                }
            }
            return v;
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            var editor = this.DataGridView.EditingControl as DataGridViewCalendarEditingControl;
            editor.Value = (DateTime?)this.Value;
            editor.Checked = editor.Value.HasValue;
        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing control that CalendarCell uses.
                return typeof(DataGridViewCalendarEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                // Return the type of the value that CalendarCell contains.

                return typeof(DateTime);
            }
        }
        public override object DefaultNewRowValue => null;
    }
}
