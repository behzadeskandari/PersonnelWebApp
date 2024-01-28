using Azure.Core;
using MediatR;
using Personnel.Api.Components.Enums;
using Personnel.Domain.Core.Extensions;

namespace Personnel.Api.Components.PersianCalendarComponent
{
    public class PersianCalendarComponentViewModel
    {
        public PersianCalendarComponentViewModel()
        {

        }
        public PersianCalendarComponentViewModel(int year, int month, int request)
        {
            Year = year;
            Month = month;
            Request = request;
        }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Request { get; set; }
        public string MonthString => ((PersianMonth)Month).Description();
    }
}
