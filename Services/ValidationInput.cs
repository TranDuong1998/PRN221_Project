using System.Text.RegularExpressions;

namespace PRN211_Project.Services
{
    public static class ValidationInput
    {
        public static bool ValidateAnyCode(string name)
        {
            string pattern = @"^([a-zA-Z]){1,10}\d{2,5}$";

            Regex regex = new Regex(pattern);

            if (name == null)
                return false;
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            return regex.IsMatch(name);
        }

        public static bool ValidateTimeSlot(string timeSlot)
        {
            string pattern = @"^([aApP]){1}\d{2}$";

            Regex regex = new Regex(pattern);

            if (timeSlot == null) return false;
            if (timeSlot.Length < 0 || timeSlot.Length > 3) return false;

            return regex.IsMatch(timeSlot);
        }

    }
}
