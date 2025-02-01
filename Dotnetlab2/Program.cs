using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabWork
{
    public static class Extensions
    {
        public static T EnsureNotNull<T>(this T value, string paramName, string message = null)
        {
            if (value == null)
                throw new ArgumentNullException(paramName, message ?? $"{paramName} не може бути null.");
            return value;
        }

        public static string ReverseString(this string input)
        {
            input.EnsureNotNull(nameof(input), "Рядок не може бути null.");
            return new string(input.Reverse().ToArray());
        }

        public static int CountOccurrences<T>(this IEnumerable<T> collection, T value) where T : IEquatable<T>
        {
            collection.EnsureNotNull(nameof(collection), "Колекція не може бути null.");
            return collection.Count(item => item.Equals(value));
        }

        public static T[] GetUniqueElements<T>(this T[] array) where T : IEquatable<T>
        {
            array.EnsureNotNull(nameof(array), "Масив не може бути null.");
            return array.Distinct().ToArray();
        }
    }

    public class ExtendedDictionaryElement<T, U, V>
    {
        public T Key { get; }
        public U Value1 { get; }
        public V Value2 { get; }

        public ExtendedDictionaryElement(T key, U value1, V value2)
        {
            Key = key.EnsureNotNull(nameof(key), "Ключ не може бути null.");
            Value1 = value1;
            Value2 = value2;
        }
    }

    public class ExtendedDictionary<T, U, V> : IEnumerable<ExtendedDictionaryElement<T, U, V>>
    {
        private readonly Dictionary<T, ExtendedDictionaryElement<T, U, V>> _dictionary = new();

        public void Add(ExtendedDictionaryElement<T, U, V> element)
        {
            element.EnsureNotNull(nameof(element), "Елемент не може бути null.");
            if (_dictionary.ContainsKey(element.Key)) throw new ArgumentException("Ключ вже існує.");

            _dictionary[element.Key] = element;
        }

        public bool Remove(T key) => _dictionary.Remove(key);

        public bool ContainsKey(T key) => _dictionary.ContainsKey(key);

        public bool ContainsValue(U value1, V value2)
        {
            return _dictionary.Values.Any(e => EqualityComparer<U>.Default.Equals(e.Value1, value1) &&
                                                EqualityComparer<V>.Default.Equals(e.Value2, value2));
        }

        public ExtendedDictionaryElement<T, U, V> this[T key] => _dictionary.ContainsKey(key)
            ? _dictionary[key]
            : throw new KeyNotFoundException("Ключ не знайдено.");

        public int Count => _dictionary.Count;

        public IEnumerator<ExtendedDictionaryElement<T, U, V>> GetEnumerator() => _dictionary.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            string sample = "hello world";
            Console.WriteLine("Оригінальний рядок: " + sample);
            Console.WriteLine("Інвертований рядок: " + sample.ReverseString());
            Console.WriteLine("Кількість входжень 'l': " + sample.CountOccurrences('l'));

            int[] numbers = { 1, 2, 2, 3, 4, 4, 4, 5 };
            Console.WriteLine("Оригінальний масив: " + string.Join(", ", numbers));
            Console.WriteLine("Кількість входжень 4: " + numbers.CountOccurrences(4));
            Console.WriteLine("Унікальні елементи: " + string.Join(", ", numbers.GetUniqueElements()));

            var extendedDictionary = new ExtendedDictionary<int, string, string>();
            extendedDictionary.Add(new ExtendedDictionaryElement<int, string, string>(1, "Перший", "Значення1"));
            extendedDictionary.Add(new ExtendedDictionaryElement<int, string, string>(2, "Другий", "Значення2"));

            Console.WriteLine("Словник містить ключ 1: " + extendedDictionary.ContainsKey(1));
            Console.WriteLine("Словник містить значення (Перший, Значення1): " + extendedDictionary.ContainsValue("Перший", "Значення1"));

            var element = extendedDictionary[1];
            Console.WriteLine($"Ключ: {element.Key}, Значення1: {element.Value1}, Значення2: {element.Value2}");

            Console.WriteLine("Ітерація через словник:");
            foreach (var item in extendedDictionary)
            {
                Console.WriteLine($"Ключ: {item.Key}, Значення1: {item.Value1}, Значення2: {item.Value2}");
            }
        }
    }
}
