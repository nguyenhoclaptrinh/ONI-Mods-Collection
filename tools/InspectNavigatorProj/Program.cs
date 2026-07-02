using System;
using System.IO;
using System.Reflection;

namespace InspectNavigator {
    class Program {
        static void Main(string[] args) {
            try {
                string managedDir = @"d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed";
                
                string[] dlls = new[] { "Assembly-CSharp-firstpass.dll", "Assembly-CSharp.dll" };
                foreach (var dll in dlls) {
                    string path = Path.Combine(managedDir, dll);
                    Assembly assembly = Assembly.LoadFrom(path);
                    foreach (Type type in assembly.GetTypes()) {
                        if (type.Name == "PriorityClass" || type.Name.Contains("PriorityClass")) {
                            Console.WriteLine($"Found {type.FullName} in {dll}");
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine("Error: " + ex.ToString());
            }
        }
    }
}
