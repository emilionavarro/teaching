using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using System.Web.UI.WebControls;

namespace TestBed.Layouts.TestBed
{
    public partial class GridCounter : LayoutsPageBase
    {
        private DataTable dt { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Classes.Cache.instance.ContainsKey("dtCache"))
            {
                Classes.Cache.instance["dtCache"] = new DataTable();
                setupDt();
            }

            dt = (DataTable)Classes.Cache.instance["dtCache"];
            setupGrid();
            CounterBtn.Click += new EventHandler(BtnClicked);
        }

        private void BtnClicked(object sender, EventArgs e)
        {
            DataRow dr = dt.NewRow();
            dr["Title"] = "WhatEver";
            dr["Count"] = dt.Rows.Count;
            dt.Rows.Add(dr);

            Response.Redirect("http://testbed-2013:55555/_layouts/15/testbed/GridCounter.aspx");
        }

        private void setupGrid()
        {
            // Binding (GridView)
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
                Test.Columns.Add(bf);
            }

            // Put to screen
            Test.AutoGenerateColumns = false;
            Test.DataSource = dt;
            Test.DataBind();
        }

        public void setupDt()
        {
            // Columns (DataTable)
            ((DataTable)Classes.Cache.instance["dtCache"]).Columns.Add(new DataColumn("Title", typeof(string)));
            ((DataTable)Classes.Cache.instance["dtCache"]).Columns.Add(new DataColumn("Count", typeof(int)));
        }
    }
}
