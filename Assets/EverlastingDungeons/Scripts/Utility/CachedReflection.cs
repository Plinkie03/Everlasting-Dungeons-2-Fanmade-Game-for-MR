using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assets.EverlastingDungeons.Scripts.Utility
{
    public static class CachedReflection
    {
        private static readonly Dictionary<Type, FieldInfo[]> FieldsArray = new();
        private static readonly Dictionary<Type, Dictionary<string, FieldInfo>> FieldsDict = new();
        private static readonly BindingFlags Flags = BindingFlags.Default | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        private static Dictionary<string, FieldInfo> GetFields(Type type)
        {
            if (FieldsDict.TryGetValue(type, out var dict))
                return dict;

            var fields = type.GetFields(Flags);
            dict = fields.ToDictionary(f => f.Name);
            FieldsDict.Add(type, dict);
            FieldsArray.Add(type, fields);
            return dict;
        }

        private static FieldInfo GetField(Type type, string name)
        {
            var dict = GetFields(type);
            return dict.TryGetValue(name, out var field) ? field : null;
        }

        public static T GetStaticValue<T>(Type type, string name)
        {
            return (T)GetField(type, name)?.GetValue(null);
        }

        public static T GetInstanceValue<T>(object instance, string name)
        {
            return (T)GetField(instance.GetType(), name)?.GetValue(instance);
        }

        public static T[] GetInstanceValues<T>(object instance)
        {
            var values = new List<T>();

            foreach (var field in GetFields(instance.GetType()).Values)
            {
                var value = field.GetValue(instance);
                if (field.IsStatic || !field.FieldType.IsSubclassOf(typeof(T)) || value == null) continue;
                values.Add((T)value);
            }

            return values.ToArray();
        }

        public static T[] GetStaticValues<T>(Type type)
        {
            var values = new List<T>();
            foreach (var field in GetFields(type).Values)
            {
                var value = field.GetValue(null);
                if (!field.IsStatic || !field.FieldType.IsSubclassOf(typeof(T)) || value == null) continue;
                values.Add((T)value);
            }
            return values.ToArray();
        }
    }
}
