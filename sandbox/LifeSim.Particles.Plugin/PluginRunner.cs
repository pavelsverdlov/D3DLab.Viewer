using System;
using System.Windows;

using D3DLab.Plugin;

namespace LifeSim.Particles.Plugin {
    public class PluginRunner : APluginRunner {
        public PluginRunner() : base("Particles life simulation", "Particles life simulation") {
        }
        public override void LoadResources(IPluginContext context) {
            context.AddResource(new ResourceDictionary {
                Source = new Uri("/LifeSim.Particles.Plugin;component/Resources.xaml", UriKind.RelativeOrAbsolute)
            });
            base.LoadResources(context);
        }
        protected override IPluginViewModel CreateViewModel(IPluginContext context) {
            return new PluginViewModel(context);
        }
        protected override IPluginWindow CreateWindow() {
            return new PluginWindow();
        }
    }
}
