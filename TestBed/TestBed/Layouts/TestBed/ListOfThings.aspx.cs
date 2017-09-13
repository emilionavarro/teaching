using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using System.Web.UI.WebControls;

namespace TestBed.Layouts.TestBed
{
    public partial class ListOfThings : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Classes.Counter.count.ContainsKey("test1"))
            {
                Classes.Counter.count.Add("test1", 0);
            }

            Button1.Click += new EventHandler(this.test);
            Counter.Text = (Classes.Counter.count["test1"]).ToString();
            setupGrid();
        }

        private void test(Object sender, EventArgs e)
        {
            // inc label
            if (IsPostBack)
                Counter.Text = (++Classes.Counter.count["test1"]).ToString();

            Response.Redirect("http://testbed-2013:55555/_layouts/15/testbed/ListOfThings.aspx");
        }

        private void setupGrid()
        {
            SPWeb thisWeb = SPContext.Current.Web;
            //SPQuery query = new SPQuery();
            SPListCollection mySPLists = thisWeb.Lists;
            DataTable dt = new DataTable();

            // Columns
            dt.Columns.Add(new DataColumn("Title", typeof(string)));
            dt.Columns.Add(new DataColumn("Count", typeof(int)));

            // Rows
            DataRow dr = dt.NewRow();
            dr["Title"] = "test";
            dr["Count"] = 1;
            dt.Rows.Add(dr);

            // Binding
            for (int i = 0; i < 2; i++)
            {
                BoundField bf = new BoundField();
                switch (i)
                {
                    case 0:
                        bf.DataField = "Title";
                        bf.HeaderText = "Title";
                        break;
                    case 1:
                        bf.DataField = "Count";
                        bf.HeaderText = "Count";
                        break;
                }

                bf.ItemStyle.Width = Unit.Pixel(100);
                bf.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                bf.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                ThingsList.Columns.Add(bf);
            }

            // Put to screen
            ThingsList.AutoGenerateColumns = false;
            ThingsList.DataSource = dt;
            ThingsList.DataBind();
        }
    }
}
