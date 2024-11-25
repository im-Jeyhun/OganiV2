namespace OganiWebApp.Extensions
{
    public  static class IsNullOrValueExtension
    {
        public static bool IsNullOrDefault<T>(T value)
        {
            return object.Equals(value, default(T));
        }
    }
}
