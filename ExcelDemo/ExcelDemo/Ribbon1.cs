﻿using System;
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
                excelApp= Globals.ThisAddIn.Application;
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
            }
        }
    }
}