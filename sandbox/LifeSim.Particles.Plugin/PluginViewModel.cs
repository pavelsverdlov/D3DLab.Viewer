using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Windows.Input;

using D3DLab.ECS;
using D3DLab.ECS.Ext;
using D3DLab.Plugin;
using D3DLab.Toolkit;
using D3DLab.Toolkit.D3Objects;

using LifeSim.Particles.Plugin.D3D;

using WPFLab;
using WPFLab.MVVM;

namespace LifeSim.Particles.Plugin {
    class PluginViewModel : BaseNotify, IPluginViewModel {
        public ICommand RestartCommand { get; }
        public PluginViewModel(IPluginContext context) {
            this.context = context;
            this.particles = new List<ParticleGameObject>();
            this.RestartCommand = new WpfActionCommand(new Action(this.OnRestart));
            this.box = new AxisAlignedBox(100f, 100f, 100f, Vector3.Zero);
        }
        private void OnRestart() {
            foreach (ParticleGameObject particleGameObject in this.particles) {
                particleGameObject.GameObject.Cleanup(this.context.Scene.Context);
            }
            Thread.Sleep(100);
            this.AddParticles(ParticleTypes.Green, 100, this.box, new Dictionary<ParticleTypes, float>
            {
                {
                    ParticleTypes.Red,
                    2f
                },
                {
                    ParticleTypes.Green,
                    -1f
                }
            });
            this.AddParticles(ParticleTypes.Red, 100, this.box, new Dictionary<ParticleTypes, float>());
            this.AddParticles(ParticleTypes.Yellow, 100, this.box, new Dictionary<ParticleTypes, float>
            {
                {
                    ParticleTypes.Green,
                    1f
                },
                {
                    ParticleTypes.Yellow,
                    -0.5f
                }
            });
        }

        public void Closed() {
        }
        public void Init() {
            this.context.Scene.SetContinuouslyRender(true);
            this.context.Scene.Context.GetSystemManager().GetSystems<PluginContainerSystem>().Single<PluginContainerSystem>().CreateNested<ParticlesGravityMovingSystem>();
            this.context.Scene.DrawObject(VisualPolylineObject.CreateBox(this.context.Scene.Context, ElementTag.New("world"), this.box.GetCornersBox(), V4Colors.White));
            this.context.Scene.GetWorld().UpdateComponent<WorldBoxComponent>(WorldBoxComponent.Create(this.box));
            this.OnRestart();
        }
        private void AddParticles(ParticleTypes type, int count, AxisAlignedBox box, Dictionary<ParticleTypes, float> relationships) {
            Vector3 vector = box.Size();
            Random random = new Random();
            for (int i = 0; i < count; i++) {
                ElementTag tag = new ElementTag($"particle_{type}_{i}");
                Vector3 vector2 = new Vector3(random.NextFloat(vector.X * -0.5f, vector.X * 0.5f), random.NextFloat(vector.Y * -0.5f, vector.Y * 0.5f), random.NextFloat(vector.Z * -0.5f, vector.Z * 0.5f));
                Vector4 color = V4Colors.White;
                switch (type) {
                    case ParticleTypes.Red:
                        color = V4Colors.Red;
                        break;
                    case ParticleTypes.Green:
                        color = V4Colors.Green;
                        break;
                    case ParticleTypes.Yellow:
                        color = V4Colors.Yellow;
                        break;
                }
                ParticleGameObject particleGameObject = new ParticleGameObject(tag, this.context.Scene.DrawPoint(tag, new PointDetails {
                    Radius = 0.1f,
                    Center = vector2,
                    Color = color
                }));

                var dir = new Vector3(random.NextFloat(0f, 1f), random.NextFloat(0f, 1f), random.NextFloat(0f, 1f)).Normalized();
                particleGameObject.GameObject.AddComponent(
                    this.context.Scene.Context, 
                    ParticleComponent.Create(type, vector2, dir, 0.2f, 0.1f));

                var component = new ParticleRelationComponent(relationships);
                particleGameObject.GameObject.AddComponent(this.context.Scene.Context, component);

                this.particles.Add(particleGameObject);
            }
        }
        
        readonly IPluginContext context;
        readonly List<ParticleGameObject> particles;
        readonly AxisAlignedBox box;
    }
}
