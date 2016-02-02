using System.Linq;
using System.Runtime.InteropServices;
using AddinX.Ribbon.Contract;
using AddinX.Ribbon.ExcelDna;
using AddinX.Ribbon.Contract.Command;

namespace WPFSample.AddIn
{
    [ComVisible(true)]
    public class AddinRibbon : RibbonFluent
    {
        protected override void CreateFluentRibbon(IRibbonBuilder build)
        {
            build.CustomUi.Ribbon.Tabs(tab =>
                tab.AddTab("Sample").SetId("CustomTab")
                    .Groups(g =>
                    {
                        g.AddGroup("Booking").SetId("SampleGroup")
                            .Items(i =>
                            {
                                i.AddButton("Meeting").SetId("MeetingIdCmd")
                                    .LargeSize().ImageMso("HappyFace").ShowLabel()
                                    .Screentip("");
                            });
                    }));

        }

        protected override void CreateRibbonCommand(IRibbonCommands cmds)
        {
            cmds.AddButtonCommand("MeetingIdCmd")
                .IsEnabled(() => AddinContext.ExcelApp.Worksheets.Any()).IsVisible(() => true)
                .Action(() => AddinContext.MainController.Sample.OpenForm());
        }

        public override void OnClosing()
        {
            AddinContext.TokenCancellationSource.Cancel();
            
            AddinContext.MainController.Dispose();
            
            AddinContext.ExcelApp.DisposeChildInstances(true);
            AddinContext.ExcelApp = null;

            AddinContext.Container.Dispose();
            AddinContext.Container = null;
        }

        public override void OnOpening()
        {
            AddinContext.ExcelApp.SheetSelectionChangeEvent += (a, e) => Ribbon?.Invalidate();
            AddinContext.ExcelApp.SheetActivateEvent += (e) => Ribbon?.Invalidate();
            AddinContext.ExcelApp.SheetChangeEvent += (a, e) => Ribbon?.Invalidate();
        }
    }
}