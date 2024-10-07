namespace MVVM.DataGridPagination.Extensions
{
    public static class PostalCodeExtend
    {
        public static string Format(this string zipCode)
        {
            return $"〒{zipCode}";
        }
    }
}

//if (zipCode.Length == 7)
//{
//    return $"{zipCode.Substring(0, 3)}-{zipCode.Substring(3, 4)}";
//}