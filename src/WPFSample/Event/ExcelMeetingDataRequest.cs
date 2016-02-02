using WPFSample.WPF.Data;

namespace WPFSample.WPF.Event
{
    public class ExcelMeetingDataRequest
    {
        public string SheetDestination { get; set; }

        public MeetingData Data { get; set; }
    }
}