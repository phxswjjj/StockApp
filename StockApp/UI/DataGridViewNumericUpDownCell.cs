using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockApp.UI
{
    internal class DataGridViewNumericUpDownCell : DataGridViewTextBoxCell
    {
        public DataGridViewNumericUpDownCell() : base()
        {
            this.Style.Format = "N2";
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            DataGridViewNumericCellEditingControl nUpDown = DataGridView.EditingControl as DataGridViewNumericCellEditingControl;
            nUpDown.Increment = this.Increment;
            nUpDown.DecimalPlaces = this.DecimalPlaces;
            nUpDown.Minimum = this.Minimum;
            nUpDown.Maximum = this.Maximum;

            // Use the default row value when Value property is null.
            if (this.Value == null)
            {
                nUpDown.Value = (Decimal?)this.DefaultNewRowValue ?? 1m;
            }
            else
            {
                //nUpDown.Value = Decimal.Parse(this.Value.ToString());
                Object trueValue = this.Value;
                nUpDown.Value = Decimal.Parse(trueValue.ToString());
            }
        }

        public override object Clone()
        {
            var obj = base.Clone() as DataGridViewNumericUpDownCell;
            obj.Increment = this.Increment;
            obj.DecimalPlaces = this.DecimalPlaces;
            obj.Minimum = this.Minimum;
            obj.Maximum = this.Maximum;
            obj.DefaultValue = this.DefaultValue;
            return obj;
        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing control 
                return typeof(DataGridViewNumericCellEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return base.ValueType;
            }
            set
            {
                base.ValueType = value;
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return this.DefaultValue;
            }
        }

        public decimal Increment { get; internal set; } = 1;
        public int DecimalPlaces { get; internal set; }
        public decimal Minimum { get; internal set; } = 1;
        public decimal Maximum { get; internal set; } = 100;
        public decimal? DefaultValue { get; internal set; }
    }
}
