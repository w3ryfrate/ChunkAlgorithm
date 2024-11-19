using Minesharp;
using System;

/* 
 *  SEEDS TO TEST:
 *  741784112
 */
namespace NoiseGeneration;

public readonly struct GenerationInstance
{
    public const int DEFAULT_WIDTH = 16;
    public const int DEFAULT_HEIGHT = 16;

    public NoiseElement[,] Elements { get; }
    public int Seed { get; }

    public GenerationInstance(NoiseElement[,] values, int seed)
    {
        Elements = values;
        Seed = seed;
    }
}

public static class Noise
{
    public static GenerationInstance ActiveGeneration { get; private set; }

    public static void NewGeneration(int seed = 0, int width = GenerationInstance.DEFAULT_WIDTH, int height = GenerationInstance.DEFAULT_HEIGHT)
    {
        ActiveGeneration = new(GenerateElementArray(seed, width, height, 1f), seed);

        for (int row = 0; row < ActiveGeneration.Elements.GetLength(0); row++)
        {
            for (int column = 0; column < ActiveGeneration.Elements.GetLength(1); column++)
            {
                NoiseElement element = ActiveGeneration.Elements[row, column];
                // Re-assign to ensure updates are reflected in the array
                ActiveGeneration.Elements[row, column] = element;
            }
        }
    }

    private static NoiseElement[,] GenerateElementArray(int seed, int width, int height, float scale)
    {
        NoiseElement[,] noise = new NoiseElement[width, height];
        Random random = new(seed);
        PerlinNoiseGenerator perlin = new(random);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float xCoord = (float)i / width * scale;
                float yCoord = (float)j / height * scale;
                float value = perlin.Generate(xCoord, yCoord);

                // Initialize and assign the NoiseElement
                noise[i, j] = new NoiseElement(value);
            }
        }

        return noise;
    }
}

internal class PerlinNoiseGenerator
{
    private readonly int[] _permutation;

    public PerlinNoiseGenerator(Random random)
    {
        _permutation = new int[512];
        for (int i = 0; i < 256; i++)
            _permutation[i] = i;

        // Shuffle using the provided seed
        for (int i = 255; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (_permutation[i], _permutation[j]) = (_permutation[j], _permutation[i]);
        }

        for (int i = 0; i < 256; i++)
            _permutation[256 + i] = _permutation[i];
    }

    public float Generate(float x, float y)
    {                   
        int xi = (int)x & 255;
        int yi = (int)y & 255;

        float xf = x - (int)x;
        float yf = y - (int)y;

        float u = Fade(xf);
        float v = Fade(yf);

        int aa = _permutation[_permutation[xi] + yi];
        int ab = _permutation[_permutation[xi] + yi + 1];
        int ba = _permutation[_permutation[xi + 1] + yi];
        int bb = _permutation[_permutation[xi + 1] + yi + 1];

        float x1, x2, y1;
        x1 = Lerp(Grad(aa, xf, yf), Grad(ba, xf - 1, yf), u);
        x2 = Lerp(Grad(ab, xf, yf - 1), Grad(bb, xf - 1, yf - 1), u);
        y1 = Lerp(x1, x2, v);

        return (y1 + 1) / 2; // Normalize to [0, 1]
    }

    private static float Fade(float t) => t * t * t * (t * (t * 6 - 15) + 10);

    private static float Lerp(float a, float b, float t) => a + t * (b - a);

    private static float Grad(int hash, float x, float y)
    {
        int h = hash & 7;
        float u = h < 4 ? x : y;
        float v = h < 4 ? y : x;
        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }
}
