using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text.Json;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.Decompiler.TypeSystem;

namespace Reflector
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("==========================================================================");
                Console.WriteLine("CÔNG CỤ DỊCH NGƯỢC ASSEMBLY-CSHARP (REFLECTOR DECOMPILER)");
                Console.WriteLine("Hướng dẫn sử dụng:");
                Console.WriteLine("  dotnet run --project tools/Reflector <ClassName1> <ClassName2> ...");
                Console.WriteLine("==========================================================================");
                return;
            }

            string managedDir = GetManagedDir();
            if (string.IsNullOrEmpty(managedDir) || !Directory.Exists(managedDir))
            {
                Console.WriteLine($"LỖI: Không tìm thấy thư mục Managed tại: '{managedDir}'");
                return;
            }

            string assemblyPath = Path.Combine(managedDir, "Assembly-CSharp.dll");
            if (!File.Exists(assemblyPath))
            {
                Console.WriteLine($"LỖI: Không tìm thấy file Assembly-CSharp.dll tại: '{assemblyPath}'");
                return;
            }

            foreach (string typeName in args)
            {
                DecompileType(assemblyPath, typeName);
            }
        }

        static string GetManagedDir()
        {
            string workspaceRoot = FindWorkspaceRoot();
            string configPath = Path.Combine(workspaceRoot, "tools", "Reflector", "config.json");
            
            if (!File.Exists(configPath))
            {
                configPath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
            }

            if (File.Exists(configPath))
            {
                try
                {
                    string jsonText = File.ReadAllText(configPath);
                    using (JsonDocument doc = JsonDocument.Parse(jsonText))
                    {
                        JsonElement root = doc.RootElement;
                        if (root.TryGetProperty("ManagedDir", out JsonElement dirElement))
                        {
                            return dirElement.GetString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi đọc file config.json: " + ex.Message);
                }
            }

            return null;
        }

        static string FindWorkspaceRoot()
        {
            string dir = Directory.GetCurrentDirectory();
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir, ".git")))
                {
                    return dir;
                }
                dir = Path.GetDirectoryName(dir);
            }
            return Directory.GetCurrentDirectory();
        }

        static void DecompileType(string assemblyPath, string typeName)
        {
            try
            {
                Console.WriteLine($"Đang dịch ngược kiểu: {typeName}...");
                
                var settings = new DecompilerSettings();
                settings.ThrowOnAssemblyResolveErrors = false;
                
                var decompiler = new CSharpDecompiler(assemblyPath, settings);
                
                var fullTypeName = new FullTypeName(typeName);
                var syntaxTree = decompiler.DecompileType(fullTypeName);
                string code = syntaxTree.ToString();

                string workspaceRoot = FindWorkspaceRoot();
                string outputDir = Path.Combine(workspaceRoot, "docs", "050-Research", "outputs");
                Directory.CreateDirectory(outputDir);
                string outputPath = Path.Combine(outputDir, $"{typeName}-decompile.cs");

                File.WriteAllText(outputPath, code);
                Console.WriteLine($"Đã xuất file dịch ngược thành công: docs/050-Research/outputs/{typeName}-decompile.cs");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LỖI khi dịch ngược {typeName}: {ex.Message}");
            }
        }
    }
}
