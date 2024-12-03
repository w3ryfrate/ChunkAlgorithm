using Microsoft.Xna.Framework.Content;

namespace Minesharp;

public static class Global
{
    public static Game GameInstance;
    public static float DeltaTime { get; private set; }

    public static SpriteBatch SpriteBatch;

    public static SpriteFont MCFontRegular;
    public static SpriteFont MCFontBig;

    public static ContentManager Content;

    public static void YieldDeltaTime(GameTime gt) => DeltaTime = (float)gt.ElapsedGameTime.TotalSeconds;
}
