using Microsoft.AspNetCore.Html;
using Personnel.Api.Components.Enums;
using Personnel.Application.Extensions;

namespace Personnel.Api.Components
{
    public class PersianCalendarComponentModel
    {
        public PersianCalendarComponentModel()
        {
            Days = new CalendarDays[42];
            for (int i = 0; i < 42; i++)
            {
                Days[i] = new CalendarDays();
            }

            CalendarData = new List<CalendarData>();

        }
        public int Year { get; set; }
        public int Month { get; set; }
        public int? ToDay { get; set; }
        public string MonthString => ((PersianMonth)Month).Description();
        public int NumberOfDaysInMonth { get; set; }
        public CalendarDays[] Days { get; set; }
        public List<CalendarData> CalendarData { get; set; }

        public static HtmlString GetDataHtml(List<CalendarData> data, int? day)
        {
            if (day == null) return new HtmlString("");
            var calendarDate = data.FirstOrDefault(x => x.DayNumber == day);
            if (calendarDate == null) return new HtmlString("");
            var htmlString = $"<span class='{calendarDate.DataClass}'>{calendarDate.Title}<span>";
            return new HtmlString(htmlString);

        }
    }

}
