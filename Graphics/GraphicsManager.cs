namespace Minesharp.Graphics;

public static class GraphicsManager
{
    public const int SCREEN_WIDTH = 800;
    public const int SCREEN_HEIGHT = 600;

    public static Matrix View { get; private set; }
    public static Matrix Projection { get; private set; }

    public static void SetScreenBounds(GraphicsDeviceManager graphics)
    {
        graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
        graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
        graphics.ApplyChanges();
    }

    public static void SetMatricesFromCamera(Camera camera)
    {
        View = camera.GetViewMatrix();
        Projection = camera.GetProjectionMatrix();
    }
}
