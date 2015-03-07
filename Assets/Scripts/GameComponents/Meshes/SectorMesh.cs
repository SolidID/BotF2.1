using Assets.Scripts.Configuration;
using UnityEngine;

namespace Assets.Scripts.GameComponents.Meshes
{
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
	public class SectorMesh : MonoBehaviour
	{
		public enum SectorMeshMode
		{
			Flat = 0,
			ThreeDimensional,
		}

		private void Start()
		{
			var filter = GetComponent<MeshFilter>();
			filter.mesh = GenerateSector(GameSettings.Instance.SectorMeshRenderMode);
			gameObject.GetComponent<MeshCollider>().sharedMesh = filter.mesh;
		}

		private static Mesh GenerateSector(SectorMeshMode sectorMeshRenderMode)
		{
			return sectorMeshRenderMode == SectorMeshMode.ThreeDimensional ? CreateThreeDimensionalMesh() : CreateFlatMesh();
		}

		private static Mesh CreateFlatMesh()
		{
			var vertices = new Vector3[6];
			var uv = new Vector2[6];

			VertexAndUvContainer res = CreateHexagonVerticesAndUVs(0);
			res.Vertices.CopyTo(vertices, 0);
			res.Uv.CopyTo(uv, 0);

			// indices for the top side
			int[] indeces = CreateIndeces(SectorMeshMode.Flat);

			var mesh = new Mesh
			{
				vertices = vertices,
				uv = uv,
				triangles = indeces
			};
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			mesh.Optimize();
			return mesh;
		}

		private static Mesh CreateThreeDimensionalMesh()
		{
			var vertices = new Vector3[12];
			var uv = new Vector2[12];
			float height = 8f;
			float upperY = height / 2;
			float lowerY = -1 * upperY;

			VertexAndUvContainer res = CreateHexagonVerticesAndUVs(upperY);
			res.Vertices.CopyTo(vertices, 0);
			res.Uv.CopyTo(uv, 0);

			res = CreateHexagonVerticesAndUVs(lowerY);
			res.Vertices.CopyTo(vertices, 6);
			res.Uv.CopyTo(uv, 6);

			// indices for the top side
			int[] indeces = CreateIndeces(SectorMeshMode.ThreeDimensional);

			var mesh = new Mesh
			{
				vertices = vertices,
				uv = uv,
				triangles = indeces
			};
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			mesh.Optimize();
			return mesh;
		}

		private static int[] CreateIndeces(SectorMeshMode mode)
		{
			switch (mode)
			{
				case SectorMeshMode.ThreeDimensional:

					return new[]
					{
						// UPPER HEX (facing to 0,1,0 [triangles clockwise])
						5, 0, 1, 5, 1, 4, 4, 1, 2, 4, 2, 3,
						// LOWER HEX (facing to 0,-1,0 [triangles counter-clockwise])
						11, 7, 6, 7, 11, 10, 7, 10, 8, 8, 10, 9,
						//// TOP-RIGHT-SIDE
						0, 6, 7, 0, 7, 1,
						//// RIGHT SIDE
						1, 7, 8, 1, 8, 2,
						//// BOTTOM-RIGH-SIDE
						2, 8, 9, 2, 9, 3,
						//// BOTTOM-LEFT-SIDE
						3, 9, 10, 10, 4, 3,
						//// LEFT-SIDE
						//10,4,5,5,11,10,
						4, 10, 11, 11, 5, 4,
						//// TOP LEFT SIDE
						5, 11, 6, 6, 0, 5
					};

				case SectorMeshMode.Flat:
					return new[]
					{
						// UPPER HEX (facing to 0,1,0 [triangles clockwise])
						5, 0, 1, 5, 1, 4, 4, 1, 2, 4, 2, 3,
					};

				default:
					return new int[] { };
			}
		}

		private static VertexAndUvContainer CreateHexagonVerticesAndUVs(float y)
		{
			var vertices = new Vector3[6];
			var uv = new Vector2[6];

			//top
			vertices[0] = new Vector3(0, y, Globals.Radius);
			uv[0] = new Vector2(0.5f, 1);
			//topright
			vertices[1] = new Vector3(Globals.HalfWidth, y, Globals.Radius / 2);
			uv[1] = new Vector2(1, 0.75f);
			//bottomright
			vertices[2] = new Vector3(Globals.HalfWidth, y, -Globals.Radius / 2);
			uv[2] = new Vector2(1, 0.25f);
			//bottom
			vertices[3] = new Vector3(0, y, -Globals.Radius);
			uv[3] = new Vector2(0.5f, 0);
			//bottomleft
			vertices[4] = new Vector3(-Globals.HalfWidth, y, -Globals.Radius / 2);
			uv[4] = new Vector2(0, 0.25f);
			//topleft
			vertices[5] = new Vector3(-Globals.HalfWidth, y, Globals.Radius / 2);
			uv[5] = new Vector2(0, 0.75f);

			return new VertexAndUvContainer { Vertices = vertices, Uv = uv };
		}

		private class VertexAndUvContainer
		{
			public Vector3[] Vertices { get; set; }
			public Vector2[] Uv { get; set; }
		}
	}
}
