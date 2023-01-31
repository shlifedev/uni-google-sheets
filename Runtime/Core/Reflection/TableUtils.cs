using System;

namespace GoogleSheet.Reflection
{
    public class TableUtils
    {
        public static string GetSpreadSheetID<T>() where T : ITable
        {
            try
            {
                var type = typeof(T);

                var field = type.GetField("spreadSheetID",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Static);
                var value = field.GetValue(null);
                return (string)value;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw new System.Exception("GetSpreadSheetId Failed Message => " + e.Message);
            }
        }
        public static string GetSheetID<T>() where T : ITable
        {
            try
            {
                var type = typeof(T);
                var field = type.GetField("spreadSheetID",
                    System.Reflection.BindingFlags.Static |
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Public);
                var value = field.GetValue(null);
                return (string)value;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw new System.Exception("Get sheetID Failed Message => " + e.Message);
            }
        }

        public static string GetSpreadSheetID(System.Type type)
        {
            try
            {
                var field = type.GetField("spreadSheetID",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Static);
                var value = field.GetValue(null);
                return (string)value;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw new System.Exception("GetSpreadSheetId Failed Message => " + e.Message);
            }
        }
        public static string GetSheetID(System.Type type)
        {
            try
            {
                var field = type.GetField("spreadSheetID",
                    System.Reflection.BindingFlags.Static |
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Public);
                var value = field.GetValue(null);
                return (string)value;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw new System.Exception("Get sheetID Failed Message => " + e.Message);
            }
        }
    }
}
