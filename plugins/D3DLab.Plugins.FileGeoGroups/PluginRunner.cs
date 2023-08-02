using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using D3DLab.Plugin;

namespace D3DLab.Plugins.FileGeoGroups {
    public class PluginRunner : APluginRunner {
        public PluginRunner() : base("File geometry group details", "allow to show/hide and filter geometry groups, determined in file.") {
        }

        public override void LoadResources(IPluginContext context) {
            context.AddResource(new ResourceDictionary {
                Source = new Uri("/D3DLab.Plugins.FileGeoGroups;component/Resources.xaml", UriKind.RelativeOrAbsolute)
            });
            base.LoadResources(context);
        }

        protected override IPluginViewModel CreateViewModel(IPluginContext context) => new MainViewModel(context);
        protected override IPluginWindow CreateWindow()  => new MainWindow();
    }
}
