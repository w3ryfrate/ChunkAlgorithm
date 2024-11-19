using Minesharp.Managers;

namespace Minesharp.World;

public enum Material
{
    DIRT,
    SAND,
    STONE
}

public class BlockGenericData
{
    public Material Material { get; }

    public Texture2D Texture { get; private set; }
    public float TimeToDestroy { get; private set; }
    public bool Collidable { get; private set; }
    public bool Gravity { get; private set; }

    public BlockGenericData(Material mat)
    {
        Material = mat;
        AssignPropetiesForMat(mat);
    }

    private void AssignPropetiesForMat(Material mat)
    {
        switch (mat)
        {
            case Material.DIRT:
                Texture = TexturesManager.GetTexture("DIRT");
                TimeToDestroy = 1f;
                Collidable = true;
                Gravity = false;
                break;

            case Material.SAND:
                Texture = TexturesManager.GetTexture("SAND");
                TimeToDestroy = 0.5f;
                Collidable = true;
                Gravity = true;
                break;

            case Material.STONE:
                Texture = TexturesManager.GetTexture("STONE");
                TimeToDestroy = 60f;
                Collidable = true;
                Gravity = false;
                break;
        }
    }
}



