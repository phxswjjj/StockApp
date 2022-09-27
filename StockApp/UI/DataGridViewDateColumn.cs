using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockApp.UI
{
    //https://learn.microsoft.com/en-us/dotnet/desktop/winforms/controls/how-to-host-controls-in-windows-forms-datagridview-cells
    internal class DataGridViewDateColumn : DataGridViewColumn
    {
        public DataGridViewDateColumn() : base(new DataGridViewDateCell())
        {
        }
        public override DataGridViewCell CellTemplate
        {
            get => base.CellTemplate;
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(DataGridViewDateCell)))
                {
                    throw new InvalidCastException("Must be a DataGridViewDateCell");
                }
                base.CellTemplate = value;
            }
        }
    }

}
