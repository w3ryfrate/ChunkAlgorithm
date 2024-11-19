﻿global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
using Minesharp.Graphics;
using Minesharp.Managers;

namespace Minesharp;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;

    private GameManager _gameManager;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
        Window.AllowAltF4 = false;
        Window.TextInput += InputManager.OnTextInput;
    }

    protected override void Initialize()
    {
        Global.Content = Content;
        Global.Game = this;

        GraphicsManager.SetScreenBounds(_graphics);

        base.Initialize();

        _gameManager = new(GraphicsDevice);
    }

    protected override void LoadContent()
    {
        Global.SpriteBatch = new SpriteBatch(GraphicsDevice);
        Global.DebugFont = Content.Load<SpriteFont>("DebuggingTools/minecraftRegular");

        TexturesManager.LoadAllTextures();
    }

    protected override void Update(GameTime gameTime)
    {
        _gameManager.Update();
        Global.YieldDeltaTime(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _gameManager.Draw(GraphicsDevice);
        
        base.Draw(gameTime);
    }

}
