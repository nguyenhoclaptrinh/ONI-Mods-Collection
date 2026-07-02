using System;
using System.Reflection;

public class Program {
    public static void Main() {
        try {
            string path = @"d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll";
            Assembly assembly = Assembly.LoadFrom(path);
            Type type = assembly.GetType("Navigator");
            if (type == null) {
                Console.WriteLine("Navigator not found");
                return;
            }
            Console.WriteLine("--- Fields ---");
            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) {
                Console.WriteLine(field.FieldType.FullName + " " + field.Name);
            }
            Console.WriteLine("--- Properties ---");
            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) {
                Console.WriteLine(prop.PropertyType.FullName + " " + prop.Name);
            }
        } catch (Exception ex) {
            Console.WriteLine("Error: " + ex.ToString());
        }
    }
}
