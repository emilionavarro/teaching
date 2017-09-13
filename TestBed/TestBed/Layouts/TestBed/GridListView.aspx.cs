using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using System.Web.UI.WebControls;

namespace TestBed.Layouts.TestBed
{
    public partial class GridListView : LayoutsPageBase
    {
        private DataTable dt { get; set; } = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            SPWeb thisWeb = SPContext.Current.Web;
            SPListCollection mySPLists = thisWeb.Lists;

            dt.Columns.Add("Title");

            DataRow dr;
            foreach(SPList temp in mySPLists)
            {
                dr = dt.NewRow();
                dr["Title"] = temp.Title;
                dt.Rows.Add(dr);
            }

            setupGrid();
        }

        public void setupGrid()
        {
            // Binding (GridView)
            for (int i = 0; i < 1; i++)
            {
                BoundField bf = new BoundField();
                switch (i)
                {
                    case 0:
                        bf.DataField = "Title";
                        bf.HeaderText = "Title";
                        break;
                }

                bf.ItemStyle.Width = Unit.Pixel(100);
                bf.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                bf.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                Lists.Columns.Add(bf);
            }

            // Put to screen
            Lists.AutoGenerateColumns = false;
            Lists.DataSource = dt;
            Lists.DataBind();
        }
    }
}
