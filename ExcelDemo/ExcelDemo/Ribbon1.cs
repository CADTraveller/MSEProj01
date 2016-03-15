using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Office = Microsoft.Office.Core;
using ExcelTools = Microsoft.Office.Tools.Excel;
using ExcelIO = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace ExcelDemo
{
    public partial class Ribbon1
    {
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            ExcelIO.Application excelApp;
            ExcelIO.Worksheet activeSheet;

            try
            {
                excelApp = Globals.ThisAddIn.Application;
                activeSheet = excelApp.ActiveSheet;
            }
            catch (Exception)
            {
                MessageBox.Show("Problem Getting Workbooks & Sheets");
                return;
            }
            ExcelIO.Range usedRange;
            try
            {

                usedRange = activeSheet.UsedRange;

            }
            catch (Exception)
            {

                MessageBox.Show("Nothing Selected to Publish");
                return;
            }

            int iNumColumns = usedRange.Columns.Count;
            int iNumRows = usedRange.Rows.Count;
            //__validate IDs
            //__validate PhaseID
            //__validate VerticalID

            List<StatusUpdate> updates = new List<StatusUpdate>();

            //__skip header row, get values
            for (int i = 2; i <= iNumRows; i++)
            {
                ExcelIO.Range cell = usedRange.Cells[i, 1];
                Guid projectID = new Guid(cell.Value.ToString());

                cell = usedRange.Cells[i, 2];
                string projectName = cell.Value.ToString();

                cell = usedRange.Cells[i, 3];
                int phaseID = Convert.ToInt32(cell.Value);

                cell = usedRange.Cells[i, 4];
                int verticalID = Convert.ToInt32(cell.Value);

                for (int k = 5; k <= iNumColumns; k++)
                {
                    StatusUpdate update = new StatusUpdate();
                    update.ProjectID = projectID;
                    update.PhaseID = phaseID;
                    update.VerticalID = verticalID;
                    update.ProjectName = projectName;
                    cell = usedRange.Cells[i, k];
                    string text = cell.Value.ToString();

                    if (k % 2 == 1) update.UpdateKey = text;
                    else update.UpdateValue = text;
                    updates.Add(update);
                }

            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(updates);
            int updateCount = updates.Count;
            try
            {

                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var result = client.UploadString("http://costcodevops.azurewebsites.net/ProjectUpdate/Update", "Post", json);
                    //var result = client.UploadString("http://costcodevops.azurewebsites.net/ProjectUpdate/Update", "Post", json);
                    Console.WriteLine(result);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            MessageBox.Show("Successfully posted " + updateCount + " updates to CostcoDevOps Azure");
        }
    }

    public partial class StatusUpdate
    {
        public System.Guid ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int PhaseID { get; set; }
        public int StatusSequence { get; set; }
        public Nullable<int> VerticalID { get; set; }
        public Nullable<System.DateTime> RecordDate { get; set; }
        public string UpdateKey { get; set; }
        public string UpdateValue { get; set; }
    }
}
