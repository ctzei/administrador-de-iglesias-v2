using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Reflection;
using System.IO;
using System.Linq.Expressions;
using System.Xml;
using Newtonsoft.Json;

namespace ExtensionMethods
{
    public static class CoreExtensions
    {
        #region DateTime
        /// <summary>Get the week number of a certain date, provided that
        /// the first day of the week is Monday, the first week of a year
        /// is the one that includes the first Thursday of that year and
        /// the last week of a year is the one that immediately precedes
        /// the first calendar week of the next year.
        /// </summary>
        /// <param name="date">Date of interest.</param>
        /// <returns>The week number.</returns>
        public static int GetWeekNumber(this DateTime date)
        {
            //Constants
            const int JAN = 1;
            const int DEC = 12;
            const int LASTDAYOFDEC = 31;
            const int FIRSTDAYOFJAN = 1;
            const int THURSDAY = 4;
            bool thursdayFlag = false;

            //Get the day number since the beginning of the year
            int dayOfYear = date.DayOfYear;

            //Get the first and last weekday of the year
            int startWeekDayOfYear = (int)(new DateTime(date.Year, JAN, FIRSTDAYOFJAN)).DayOfWeek;
            int endWeekDayOfYear = (int)(new DateTime(date.Year, DEC, LASTDAYOFDEC)).DayOfWeek;

            //Compensate for using monday as the first day of the week
            if (startWeekDayOfYear == 0)
                startWeekDayOfYear = 7;
            if (endWeekDayOfYear == 0)
                endWeekDayOfYear = 7;

            //Calculate the number of days in the first week
            int daysInFirstWeek = 8 - (startWeekDayOfYear);

            //Year starting and ending on a thursday will have 53 weeks
            if (startWeekDayOfYear == THURSDAY || endWeekDayOfYear == THURSDAY)
                thursdayFlag = true;

            //We begin by calculating the number of FULL weeks between
            //the year start and our date. The number is rounded up so
            //the smallest possible value is 0.
            int fullWeeks = (int)Math.Ceiling((dayOfYear - (daysInFirstWeek)) / 7.0);
            int resultWeekNumber = fullWeeks;

            //If the first week of the year has at least four days, the
            //actual week number for our date can be incremented by one.
            if (daysInFirstWeek >= THURSDAY)
                resultWeekNumber = resultWeekNumber + 1;

            //If the week number is larger than 52 (and the year doesn't
            //start or end on a thursday), the correct week number is 1.
            if (resultWeekNumber > 52 && !thursdayFlag)
                resultWeekNumber = 1;

            //If the week number is still 0, it means that we are trying
            //to evaluate the week number for a week that belongs to the
            //previous year (since it has 3 days or less in this year).
            //We therefore execute this function recursively, using the
            //last day of the previous year.
            if (resultWeekNumber == 0)
                resultWeekNumber = GetWeekNumber(new DateTime(date.Year - 1, DEC, LASTDAYOFDEC));
            return resultWeekNumber;
        }

        /// <summary>
        /// Get the first date of the week for a certain date, provided
        /// that the first day of the week is Monday, the first week of
        /// a year is the one that includes the first Thursday of that
        /// year and the last week of a year is the one that immediately
        /// precedes the first calendar week of the next year.
        /// </summary>
        /// <param name="date">ISO 8601 date of interest.</param>
        /// <returns>The first week date.</returns>
        public static DateTime GetFirstDateOfWeek(this DateTime date)
        {
            if (date == DateTime.MinValue)
                return date;

            int week = date.GetWeekNumber();
            while (week == date.GetWeekNumber())
                date = date.AddDays(-1);
            return date.AddDays(1).Date;
        }

        /// <summary>
        /// Get the last date of the week for a certain date, provided
        /// that the first day of the week is Monday, the first week of
        /// a year is the one that includes the first Thursday of that
        /// year and the last week of a year is the one that immediately
        /// precedes the first calendar week of the next year.
        /// </summary>
        /// <param name="date">ISO 8601 date of interest.</param>
        /// <returns>The first week date.</returns>
        public static DateTime GetLastDateOfWeek(this DateTime date)
        {
            if (date == DateTime.MaxValue)
                return date;

            int week = date.GetWeekNumber();
            while (week == date.GetWeekNumber())
                date = date.AddDays(1);
            return date.AddDays(-1).Date;
        }

        public static string ToFullDateString(this DateTime date)
        {
            return date.ToString("dd/MMM/yyyy");
        }

        public static DateTime PreviousFriday(this DateTime date)
        {
            return date.AddDays(-((int)date.DayOfWeek) + 2);
        }

        public static DateTime PreviousSaturday(this DateTime date)
        {
            return date.AddDays(-((int)date.DayOfWeek) + 1);
        }

        public static DateTime PreviousSunday(this DateTime date)
        {
            return date.AddDays(-((int)date.DayOfWeek));
        }

        #endregion

        #region MailMessage
        public static void Save(this MailMessage Message, string FileName)
        {
            Assembly assembly = typeof(SmtpClient).Assembly;
            Type _mailWriterType =
              assembly.GetType("System.Net.Mail.MailWriter");

            using (FileStream _fileStream =
                   new FileStream(FileName, FileMode.Create))
            {
                // Get reflection info for MailWriter contructor
                ConstructorInfo _mailWriterContructor =
                    _mailWriterType.GetConstructor(
                        BindingFlags.Instance | BindingFlags.NonPublic,
                        null,
                        new Type[] { typeof(Stream) },
                        null);

                // Construct MailWriter object with our FileStream
                object _mailWriter =
                  _mailWriterContructor.Invoke(new object[] { _fileStream });

                // Get reflection info for Send() method on MailMessage
                MethodInfo _sendMethod =
                    typeof(MailMessage).GetMethod(
                        "Send",
                        BindingFlags.Instance | BindingFlags.NonPublic);

                // Call method passing in MailWriter
                _sendMethod.Invoke(
                    Message,
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new object[] { _mailWriter, true },
                    null);

                // Finally get reflection info for Close() method on our MailWriter
                MethodInfo _closeMethod =
                    _mailWriter.GetType().GetMethod(
                        "Close",
                        BindingFlags.Instance | BindingFlags.NonPublic);

                // Call close method
                _closeMethod.Invoke(
                    _mailWriter,
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new object[] { },
                    null);
            }
        }
        #endregion

        #region Object

        public static string ToJson(this object obj)
        {
            string json = JsonConvert.SerializeObject(obj, new Newtonsoft.Json.Converters.IsoDateTimeConverter());
            return (json.StartsWith("(") && json.EndsWith(")") ? json : "(" + json + ")");
        }

        #endregion

        #region String

        public static bool ToBool(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Trim().ToLowerInvariant();
                if ((str.Equals("yes", StringComparison.OrdinalIgnoreCase)) || (str.Equals("true", StringComparison.OrdinalIgnoreCase)) || (str.Equals("1", StringComparison.OrdinalIgnoreCase)))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public static int ToInt(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                int rtn;
                if (int.TryParse(str, out rtn))
                {
                    return rtn;
                }
                else
                {
                    return 0;
                }
            }
            else
                return 0;
        }

        #endregion
    }
}
