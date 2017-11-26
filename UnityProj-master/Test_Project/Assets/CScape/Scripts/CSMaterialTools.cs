#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class CSMaterialTools : MonoBehaviour
{

    //public Texture2D[] maskTex;
    //public Texture2D[] aoTex;
    public int slices = 40;
    public int slicesSurface = 10;
    public int slicesBlinds = 10;
    public int slicesStreets = 2;
    public int slicesDirt_Illumination = 10;
    public Texture2DArray normalArray;
    public Texture2DArray maskArray;
    public Texture2DArray surfaceArray;
    public Texture2DArray surfaceNormalArray;
    public Texture2DArray blindsArray;
    public Texture2DArray streetsArray;
    public Texture2DArray Dirt_IlluminationArray;
    public Material cityMaterial;
    public Material streetsMaterial;
    
    public int size;
    public int surfaceSize;
    public int blindsSize;
    public int streetsSize;
    public int Dirt_IlluminationSize;
    public bool generate = false;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        if (generate)
        {
            CreateNormal();
            CreateMaskDepth();
            CreateMaterials();
            CreateBlinds();
            CreateStreets();
            CreateMaterialsNormal();
      //      CreateDirt();

            generate = false;
        }
    }

    // Update is called once per frame


    // Update is called once per frame
    public void CreateNormal()
    {
        // CHANGEME: Filepath must be under "Resources" and named appropriately. Extension is ignored.
        // {0:000} means zero padding of 3 digits, i.e. 001, 002, 003 ... 010, 011, 012, ...
        string filePattern = "Normal/normal_{0:000}";
        string filePatternAO = "AO/ao_{0:000}";

        // CHANGEME: Number of textures you want to add in the array


        // CHANGEME: TextureFormat.RGB24 is good for PNG files with no alpha channels. Use TextureFormat.RGB32 with alpha.
        // See Texture2DArray in unity scripting API.
        Texture2DArray textureArray = new Texture2DArray(size, size, slices, TextureFormat.ARGB32, true, true);

        // CHANGEME: If your files start at 001, use i = 1. Otherwise change to what you got.
        for (int i = 0; i < slices; i++)
        {
            string filename = string.Format(filePattern, i + 1);
            string filenameAO = string.Format(filePatternAO, i + 1);
            Debug.Log("Loading " + filename);
            Texture2D tex = (Texture2D)Resources.Load(filename);
            Texture2D texAO = (Texture2D)Resources.Load(filenameAO);
            //tex.Resize(size, size, TextureFormat.ARGB32, false);
            //texAO.Resize(size, size, TextureFormat.ARGB32, false);
            //tex.Apply();
            //texAO.Apply();

            Texture2D scaleTex = scaled(tex, size, size, 0);
            Texture2D scaleTexAO = scaled(texAO, size, size, 0);
            Color[] texCol = scaleTex.GetPixels(0);
            Color[] texColAO = scaleTexAO.GetPixels(0);
            Color[] texComposite = texCol;
            for (int j = 0; j < texCol.Length; j++)
            {
                texComposite[j] = new Color(texCol[j].r, texCol[j].g, texCol[j].b, texColAO[j].r);
            }
            textureArray.SetPixels(texComposite, i, 0);
        }
        textureArray.Apply();

        // CHANGEME: Path where you want to save the texture array. It must end in .asset extension for Unity to recognise it.
        string path = "Assets/CScape/Editor/Resources/TextureArrays/NormaltextureArray.asset";
        AssetDatabase.CreateAsset(textureArray, path);
        Debug.Log("Saved asset to " + path);
        normalArray = Resources.Load("TextureArrays/NormaltextureArray") as Texture2DArray;
        cityMaterial.SetTexture("_NormalTextureArray", normalArray);


    }


    public void CreateMaskDepth()
    {
        // CHANGEME: Filepath must be under "Resources" and named appropriately. Extension is ignored.
        // {0:000} means zero padding of 3 digits, i.e. 001, 002, 003 ... 010, 011, 012, ...
        string filePattern = "Mask/diff_{0:000}";
        string filePatternAO = "Depth/depth_{0:000}";

        Texture2DArray textureArray = new Texture2DArray(size, size, slices, TextureFormat.ARGB32, true, true);

        // CHANGEME: If your files start at 001, use i = 1. Otherwise change to what you got.
        for (int i = 0; i < slices; i++)
        {
            string filename = string.Format(filePattern, i + 1);
            string filenameAO = string.Format(filePatternAO, i + 1);
            Debug.Log("Loading " + filename);
            Texture2D tex = (Texture2D)Resources.Load(filename);
            Texture2D texAO = (Texture2D)Resources.Load(filenameAO);
            //tex.Resize(size, size, TextureFormat.ARGB32, false);
            //texAO.Resize(size, size, TextureFormat.ARGB32, false);
            //tex.Apply();
            //texAO.Apply();

            Texture2D scaleTex = scaled(tex, size, size, 0);
            Texture2D scaleTexAO = scaled(texAO, size, size, 0);
            Color[] texCol = scaleTex.GetPixels(0);
            Color[] texColAO = scaleTexAO.GetPixels(0);
            Color[] texComposite = texCol;
            for (int j = 0; j < texCol.Length; j++)
            {
                texComposite[j] = new Color(texCol[j].r, texCol[j].g, texCol[j].b, texColAO[j].r);
            }
            
            textureArray.SetPixels(texComposite, i, 0);
        }
        textureArray.Apply();

        // CHANGEME: Path where you want to save the texture array. It must end in .asset extension for Unity to recognise it.
        string path = "Assets/CScape/Editor/Resources/TextureArrays/MaskDepthArray.asset";
        AssetDatabase.CreateAsset(textureArray, path);
        Debug.Log("Saved asset to " + path);
        maskArray = Resources.Load("TextureArrays/MaskDepthArray") as Texture2DArray;
        cityMaterial.SetTexture("_MaskTexArray", maskArray);


    }


    public void CreateMaterials()
    {
        // CHANGEME: Filepath must be under "Resources" and named appropriately. Extension is ignored.
        // {0:000} means zero padding of 3 digits, i.e. 001, 002, 003 ... 010, 011, 012, ...
        string filePattern = "Surfaces/basecolor_surface_{0:00}";
       
        string filePatternAO = "Surfaces/roughness_surface_{0:00}";
        // string filePatternAO = "CScapeCDK/wallTextures/wall_{0:000}";

        Texture2DArray textureArray = new Texture2DArray(surfaceSize, surfaceSize, slicesSurface, TextureFormat.ARGB32, true, true);

        // CHANGEME: If your files start at 001, use i = 1. Otherwise change to what you got.
        for (int i = 0; i < slicesSurface; i++)
        {
            string filename = string.Format(filePattern, i + 1);
            string filenameAO = string.Format(filePatternAO, i + 1);
            Debug.Log("Loading " + filename);
            Texture2D tex = (Texture2D)Resources.Load(filename);
            Texture2D texAO = (Texture2D)Resources.Load(filenameAO);
            //tex.Resize(size, size, TextureFormat.ARGB32, false);
            //texAO.Resize(size, size, TextureFormat.ARGB32, false);
            //tex.Apply();
            //texAO.Apply();

            Texture2D scaleTex = scaled(tex, surfaceSize, surfaceSize, 0);
            Texture2D scaleTexAO = scaled(texAO, surfaceSize, surfaceSize, 0);
            Color[] texCol = scaleTex.GetPixels(0);
            Color[] texColAO = scaleTexAO.GetPixels(0);
            Color[] texComposite = texCol;
            for (int j = 0; j < texCol.Length; j++)
            {
                texComposite[j] = new Color(texCol[j].r, texCol[j].g, texCol[j].b, texColAO[j].r);
            }
            textureArray.SetPixels(texCol, i, 0);
        }
        textureArray.Apply();

        // CHANGEME: Path where you want to save the texture array. It must end in .asset extension for Unity to recognise it.
        string path = "Assets/CScape/Editor/Resources/TextureArrays/SurfaceArray.asset";
        AssetDatabase.CreateAsset(textureArray, path);
        Debug.Log("Saved asset to " + path);
        surfaceArray = Resources.Load("TextureArrays/SurfaceArray") as Texture2DArray;
        cityMaterial.SetTexture("_SurfaceAray", surfaceArray);
        cityMaterial.SetTexture("_WallsArray", surfaceArray);


    }

    public void CreateMaterialsNormal()
    {
        // CHANGEME: Filepath must be under "Resources" and named appropriately. Extension is ignored.
        // {0:000} means zero padding of 3 digits, i.e. 001, 002, 003 ... 010, 011, 012, ...
        //  metallic_surface
        string filePattern = "Surfaces/normal_surface_{0:00}";
        string filePatternAO = "Surfaces/metallic_surface_{0:00}";

        Texture2DArray textureArray = new Texture2DArray(surfaceSize, surfaceSize, slicesSurface, TextureFormat.ARGB32, true, true);

        // CHANGEME: If your files start at 001, use i = 1. Otherwise change to what you got.
        for (int i = 0; i < slicesSurface; i++)
        {
            string filename = string.Format(filePattern, i + 1);
            string filenameAO = string.Format(filePatternAO, i + 1);
            Debug.Log("Loading " + filename);
            Texture2D tex = (Texture2D)Resources.Load(filename);
            Texture2D texAO = (Texture2D)Resources.Load(filenameAO);
            //tex.Resize(size, size, TextureFormat.ARGB32, false);
            //texAO.Resize(size, size, TextureFormat.ARGB32, false);
            //tex.Apply();
            //texAO.Apply();

            Texture2D scaleTex = scaled(tex, surfaceSize, surfaceSize, 0);
            Texture2D scaleTexAO = scaled(texAO, surfaceSize, surfaceSize, 0);
            Color[] texCol = scaleTex.GetPixels(0);
            Color[] texColAO = scaleTexAO.GetPixels(0);
            Color[] texComposite = texCol;
            for (int j = 0; j < texCol.Length; j++)
            {
                texComposite[j] = new Color(texCol[j].r, texCol[j].g, texCol[j].b, texColAO[j].r);
            }
            textureArray.SetPixels(texCol, i, 0);
        }
        textureArray.Apply();

        // CHANGEME: Path where you want to save the texture array. It must end in .asset extension for Unity to recognise it.
        string path = "Assets/CScape/Editor/Resources/TextureArrays/SurfaceNormalArray.asset";
        AssetDatabase.CreateAsset(textureArray, path);
        Debug.Log("Saved asset to " + path);
        surfaceNormalArray = Resources.Load("TextureArrays/SurfaceNormalArray") as Texture2DArray;
        cityMaterial.SetTexture("_SurfaceNormalArray", surfaceNormalArray);
        //  cityMaterial.SetTexture("_WallsNormalArray", surfaceArray);


    }

    public void CreateDirt()
    {
        // CHANGEME: Filepath must be under "Resources" and named appropriately. Extension is ignored.
        // {0:000} means zero padding of 3 digits, i.e. 001, 002, 003 ... 010, 011, 012, ...
        //  metallic_surface
        string filePattern = "Dirt_Illumination/dirt_{0:00}";
        string filePatternAO = "Dirt_Illumination/illum_{0:00}";

        Texture2DArray textureArray = new Texture2DArray(Dirt_IlluminationSize, Dirt_IlluminationSize, slicesDirt_Illumination, TextureFormat.ARGB32, true, true);

        // CHANGEME: If your files start at 001, use i = 1. Otherwise change to what you got.
        for (int i = 0; i < slicesDirt_Illumination; i++)
        {
            string filename = string.Format(filePattern, i + 1);
            string filenameAO = string.Format(filePatternAO, i + 1);
            Debug.Log("Loading " + filename);
            Texture2D tex = (Texture2D)Resources.Load(filename);
            Texture2D texAO = (Texture2D)Resources.Load(filenameAO);
            //tex.Resize(size, size, TextureFormat.ARGB32, false);
            //texAO.Resize(size, size, TextureFormat.ARGB32, false);
            //tex.Apply();
            //texAO.Apply();

            Texture2D scaleTex = scaled(tex, Dirt_IlluminationSize, Dirt_IlluminationSize, 0);
            Texture2D scaleTexAO = scaled(texAO, Dirt_IlluminationSize, Dirt_IlluminationSize, 0);
            Color[] texCol = scaleTex.GetPixels(0);
            Color[] texColAO = scaleTexAO.GetPixels(0);
            Color[] texComposite = texCol;
            for (int j = 0; j < texCol.Length; j++)
            {
                texComposite[j] = new Color(texCol[j].r, texColAO[j].r, texCol[j].b, texColAO[j].r);
            }
            textureArray.SetPixels(texCol, i, 0);
        }
        textureArray.Apply();

        // CHANGEME: Path where you want to save the texture array. It must end in .asset extension for Unity to recognise it.
        string path = "Assets/CScape/Editor/Resources/TextureArrays/Dirt_IlluminationArray.asset";
        AssetDatabase.CreateAsset(textureArray, path);
        Debug.Log("Saved asset to " + path);
        Dirt_IlluminationArray = Resources.Load("TextureArrays/Dirt_IlluminationArray") as Texture2DArray;
        cityMaterial.SetTexture("_SurfaceNormalArray", Dirt_IlluminationArray);
        //  cityMaterial.SetTexture("_WallsNormalArray", surfaceArray);


    }

    public void CreateBlinds()
    {
        // CHANGEME: Filepath must be under "Resources" and named appropriately. Extension is ignored.
        // {0:000} means zero padding of 3 digits, i.e. 001, 002, 003 ... 010, 011, 012, ...
        string filePattern = "Blinds/Blinds_{0:000}";
       // string filePatternAO = "CScapeCDK/Blinds/Blinds_{0:000}";

        Texture2DArray textureArray = new Texture2DArray(blindsSize, blindsSize, slicesBlinds, TextureFormat.ARGB32, true, true);

        // CHANGEME: If your files start at 001, use i = 1. Otherwise change to what you got.
        for (int i = 0; i < slicesBlinds; i++)
        {
            string filename = string.Format(filePattern, i + 1);
       //     string filenameAO = string.Format(filePatternAO, i + 1);
            Debug.Log("Loading " + filename);
            Texture2D tex = (Texture2D)Resources.Load(filename);
      //      Texture2D texAO = (Texture2D)Resources.Load(filenameAO);
            //tex.Resize(size, size, TextureFormat.ARGB32, false);
            //texAO.Resize(size, size, TextureFormat.ARGB32, false);
            //tex.Apply();
            //texAO.Apply();

            Texture2D scaleTex = scaled(tex, blindsSize, blindsSize, 0);
            Texture2D scaleTexAO = scaled(tex, blindsSize, blindsSize, 0);
            Color[] texCol = scaleTex.GetPixels(0);
            Color[] texColAO = scaleTexAO.GetPixels(0);
            Color[] texComposite = texCol;
            for (int j = 0; j < texCol.Length; j++)
            {
                texComposite[j] = new Color(texCol[j].r, texCol[j].g, texCol[j].b, texColAO[j].a);
            }
            textureArray.SetPixels(texCol, i, 0);
        }
        textureArray.Apply();

        // CHANGEME: Path where you want to save the texture array. It must end in .asset extension for Unity to recognise it.
        string path = "Assets/CScape/Editor/Resources/TextureArrays/blindsArray.asset";
        AssetDatabase.CreateAsset(textureArray, path);
        Debug.Log("Saved asset to " + path);
        blindsArray = Resources.Load("TextureArrays/blindsArray") as Texture2DArray;
        cityMaterial.SetTexture("_BlindsArray", blindsArray);


    }

    public void CreateStreets()
    {
        // CHANGEME: Filepath must be under "Resources" and named appropriately. Extension is ignored.
        // {0:000} means zero padding of 3 digits, i.e. 001, 002, 003 ... 010, 011, 012, ...
        string filePattern = "Street/StreetMap_{0:000}";
     //   string filePatternAO = "CScapeCDK/Street/StreetMap_{0:000}";

        Texture2DArray textureArray = new Texture2DArray(streetsSize, streetsSize, slicesStreets, TextureFormat.ARGB32, true, true);

        // CHANGEME: If your files start at 001, use i = 1. Otherwise change to what you got.
        for (int i = 0; i < slicesStreets; i++)
        {
            string filename = string.Format(filePattern, i + 1);
      //      string filenameAO = string.Format(filePatternAO, i + 1);
            Debug.Log("Loading " + filename);
            Texture2D tex = (Texture2D)Resources.Load(filename);
        //    Texture2D texAO = (Texture2D)Resources.Load(filenameAO);
            //tex.Resize(size, size, TextureFormat.ARGB32, false);
            //texAO.Resize(size, size, TextureFormat.ARGB32, false);
            //tex.Apply();
            //texAO.Apply();

            Texture2D scaleTex = scaled(tex, streetsSize, streetsSize, 0);
            Texture2D scaleTexAO = scaled(tex, streetsSize, streetsSize, 0);
            Color[] texCol = scaleTex.GetPixels(0);
            Color[] texColAO = scaleTexAO.GetPixels(0);
            Color[] texComposite = texCol;
            for (int j = 0; j < texCol.Length; j++)
            {
                texComposite[j] = new Color(texCol[j].r, texCol[j].g, texCol[j].b, texColAO[j].a);
            }
            textureArray.SetPixels(texCol, i, 0);
        }
        textureArray.Apply();


        string path = "Assets/CScape/Editor/Resources/TextureArrays/streetsArray.asset";
        AssetDatabase.CreateAsset(textureArray, path);
        Debug.Log("Saved asset to " + path);
        streetsArray = Resources.Load("TextureArrays/streetsArray") as Texture2DArray;
        streetsMaterial.SetTexture("_StreetsArray", streetsArray);


    }

    public static Texture2D scaled(Texture2D src, int width, int height, FilterMode mode = FilterMode.Trilinear)
    {
        Rect texR = new Rect(0, 0, width, height);
        _gpu_scale(src, width, height, mode);

        //Get rendered data back to a new texture
        Texture2D result = new Texture2D(width, height, TextureFormat.ARGB32, true);
        result.Resize(width, height);
        result.ReadPixels(texR, 0, 0, true);
        return result;
    }

    /// <summary>
    /// Scales the texture data of the given texture.
    /// </summary>
    /// <param name="tex">Texure to scale</param>
    /// <param name="width">New width</param>
    /// <param name="height">New height</param>
    /// <param name="mode">Filtering mode</param>
    public static void scale(Texture2D tex, int width, int height, FilterMode mode = FilterMode.Trilinear)
    {
        Rect texR = new Rect(0, 0, width, height);
        _gpu_scale(tex, width, height, mode);

        // Update new texture
        tex.Resize(width, height);
        tex.ReadPixels(texR, 0, 0, true);
        tex.Apply(true);        //Remove this if you hate us applying textures for you :)
    }

    // Internal unility that renders the source texture into the RTT - the scaling method itself.
    static void _gpu_scale(Texture2D src, int width, int height, FilterMode fmode)
    {
        //We need the source texture in VRAM because we render with it
        src.filterMode = fmode;
        src.Apply(true);

        //Using RTT for best quality and performance. Thanks, Unity 5
        RenderTexture rtt = new RenderTexture(width, height, 32);

        //Set the RTT in order to render to it
        Graphics.SetRenderTarget(rtt);

        //Setup 2D matrix in range 0..1, so nobody needs to care about sized
        GL.LoadPixelMatrix(0, 1, 1, 0);

        //Then clear & draw the texture to fill the entire RTT.
        GL.Clear(true, true, new Color(0, 0, 0, 0));
        Graphics.DrawTexture(new Rect(0, 0, 1, 1), src);
    }
}
#endif