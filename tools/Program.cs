using System;
using System.Reflection;

namespace InspectNavigator {
    class Program {
        static void Main(string[] args) {
            try {
                string path = @"d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll";
                Assembly assembly = Assembly.LoadFrom(path);
                Type? type = assembly.GetType("FetchList2");
                if (type == null) {
                    Console.WriteLine("FetchList2 not found");
                    return;
                }
                Console.WriteLine("--- Methods named Add ---");
                foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) {
                    if (method.Name == "Add") {
                        string parameters = "";
                        foreach (ParameterInfo p in method.GetParameters()) {
                            parameters += $"{p.ParameterType.FullName} {p.Name}, ";
                        }
                        Console.WriteLine($"{method.ReturnType.FullName} Add({parameters.TrimEnd(' ', ',')})");
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine("Error: " + ex.ToString());
            }
        }
    }
}
