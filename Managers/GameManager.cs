using Minesharp.Graphics;
using Minesharp.World;
using NoiseGeneration;

namespace Minesharp.Managers;

public class GameManager
{
    public Chunk ActiveChunk { get; }
    public Camera Camera { get; }
    public Color BackgroundColor;

    public GameManager(GraphicsDevice _graphics)
    {
        ActiveChunk = new(this, _graphics);

        Noise.NewGeneration(741784112);
        ActiveChunk.GenerateChunk();

        Camera = new Camera(Vector3.Zero, 20f, 75f, 40f, _graphics);
    }

    public void Update()
    {
        InputManager.Update(this);
        Camera.Update();
    }

    public void Draw(GraphicsDevice graphics)
    {
        BackgroundColor = new(0.01f, 0.25f, 0.98f);
        graphics.Clear(BackgroundColor);
        GraphicsManager.SetMatricesFromCamera(Camera);

        // Save the current graphics device state
        var graphicsState = new GraphicsDeviceState();

        ActiveChunk.Draw();

        graphicsState.Save(graphics);
        graphics.DepthStencilState = DepthStencilState.None;

        Global.SpriteBatch.Begin(SpriteSortMode.Immediate);

        Global.SpriteBatch.DrawString(Global.MCFontRegular, InputManager.SeedInput, new(0, 60), Color.Black);
        if (Camera.Freezed)
            Global.SpriteBatch.DrawString(Global.MCFontBig, "PAUSED!", new Vector2(GraphicsManager.SCREEN_WIDTH / 2 - 100, 100), Color.White); 
        Global.SpriteBatch.DrawString(Global.MCFontRegular, $"Elapsed Time: {ActiveChunk.TimeToGenerateChunk}ms", new Vector2(0, 40), Color.Black);
        Global.SpriteBatch.DrawString(Global.MCFontRegular, $"Seed: {Noise.ActiveGeneration.Seed}", new Vector2(0, 20), Color.Black);
        Global.SpriteBatch.DrawString(Global.MCFontRegular, $"Camera Position: ({(int)Camera.Position.X}, {(int)Camera.Position.Y}, {(int)Camera.Position.Z})", Vector2.Zero, Color.Black);

        Global.SpriteBatch.End();

        // Restore the graphics device state to continue 3D drawing
        graphics.DepthStencilState = DepthStencilState.Default;
        graphicsState.Restore(graphics);
    }
}
