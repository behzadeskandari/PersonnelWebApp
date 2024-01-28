using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Personnel.Domain.Core.Interfaces;

namespace Personnel.Domain.Core.Extensions
{
    public static class CoreExtensions
    {
        public static int GetAge(this DateTime brithDate)
        {
            // Save today's date.
            var today = DateTime.Today;
            // Calculate the age.
            int age = today.Year - brithDate.Year;
            // Go back to the year the person was born in case of a leap year
            if (brithDate > today.AddYears(-age)) age--;
            return age;
        }

        public static bool IsNullOrDefault<T>(this T? value) where T : struct
        {
            return default(T).Equals(value.GetValueOrDefault());
        }

        public static string ToPersianNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            string[] persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (int j = 0; j < persian.Length; j++)
                input = input.Replace(j.ToString(), persian[j]);

            return input;
        }
        public static DateTime ConvertToGregorian(this DateTime obj)
        {
            GregorianCalendar gregorian = new GregorianCalendar();
            int y = gregorian.GetYear(obj);
            int m = gregorian.GetMonth(obj);
            int d = gregorian.GetDayOfMonth(obj);
            DateTime gregorianDate = new DateTime(y, m, d);
            var result = gregorianDate.ToString(CultureInfo.InvariantCulture);
            DateTime dt = Convert.ToDateTime(result);
            return dt;
        }



        public static SecureString ConvertStringToSecureString(this string data)
        {
            var secure = new SecureString();

            foreach (var character in data.ToCharArray())
                secure.AppendChar(character);

            secure.MakeReadOnly();
            return secure;

        }


        public static string Description(this Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // return description
            return attributes.Any() ? ((DescriptionAttribute)attributes.ElementAt(0)).Description : "Description Not Found";
        }

        public static DateTime ConvertToPersianDate(this DateTime obj)
        {
            PersianCalendar pc = new PersianCalendar();
            return DateTime.Parse(string.Format("{0}/{1}/{2} {3}:{4}:{5}", pc.GetYear(obj), pc.GetMonth(obj), pc.GetDayOfMonth(obj), pc.GetHour(obj), pc.GetMinute(obj), pc.GetSecond(obj)));
        }

        public static DateTime ConvertToPersianDate2(this DateTime obj)
        {
            PersianCalendar pc = new PersianCalendar();
            return DateTime.Parse(string.Format("{0}/{1}/{2}", pc.GetYear(obj), pc.GetMonth(obj), pc.GetDayOfMonth(obj)));
        }


        public static string ToPersianDate(this DateTime obj)
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime ex;
            if (DateTime.TryParse(string.Format("{0}/{1}/{2}", pc.GetYear(obj), pc.GetMonth(obj), pc.GetDayOfMonth(obj)), out ex))
                return ex.ToString("yyyy/MM/dd");

