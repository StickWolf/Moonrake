using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameEngine
{
    public static class EqualChecker
    {
        public static bool AreEqual(object obj1, object obj2)
        {
            var type1 = obj1.GetType();
            var type2 = obj2.GetType();

            if (type1 != type2)
            {
                return false;
            }

            var obj1Props = GetComparableData(obj1);
            var obj2Props = GetComparableData(obj2);

            foreach (var kvp1 in obj1Props)
            {
                if (!obj2Props.ContainsKey(kvp1.Key))
                {
                    return false;
                }

                if (obj2Props[kvp1.Key] != kvp1.Value)
                {
                    return false;
                }
            }

            return true;
        }

        private static Dictionary<string, string> GetComparableData(object obj)
        {
            var compData = new Dictionary<string, string>();
            var publicAndNonPublic = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var properties = obj.GetType().GetProperties(publicAndNonPublic).ToList();

            // Remove the TrackingId property as we know these will be different
            properties = properties.Where(p => p.Name != "TrackingId").ToList();

            foreach (var prop in properties)
            {
                if (prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string))
                {
                    var propValue = prop.GetValue(obj).ToString();
                    compData.Add(prop.Name, propValue);
                }
                else
                {
                    // If there are properties that we don't know how to get a string representation for
                    // Then just add a random guid as the string. This will make the object not equal
                    // but it's safer than just ignoring this property and trying to figure out why
                    // random items are dissapearing
                    compData.Add(prop.Name, Guid.NewGuid().ToString());
                }
            }

            var fields = obj.GetType().GetFields(publicAndNonPublic);
            foreach (var field in fields)
            {
                if (field.FieldType.IsPrimitive || field.FieldType == typeof(string))
                {
                    var fieldValue = field.GetValue(obj).ToString();
                    compData.Add(field.Name, fieldValue);
                }
                else
                {
                    compData.Add(field.Name, Guid.NewGuid().ToString());
                }
            }

            return compData;
        }
    }
}
