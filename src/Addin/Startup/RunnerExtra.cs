using AddinX.Bootstrap.Contract;
using AddinX.Wpf.Contract;
using AddinX.Wpf.Implementation;
using Autofac;
using WPFSample.AddIn.Manipulation;

namespace WPFSample.AddIn.Startup
{
    internal class RunnerExtra : IRunner
    {
        public void Execute(IRunnerMain bootstrap)
        {
            var bootstrapper = (Bootstrapper)bootstrap;
            bootstrapper?.Builder.RegisterType<ExcelInteraction>();

            bootstrapper?.Builder.RegisterType<ExcelDnaWpfHelper>().As<IWpfHelper>();
        }
    }
}