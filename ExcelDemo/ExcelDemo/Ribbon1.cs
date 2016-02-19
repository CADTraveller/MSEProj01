using System;
using System.Collections.Generic;
using System.Linq;
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
            ExcelTools.Workbook workbook;
            ExcelIO.Sheets sheets;
            try
            {
                excelApp = Globals.ThisAddIn.Application;
                workbook = Globals.Factory.GetVstoObject(excelApp.ActiveWorkbook);
                sheets = workbook.Worksheets;
            }
            catch (Exception)
            {
                MessageBox.Show("Problem Getting Workbooks & Sheets");
                return;
            }
            ExcelIO.Range selection;
            try
            {
                selection = excelApp.Selection as ExcelIO.Range;

            }
            catch (Exception)
            {

                MessageBox.Show("Nothing Selected to Publish");
                return;
            }

            int iNumColumns = selection.Columns.Count;
            int iNumRows = selection.Rows.Count;
            string[] alphas = new string[]{"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P",
            "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            
            List<StatusUpdate> updates = new List<StatusUpdate>();

            //__skip header row, get values
            for (int i = 2; i < iNumRows; i++)
            {
                StatusUpdate update = new StatusUpdate();
                ExcelIO.Range cell = selection.Cells[i, 1];
                Guid projectID = new Guid(cell.Value.ToString());
                update.ProjectID = projectID;

                cell = selection.Cells[i, 2];
                int phaseID = Convert.ToInt32(cell.Value);

                cell = selection.Cells[i, 3];
                int verticalID = Convert.ToInt32(cell.Value);


                for (int k = 4; k < alphas.Count(); k++)
                {
                    cell = selection.Cells[i, k];
                    string text = cell.Value.ToString();
                    if (string.IsNullOrEmpty(text)) break;
                    if (k % 2 == 0) update.UpdateKey = text;
                    else update.UpdateValue = text;
                }

                updates.Add(update);
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(updates);
        }
    }

    public partial class StatusUpdate
    {
        public System.Guid ProjectID { get; set; }
        public int PhaseID { get; set; }
        public int StatusSequence { get; set; }
        public Nullable<int> VerticalID { get; set; }
        public Nullable<System.DateTime> RecordDate { get; set; }
        public string UpdateKey { get; set; }
        public string UpdateValue { get; set; }
    }
}
