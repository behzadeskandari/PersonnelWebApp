using System.ComponentModel;

namespace Personnel.Api.Models
{
    public class BaseAdminModel
    {
        [DisplayName("شناسه")]
        public virtual int Id { get; set; }

        public virtual int CurrentUserId { get; set; }

        public virtual bool PermissionBtnAdd { get; set; }

        public virtual bool PermissionBtnEdit { get; set; }

        public virtual bool PermissionBtnDelete { get; set; }
    }
}
