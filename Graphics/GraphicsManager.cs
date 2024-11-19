namespace Minesharp.Graphics;

public static class GraphicsManager
{
    public const int SCREEN_WIDTH = 1600;
    public const int SCREEN_HEIGHT = 900;

    public static Matrix View { get; private set; }
    public static Matrix Projection { get; private set; }

    public static void SetScreenBounds(GraphicsDeviceManager graphics)
    {
        graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
        graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
        graphics.ApplyChanges();
    }

    public static void YieldViewAndProjectionMatrices(Camera camera)
    {
        View = camera.GetViewMatrix();
        Projection = camera.GetProjectionMatrix();
    }
}
