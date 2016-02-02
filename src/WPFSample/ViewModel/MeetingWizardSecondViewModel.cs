using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Prism.Events;
using Prism.Mvvm;
using WPFSample.WPF.Data;
using WPFSample.WPF.Event;

namespace WPFSample.WPF.ViewModel
{
    public class MeetingWizardSecondViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private DateTime dateOfMeeting;
        private CustomTime endingTime;
        private IList<CustomTime> endTimes;
        private string name;

        private string selectedSheet;
        private CustomTime startingTime;

        private IList<CustomTime> startTimes;
        private readonly SubscriptionToken worksheetNamesToken;


        public MeetingWizardSecondViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            worksheetNamesToken = eventAggregator
                .GetEvent<PubSubEvent<ExcelWorksheetNamesResponse>>()
                .Subscribe(GetListWorksheetFromExcel);

            eventAggregator.GetEvent<PubSubEvent<ExcelWorksheetNamesRequest>>()
                .Publish(new ExcelWorksheetNamesRequest());

            LoadTimes();
            DateOfMeeting = DateTime.Now;
        }

        private void GetListWorksheetFromExcel(ExcelWorksheetNamesResponse obj)
        {
           eventAggregator.GetEvent<PubSubEvent<ExcelWorksheetNamesResponse>>()
                .Unsubscribe(worksheetNamesToken);

            WorksheetName = new ObservableCollection<string>(obj.SheetNames);

            SelectedSheet = WorksheetName.First();
        }


        public ObservableCollection<CustomTime> StartTimes
                    => new ObservableCollection<CustomTime>(startTimes);

        public ObservableCollection<CustomTime> EndTimes
                    => new ObservableCollection<CustomTime>(endTimes);

        [Required]
        [MaxLength(40)]
        [MinLength(5)]
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                SendMessageThatChangedOccured();
                ValidateProperty(nameof(Name), value);
                SetProperty(ref name, value);
                SendMessageIfEverythingIsOkay();
            }
        }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime DateOfMeeting
        {
            get { return dateOfMeeting; }
            set
            {
                if (dateOfMeeting == value) return;

                ValidateProperty(nameof(DateOfMeeting), value);
                SetProperty(ref dateOfMeeting
                    , value.Date < DateTime.Now.Date
                        ? DateTime.Now
                        : value);
                LoadTimes();
                StartingTime = dateOfMeeting.Date == DateTime.Now.Date
                    ? GetNextAvailableTime(DateTime.Now.Hour, DateTime.Now.Minute, StartTimes)
                    : GetNextAvailableTime(08, 00, startTimes);
            }
        }


        [DataType(DataType.DateTime)]
        [Required]
        public CustomTime StartingTime
        {
            get { return startingTime; }
            set
            {
                if (startingTime == value) return;

                ValidateProperty(nameof(StartingTime), value);
                SetProperty(ref startingTime, value);
                var tmpValue = startingTime.CurrentTime.AddHours(1);
                EndingTime = GetNextAvailableTime(tmpValue.Hour, tmpValue.Minute, endTimes);
            }
        }

        [DataType(DataType.DateTime)]
        [Required]
        public CustomTime EndingTime
        {
            get { return endingTime; }
            set
            {
                if (endingTime == value) return;

                ValidateProperty(nameof(EndingTime), value);
                SetProperty(ref endingTime, value);
            }
        }

        public ObservableCollection<string> WorksheetName { get; set; }

        [Required]
        public string SelectedSheet
        {
            get { return selectedSheet; }
            set
            {
                if (selectedSheet == value) return;
                ValidateProperty(nameof(SelectedSheet), value);
                SetProperty(ref selectedSheet, value);
            }
        }

        public ExcelMeetingDataRequest GetData()
        {
            return new ExcelMeetingDataRequest
            {
                SheetDestination = selectedSheet,
                Data = new MeetingData
                {
                    Name = name,
                    Date = DateOfMeeting.Date,
                    StartingTime = startingTime.CurrentTime,
                    EndTime = endingTime.CurrentTime,
                },
            };
        }

        private void SendMessageIfEverythingIsOkay()
        {
            var isOk = name != null && name.Length > 4 && name.Length < 40;

            eventAggregator.GetEvent<PubSubEvent<ButtonEnabled>>()
                .Publish(new ButtonEnabled { IsNextButtonEnabled = isOk });
        }

        public void SendMessageThatChangedOccured()
        {
            eventAggregator.GetEvent<PubSubEvent<ButtonEnabled>>()
                .Publish(new ButtonEnabled { IsNextButtonEnabled = false });
        }

        private void LoadTimes()
        {
            startTimes = new List<CustomTime>();
            endTimes = new List<CustomTime>();
            for (var h = 8; h < 18; h++)
            {
                for (var m = 0; m < 4; m++)
                {
                    var t = new CustomTime(dateOfMeeting, h, m * 15);
                    startTimes.Add(t);

                    t = new CustomTime(dateOfMeeting, h + 1, m * 15);
                    endTimes.Add(t);
                }
            }
            OnPropertyChanged(nameof(StartTimes));
            OnPropertyChanged(nameof(EndTimes));
        }

        private CustomTime GetNextAvailableTime(int hour, int minute, IList<CustomTime> times)
        {
            var adjustedMinute = minute - (minute % 15);
            adjustedMinute = adjustedMinute > 0 ? adjustedMinute - 1 : adjustedMinute;
            var baseTime = new CustomTime(DateOfMeeting, hour, adjustedMinute);
            var list = times.Where(t => t.CurrentTime > baseTime.CurrentTime)
                .Select(t => t).ToList();
            return list.Any() ? list[0] : times.First();
        }

        protected virtual void ValidateProperty(string propertyName, object value)
        {
            var context = new ValidationContext(this, null, null)
            { MemberName = propertyName };
            Validator.ValidateProperty(value, context);
        }
    }
}