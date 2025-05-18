using System.Globalization;

namespace MockPars.Application.Extention
{
    public static class DateExtension
    {
        public static string ToShamsi(this DateTime date)
        {
            var prCalender = new PersianCalendar();
            return prCalender.GetYear(date).ToString("00") + "/"
                + prCalender.GetMonth(date).ToString("00") + "/" + prCalender.GetDayOfMonth(date).ToString("00");
        }
        public static string ToShamsi(this DateTime? date)
        {
            var prCalender = new PersianCalendar();
            return prCalender.GetYear(date.Value).ToString("00") + "/"
                + prCalender.GetMonth(date.Value).ToString("00") + "/" + prCalender.GetDayOfMonth(date.Value).ToString("00");
        }
        public static DateTime ToMiladi(this string persianDate)
        {
            string[] persianDateText = persianDate.Split('/');

            PersianCalendar persianCalendar = new PersianCalendar();
            var convertedDateResult = new DateTime(int.Parse(persianDateText[0]), int.Parse(persianDateText[1]), int.Parse(persianDateText[2]), persianCalendar);
            return convertedDateResult;
        }

    }
}
