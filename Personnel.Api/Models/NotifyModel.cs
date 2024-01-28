using Personnel.Api.Enums;

namespace Personnel.Api.Models
{
    public class NotifyModel
    {
        public NotifyType Type { get; set; }
        public string Message { get; set; }
    }
}
