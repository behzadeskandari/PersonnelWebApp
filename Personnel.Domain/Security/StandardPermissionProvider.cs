using MediatR;
using Personnel.Domain.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Domain.Security
{
    public partial class StandardPermissionProvider : IPermissionProvider
    {

        // نقش کاربران
        public static readonly PermissionRecord AddUserRole = new PermissionRecord { Name = "افزودن نقش کاربر", SystemName = "AddUserRole", Category = "Users" };
        public static readonly PermissionRecord UpdateUserRole = new PermissionRecord { Name = "ویرایش نقش کاربر", SystemName = "UpdateUserRole", Category = "Users" };
        public static readonly PermissionRecord DeleteUserRole = new PermissionRecord { Name = "حذف نقش کاربر", SystemName = "DeleteUserRole", Category = "Users" };
        public static readonly PermissionRecord UserRoleList = new PermissionRecord { Name = "لیست نقش کاربران", SystemName = "UserRoleList", Category = "Users" };

        // مکان کاربران
        public static readonly PermissionRecord AddUserLocation = new PermissionRecord { Name = "افزودن مکان کاربر", SystemName = "AddUserLocation", Category = "Users" };
        public static readonly PermissionRecord UpdateUserLocation = new PermissionRecord { Name = "ویرایش مکان کاربر", SystemName = "UpdateUserLocation", Category = "Users" };
        public static readonly PermissionRecord DeleteUserLocation = new PermissionRecord { Name = "حذف مکان کاربر", SystemName = "DeleteUserLocation", Category = "Users" };
        public static readonly PermissionRecord UserLocationList = new PermissionRecord { Name = "لیست مکان کاربران", SystemName = "UserLocationList", Category = "Users" };

        // کاربران
        public static readonly PermissionRecord ManageUsers = new PermissionRecord { Name = "مدیریت کاربر", SystemName = "ManageUsers", Category = "Users" };
        public static readonly PermissionRecord AddUser = new PermissionRecord { Name = "افزودن کاربر", SystemName = "AddUser", Category = "Users" };
        public static readonly PermissionRecord UpdateUser = new PermissionRecord { Name = "ویرایش کاربر", SystemName = "UpdateUser", Category = "Users" };
        public static readonly PermissionRecord DeleteUser = new PermissionRecord { Name = "حذف کاربر", SystemName = "DeleteUser", Category = "Users" };
        public static readonly PermissionRecord UserList = new PermissionRecord { Name = "لیست کاربران", SystemName = "UserList", Category = "Users" };
        public static readonly PermissionRecord UserChangePassword = new PermissionRecord { Name = "تغییر کلمه عبور کاربر", SystemName = "UserChangePassword", Category = "Users" };
        public static readonly PermissionRecord UserSendWelcomeMessage = new PermissionRecord { Name = "ارسال پیغام خوش آمدگویی به کاربر", SystemName = "UserSendWelcomeMessage", Category = "Users" };
        public static readonly PermissionRecord UserReSendActivationMessage = new PermissionRecord { Name = "ارسال دوباره پیغام فعال سازی حساب کاربر", SystemName = "UserReSendActivationMessage", Category = "Users" };
        public static readonly PermissionRecord UserSendEmailOrPm = new PermissionRecord { Name = "ارسال پیغام به کاربر از طریق ایمیل و پیامک", SystemName = "UserSendEmailOrPm", Category = "Users" };
        public static readonly PermissionRecord UserReport = new PermissionRecord { Name = "مشاهده گزارش کاربر", SystemName = "UserReport", Category = "Users" };
        public static readonly PermissionRecord ManagerOfUserManagment = new PermissionRecord { Name = "مدیریت قسمت مدیر کاربر", SystemName = "ManagerOfUserManagment", Category = "Users" };
        public static readonly PermissionRecord PersonelList = new PermissionRecord { Name = "لیست پرسنل", SystemName = "PersonelList", Category = "PersonelManagement" };
        public static readonly PermissionRecord PersonelEdit = new PermissionRecord { Name = "ویرایش پرسنل", SystemName = "PersonelEdit", Category = "PersonelManagement" };
        public static readonly PermissionRecord PersonelAdd = new PermissionRecord { Name = "ایجاد پرسنل", SystemName = "PersonelAdd", Category = "PersonelManagement" };


        public IEnumerable<DefaultPermissionRecord> GetDefaultPermissions()
        {
            return new[]
              {
                new DefaultPermissionRecord
                {
                    UserRoleSystemName = SystemUserRoleNames.Administrators,
                    PermissionRecords = new[]
                    {
                        AddUserLocation ,
                        UpdateUserLocation,
                        DeleteUserLocation,
                        UserLocationList ,
                        ManageUsers,
                        AddUser,
                        UpdateUser,
                        DeleteUser,
                        UserList,
                        UserChangePassword,
                        UserSendWelcomeMessage,
                        UserReSendActivationMessage,
                        UserSendEmailOrPm,
                        UserReport,
                        AddUserRole,
                        UpdateUserRole,
                        DeleteUserRole,
                        UserRoleList,
                        ManagerOfUserManagment,
                        PersonelList,
                        PersonelEdit,
                        PersonelAdd,

                    }
                }

            };
        }

        public IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[]
         {
             
                AddUserLocation ,
                UpdateUserLocation,
                DeleteUserLocation,
                UserLocationList ,
                ManageUsers,
                AddUser,
                UpdateUser,
                DeleteUser,
                UserList,
                UserChangePassword,
                UserSendWelcomeMessage,
                UserReSendActivationMessage,
                UserSendEmailOrPm,
                UserReport,
                AddUserRole,
                UpdateUserRole,
                DeleteUserRole,
                UserRoleList,
                ManagerOfUserManagment,
                PersonelList,
                PersonelEdit,
                PersonelAdd,
           };

        }
    }
}
