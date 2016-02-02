using System;
using System.Threading;
using System.Windows;
using ExcelDna.Integration;
using ExcelDna.Logging;
using WPFSample.AddIn.Startup;
using Application = NetOffice.ExcelApi.Application;

namespace WPFSample.AddIn
{
    public class Program : IExcelAddIn
    {
        public void AutoOpen()
        {
            try
            {
                // Token cancellation is useful to close all existing Tasks<> before leaving the application
                AddinContext.TokenCancellationSource = new CancellationTokenSource();

                // The Excel Application object
                AddinContext.ExcelApp = new Application(null, ExcelDnaUtil.Application);
                
                // Start the bootstrapper now
                new Bootstrapper(AddinContext.TokenCancellationSource.Token).Start();
            }
            catch (Exception e)
            {
                LogDisplay.RecordLine(e.Message);
                LogDisplay.RecordLine(e.StackTrace);
                LogDisplay.Show();
            }          
        }

        public void AutoClose()
        {
            throw new NotImplementedException();
        }
    }
}