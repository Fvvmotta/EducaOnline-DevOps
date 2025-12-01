namespace EducaOnline.Core.Utils
{
    public static class StringUtils
    {
        public static string ApenasNumeros(this string str, string reference)
        {
            return reference.ApenasNumeros();
        }

        public static string ApenasNumeros(this string str)
        {
            return new string(str.Where(char.IsDigit).ToArray());
        }
    }
}