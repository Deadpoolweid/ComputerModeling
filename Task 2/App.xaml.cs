using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Task_2
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        ///// <summary>
        ///// Application Entry Point.
        ///// </summary>
        //[System.STAThreadAttribute()]
        //[System.Diagnostics.DebuggerNonUserCodeAttribute()]
        //[System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        //public static void Main() {
        //    var assemblies = new Dictionary<string, Assembly>();
        //    var executingAssembly = Assembly.GetExecutingAssembly();
        //    var resources = executingAssembly.GetManifestResourceNames().Where(n => n.EndsWith(".dll"));

        //    foreach (string resource in resources)
        //    {
        //        using (var stream = executingAssembly.GetManifestResourceStream(resource))
        //        {
        //            if (stream == null)
        //                continue;

        //            var bytes = new byte[stream.Length];
        //            stream.Read(bytes, 0, bytes.Length);
        //            try
        //            {
        //                assemblies.Add(resource, Assembly.Load(bytes));
        //            }
        //            catch (Exception ex)
        //            {
        //                System.Diagnostics.Debug.Print(string.Format("Failed to load: {0}, Exception: {1}", resource, ex.Message));
        //            }
        //        }
        //    }

        //    AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
        //    {
        //        var assemblyName = new AssemblyName(e.Name);

        //        var path = string.Format("{0}.dll", assemblyName.Name);

        //        if (assemblies.ContainsKey(path))
        //        {
        //            return assemblies[path];
        //        }

        //        return null;
        //    };

        //    Task_2.App app = new Task_2.App();
        //    app.InitializeComponent();
        //    app.Run();
        //}
    }
}
