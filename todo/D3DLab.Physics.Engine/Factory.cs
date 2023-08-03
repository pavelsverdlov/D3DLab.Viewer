using D3DLab.Physics.Engine.Bepu;

namespace D3DLab.Physics.Engine {

    public static class PhysicalComponentFactory {
        public static PhysicalComponent CreateAABB() {
            return new DynamicAABBPhysicalComponent();
        }
        public static PhysicalComponent CreateStaticAABB() {
            return new StaticAABBPhysicalComponent();
        }
        public static PhysicalComponent CreateStaticMesh() {
            return new StaticMeshPhysicalComponent();
        }

        public static PhysicalComponent CreateMesh() {
            return new DynamicMeshPhysicalComponent();
        }
        
    }
}

