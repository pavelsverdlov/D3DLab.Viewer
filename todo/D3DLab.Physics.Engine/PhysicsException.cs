using System;

namespace D3DLab.Physics.Engine {
    public class PhysicsException : Exception {
        public PhysicsException() {
        }

        public PhysicsException(string message) : base(message) {
        }
    }
}
