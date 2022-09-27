using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockApp.UI
{
    internal class DataGridViewNumericColumn : DataGridViewColumn
    {
        public decimal DefaultValue
        {
            set
            {
                var cell = this.CellTemplate as DataGridViewNumericUpDownCell;
                cell.DefaultValue = value;
            }
        }
        public decimal Increment
        {
            set
            {
                var cell = this.CellTemplate as DataGridViewNumericUpDownCell;
                cell.Increment = value;
            }
        }
        public int DecimalPlaces
        {
            set
            {
                var cell = this.CellTemplate as DataGridViewNumericUpDownCell;
                cell.DecimalPlaces = value;
            }
        }
        public decimal Minimum
        {
            set
            {
                var cell = this.CellTemplate as DataGridViewNumericUpDownCell;
                cell.Minimum = value;
            }
        }
        public decimal Maximum
        {
            set
            {
                var cell = this.CellTemplate as DataGridViewNumericUpDownCell;
                cell.Maximum = value;
            }
        }
        public DataGridViewNumericColumn() : base(new DataGridViewNumericUpDownCell())
        {
            this.ValueType = typeof(decimal?);
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is correct.
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewNumericUpDownCell)))
                {
                    throw new InvalidCastException("Must be a NumericUpDownCell");
                }
                base.CellTemplate = value;
            }
        }
    }
}
