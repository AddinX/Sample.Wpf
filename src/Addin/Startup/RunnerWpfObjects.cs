using AddinX.Bootstrap.Contract;
using Autofac;
using WPFSample.WPF.View;
using WPFSample.WPF.ViewModel;

namespace WPFSample.AddIn.Startup
{
    internal class RunnerWpfObjects : IRunner
    {
        public void Execute(IRunnerMain bootstrap)
        {
            var bootstrapper = (Bootstrapper)bootstrap;

            bootstrapper?.Builder.RegisterType<MainWindow>();
            bootstrapper?.Builder.RegisterType<MainWindowViewModel>();

            bootstrapper?.Builder.RegisterType<MeetingWizardContainerViewModel>();
            bootstrapper?.Builder.RegisterType<MeetingController>();

            bootstrapper?.Builder.RegisterType<MeetingWizardContainerView>();
            bootstrapper?.Builder.RegisterType<MeetingWizardFirstView>();
            bootstrapper?.Builder.RegisterType<MeetingWizardSecondView>();
            bootstrapper?.Builder.RegisterType<MeetingWizardLastView>();
            
            bootstrapper?.Builder.RegisterType<MeetingWizardFirstViewModel>();
            bootstrapper?.Builder.RegisterType<MeetingWizardSecondViewModel>();
            bootstrapper?.Builder.RegisterType<MeetingWizardLastViewModel>();
        }
    }
}