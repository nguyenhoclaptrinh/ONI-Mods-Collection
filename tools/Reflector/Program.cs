using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text.Json;

namespace Reflector
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("==========================================================================");
                Console.WriteLine("CÔNG CỤ PHẢN CHIẾU ASSEMBLY-CSHARP (REFLECTOR)");
                Console.WriteLine("Hướng dẫn sử dụng:");
                Console.WriteLine("  dotnet run --project tools/Reflector <ClassName1> <ClassName2> ...");
                Console.WriteLine("Ví dụ:");
                Console.WriteLine("  dotnet run --project tools/Reflector KCrashReporter SaveLoader");
                Console.WriteLine("==========================================================================");
                return;
            }

            // 1. Tìm config.json để lấy đường dẫn game
            string managedDir = GetManagedDir();
            if (string.IsNullOrEmpty(managedDir) || !Directory.Exists(managedDir))
            {
                Console.WriteLine($"LỖI: Không tìm thấy thư mục Managed của game tại: '{managedDir}'");
                Console.WriteLine("Vui lòng cấu hình đường dẫn chính xác trong file 'tools/Reflector/config.json'.");
                return;
            }

            // 2. Thiết lập AssemblyResolve
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

            // 3. Load Assembly-CSharp.dll và chạy phản chiếu
            try
            {
                string assemblyPath = Path.Combine(managedDir, "Assembly-CSharp.dll");
                if (!File.Exists(assemblyPath))
                {
                    Console.WriteLine($"LỖI: Không tìm thấy file Assembly-CSharp.dll tại: '{assemblyPath}'");
                    return;
                }

                Assembly assembly = Assembly.LoadFrom(assemblyPath);
                foreach (string typeName in args)
                {
                    ReflectType(assembly, typeName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LỖI hệ thống: " + ex.ToString());
            }
        }

        static string GetManagedDir()
        {
            string workspaceRoot = FindWorkspaceRoot();
            string configPath = Path.Combine(workspaceRoot, "tools", "Reflector", "config.json");
            
            if (!File.Exists(configPath))
            {
                // Fallback nếu chạy trong thư mục project
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

        static void ReflectType(Assembly assembly, string typeName)
        {
            Type type = assembly.GetType(typeName);
            if (type == null)
            {
                Console.WriteLine($"=== LỖI: Không tìm thấy kiểu '{typeName}' trong Assembly ===");
                return;
            }

            Console.WriteLine($"Đang phản chiếu kiểu: {typeName}...");

            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            
            var fieldsList = new List<string>();
            var propertiesList = new List<string>();
            var methodsList = new List<string>();

            // Quét các Fields
            foreach (var f in type.GetFields(flags))
            {
                string modifiers = GetFieldModifiers(f);
                string typeStr = GetTypeName(f.FieldType);
                fieldsList.Add($"        {modifiers} {typeStr} {f.Name};");
            }

            // Quét các Properties
            foreach (var p in type.GetProperties(flags))
            {
                string modifiers = GetPropertyModifiers(p);
                string typeStr = GetTypeName(p.PropertyType);
                string accessors = "";
                if (p.CanRead && p.CanWrite) accessors = "{ get; set; }";
                else if (p.CanRead) accessors = "{ get; }";
                else if (p.CanWrite) accessors = "{ set; }";
                propertiesList.Add($"        {modifiers} {typeStr} {p.Name} {accessors}");
            }

            // Quét các Methods
            foreach (var m in type.GetMethods(flags))
            {
                // Bỏ qua getter/setter của properties để tránh lặp
                if (m.IsSpecialName && (m.Name.StartsWith("get_") || m.Name.StartsWith("set_")))
                    continue;

                string modifiers = GetMethodModifiers(m);
                string returnTypeStr = GetTypeName(m.ReturnType);
                var paramsList = new List<string>();
                foreach (var p in m.GetParameters())
                {
                    string pType = GetTypeName(p.ParameterType);
                    paramsList.Add($"{pType} {p.Name}");
                }
                methodsList.Add($"        {modifiers} {returnTypeStr} {m.Name}({string.Join(", ", paramsList)});");
            }

            // Tạo mã nguồn giả lập dạng C# đẹp mắt
            string fieldsText = fieldsList.Count > 0 ? string.Join("\n", fieldsList) : "        // (Không có)";
            string propertiesText = propertiesList.Count > 0 ? string.Join("\n", propertiesList) : "        // (Không có)";
            string methodsText = methodsList.Count > 0 ? string.Join("\n", methodsList) : "        // (Không có)";

            string workspaceRoot = FindWorkspaceRoot();
            string outputDir = Path.Combine(workspaceRoot, "docs", "050-Research", "outputs");
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, $"{typeName}-reflection.md");

            string fileContent = $@"---
id: {typeName.ToLower()}-reflection
type: research-output
status: done
created: {DateTime.Now:yyyy-MM-dd}
source: Reflector tool
---
# Phản chiếu kiểu {typeName}

## Mục đích
Quét cấu trúc kiểu {typeName} từ Assembly-CSharp.dll để hỗ trợ phát triển mod.

## Kết quả
```csharp
namespace Game
{{
    public class {typeName}
    {{
        // ==========================================
        // --- FIELDS ---
        // ==========================================
{fieldsText}

        // ==========================================
        // --- PROPERTIES ---
        // ==========================================
{propertiesText}

        // ==========================================
        // --- METHODS ---
        // ==========================================
{methodsText}
    }}
}}
```

## Kết luận & Áp dụng
<Dùng kiến thức này như thế nào trong implementation>
";

            File.WriteAllText(outputPath, fileContent);
            Console.WriteLine($"Đã xuất file thành công: docs/050-Research/outputs/{typeName}-reflection.md");
        }

        static string GetTypeName(Type type)
        {
            if (type == typeof(void)) return "void";
            if (type == typeof(int)) return "int";
            if (type == typeof(uint)) return "uint";
            if (type == typeof(long)) return "long";
            if (type == typeof(ulong)) return "ulong";
            if (type == typeof(short)) return "short";
            if (type == typeof(ushort)) return "ushort";
            if (type == typeof(byte)) return "byte";
            if (type == typeof(sbyte)) return "sbyte";
            if (type == typeof(bool)) return "bool";
            if (type == typeof(float)) return "float";
            if (type == typeof(double)) return "double";
            if (type == typeof(decimal)) return "decimal";
            if (type == typeof(string)) return "string";
            if (type == typeof(object)) return "object";

            if (type.IsGenericType)
            {
                var name = type.Name;
                int backtickIndex = name.IndexOf('`');
                if (backtickIndex > 0)
                {
                    name = name.Substring(0, backtickIndex);
                }
                var genericArguments = type.GetGenericArguments();
                var argNames = new List<string>();
                foreach (var arg in genericArguments)
                {
                    argNames.Add(GetTypeName(arg));
                }
                return $"{name}<{string.Join(", ", argNames)}>";
            }

            return type.Name;
        }

        static string GetFieldModifiers(FieldInfo f)
        {
            var list = new List<string>();
            if (f.IsPublic) list.Add("public");
            else if (f.IsPrivate) list.Add("private");
            else if (f.IsFamily) list.Add("protected");
            else if (f.IsAssembly) list.Add("internal");

            if (f.IsStatic) list.Add("static");
            if (f.IsInitOnly) list.Add("readonly");

            return string.Join(" ", list);
        }

        static string GetPropertyModifiers(PropertyInfo p)
        {
            var m = p.GetMethod ?? p.SetMethod;
            if (m == null) return "public";

            var list = new List<string>();
            if (m.IsPublic) list.Add("public");
            else if (m.IsPrivate) list.Add("private");
            else if (m.IsFamily) list.Add("protected");
            else if (m.IsAssembly) list.Add("internal");

            if (m.IsStatic) list.Add("static");

            return string.Join(" ", list);
        }

        static string GetMethodModifiers(MethodInfo m)
        {
            var list = new List<string>();
            if (m.IsPublic) list.Add("public");
            else if (m.IsPrivate) list.Add("private");
            else if (m.IsFamily) list.Add("protected");
            else if (m.IsAssembly) list.Add("internal");

            if (m.IsStatic) list.Add("static");
            else if (m.IsVirtual && !m.IsFinal) list.Add("virtual");

            return string.Join(" ", list);
        }
    }
}
