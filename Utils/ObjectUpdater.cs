namespace RestApiApp.Utils
{
    public static class ObjectUpdateAsyncr
    {
        public static void UpdateAsyncNonNullProperties<T>(T source, T tarGet)
        {
            var props = typeof(T).GetProperties()
                .Where(p => p.CanRead && p.CanWrite);

            foreach (var prop in props)
            {
                var value = prop.GetValue(source);
                var isDefault = value == null ||
                    (prop.PropertyType.IsValueType && value.Equals(Activator.CreateInstance(prop.PropertyType)));

                if (!isDefault)
                {
                    prop.SetValue(tarGet, value);
                }
            }
        }
    }
}