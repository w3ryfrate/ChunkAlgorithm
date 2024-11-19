
using System.Collections.Generic;

namespace Minesharp.Managers;

public static class TexturesManager
{
    private static Dictionary<string, Texture2D> _textures;

    public static void LoadAllTextures()
    {
        _textures = new()
        {
            { "DIRT",  Global.Content.Load<Texture2D>("Textures/dirtTex") },

            // To be added, giggity giggity 
            //{ "SAND",  Global.Content.Load<Texture2D>("Textures/sandTex") },
            //{ "STONE",  Global.Content.Load<Texture2D>("Textures/stoneTex") },
        };
    }

    public static Texture2D GetTexture(string name)
    {
        return _textures[name];
    }
}
