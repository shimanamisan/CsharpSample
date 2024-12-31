namespace MVVM.MultiWindowSample.Extensions
{
    public static class PostalCodeExtend
    {
        public static string Format(this string zipCode)
        {
            return $"〒{zipCode}";
        }
    }
}
