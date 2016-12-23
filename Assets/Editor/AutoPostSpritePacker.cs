using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AutoPostSpritePacker : AssetPostprocessor
{
    public void OnPostprocessTexture(Texture2D texture)
    {
        if(assetPath.StartsWith("Assets/Textures"))
        {
            string AtlasName = new DirectoryInfo(Path.GetDirectoryName(assetPath)).Name;
            TextureImporter textureImporter = assetImporter as TextureImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spritePackingTag = AtlasName;
            textureImporter.mipmapEnabled = false;
            textureImporter.allowAlphaSplitting = true;
            textureImporter.alphaIsTransparency = true;
            textureImporter.textureCompression = TextureImporterCompression.Compressed;
        }
    }
}
