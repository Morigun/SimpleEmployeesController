using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEmployeesController
{
    public static class Extensions
    {
        /// <summary>
        /// Получение аттрибута у свойства по типу, если не найдет вернет null
        /// </summary>
        /// <typeparam name="T">Тип аттрибута для поиска</typeparam>
        /// <param name="property">значение аттрибута</param>
        /// <returns></returns>
        public static T? GetAttribute<T>(this PropertyInfo property) where T : Attribute
        {
            if (property == null)
                return null;
            var attribute = property.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            return (T?)attribute;
        }
        public static string? ToFirstUpLetter(this string? word)
        {
            if (string.IsNullOrEmpty(word)) return word;
            if (word.Length == 1) return word.ToUpper();
            return $"{word.Substring(0, 1).ToUpperInvariant()}{word.Substring(1).ToLowerInvariant()}";
        }
    }
}
