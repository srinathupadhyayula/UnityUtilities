using System;

namespace Utilities.Conversion
{
    internal class TypeConverter<TSource, TDestination> : ITypeConverter<TSource, TDestination>
    {
        private readonly Func<TSource, TDestination> m_conversion;
        
        public TypeConverter(Func<TSource, TDestination> conversion)
        {
            m_conversion = conversion;
        }

        public TDestination Convert(object value)
        {
            return m_conversion((TSource) value);
        }

        public TDestination Convert(TSource value)
        {
            return m_conversion(value);
        }
    }

    internal static class TypeConversion<TSource, TDestination>
    {
        private static ITypeConverter<TSource, TDestination> s_converter;

        /// <summary>
        /// Registers a strongly typed converter
        /// </summary>
        /// <param name="converter"></param>
        /// <typeparam name="TSource"></typeparam>
        internal static void Register(ITypeConverter<TSource, TDestination> converter)
        {
            s_converter = converter;
        }
        
        /// <summary>
        /// Unregister the given type converter
        /// </summary>
        internal static void Unregister()
        {
            s_converter = null;
        }

        /// <summary>
        /// Fast conversion path where types are known ahead of time
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TDestination Convert(TSource value)
        {
            return s_converter.Convert(value);
        }
    }
}
