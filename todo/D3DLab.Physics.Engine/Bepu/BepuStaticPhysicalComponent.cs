using BepuUtilities;

using D3DLab.ECS;

namespace D3DLab.Physics.Engine.Bepu {
    class StaticAABBPhysicalComponent : PhysicalComponent {

        public BoundingBox AABBox;

        public StaticAABBPhysicalComponent() {
            
        }

        internal override bool TryConstructBody(GraphicEntity entity, IPhysicsShapeConstructor constructor) {
            return constructor.TryConstructShape(entity, this);
        }
    }
    class StaticMeshPhysicalComponent : PhysicalComponent {

        public StaticMeshPhysicalComponent() {

        }

        internal override bool TryConstructBody(GraphicEntity entity, IPhysicsShapeConstructor constructor) {
            return constructor.TryConstructShape(entity, this);
        }
    }

    
}
