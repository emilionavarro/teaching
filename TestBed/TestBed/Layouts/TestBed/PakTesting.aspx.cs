using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using System.Web.UI.WebControls;
using System.Web;

namespace TestBed.Layouts.TestBed
{
    public partial class PakTesting : LayoutsPageBase
    {
        // Share point
        private SPWeb sPWeb { get; set; }
        private SPContentTypeCollection sPContentTypes { get; set; }
        private DataTable dt { get; set; }
        private int pageSize { get; set; } = 5;
        private int pageIndex { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Setup Sharepoint
            sPWeb = SPContext.Current.Web;
            sPContentTypes = sPWeb.ContentTypes;

            // DataGrid 
            dt = new DataTable();

            // Get request
            if(Request.QueryString["Page"] != null)
            {

                pageIndex = Convert.ToInt32(Request.QueryString["Page"]);
            }
            else
            {
                pageIndex = 1;
            }

            // Build the grid
            setupDataTable();
            populateGrid(pageIndex);
            setupGrid();
            btnEnables();

            // Setup events
            First.Click += new EventHandler(firstPage);
            Previous.Click += new EventHandler(prevPage);
            Next.Click += new EventHandler(nextPage);
            Last.Click += new EventHandler(lastPage);
        }

        private void setupDataTable()
        {
            // Data table
            dt.Columns.Add(new DataColumn("Content Type", typeof(string)));
        }

        private void setupGrid()
        {
            // Bind columns of the data table to the gridview
            BoundField bf = new BoundField();
            bf.DataField = "Content Type";
            bf.HeaderText = "Type";
            bf.ItemStyle.Width = Unit.Pixel(200);
            bf.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            bf.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            PAKGrid.Columns.Add(bf);

            // Now bind the data table to the gridview
            PAKGrid.AutoGenerateColumns = false;
            PAKGrid.DataSource = dt;
            PAKGrid.DataBind();
        }

        private void populateGrid(int page)
        {
            // Clear rows
            dt.Rows.Clear();

            // Build rows
            int counter = 0;
            bool found = false;
            foreach (SPContentType typeC in sPContentTypes)
            {
                if (counter < (page * pageSize) &&
                    counter >= ((page - 1) * pageSize))
                {
                    found = true;
                    DataRow dr = dt.NewRow();
                    dr["Content Type"] = typeC.Name;
                    dt.Rows.Add(dr);
                }
                else if (found)
                {
                    break;
                }
                counter++;
            }

            // Set page number
            PageNum.Text = page.ToString();
        }

        private void firstPage(Object Sender, EventArgs e)
        {
            pageIndex = 1;
            customRedirect();
        }
        private void prevPage(Object Sender, EventArgs e)
        {
            pageIndex--;
            customRedirect();
        }
        private void nextPage(Object Sender, EventArgs e)
        {
            pageIndex++;
            customRedirect();
        }
        private void lastPage(Object Sender, EventArgs e)
        {
            pageIndex = (sPContentTypes.Count / pageSize);
            customRedirect();
        }

        private void customRedirect()
        {
            Response.Redirect(string.Format("{0}?Page={1}",
                "http://testbed-2013:55555/_layouts/15/testbed/PAKTesting.aspx",
                pageIndex));
        }

        private void btnEnables()
        {
            Previous.Enabled = true;
            Next.Enabled = true;

            if (pageIndex == 1)
            {
                Previous.Enabled = false;
            }
            else if (pageIndex == (sPContentTypes.Count / pageSize))
            {
                Next.Enabled = false;
            }
        }
    }
}
