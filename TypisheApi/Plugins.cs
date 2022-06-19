using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TypisheApi
{
    public static class Plugins
    {
        public static ClassHeader GetItems(string pluginsFolder)
        {
            var header = new ClassHeader();

            try
            {
                var activePluginPaths = Directory.GetDirectories(pluginsFolder);

                foreach (var pluginPath in activePluginPaths)
                {
                    var dllPath = Path.Combine(pluginPath, new FileInfo(pluginPath).Name) + ".dll";
                    var pluginAssembly = Assembly.LoadFile(dllPath);

                    var type = pluginAssembly.GetTypes().ToList().Find(a => typeof(IClassHeader).IsAssignableFrom(a));
                    var headerPart = (IClassHeader) Activator.CreateInstance(type);

                    if (headerPart != null)
                    {
                        if (headerPart.StartableClasses != null)
                        {
                            header.StartableClasses.AddRange(headerPart.StartableClasses);
                        }

                        if (headerPart.GuiControls != null)
                        {
                            header.GuiControls.AddRange(headerPart.GuiControls);
                        }
                    } 
                }
            }

            catch (Exception ex)
            {
                header.StartableClasses.Clear();
                header.GuiControls.Clear();
            }

            return header;
        }
    }
}
