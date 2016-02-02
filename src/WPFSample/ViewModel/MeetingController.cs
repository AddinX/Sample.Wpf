using Prism.Events;
using WPFSample.WPF.Event;

namespace WPFSample.WPF.ViewModel
{
    public class MeetingController
    {
        private readonly IEventAggregator eventAggregator;
        private readonly MeetingWizardLastViewModel meetingWizardLastVm;
        private readonly MeetingWizardSecondViewModel meetingWizardSecondVm;
        private readonly MeetingWizardFirstViewModel meetingWizardFirstVm;
        private SubscriptionToken subscriptionToken;

        public MeetingController(MeetingWizardFirstViewModel meetingWizardFirstVm
            , MeetingWizardSecondViewModel meetingWizardSecondVm
            , MeetingWizardLastViewModel meetingWizardLastVm
            , IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.meetingWizardFirstVm = meetingWizardFirstVm;
            this.meetingWizardSecondVm = meetingWizardSecondVm;
            this.meetingWizardLastVm = meetingWizardLastVm;
        }

        public void SendData()
        {
            SubscribeToExcelReply();
            SendDataToExcelAsync();
        }

        private void SubscribeToExcelReply()
        {
            subscriptionToken = eventAggregator
                .GetEvent<PubSubEvent<ExcelMeetingDataResponse>>()
                .Subscribe(UpdateEnableForFinishButton);
        }

        private void UpdateEnableForFinishButton(ExcelMeetingDataResponse obj)
        {
            eventAggregator
                .GetEvent<PubSubEvent<ExcelMeetingDataResponse>>()
                .Unsubscribe(subscriptionToken);

            eventAggregator.GetEvent<PubSubEvent<ButtonEnabled>>()
                .Publish(new ButtonEnabled
                {
                    IsFinishButtonEnabled = true
                    ,
                    IsNextButtonEnabled = false
                });
            meetingWizardLastVm.IsJobInProgress = false;
            meetingWizardLastVm.IsSuccess = obj.ProcessCompletedSuccessfully;
        }


        private void SendDataToExcelAsync()
        {
            var data = meetingWizardSecondVm.GetData();
           // Send Data to Excel
            eventAggregator
                .GetEvent<PubSubEvent<ExcelMeetingDataRequest>>()
                .Publish(data);
        }

        public object GetNextViewModel(object currentViewModel)
        {
            switch (currentViewModel.GetType().Name)
            {
                case "MeetingWizardFirstViewModel":
                    return meetingWizardSecondVm;

                case "MeetingWizardSecondViewModel":
                    return meetingWizardLastVm;
            }
            return meetingWizardFirstVm;
        }

        public object GetPreviousViewModel(object currentViewModel)
        {
            return meetingWizardFirstVm;
        }
    }
}