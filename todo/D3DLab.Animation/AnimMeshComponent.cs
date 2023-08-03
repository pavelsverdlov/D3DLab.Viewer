using System.Numerics;

using D3DLab.ECS;

namespace D3DLab.Animation {
    public class MeshAnimationComponent : GraphicComponent {
        public const int Slot = 4;

        public Matrix4x4[] Bones;

        public readonly string AnimationName;

        public MeshAnimationComponent(string animationName) {            
            this.AnimationName = animationName;
        }        
    }
}
