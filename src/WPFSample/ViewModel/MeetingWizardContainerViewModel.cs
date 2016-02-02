using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using WPFSample.WPF.Event;

namespace WPFSample.WPF.ViewModel
{
    public class MeetingWizardContainerViewModel : BindableBase
    {
        private readonly MeetingController controller;
        private readonly IEventAggregator eventAggregator;
        private readonly SubscriptionToken subscriptionToken;

        private object currentViewModel;
        private bool isBackBtnVisible;
        private bool isFinishBtnVisible;
        private bool isNextBtnVisible;

        public MeetingWizardContainerViewModel(IEventAggregator eventAggregator, MeetingController controller)
        {
            this.eventAggregator = eventAggregator;
            this.controller = controller;

            LoadCommands();

            CurrentViewModel = controller.GetNextViewModel(string.Empty);
            IsNextBtnVisible = true;
            IsBackBtnVisible = false;
            IsFinishBtnVisible = false;

            IsFinishButtonEnabled = false;
            IsNextButtonEnabled = true;

            subscriptionToken = eventAggregator
                .GetEvent<PubSubEvent<ButtonEnabled>>()
                .Subscribe(UpdateEnableForNextButton);
        }

        public bool IsNextButtonEnabled { get; set; }

        public bool IsBackButtonEnabled { get; set; }

        public object CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }


        public bool IsBackBtnVisible
        {
            get { return isBackBtnVisible; }
            set { SetProperty(ref isBackBtnVisible, value); }
        }

        public bool IsNextBtnVisible
        {
            get { return isNextBtnVisible; }
            set { SetProperty(ref isNextBtnVisible, value);}
        }

        public bool IsFinishBtnVisible
        {
            get { return isFinishBtnVisible; }
            set { SetProperty(ref isFinishBtnVisible, value);}
        }

        public ICommand BackCommand { get; private set; }

        public ICommand NextCommand { get; private set; }

        public ICommand FinishCommand { get; private set; }

        public bool IsFinishButtonEnabled { get; set; }

        private void LoadCommands()
        {
            BackCommand = new DelegateCommand(BackAction, () => IsBackButtonEnabled);
            NextCommand = new DelegateCommand(NextAction, () => IsNextButtonEnabled);
            FinishCommand = new DelegateCommand(FinishAction, () => IsFinishButtonEnabled);
        }

        private void FinishAction()
        {   
            Application.Current.MainWindow.Close();
        }

        private void UpdateEnableForNextButton(ButtonEnabled obj)
        {
            IsNextButtonEnabled = obj.IsNextButtonEnabled;
            IsFinishButtonEnabled = obj.IsFinishButtonEnabled;
            if (IsFinishButtonEnabled)
            {
                eventAggregator
                    .GetEvent<PubSubEvent<ButtonEnabled>>()
                    .Unsubscribe(subscriptionToken);
            }
            // Run on the UI Thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                ((DelegateCommand) FinishCommand).RaiseCanExecuteChanged();
                ((DelegateCommand) NextCommand).RaiseCanExecuteChanged();
            });
        }

        private void NextAction()
        {
            CurrentViewModel = controller.GetNextViewModel(CurrentViewModel);
            switch (CurrentViewModel.GetType().Name)
            {
                case "MeetingWizardSecondViewModel":
                    IsNextBtnVisible = true;
                    IsBackBtnVisible = true;
                    IsFinishBtnVisible = false;
                    IsNextButtonEnabled = false;
                    IsBackButtonEnabled = true;
                    break;

                case "MeetingWizardLastViewModel":
                    controller.SendData();
                    IsNextBtnVisible = false;
                    IsBackBtnVisible = false;
                    IsFinishBtnVisible = true;
                    break;
            }
            ((DelegateCommand)FinishCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)NextCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)BackCommand).RaiseCanExecuteChanged();
        }

        private void BackAction()
        {
            CurrentViewModel = controller.GetPreviousViewModel(currentViewModel);
            IsNextBtnVisible = true;
            IsBackBtnVisible = false;
            IsFinishBtnVisible = false;
            IsNextButtonEnabled = true;
            ((DelegateCommand)NextCommand).RaiseCanExecuteChanged();
        }
    }
}