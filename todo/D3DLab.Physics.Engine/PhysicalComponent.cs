using D3DLab.ECS;

namespace D3DLab.Physics.Engine {
    public abstract class PhysicalComponent : GraphicComponent {
        internal bool IsConstructed { get; set; }
        internal bool IsStatic { get; set; }



        internal int ShapeIndex;
        internal abstract bool TryConstructBody(GraphicEntity entity, IPhysicsShapeConstructor constructor);
       // internal abstract bool NeedConstruct(GraphicEntity entity, IPhysicsShapeConstructor constructor);
    }
}

