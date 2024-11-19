using Minesharp.Graphics;
using Minesharp.World;
using NoiseGeneration;

namespace Minesharp.Managers;

public class GameManager
{
    public Chunk ActiveChunk { get; }
    public Camera Camera { get; }
    public Color BackgroundColor { get; } 

    public GameManager(GraphicsDevice _graphics)
    {
        ActiveChunk = new(this, _graphics);
        BackgroundColor = new(135, 206, 255);

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
        graphics.Clear(BackgroundColor);
        GraphicsManager.YieldViewAndProjectionMatrices(Camera);

        // Save the current graphics device state
        var graphicsState = new GraphicsDeviceState();

        ActiveChunk.Draw();

        graphicsState.Save(graphics);
        graphics.DepthStencilState = DepthStencilState.None;

        Global.SpriteBatch.Begin(SpriteSortMode.Immediate);

        Global.SpriteBatch.DrawString(Global.DebugFont, InputManager.SeedInput, new(0, 60), Color.Black);
        Global.SpriteBatch.DrawString(Global.DebugFont, $"Elapsed Time: {ActiveChunk.TimeToGenerateChunk}ms", new Vector2(0, 40), Color.Black);
        Global.SpriteBatch.DrawString(Global.DebugFont, $"Seed: {Noise.ActiveGeneration.Seed}", new Vector2(0, 20), Color.Black);
        Global.SpriteBatch.DrawString(Global.DebugFont, $"Camera Position: ({(int)Camera.Position.X}, {(int)Camera.Position.Y}, {(int)Camera.Position.Z})", Vector2.Zero, Color.Black);

        Global.SpriteBatch.End();

        // Restore the graphics device state to continue 3D drawing
        graphics.DepthStencilState = DepthStencilState.Default;
        graphicsState.Restore(graphics);
    }
}
