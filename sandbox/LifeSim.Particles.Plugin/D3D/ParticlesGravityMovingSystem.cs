using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using D3DLab.ECS;
using D3DLab.ECS.Components;
using D3DLab.ECS.Ext;
using D3DLab.Plugin;
using D3DLab.SDX.Engine.Components;

namespace LifeSim.Particles.Plugin.D3D {
    class ParticlesGravityMovingSystem : BaseEntitySystem, IGraphicSystemContextDependent, IPluginGraphicSystem, INestedGraphicSystem, IGraphicSystem, IDisposable {
        public IContextState? ContextState { get;  set; }
        public int OrderId { get; set; }
        protected override void Executing(ISceneSnapshot snapshot) {
            IEntityManager entityManager = this.ContextState.GetEntityManager();
            WorldBoxComponent component = entityManager.GetEntity(snapshot.WorldTag).GetComponent<WorldBoxComponent>();
            Dictionary<ParticleTypes, List<GraphicEntity>> dictionary = new Dictionary<ParticleTypes, List<GraphicEntity>>();
            List<GraphicEntity> list = new List<GraphicEntity>();
            foreach (GraphicEntity item in entityManager.GetEntities()) {
                ParticleComponent particleComponent;
                bool flag = item.TryGetComponent<ParticleComponent>(out particleComponent);
                if (flag) {
                    List<GraphicEntity> list2;
                    bool flag2 = !dictionary.TryGetValue(particleComponent.Type, out list2);
                    if (flag2) {
                        list2 = new List<GraphicEntity>();
                        dictionary.Add(particleComponent.Type, list2);
                    }
                    list2.Add(item);
                    list.Add(item);
                }
            }
            foreach (GraphicEntity graphicEntity in list) {
                ParticleRelationComponent component2 = graphicEntity.GetComponent<ParticleRelationComponent>();
                ParticleComponent component3 = graphicEntity.GetComponent<ParticleComponent>();
                bool flag3 = component2.Relationships.Any();
                Vector3 vector;
                if (flag3) {
                    float num = 0f;
                    float num2 = 0f;
                    float num3 = 0f;
                    foreach (KeyValuePair<ParticleTypes, float> keyValuePair in component2.Relationships) {
                        List<GraphicEntity> list3 = dictionary[keyValuePair.Key];
                        foreach (GraphicEntity graphicEntity2 in list3) {
                            ParticleComponent component4 = graphicEntity2.GetComponent<ParticleComponent>();
                            float num4 = component4.Position.X - component3.Position.X;
                            float num5 = component4.Position.Y - component3.Position.Y;
                            float num6 = component4.Position.Z - component3.Position.Z;
                            float num7 = MathF.Sqrt(num4 * num4 + num5 * num5 + num6 * num6);
                            float num8 = component4.Radius + component3.Radius;
                            float num9 = keyValuePair.Value * 2f * component3.Radius / num7;
                            bool flag4 = num7 > num8;
                            if (flag4) {
                                num += num9 * num4;
                                num2 += num9 * num5;
                                num3 += num9 * num6;
                            }
                        }
                    }
                    vector = component3.Position.Add(num, num2, num3);
                } else {
                    vector = component3.Position + component3.Direction * component3.Velocity;
                }
                TransformComponent com = graphicEntity.GetComponent<TransformComponent>();
                AxisAlignedBox box = component.Box;
                float radius = component3.Radius;
                bool flag5 = box.ContainsSphere(vector, radius) != AlignedBoxContainmentType.Contains;
                if (flag5) {
                    Vector3 direction = component3.Direction;
                    bool flag6 = box.Minimum.X + radius > vector.X || vector.X > box.Maximum.X - radius || box.Maximum.X - box.Minimum.X < radius;
                    if (flag6) {
                        direction = new Vector3(component3.Direction.X * -1f, component3.Direction.Y, component3.Direction.Z);
                    }
                    bool flag7 = box.Minimum.Y + radius > vector.Y || vector.Y > box.Maximum.Y - radius || box.Maximum.Y - box.Minimum.Y < radius;
                    if (flag7) {
                        direction = new Vector3(component3.Direction.X, component3.Direction.Y * -1f, component3.Direction.Z);
                    }
                    bool flag8 = box.Minimum.Z + radius > vector.Z || vector.Z > box.Maximum.Z - radius || box.Maximum.Z - box.Minimum.Z < radius;
                    if (flag8) {
                        direction = new Vector3(component3.Direction.X, component3.Direction.Y, component3.Direction.Z * -1f);
                    }
                    direction.Normalize();
                    component3.Direction = direction;
                    vector = component3.Position + component3.Direction * component3.Velocity;
                }
                Matrix4x4 matrix4x = Matrix4x4.CreateTranslation(vector - component3.Position);
                com = com.Multiply(matrix4x);
                D3DRenderComponent d3DRenderComponent;
                bool flag9 = graphicEntity.TryGetComponent<D3DRenderComponent>(out d3DRenderComponent);
                if (flag9) {
                    d3DRenderComponent.TransformWorldBuffer.Dispose();
                }
                graphicEntity.UpdateComponent<TransformComponent>(com);
                graphicEntity.UpdateComponent<ParticleComponent>(ParticleComponent.Create(component3.Type, vector, component3.Direction, component3.Velocity, component3.Radius));
            }
        }
    }
}
