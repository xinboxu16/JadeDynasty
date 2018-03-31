using UnityEngine;
using System.Collections;
using UnityEditor;

public class ArtistFont : MonoBehaviour
{
    public static void BatchCreateArtistFont(float offset)
    {
        string dirName = "";
        string fntname = EditorUtils.SelectObjectPathInfo(ref dirName).Split('.')[0];
        Debug.Log(fntname);
        Debug.Log(dirName);

        string fntFileName = dirName + fntname + ".fnt";

        Font CustomFont = AssetDatabase.LoadAssetAtPath(dirName + fntname + ".fontsettings", typeof(Font)) as Font;
        if (CustomFont == null)
        {
            CustomFont = new Font();
            AssetDatabase.CreateAsset(CustomFont, dirName + fntname + ".fontsettings");
        }
        else
        {
            EditorUtility.SetDirty(CustomFont);
        }

        TextAsset BMFontText = null;
        {
            BMFontText = AssetDatabase.LoadAssetAtPath(fntFileName, typeof(TextAsset)) as TextAsset;
        }

        BMFont mbFont = new BMFont();
        BMFontReader.Load(mbFont, BMFontText.name, BMFontText.bytes);  // 借用NGUI封装的读取类
        CharacterInfo[] characterInfo = new CharacterInfo[mbFont.glyphs.Count];
        for (int i = 0; i < mbFont.glyphs.Count; i++)
        {
            BMGlyph bmInfo = mbFont.glyphs[i];
            CharacterInfo info = new CharacterInfo();
            info.index = bmInfo.index;
            float uvx = (float)bmInfo.x / (float)mbFont.texWidth;
            float uvy = 1 - (float)bmInfo.y / (float)mbFont.texHeight;
            float uvw = (float)bmInfo.width / (float)mbFont.texWidth;
            float uvh = -1f * (float)bmInfo.height / (float)mbFont.texHeight;

            info.uvBottomLeft = new Vector2(uvx, uvy);
            info.uvBottomRight = new Vector2(uvx + uvw, uvy);
            info.uvTopLeft = new Vector2(uvx, uvy + uvh);
            info.uvTopRight = new Vector2(uvx + uvw, uvy + uvh);

            info.minX = bmInfo.offsetX;
            info.minY = (int)((float)bmInfo.offsetY - (float)bmInfo.height * (offset));
            info.glyphWidth = bmInfo.width;
            info.glyphHeight = -bmInfo.height;
            info.advance = bmInfo.advance;

            characterInfo[i] = info;
        }
        CustomFont.characterInfo = characterInfo;


        string textureFilename = dirName + mbFont.spriteName + ".png";
        Material mat = null;
        {
            Shader shader = Shader.Find("Transparent/Diffuse");
            mat = new Material(shader);
            Texture tex = AssetDatabase.LoadAssetAtPath(textureFilename, typeof(Texture)) as Texture;
            mat.SetTexture("_MainTex", tex);
            AssetDatabase.CreateAsset(mat, dirName + fntname + ".mat");
            //AssetDatabase.SaveAssets();
        }
        CustomFont.material = mat;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.SetDirty(CustomFont);
    }
}
