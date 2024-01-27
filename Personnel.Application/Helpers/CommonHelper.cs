using Personnel.Domain.Dtos;
using Personnel.Domain.Entities;
using Personnel.Infra.Data.CrossCutting.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Personnel.Application.Helpers
{
    public static class CommonHelper
    {
        //public static string BaseDirectory { get; set; }

        public static bool IsValidNationalCode(string nationalCode)
        {
            //در صورتی که کد ملی وارد شده تهی باشد

            if (string.IsNullOrEmpty(nationalCode))
                return false;
            // throw new Exception("لطفا کد ملی را صحیح وارد نمایید");


            //در صورتی که کد ملی وارد شده طولش کمتر از 10 رقم باشد
            if (nationalCode.Length != 10)
                return false;
            //throw new Exception("طول کد ملی باید ده کاراکتر باشد");

            //در صورتی که کد ملی ده رقم عددی نباشد
            var regex = new Regex(@"\d{10}");
            if (!regex.IsMatch(nationalCode))
                return false;
            //throw new Exception("کد ملی تشکیل شده از ده رقم عددی می‌باشد؛ لطفا کد ملی را صحیح وارد نمایید");

            //در صورتی که رقم‌های کد ملی وارد شده یکسان باشد
            var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999", "0123456789", "9876543210" };
            if (allDigitEqual.Contains(nationalCode)) return false;


            //عملیات شرح داده شده در بالا
            var chArray = nationalCode.ToCharArray();
            var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
            var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
            var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
            var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
            var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
            var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
            var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
            var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
            var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
            var a = Convert.ToInt32(chArray[9].ToString());

            var b = (((((((num0 + num2) + num3) + num4) + num5) + num6) + num7) + num8) + num9;
            var c = b % 11;

            return (((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a)));
        }

        public static bool IsValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
            return result;
        }

        public static bool IsValidMobile(string mobile)
        {
            if (String.IsNullOrEmpty(mobile))
                return false;
            mobile = mobile.Trim();
            var result = Regex.IsMatch(mobile, "^09[0|1|2|3][0-9]{8}$", RegexOptions.IgnoreCase);
            return result;
        }


        public static string OrderByQuery(this string queryString, PageFilterDto command)
        {

            if (command.Sort.Any())
            {
                var s = command.Sort.First();

                if (s.Dir == "asc")
                    queryString += " order by " + s.Field + " ";
                else
                    queryString += " order by " + s.Field + " desc ";
            }
            else
            {
                queryString += " order by Id desc ";
            }

            queryString += "OFFSET " + (command.Page - 1) * command.PageSize + " ROWS ";
            queryString += "FETCH NEXT " + command.PageSize + " ROWS ONLY";
            return queryString;
        }

        public static IQueryable<TEntity> ToPagination<TEntity>(this IQueryable<TEntity> query, int page, int count) where TEntity : BaseEntityDto
        {
            return page == 1 ? query.Take(count) : query.Skip((page - 1) * count).Take(count);
        }

        public static IQueryable<TEntity> ApplySort<TEntity>(this IQueryable<TEntity> query, string orderByQueryString) where TEntity : BaseEntityDto
        {
            //if (!query.Any()) // this is very bad idea! Any() executes query...!
            //    return query;

            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                query = query.OrderBy(x => x.Id);
                return query;
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                query = query.OrderBy(x => x.Id);
                return query;
            }
            return query.OrderBy(a => orderQuery);
            //query= query.OrderBy(orderQuery);
        }


        public static IQueryable<T> OrderByCommand<T>(this IQueryable<T> query, PageFilterDto command)
            where T : BaseEntity
        {
            if (command.Sort.Any())
            {
                var s = command.Sort.First();
                try
                {
                    if (s.Dir == "asc")
                        return query.OrderBy(s.Field);
                    else
                        return query.OrderByDescending(s.Field);
                }
                catch (Exception)
                {
                    return query.OrderByDescending(c => c.Id);
                }
            }
            else
            {
                return query.OrderByDescending(c => c.Id);
            }
        }

        public static IQueryable<T> DtoOrderByCommand<T>(this IQueryable<T> query, PageFilterDto command)
            where T : BaseEntityDto
        {
            if (command.Sort.Any())
            {
                var s = command.Sort.First();
                try
                {
                    if (string.IsNullOrEmpty(s.Field))
                        return query.OrderByDescending(c => c.Id);

                    if (s.Dir == "asc")
                        return query.OrderBy(s.Field);
                    else
                        return query.OrderByDescending(s.Field);
                }
                catch (Exception)
                {
                    return query.OrderByDescending(c => c.Id);
                }
            }
            else
            {
                return query.OrderByDescending(c => c.Id);
            }
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderBy(ToLambda<T>(propertyName));
        }

        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(ToLambda<T>(propertyName));
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

        public static int CookieAuthExpires { get { return 60; } }

        public static string MapPath(string path)
        {
            //not hosted. For example, run in unit tests
            path = path.Replace("~/", "").TrimStart('/').Replace("/", "\\");
            return Path.Combine(Directory.GetCurrentDirectory(), path);
        }
        /// <summary>
        /// اختلاف بین 2 جمله
        /// </summary>
        /// <param name="text1"></param>
        /// <param name="text2"></param>
        /// <returns></returns>
        public static int CountOfDifferencebetweenWords(string text1, string text2)
        {
            string s1 = Regex.Replace(text1, @"\s", "");
            string s2 = Regex.Replace(text2, @"\s", "");

            s1 = Regex.Replace(s1, @"\s", "");
            s2 = Regex.Replace(s2, @"\s", "");

            int ch = s1.Count();
            int ch2 = s2.Count();
            char[] c1 = s1.ToCharArray();
            char[] c2 = s2.ToCharArray();
            char[] r1;
            char[] r2;
            int result;
            bool flag = false;
            for (int i = 0; i < c1.Length; i++)
            {
                for (int j = 0; j < c2.Length; j++)
                {
                    var x = c1[i];
                    var y = c2[j];
                    if (c1[i] == c2[j])
                    {

                        c1 = c1.RemoveAt(i);
                        c2 = c2.RemoveAt(j);
                        flag = true;
                        i--;
                        break;
                    }
                }
            }
            if (flag == true)
            {
                result = c1.Count() + c2.Count();
            }
            else
            {
                result = c1.Count() + c2.Count();
            }
            return result;
        }


        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            T[] dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }

        public static TypeConverter GetPortalCustomTypeConverter(Type type)
        {
            //we can't use the following code in order to register our custom type descriptors
            //TypeDescriptor.AddAttributes(typeof(List<int>), new TypeConverterAttribute(typeof(GenericListTypeConverter<int>)));
            //so we do it manually here

            if (type == typeof(List<int>))
                return new GenericListTypeConverter<int>();
            if (type == typeof(List<decimal>))
                return new GenericListTypeConverter<decimal>();
            if (type == typeof(List<string>))
                return new GenericListTypeConverter<string>();


            return TypeDescriptor.GetConverter(type);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value != null)
            {
                var sourceType = value.GetType();

                TypeConverter destinationConverter = GetPortalCustomTypeConverter(destinationType);
                TypeConverter sourceConverter = GetPortalCustomTypeConverter(sourceType);
                if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
                    return destinationConverter.ConvertFrom(null, culture, value);
                if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
                    return sourceConverter.ConvertTo(null, culture, value, destinationType);
                if (destinationType.IsEnum && value is int)
                    return Enum.ToObject(destinationType, (int)value);
                if (!destinationType.IsInstanceOfType(value))
                    return Convert.ChangeType(value, destinationType, culture);
            }
            return value;
        }

        public static T To<T>(object value)
        {
            //return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            return (T)To(value, typeof(T));
        }


    }

}
