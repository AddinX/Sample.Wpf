using WPFSample.WPF.ViewModel;

namespace WPFSample.WPF.View
{
    /// <summary>
    /// Interaction logic for MeetingWizardContainerView.xaml
    /// </summary>
    public partial class MeetingWizardContainerView 
    {
        public MeetingWizardContainerView()
        {
            InitializeComponent();
            //var eventAgg = new EventAggregator();
            //DataContext = new MeetingWizardContainerViewModel(eventAgg, new MeetingController(
            //    new MeetingWizardFirstViewModel(),
            //    new MeetingWizardSecondViewModel(eventAgg),
            //    new MeetingWizardLastViewModel(eventAgg),eventAgg));
        }
    }
}