            return string.Format("{0}/{1}/{2}", pc.GetYear(obj), pc.GetMonth(obj), pc.GetDayOfMonth(obj));
        }

        public static string ToPersianFullDate(this DateTime obj)
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime ex;
            if (DateTime.TryParse(string.Format("{0}/{1}/{2} {3}:{4}:{5}", pc.GetYear(obj), pc.GetMonth(obj), pc.GetDayOfMonth(obj), pc.GetHour(obj), pc.GetMinute(obj), pc.GetSecond(obj)), out ex))
                return ex.ToString("yyyy/MM/dd-HH:mm", CultureInfo.CurrentCulture);
            return "";
        }

        public static string ToPersianFullDateAmPm(this DateTime obj)
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime ex;
            if (DateTime.TryParse(string.Format("{0}/{1}/{2} {3}:{4}:{5}", pc.GetYear(obj), pc.GetMonth(obj), pc.GetDayOfMonth(obj), pc.GetHour(obj), pc.GetMinute(obj), pc.GetSecond(obj)), out ex))
                return ex.ToString("yyyy/MM/dd-hh:mm tt", CultureInfo.CurrentCulture);
            return "";
        }

        public static int GetCurrentPersianYear(this DateTime obj)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(obj);
        }


        public static int GetCurrentPersianMonth(this DateTime obj)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetMonth(obj);
        }
        public static DateTime ConvertToPersianDateWithDetails(this DateTime obj)
        {
            PersianCalendar pc = new PersianCalendar();
            return DateTime.Parse(string.Format("{0}/{1}/{2} {3}:{4}:{5}",
                pc.GetYear(obj), pc.GetMonth(obj), pc.GetDayOfMonth(obj),
                pc.GetHour(obj), pc.GetMinute(obj), pc.GetSecond(obj)));
        }

        public static string WeekDayByDate(DateTime date)
        {

            switch (date.DayOfWeek)
            {
                case DayOfWeek.Friday:
                    return "جمعه";
                case DayOfWeek.Saturday:
                    return "شنبه";
                case DayOfWeek.Sunday:
                    return "یکشنبه";
                case DayOfWeek.Monday:
                    return "دوشنبه";
                case DayOfWeek.Tuesday:
                    return "سه شنبه";
                case DayOfWeek.Wednesday:
                    return "چهار شنبه";
                case DayOfWeek.Thursday:
                    return "پنج شنبه";
                default:
                    return "";
            }
        }



        public static string TimeAgo(this DateTime dateTime)
        {
            string result = string.Empty;
            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} ثانیه قبل", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("حدود {0} دقیقه قبل", timeSpan.Minutes) :
                    "حدود یک دقیقه قبل";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("حدود {0} ساعت قبل", timeSpan.Hours) :
                    "حدود یک ساعت قبل";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("حدود {0} روز قبل", timeSpan.Days) :
                    "دیروز";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("حدود {0} ماه قبل", timeSpan.Days / 30) :
                    "حدود یک ماه قبل";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("حدود {0} سال قبل", timeSpan.Days / 365) :
                    "حدود یک سال قبل";
            }

            return result;
        }

        public static string MonthTitle(this int m)
        {
            switch (m)
            {
                case 1: return "فروردین";
                case 2: return "اردیبهشت";
                case 3: return "خرداد";
                case 4: return "تیر";
                case 5: return "مرداد";
                case 6: return "شهریور";
                case 7: return "مهر";
                case 8: return "آبان";
                case 9: return "آذر";
                case 10: return "دی";
                case 11: return "بهمن";
                case 12: return "اسفند";
                default:
                    return "";
            }
        }

        public static DateTime PersianToGeregorian(string persianDate)
        {

            var persianDateYear = persianDate.Split('/')[0];
            int persianDateYearLenth = persianDateYear.Length;
            if (persianDateYearLenth < 4)
            {
                for (int i = 0; i < 4 - persianDateYearLenth; i++)
                {
                    persianDateYear = "0" + persianDateYear;
                }
            }
            var persianDateMount = persianDate.Split('/')[1];

            int persianDateMountLenth = persianDateMount.Length;
            if (persianDateMountLenth < 2)
            {
                for (int i = 0; i < 2 - persianDateMountLenth; i++)
                {
                    persianDateMount = "0" + persianDateMount;
                }
            }

            var persianDateDay = persianDate.Split('/')[2];
            int persianDateDayLenth = persianDateDay.Length;
            if (persianDateDayLenth <= 2)
            {
                for (int i = 0; i < 2 - persianDateDayLenth; i++)
                {
                    persianDateDay = "0" + persianDateDay;
                }
            }
            persianDate = persianDateYear + "/" + persianDateMount + "/" + persianDateDay;
            int year = Convert.ToInt32(persianDate.Substring(0, 4));
            int month = Convert.ToInt32(persianDate.Substring(5, 2));
            int day = Convert.ToInt32(persianDate.Substring(8, 2));
            DateTime georgianDateTime = new DateTime(year, month, day, new PersianCalendar());
            // string temp = georgianDateTime.Year + "/" + georgianDateTime.Month + "/" + georgianDateTime.Day;
            return georgianDateTime;
        }


        public static string ReviewAndModifyTheNationalCodeForLegal(string text)
        {
            if (text == null)
                return "";
            if (text.Trim() == "")
                return "";
            if (text.Length == 10)
                return text;
            int tempCount = text.Length;
            string result = "";
            int sub = (10 - tempCount);
            for (int j = 0; j < sub; j++)
            {
                text = "0" + text;
            }

            result = text;
            return result;
        }

        public static string ReviewAndModifyTheNationalCodeForReal(string text)
        {
            if (text == null)
                return "";
            if (text.Trim() == "")
                return "";
            if (text.Length == 10)
                return text;
            int tempCount = text.Length;
            string result = "";
            int sub = (10 - tempCount);
            for (int j = 0; j < sub; j++)
            {
                text = "0" + text;
            }

            result = text;
            return result;
        }


        public static DateTime? ShamsiToMiladiInMortgageSepahReport(string persianDate)
        {

            var persianDateYear = persianDate.Split('/')[0];
            int persianDateYearLenth = persianDateYear.Length;
            if (persianDateYearLenth < 4)
            {
                for (int i = 0; i < 4 - persianDateYearLenth; i++)
                {
                    persianDateYear = "0" + persianDateYear;
                }
            }
            var persianDateMount = persianDate.Split('/')[1];

            int persianDateMountLenth = persianDateMount.Length;
            if (persianDateMountLenth < 2)
            {
                for (int i = 0; i < 2 - persianDateMountLenth; i++)
                {
                    persianDateMount = "0" + persianDateMount;
                }
            }

            var persianDateDay = persianDate.Split('/')[2];
            int persianDateDayLenth = persianDateDay.Length;
            if (persianDateDayLenth <= 2)
            {
                for (int i = 0; i < 2 - persianDateDayLenth; i++)
                {
                    persianDateDay = "0" + persianDateDay;
                }
            }
            persianDate = persianDateYear + "/" + persianDateMount + "/" + persianDateDay;
            int year = Convert.ToInt32(persianDate.Substring(0, 4));
            int month = Convert.ToInt32(persianDate.Substring(5, 2));
            int day = Convert.ToInt32(persianDate.Substring(8, 2));
            DateTime georgianDateTime = new DateTime(year, month, day, new PersianCalendar());
            // string temp = georgianDateTime.Year + "/" + georgianDateTime.Month + "/" + georgianDateTime.Day;
            return georgianDateTime;
        }


        public static bool PhoneNumberValidation(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }
            var isValid = Regex.Match(phoneNumber, @"^0\d{2,3}\d{8}$").Success;
            if (!isValid)
                return false;
            return true;
        }

        public static bool DecimalNumberValidation(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return false;
            }
            decimal testNumber;
            if (decimal.TryParse(number, out testNumber))
                return true;
            return true;
        }

        public static bool IntNumberValidation(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return false;
            }

            int testNumber;
            if (int.TryParse(number, out testNumber))
                return true;
            return false;
        }
        public static bool ValidNationalCodeReal(this string nationalCode)
        {
            if (string.IsNullOrEmpty(nationalCode))
                return false;
            if (nationalCode.Length != 10)
                return false;
            try
            {
                char[] chArray = nationalCode.ToCharArray();
                int[] numArray = new int[chArray.Length];
                for (int i = 0; i < chArray.Length; i++)
                {
                    numArray[i] = (int)char.GetNumericValue(chArray[i]);
                }
                int num2 = numArray[9];
                switch (nationalCode)
                {
                    case "0000000000":
                    case "1111111111":
                    case "2222222222":
                    case "3333333333":
                    case "4444444444":
                    case "5555555555":
                    case "6666666666":
                    case "7777777777":
                    case "8888888888":
                    case "9999999999":
                        return false;
                }
                int num3 = ((((((((numArray[0] * 10) + (numArray[1] * 9)) + (numArray[2] * 8)) + (numArray[3] * 7)) + (numArray[4] * 6)) + (numArray[5] * 5)) + (numArray[6] * 4)) + (numArray[7] * 3)) + (numArray[8] * 2);
                int num4 = num3 - ((num3 / 11) * 11);
                if ((((num4 == 0) && (num2 == num4)) || ((num4 == 1) && (num2 == 1))) || ((num4 > 1) && (num2 == Math.Abs((int)(num4 - 11)))))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool ValidateMilitaryRetiredPersonelCode(this string personelCode)
        {
            if (string.IsNullOrEmpty(personelCode))
                return true;
            if (!(personelCode.Length == 9 || personelCode.Length == 13))
                return false;

            if (personelCode.Length == 13)
            {
                var lastNumbers = personelCode.Substring(11, 2);
                if (lastNumbers != "00")
                    return false;
            }
            var toVerify = personelCode.Length == 9 ? personelCode : personelCode.Substring(2, 9);
            try
            {
                char[] chArray = toVerify.ToCharArray();
                int[] numArray = new int[chArray.Length];

                for (int i = 0; i < chArray.Length; i++)
                {
                    numArray[i] = (int)char.GetNumericValue(chArray[i]);
                }
                int num2 = numArray[8];
                switch (toVerify)
                {
                    case "000000000":
                    case "111111111":
                    case "222222222":
                    case "333333333":
                    case "444444444":
                    case "555555555":
                    case "666666666":
                    case "777777777":
                    case "888888888":
                    case "999999999":
                        return false;
                }
                int num3 = ((((((((numArray[0] * 3) + (numArray[1] * 2)) + (numArray[2] * 7)) + (numArray[3] * 6)) + (numArray[4] * 5)) + (numArray[5] * 4)) + (numArray[6] * 3)) + (numArray[7] * 2));
                int num4 = num3 - ((num3 / 11) * 11);
                if (num4 == num2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ValidPostalCode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            var isValid = Regex.Match(value, @"^\d{10}$").Success;
            if (!isValid)
                return false;
            return true;
        }

        public static bool ValidateNationalEconomicIdentity(this string nationalCode)
        {
            if (string.IsNullOrEmpty(nationalCode))
                return true;
            if (nationalCode.Length > 11)
                return false;
            if (nationalCode.Length < 11)
                return false;
            if (!Regex.IsMatch(nationalCode, @"[0-9]+"))
                return false;
            string nationalCodeWithoutControlDigit = nationalCode.Substring(0, nationalCode.Length - 1);
            char controlDigit = nationalCode[10];
            char deci = nationalCode[9];
            int toInt = (int)char.GetNumericValue(deci) + 2;
            int[] multiplier = { 29, 27, 23, 19, 17, 29, 27, 23, 19, 17 };
            int sum = 0;
            for (int i = 0; i < nationalCodeWithoutControlDigit.Length; i++)
            {
                int temp = ((int)char.GetNumericValue(nationalCodeWithoutControlDigit[i]) + toInt) * multiplier[i];
                sum += temp;

            }

            int modBy11 = sum % 11;
            if (modBy11 == 10)
            {
                modBy11 = 0;
            }

            if (modBy11 == (int)char.GetNumericValue(controlDigit))
                return true;

            return false;
        }
        public static bool ValidateNationalCodeOrNationalEconomicIdentity(this string nationalCode)
        {
            return (ValidateNationalEconomicIdentity(nationalCode) || ValidNationalCodeReal(nationalCode));
        }
        public static DateTime? ShamsiToMiladi(string persianDate)
        {

            var persianDateYear = persianDate.Split('/')[0];
            int persianDateYearLenth = persianDateYear.Length;
            if (persianDateYearLenth < 4)
            {
                for (int i = 0; i < 4 - persianDateYearLenth; i++)
                {
                    persianDateYear = "0" + persianDateYear;
                }
            }
            var persianDateMount = persianDate.Split('/')[1];

            int persianDateMountLenth = persianDateMount.Length;
            if (persianDateMountLenth < 2)
            {
                for (int i = 0; i < 2 - persianDateMountLenth; i++)
                {
                    persianDateMount = "0" + persianDateMount;
                }
            }

            var persianDateDay = persianDate.Split('/')[2];
            int persianDateDayLenth = persianDateDay.Length;
            if (persianDateDayLenth <= 2)
            {
                for (int i = 0; i < 2 - persianDateDayLenth; i++)
                {
                    persianDateDay = "0" + persianDateDay;
                }
            }
            persianDate = persianDateYear + "/" + persianDateMount + "/" + persianDateDay;
            int year = Convert.ToInt32(persianDate.Substring(0, 4));
            int month = Convert.ToInt32(persianDate.Substring(5, 2));
            int day = Convert.ToInt32(persianDate.Substring(8, 2));
            DateTime georgianDateTime = new DateTime(year, month, day, new PersianCalendar());
            // string temp = georgianDateTime.Year + "/" + georgianDateTime.Month + "/" + georgianDateTime.Day;
            return georgianDateTime;
        }


        public static bool ValidMobile(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            if (value[0] != '0')
            {
                return false;
            }
            var isValid = Regex.Match(value, @"^(0)?9\d{9}$").Success;
            if (!isValid)
                return false;
            return true;
        }


        public static bool ValidateMobileRegex(this string mobileNumber)
        {
            return Regex.IsMatch(mobileNumber, @"^(09)[01239][0-9]\d{7}");
        }

        public static bool ValidHourAndMinutes(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            var isValid = Regex.Match(value, @"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$").Success;
            if (!isValid)
                return false;
            return true;
        }

        public static int GetHourAndMinutesFormatAndReturnMinutes(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            var isValid = Regex.Match(value, @"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$").Success;
            if (!isValid)
                return 0;
            string[] times = value.Split(':');
            int hourTemp = Convert.ToInt32(times[0]);
            int minuteTemp = Convert.ToInt32(times[1]);
            int minute = hourTemp * 60;
            minute += minuteTemp;
            return minute;
        }

        public static bool ComPareTwoTimeInHourAndMinutesFormat(string valueFrom, string valueTo)
        {
            if (string.IsNullOrEmpty(valueFrom))
            {
                return false;
            }
            if (string.IsNullOrEmpty(valueTo))
            {
                return false;
            }
            var isValidValueFrom = Regex.Match(valueFrom, @"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$").Success;
            if (!isValidValueFrom)
                return false;

            var isValidValueTo = Regex.Match(valueFrom, @"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$").Success;
            if (!isValidValueTo)
                return false;

            string[] from = valueFrom.Split(':');
            int hourFromTemp = Convert.ToInt32(from[0]);
            int minuteFromTemp = Convert.ToInt32(from[1]);

            string[] to = valueTo.Split(':');
            int hourToTemp = Convert.ToInt32(to[0]);
            int minuteToTemp = Convert.ToInt32(to[1]);

            if (hourFromTemp > hourToTemp)
            {
                return false;
            }
            if (hourFromTemp == hourToTemp)
            {
                if (minuteFromTemp > minuteToTemp)
                {
                    return false;
                }
            }

            return true;
        }


        public static string GetMinutesAndReturnHourAndMinutesFormat(this int value)
        {
            if (value == 0)
            {
                return "";
            }

            int hours = value / 60;
            int minutes = value % 60;
            string hourString = hours.ToString();
            if (hours < 10)
            {
                hourString = "0" + hourString;
            }
            string minutesString = minutes.ToString();
            if (minutes < 10)
            {
                minutesString = "0" + minutesString;
            }
            return hourString + ":" + minutesString;

        }

        public static string GetTimeDurationInMinutesAndReturnFormatString(string duration)
        {
            if (duration != "0")
            {
                TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(duration));
                if (Convert.ToInt32(duration) <= 60)
                {
                    return string.Format("{0:D2} ثانیه ",
                        t.Seconds);
                }
                if (Convert.ToInt32(duration) >= 60 && Convert.ToInt32(duration) <= 3600)
                {
                    return string.Format("{0:D2}دقیقه و {1:D2} ثانیه",
                        t.Minutes,
                        t.Seconds);
                }
                return string.Format("{0:D2} ساعت و {1:D2}دقیقه و {2:D2} ثانیه",
                       t.Hours,
                       t.Minutes,
                       t.Seconds);
            }
            return "";

        }

        public static string CreatePasswordForWhistle()
        {
            Random random = new Random();
            string[] st = new string[7];
            const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 7)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static bool EmailValidation(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            var isNumberIsValid = Regex.Match(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$").Success;
            if (!isNumberIsValid)
                return false;
            return true;
        }

        public static bool BankCardNumberValidation(string cardNumber)
        {
            cardNumber = cardNumber.Replace("-", "");

            var array = cardNumber.Select(ch => ch - '0').ToArray();

            int sum = 0, result = 0;
            for (int i = 0; i < array.Length; i++)
            {

                if ((i + 1) % 2 == 0)
                {
                    result = array[i] * 1;
                }
                else
                {
                    result = array[i] * 2;
                }
                result = result > 9 ? result - 9 : result;

                sum += result;
            }

            if (sum % 10 == 0)
            {
                return true;
            }
            return false;

        }



        public static string PassTime(this DateTime value)
        {
            DateTime dtNow = DateTime.Now;

            TimeSpan dt = (dtNow - value);


            string Text = "در ";

            if (dt.Days > 0)
            {
                Text += dt.Days + "روز ، ";
            }
            if (dt.Hours > 0)
            {
                Text += dt.Hours + "ساعت ، ";
            }

            if (dt.Minutes > 0)
            {
                Text += dt.Minutes + "دقیقه  ";
            }
            Text += " قبل";
            return Text;

        }

        

        public static bool ValidateImageAttachment(string contectType)
        {
            if (string.IsNullOrEmpty(contectType))
            {
                return false;
            }
            if (!string.Equals(contectType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(contectType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(contectType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(contectType, "image/gif", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(contectType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(contectType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }

        public static byte[] ConvertStreamToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static string GetSettingKey<T, TPropType>(this T entity,
            Expression<Func<T, TPropType>> keySelector)
            where T : ISettings, new()
        {
            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                       "Expression '{0}' refers to a field, not a property.",
                       keySelector));
            }

            var key = typeof(T).Name + "." + propInfo.Name;
            return key;
        }

    }


    public class StringConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return JsonConvert.DeserializeObject<T>((string)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

    }

}
