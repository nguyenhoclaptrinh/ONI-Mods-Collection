using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Reflector
{
    class Program
    {
        static void Main(string[] args)
        {
            string managedDir = @"D:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed";
            
            AppDomain.CurrentDomain.AssemblyResolve += (sender, resolveArgs) =>
            {
                string name = new AssemblyName(resolveArgs.Name).Name;
                string path = Path.Combine(managedDir, name + ".dll");
                if (File.Exists(path))
                {
                    return Assembly.LoadFrom(path);
                }
                return null;
            };

            Assembly assembly = Assembly.LoadFrom(Path.Combine(managedDir, "Assembly-CSharp.dll"));
            
            string[] possibleConfigTypes = new string[] {
                "BottleEmptierConfig",
                "BottleEmptierGasConfig",
                "BottleEmptierConduitLiquidConfig",
                "BottleEmptierConduitGasConfig",
                "LiquidPumpingStationConfig",
                "LiquidBottlerConfig",
                "GasBottlerConfig"
            };

            string outputPath = @"d:\Documents\Klei\OxygenNotIncluded\mods\Local\docs\050-Research\outputs\config_types_check.txt";
            using (var writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("=== Checking Config Types in Assembly-CSharp ===");
                foreach (var typeName in possibleConfigTypes)
                {
                    Type t = assembly.GetType(typeName);
                    if (t != null)
                    {
                        writer.WriteLine($"Type: {typeName} -> Found!");
                        var idField = t.GetField("ID", BindingFlags.Static | BindingFlags.Public);
                        if (idField != null)
                        {
                            writer.WriteLine($"  ID Value: {idField.GetValue(null)}");
                        }
                    }
                    else
                    {
                        writer.WriteLine($"Type: {typeName} -> NOT Found!");
                    }
                }
            }
            Console.WriteLine("Done checking config types!");
        }
    }
}
