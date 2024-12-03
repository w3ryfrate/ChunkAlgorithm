using Microsoft.Xna.Framework.Input;
using NoiseGeneration;
using System;
using System.Linq;

namespace Minesharp.Managers;

public static class InputManager
{
    public const Keys DEBUG_KEY = Keys.Tab;

    private static bool _inputAllowed = true;

    public static string SeedInput = string.Empty;
    public static bool DebugKeyPressed { get; private set; }

    private readonly static Random _random = new();
    private static KeyboardState KeyInput;

    public static void Update(GameManager gameManager)
    {
        InputListener(gameManager);
        KeyInput = Keyboard.GetState();
    }

    private static void InputListener(GameManager gameManager)
    {
        if (KeyInput.IsKeyDown(Keys.Escape))
        {
            Global.GameInstance.IsMouseVisible = true;
            gameManager.Camera.Freezed = true;
        }
        else if (KeyInput.IsKeyDown(Keys.L))
        {
            Global.GameInstance.IsMouseVisible = false;
            gameManager.Camera.Freezed = false;
        }

        if (KeyInput.IsKeyUp(Keys.R))
        {
            _inputAllowed = true;
        }

        if (KeyInput.IsKeyDown(Keys.R) && _inputAllowed)
        {
            int newSeed = _random.Next(0, int.MaxValue);
            Noise.NewGeneration(newSeed);
            gameManager.ActiveChunk.GenerateChunk();
            _inputAllowed = false;
        }

        /*
         *  FIX: Doesn't work at all for some reason, probably an issue with
         *  TryParse??
         */
        if (KeyInput.IsKeyDown(Keys.G) &&
            !string.IsNullOrEmpty(SeedInput) &&
            int.TryParse(SeedInput, out int seedValue) &&
            seedValue <= int.MaxValue)
        {
            Noise.NewGeneration(seedValue);
            gameManager.ActiveChunk.GenerateChunk();
            SeedInput = string.Empty;
        }

        DebugKeyPressed = KeyInput.IsKeyDown(DEBUG_KEY);
    }

    public static void OnTextInput(object sender, TextInputEventArgs e)
    {
        if (char.IsDigit(e.Character) && SeedInput.Length < 10)
        {
            SeedInput += e.Character.ToString();
        }
        else
        {
            SeedInput = string.Empty;
        }
    }
}
