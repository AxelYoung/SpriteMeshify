using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMeshify : MonoBehaviour
{
    public Texture2D goalSprite;
    public Material meshMaterial;
    public float pixelsPerUnit = 16f;
    float spriteHeight;
    float spriteWidth;

    GameObject meshObject;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    int currentPixel = 0;

    List<Vector3> meshVerticies = new List<Vector3>();
    List<int> meshTriangles = new List<int>();
    List<Vector2> meshUVs = new List<Vector2>();

    void Start()
    {
        meshObject = new GameObject();
        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshFilter.mesh = Meshify(goalSprite);
        meshRenderer.material = meshMaterial;
    }

    Mesh Meshify(Texture2D sprite)
    {
        spriteWidth = sprite.width;
        spriteHeight = sprite.height;
        Mesh mesh = new Mesh();
        for (int x = 0; x < spriteWidth; x++)
        {
            for (int y = 0; y < spriteHeight; y++)
            {
                if (sprite.GetPixel(x, y).a != 0)
                {
                    meshVerticies.Add(new Vector3(x / pixelsPerUnit, 1 / (pixelsPerUnit * 2), y / pixelsPerUnit));
                    meshVerticies.Add(new Vector3(x / pixelsPerUnit, 1 / (pixelsPerUnit * 2), (y + 1) / pixelsPerUnit));
                    meshVerticies.Add(new Vector3((x + 1) / pixelsPerUnit, 1 / (pixelsPerUnit * 2), (y + 1) / pixelsPerUnit));
                    meshVerticies.Add(new Vector3((x + 1) / pixelsPerUnit, 1 / (pixelsPerUnit * 2), y / pixelsPerUnit));

                    meshUVs.Add(new Vector2(x / spriteWidth, y / spriteHeight));
                    meshUVs.Add(new Vector2(x / spriteWidth, (y + 1) / spriteHeight));
                    meshUVs.Add(new Vector2((x + 1) / spriteWidth, (y + 1) / spriteHeight));
                    meshUVs.Add(new Vector2((x + 1) / spriteWidth, y / spriteHeight));

                    meshTriangles.Add(currentPixel * 4);
                    meshTriangles.Add((currentPixel * 4) + 1);
                    meshTriangles.Add((currentPixel * 4) + 2);
                    meshTriangles.Add((currentPixel * 4) + 2);
                    meshTriangles.Add((currentPixel * 4) + 3);
                    meshTriangles.Add(currentPixel * 4);

                    currentPixel++;

                    meshVerticies.Add(new Vector3(x / pixelsPerUnit, -1 / (pixelsPerUnit * 2), y / pixelsPerUnit));
                    meshVerticies.Add(new Vector3(x / pixelsPerUnit, -1 / (pixelsPerUnit * 2), (y + 1) / pixelsPerUnit));
                    meshVerticies.Add(new Vector3((x + 1) / pixelsPerUnit, -1 / (pixelsPerUnit * 2), (y + 1) / pixelsPerUnit));
                    meshVerticies.Add(new Vector3((x + 1) / pixelsPerUnit, -1 / (pixelsPerUnit * 2), y / pixelsPerUnit));

                    meshUVs.Add(new Vector2(x / spriteWidth, y / spriteHeight));
                    meshUVs.Add(new Vector2(x / spriteWidth, (y + 1) / spriteHeight));
                    meshUVs.Add(new Vector2((x + 1) / spriteWidth, (y + 1) / spriteHeight));
                    meshUVs.Add(new Vector2((x + 1) / spriteWidth, y / spriteHeight));

                    meshTriangles.Add((currentPixel * 4) + 2);
                    meshTriangles.Add((currentPixel * 4) + 1);
                    meshTriangles.Add(currentPixel * 4);
                    meshTriangles.Add(currentPixel * 4);
                    meshTriangles.Add((currentPixel * 4) + 3);
                    meshTriangles.Add((currentPixel * 4) + 2);

                    currentPixel++;

                    if (x + 1 < sprite.width)
                    {
                        if (sprite.GetPixel(x + 1, y).a == 0)
                        {
                            GenerateRightSide(x, y);
                        }
                    }
                    else
                    {
                        GenerateRightSide(x, y);
                    }

                    if (y + 1 < sprite.height)
                    {
                        if (sprite.GetPixel(x, y + 1).a == 0)
                        {
                            GenerateFrontSide(x, y);
                        }
                    }
                    else
                    {
                        GenerateFrontSide(x, y);
                    }

                    if (x - 1 >= 0)
                    {
                        if (sprite.GetPixel(x - 1, y).a == 0)
                        {
                            GenerateLeftSide(x, y);
                        }
                    }
                    else
                    {
                        GenerateLeftSide(x, y);
                    }

                    if (y - 1 >= 0)
                    {
                        if (sprite.GetPixel(x, y - 1).a == 0)
                        {
                            GenerateBackSide(x, y);
                        }
                    }
                    else
                    {
                        GenerateBackSide(x, y);
                    }
                }
            }
        }

        mesh.vertices = meshVerticies.ToArray();
        mesh.triangles = meshTriangles.ToArray();
        mesh.uv = meshUVs.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }

    void GenerateLeftSide(int x, int y)
    {

        meshVerticies.Add(new Vector3(x / pixelsPerUnit, -1 / (pixelsPerUnit * 2), (y + 1) / pixelsPerUnit));
        meshVerticies.Add(new Vector3(x / pixelsPerUnit, -1 / (pixelsPerUnit * 2), y / pixelsPerUnit));
        meshVerticies.Add(new Vector3(x / pixelsPerUnit, 1 / (pixelsPerUnit * 2), (y + 1) / pixelsPerUnit));
        meshVerticies.Add(new Vector3(x / pixelsPerUnit, 1 / (pixelsPerUnit * 2), y / pixelsPerUnit));

        meshUVs.Add(new Vector2(x / spriteWidth, y / spriteHeight));
        meshUVs.Add(new Vector2((x + 1) / spriteWidth, y / spriteHeight));
        meshUVs.Add(new Vector2((x + 1) / spriteWidth, (y + 1) / spriteHeight));
        meshUVs.Add(new Vector2(x / spriteWidth, (y + 1) / spriteHeight));

        meshTriangles.Add(currentPixel * 4);
        meshTriangles.Add((currentPixel * 4) + 2);
        meshTriangles.Add((currentPixel * 4) + 3);
        meshTriangles.Add((currentPixel * 4) + 3);
        meshTriangles.Add((currentPixel * 4) + 1);
        meshTriangles.Add(currentPixel * 4);

        currentPixel++;
    }

    void GenerateRightSide(int x, int y)
    {

        meshVerticies.Add(new Vector3((x + 1) / pixelsPerUnit, -1 / (pixelsPerUnit * 2), (y + 1) / pixelsPerUnit));
        meshVerticies.Add(new Vector3((x + 1) / pixelsPerUnit, -1 / (pixelsPerUnit * 2), y / pixelsPerUnit));
        meshVerticies.Add(new Vector3((x + 1) / pixelsPerUnit, 1 / (pixelsPerUnit * 2), (y + 1) / pixelsPerUnit));
        meshVerticies.Add(new Vector3((x + 1) / pixelsPerUnit, 1 / (pixelsPerUnit * 2), y / pixelsPerUnit));

        meshUVs.Add(new Vector2(x / spriteWidth, y / spriteHeight));
        meshUVs.Add(new Vector2((x + 1) / spriteWidth, y / spriteHeight));
        meshUVs.Add(new Vector2((x + 1) / spriteWidth, (y + 1) / spriteHeight));
        meshUVs.Add(new Vector2(x / spriteWidth, (y + 1) / spriteHeight));

        meshTriangles.Add((currentPixel * 4) + 3);
        meshTriangles.Add((currentPixel * 4) + 2);
        meshTriangles.Add(currentPixel * 4);
        meshTriangles.Add(currentPixel * 4);
        meshTriangles.Add((currentPixel * 4) + 1);
        meshTriangles.Add((currentPixel * 4) + 3);

        currentPixel++;
    }

    void GenerateFrontSide(int x, int y)
    {
        meshVerticies.Add(new Vector3((x + 1) / pixelsPerUnit, -1 / (pixelsPerUnit * 2), (y + 1) / pixelsPerUnit));
        meshVerticies.Add(new Vector3(x / pixelsPerUnit, -1 / (pixelsPerUnit * 2), (y + 1) / pixelsPerUnit));
        meshVerticies.Add(new Vector3((x + 1) / pixelsPerUnit, 1 / (pixelsPerUnit * 2), (y + 1) / pixelsPerUnit));
        meshVerticies.Add(new Vector3(x / pixelsPerUnit, 1 / (pixelsPerUnit * 2), (y + 1) / pixelsPerUnit));

        meshUVs.Add(new Vector2(x / spriteWidth, y / spriteHeight));
        meshUVs.Add(new Vector2((x + 1) / spriteWidth, y / spriteHeight));
        meshUVs.Add(new Vector2((x + 1) / spriteWidth, (y + 1) / spriteHeight));
        meshUVs.Add(new Vector2(x / spriteWidth, (y + 1) / spriteHeight));

        meshTriangles.Add(currentPixel * 4);
        meshTriangles.Add((currentPixel * 4) + 2);
        meshTriangles.Add((currentPixel * 4) + 3);
        meshTriangles.Add((currentPixel * 4) + 3);
        meshTriangles.Add((currentPixel * 4) + 1);
        meshTriangles.Add(currentPixel * 4);

        currentPixel++;
    }

    void GenerateBackSide(int x, int y)
    {
        meshVerticies.Add(new Vector3((x + 1) / pixelsPerUnit, -1 / (pixelsPerUnit * 2), y / pixelsPerUnit));
        meshVerticies.Add(new Vector3(x / pixelsPerUnit, -1 / (pixelsPerUnit * 2), y / pixelsPerUnit));
        meshVerticies.Add(new Vector3((x + 1) / pixelsPerUnit, 1 / (pixelsPerUnit * 2), y / pixelsPerUnit));
        meshVerticies.Add(new Vector3(x / pixelsPerUnit, 1 / (pixelsPerUnit * 2), y / pixelsPerUnit));

        meshUVs.Add(new Vector2(x / spriteWidth, y / spriteHeight));
        meshUVs.Add(new Vector2((x + 1) / spriteWidth, y / spriteHeight));
        meshUVs.Add(new Vector2((x + 1) / spriteWidth, (y + 1) / spriteHeight));
        meshUVs.Add(new Vector2(x / spriteWidth, (y + 1) / spriteHeight));

        meshTriangles.Add((currentPixel * 4) + 3);
        meshTriangles.Add((currentPixel * 4) + 2);
        meshTriangles.Add(currentPixel * 4);
        meshTriangles.Add(currentPixel * 4);
        meshTriangles.Add((currentPixel * 4) + 1);
        meshTriangles.Add((currentPixel * 4) + 3);

        currentPixel++;
    }
}
