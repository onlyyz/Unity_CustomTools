using System.Collections.Generic;
using UnityEngine;

namespace CustomPlugins.Gradients
{
    public class GradientData
    {
        public static string
            Gradient = "_Gradient";
    }
    
    public static class GradientLibrary
    {
        public static void GradientMapToShader(Material mat,Texture2D tex)
        {
            mat.SetTexture(GradientData.Gradient,tex);
        }
        public static void GradientMapToShader(Texture2D tex)
        {
            Shader.SetGlobalTexture(GradientData.Gradient,tex);
        }
        
        //Create Texture
        public static Texture2D Create( List<Gradient> Gradient, int width = 32, int height = 1)
        {
            var _GradientMap = new Texture2D(width, height, TextureFormat.ARGB32, false);
            _GradientMap.filterMode = FilterMode.Bilinear;
            float inv = 1f / (width - 1);

            int eachHeight = height / 1;
            if (Gradient.Count != 0)
            {
                eachHeight = height / Gradient.Count;
            }

            int howMany = 0;
            while (howMany != Gradient.Count)
            {
                int start = height - eachHeight * howMany - 1;
                int end = start - eachHeight;
                for (int y = start; y > end; y--)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var t = x * inv;
                        Color col = Gradient[howMany].Evaluate(t);
                        _GradientMap.SetPixel(x, y, col);
                    }
                }
                howMany++;
            }
            _GradientMap.Apply();
            return _GradientMap;
        }
        
        //Save Texture
        
    }
}
