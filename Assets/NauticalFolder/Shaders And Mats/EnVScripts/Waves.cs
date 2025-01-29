using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Waves : MonoBehaviour
{
    public int dimension = 10;
    public float UVScale;
    public Octave[] octaves;
   

    protected MeshFilter MeshFilter;
    //public MeshFilter MeshFilter;
    protected Mesh Mesh;
    // Start is called before the first frame update
    void Start()
    {
        //Mesh Setup
        Mesh = new Mesh();
        Mesh.name = gameObject.name;

        Mesh.vertices = GenerateVerts();
        Mesh.triangles = GenerateTries();
        Mesh.uv = GenerateUVs();
        Mesh.RecalculateBounds();
        Mesh.RecalculateNormals();

        MeshFilter = gameObject.AddComponent<MeshFilter>();
        //MeshFilter = gameObject.GetComponent<MeshFilter>(); //change made
        MeshFilter.mesh = Mesh;
        
    }

    public float GenerateHeight( Vector3 position)
    {
        //scale Factor and position in local space
        var scale = new Vector3(1/transform.lossyScale.x, 0 , 1 / transform.lossyScale.z);
        var localPos = Vector3.Scale((position - transform.position), scale);

        //get edge points
        var p1 = new Vector3(Mathf.Floor(localPos.x) , 0, Mathf.Floor(localPos.z));
        var p2 = new Vector3(Mathf.Floor(localPos.x) , 0, Mathf.Ceil(localPos.z));
        var p3 = new Vector3(Mathf.Ceil(localPos.x) , 0, Mathf.Floor(localPos.z));
        var p4 = new Vector3(Mathf.Ceil(localPos.x) , 0, Mathf.Ceil(localPos.z));


        //clamp if the position is outside the plane
        p1.x = Mathf.Clamp(p1.x, 0, dimension);
        p1.z = Mathf.Clamp(p1.z, 0 , dimension);

        p2.x = Mathf.Clamp(p2.x, 0, dimension);
        p2.z = Mathf.Clamp(p2.z, 0 , dimension);

        p3.x = Mathf.Clamp(p3.x, 0, dimension);
        p3.z = Mathf.Clamp(p3.z, 0 , dimension);

        p4.x = Mathf.Clamp(p4.x, 0, dimension);
        p4.z = Mathf.Clamp(p4.z, 0 , dimension);

        //get the max distance to one of the edges and take the to compute max distance
        var max = Mathf.Max(Vector3.Distance(p1, localPos), Vector3.Distance(p2, localPos), Vector3.Distance(p3, localPos), Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        var distance = (max - Vector3.Distance(p1, localPos)) + (max - Vector3.Distance(p2, localPos)) + (max - Vector3.Distance(p3, localPos)) + (max - Vector3.Distance(p4, localPos) + Mathf.Epsilon) ;

        // weighted sum 
        var height = Mesh.vertices[index((int)p1.x, (int)p1.z)].y * (max - Vector3.Distance(p1, localPos))
         + Mesh.vertices[index((int)p2.x, (int)p2.z)].y * (max - Vector3.Distance(p2, localPos))
         + Mesh.vertices[index((int)p3.x, (int)p3.z)].y * (max - Vector3.Distance(p3, localPos))
         + Mesh.vertices[index((int)p4.x, (int)p4.z)].y * (max - Vector3.Distance(p4, localPos));

         //scale
         return height * transform.lossyScale.y/distance;

    }


    private Vector3[] GenerateVerts()
    {   
        var verts = new Vector3[(dimension +1) * (dimension +1)];

        // eqwually distributing verts
        for (int x = 0; x <= dimension; x++)
        {
            for(int z = 0; z <= dimension; z++)
            {
                verts[index(x,z)] = new Vector3(x,0,z);
            }
        }


        return verts;
    }

    private int index(int x, int z)
    {
        return x * (dimension +1) + z;
    }

    private int[] GenerateTries()
    {   
        var tries = new int[Mesh.vertices.Length * 6];

        //two triangles are one tile
        for(int x = 0; x < dimension; x++)
        {
            for(int z = 0; z < dimension; z++)
            {
                tries[index(x,z) * 6 + 0] = index(x,z);

                tries[index(x,z) * 6 + 1] = index(x + 1,z + 1);

                tries[index(x,z) * 6 + 2] = index(x + 1,z);

                tries[index(x,z) * 6 + 3] = index(x,z);

                tries[index(x,z) * 6 + 4] = index(x,z + 1);

                tries[index(x,z) * 6 + 5] = index(x + 1,z + 1);
            }
        }


        return tries;
    }

    private Vector2[] GenerateUVs()
    {
        var uvs = new Vector2[Mesh.vertices.Length];

        // always set one uv over n tiles than flip the uv and set it again
        for(int x = 0; x<= dimension; x++)
        {
            for(int z = 0; z <= dimension; z++)
            {
                var vec = new Vector2((x / UVScale) % 2 ,(z/UVScale) % 2 );
                uvs[index(x,z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x , vec.y <= 1 ? vec.y : 2 - vec.y);
            }
        }


        return uvs;
    }

    // Update is called once per frame

    void Update()
    {
        var verts = Mesh.vertices;
        for(int x = 0; x <= dimension; x++)
        {
            for(int z = 0; z <= dimension; z++)
            {
                var y = 0f;
                for(int O = 0; O < octaves.Length; O ++ )
                {
                    if(octaves[O].alternate)
                    {
                        var perl = Mathf.PerlinNoise((x * octaves[O].scale.x)/ dimension, (z * octaves[O].scale.y)/dimension) * Mathf.PI * 2f;
                        y += Mathf.Cos(perl + octaves[O].speed.magnitude * Time.time) * octaves[O].height;
                    }
                    else
                    {
                        var perl = Mathf.PerlinNoise((x * octaves[O].scale.x + Time.time * octaves[O].speed.x)/dimension, (z * octaves[O].scale.y + Time.time * octaves[O].speed.y) / dimension) - 0.5f; // perlinNoise gives value b/w 0 and 1 hence -0.5f. so that waves are made near the base(0).
                        y += perl * octaves[O].height;
                    }
                }
                verts[index(x,z)] = new Vector3(x,y,z);
            }
        }

        Mesh.vertices = verts;
        Mesh.RecalculateNormals();
    }

    [Serializable]
    public struct Octave
    {
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }

}
