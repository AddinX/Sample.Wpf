using Prism.Mvvm;

namespace WPFSample.WPF.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public MeetingWizardContainerViewModel MeetingWizardContainerViewModel { get;  private set; }

        public MainWindowViewModel(MeetingWizardContainerViewModel meetingWizardContainerViewModel)
        {
            MeetingWizardContainerViewModel = meetingWizardContainerViewModel;
        }
    }
}