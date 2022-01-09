using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using Translation;

namespace UI
{
	/// <summary>
	/// Summary description for GridViewManager
	/// </summary>
	public static class GridViewManager
	{
		public static void AddBoundFieldColumn(GridView gv, string dataField, HorizontalAlign align)
		{
			var bf = new BoundField { HeaderText = TranslationManager.Translate(dataField), DataField = dataField, SortExpression = dataField};
			bf.HeaderStyle.Height = 20;
			bf.HeaderStyle.HorizontalAlign = align;
			bf.ItemStyle.HorizontalAlign = align;

            gv.Columns.Add(bf);
		}

		public static void AddHyperLinkFieldColumn(GridView gv, string dataField, HorizontalAlign align)
		{
			var hlf = new HyperLinkField { HeaderText = TranslationManager.Translate(dataField), DataTextField = dataField, SortExpression = dataField};
			hlf.HeaderStyle.Height = 20;
			hlf.HeaderStyle.HorizontalAlign = align;
			hlf.ItemStyle.HorizontalAlign = align;

			gv.Columns.Add(hlf);
		}

		public static void AddButtonFieldColumn(GridView gv, string dataField, HorizontalAlign align)
		{
			var hlf = new ButtonField() { HeaderText = TranslationManager.Translate(dataField), DataTextField = dataField };
			hlf.HeaderStyle.Height = 20;
			hlf.HeaderStyle.HorizontalAlign = align;
			hlf.ItemStyle.HorizontalAlign = align;

			gv.Columns.Add(hlf);
		}

		public static void AddRow(DataTable dt, Dictionary<string, string> rowData)
		{
			if (rowData == null) return;
			var row = dt.NewRow();
			foreach (var data in rowData)
			{
				row[data.Key] = data.Value;
			}
			dt.Rows.Add(row);
		}

		public static void AddRowToTop(DataTable dt, Dictionary<string, string> rowData)
		{
			var row = dt.NewRow();
			foreach (var data in rowData)
			{
				row[data.Key] = data.Value;
			}
			dt.Rows.InsertAt(row, 0);
		}

		public static void SetRowStyle(GridViewRow row, Color foreColor, bool fontBold)
		{
			for (var i = 0; i < row.Cells.Count; i++)
			{
				row.Cells[i].ForeColor = foreColor;
				row.Cells[i].Font.Bold = fontBold;
			}
		}
	}
}