using System.Numerics;

using D3DLab.Toolkit.Math3D;

namespace D3DLab.Game.Toolkit {
    public struct SkyPlaneData {
        public static SkyPlaneData Default {
            get {
                return new SkyPlaneData {
                    PlaneResolution = 10,
                    PlaneWidth = 10.0f,
                    PlaneTop = 0.5f,
                    PlaneBottom = 0.0f,
                    TextureRepeat = 4
                };
            }
        }

        public int PlaneResolution;
        public float PlaneWidth;
        public float PlaneTop;
        public float PlaneBottom;
        public int TextureRepeat;
    }

    public class GeometryBuilder {
        public static ImmutableGeometryData BuildSkyPlane(SkyPlaneData data) {
            var count = (data.PlaneResolution + 1) * (data.PlaneResolution + 1);
            var points = new Vector3[count];
            var tex = new Vector2[count];

            // Determine the size of each quad on the sky plane.
            float quadSize = data.PlaneWidth / (float)data.PlaneResolution;
            // Calculate the radius of the sky plane based on the width.
            float radius = data.PlaneWidth / 2.0f;
            // Calculate the height constant to increment by.
            float constant = (data.PlaneTop - data.PlaneBottom) / (radius * radius);
            // Calculate the texture coordinate increment value.
            float textureDelta = (float)data.TextureRepeat / (float)data.PlaneResolution;

            // Loop through the sky plane and build the coordinates based on the increment values given.
            for (int j = 0; j <= data.PlaneResolution; j++) {
                for (int i = 0; i <= data.PlaneResolution; i++) {
                    // Calculate the vertex coordinates.
                    float positionX = (-0.5f * data.PlaneWidth) + ((float)i * quadSize);
                    float positionZ = (-0.5f * data.PlaneWidth) + ((float)j * quadSize);
                    float positionY = data.PlaneTop - (constant * ((positionX * positionX) + (positionZ * positionZ)));

                    // Calculate the texture coordinates.
                    float tu = (float)i * textureDelta;
                    float tv = (float)j * textureDelta;

                    // Calculate the index into the sky plane array to add this coordinate.
                    int index = j * (data.PlaneResolution + 1) + i;

                    // Add the coordinates to the sky plane array.
                    points[index] = new Vector3(positionX, positionY, positionZ);
                    tex[index] = new Vector2(tu, tv);
                }
            }
            var vertexCount = (data.PlaneResolution + 1) * (data.PlaneResolution + 1) * 6;

            var indices = new int[vertexCount];
            var positions = new Vector3[vertexCount];
            var texture = new Vector2[vertexCount];

            // Initialize the index into the vertex array.
            int indx = 0;
            // Load the vertex and index array with the sky plane array data.
            for (int j = 0; j < data.PlaneResolution; j++) {
                for (int i = 0; i < data.PlaneResolution; i++) {
                    int index1 = j * (data.PlaneResolution + 1) + i;
                    int index2 = j * (data.PlaneResolution + 1) + (i + 1);
                    int index3 = (j + 1) * (data.PlaneResolution + 1) + i;
                    int index4 = (j + 1) * (data.PlaneResolution + 1) + (i + 1);

                    // Triangle 1 - Upper Left
                    positions[indx] = points[index1];
                    texture[indx] = tex[index1];
                    indices[indx] = indx;
                    indx++;

                    // Triangle 1 - Upper Right
                    positions[indx] = points[index2];
                    texture[indx] = tex[index2];
                    indices[indx] = indx;
                    indx++;

                    // Triangle 1 - Bottom Left
                    positions[indx] = points[index3];
                    texture[indx] = tex[index3];
                    indices[indx] = indx;
                    indx++;

                    // Triangle 2 - Bottom Left
                    positions[indx] = points[index3];
                    texture[indx] = tex[index3];
                    indices[indx] = indx;
                    indx++;

                    // Triangle 2 - Upper Right
                    positions[indx] = points[index2];
                    texture[indx] = tex[index2];
                    indices[indx] = indx;
                    indx++;

                    // Triangle 2 - Bottom Right
                    positions[indx] = points[index4];
                    texture[indx] = tex[index4];
                    indices[indx] = indx;
                    indx++;
                }
            }
            //TODO: calculate normals
            return new ImmutableGeometryData(positions, Array.Empty<Vector3>(), indices, texture);
        }
    }
}