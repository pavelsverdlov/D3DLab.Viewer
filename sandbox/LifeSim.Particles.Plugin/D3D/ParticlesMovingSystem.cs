using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using D3DLab.ECS;
using D3DLab.ECS.Components;
using D3DLab.Plugin;
using D3DLab.SDX.Engine.Components;
using D3DLab.Toolkit.Components;

namespace LifeSim.Particles.Plugin.D3D {
    class ParticlesMovingSystem : BaseEntitySystem, IGraphicSystemContextDependent, IPluginGraphicSystem, INestedGraphicSystem, IGraphicSystem, IDisposable {
        public IContextState? ContextState { get; set; }
        public int OrderId { get; set; }
        
        protected override void Executing(ISceneSnapshot snapshot) {
            IEntityManager entityManager = this.ContextState.GetEntityManager();
            WorldBoxComponent component = entityManager.GetEntity(snapshot.WorldTag).GetComponent<WorldBoxComponent>();
            foreach (GraphicEntity graphicEntity in entityManager.GetEntities()) {
                ParticleComponent particleComponent;
                TransformComponent com;
                RenderableComponent renderableComponent;
                bool flag = graphicEntity.TryGetComponents<ParticleComponent, TransformComponent, RenderableComponent>(out particleComponent, out com, out renderableComponent) && renderableComponent.IsRenderable;
                if (flag) {
                    try {
                        Vector3 vector = particleComponent.Position + particleComponent.Direction * particleComponent.Velocity;
                        bool flag2 = component.Box.ContainsSphere(vector, particleComponent.Radius) != AlignedBoxContainmentType.Contains;
                        if (flag2) {
                            particleComponent.Direction *= -1f;
                            vector = particleComponent.Position + particleComponent.Direction * particleComponent.Velocity;
                        }
                        Matrix4x4 matrix4x = Matrix4x4.CreateTranslation(vector - particleComponent.Position);
                        com = com.Multiply(matrix4x);
                        D3DRenderComponent d3DRenderComponent;
                        bool flag3 = graphicEntity.TryGetComponent<D3DRenderComponent>(out d3DRenderComponent);
                        if (flag3) {
                            d3DRenderComponent.TransformWorldBuffer.Dispose();
                        }
                        graphicEntity.UpdateComponent<TransformComponent>(com);
                        graphicEntity.UpdateComponent<ParticleComponent>(ParticleComponent.Create(particleComponent.Type, vector, particleComponent.Direction, particleComponent.Velocity, particleComponent.Radius));
                    } catch (Exception ex) {
                        ex.ToString();
                    }
                }
            }
        }
    }
}
