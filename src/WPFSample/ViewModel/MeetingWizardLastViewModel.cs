using Prism.Events;
using Prism.Mvvm;

namespace WPFSample.WPF.ViewModel
{
    public class MeetingWizardLastViewModel : BindableBase
    {
        private readonly IEventAggregator eventAgg;

        public MeetingWizardLastViewModel(IEventAggregator eventAgg)
        {
            this.eventAgg = eventAgg;
            IsJobInProgress = true;
            IsSuccess = false;
        }

        private bool isJobInProgress;

        public bool IsJobInProgress
        {
            get { return isJobInProgress; }
            set
            {
                if (isJobInProgress == value) return;
                SetProperty(ref isJobInProgress, value);
            }
        }

        private bool isSuccess;

        public bool IsSuccess
        {
            get { return isSuccess; }
            set
            {
                if (isSuccess == value) return;
                SetProperty(ref isSuccess, value); 
            }
        }
    }
}