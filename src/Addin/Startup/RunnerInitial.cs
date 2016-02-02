using AddinX.Bootstrap.Contract;
using AddinX.Logging;
using Autofac;
using Prism.Events;
using ILogger = AddinX.Logging.ILogger;

namespace WPFSample.AddIn.Startup
{
    internal class RunnerInitial : IRunner
    {
        public void Execute(IRunnerMain bootstrap)
        {
            var bootstrapper = (Bootstrapper)bootstrap;

            // Excel Application
            bootstrapper?.Builder.RegisterInstance(AddinContext.ExcelApp).ExternallyOwned();

            // Ribbon
            bootstrapper?.Builder.RegisterInstance(new AddinRibbon());

            // ILogger
            bootstrapper?.Builder.RegisterInstance<ILogger>(new SerilogLogger());

            // Event Aggregator
            bootstrapper?.Builder.RegisterInstance<IEventAggregator>(new EventAggregator());

        }
    }
}