using Minesharp.Logic;
using Minesharp.Managers;
using NoiseGeneration;
using System;
using System.Diagnostics;
using System.Linq;

namespace Minesharp.World;

public class Chunk
{
    // It's a unique Tag for this Chunk
    public int Tag { get; }

    public const int CHUNK_SIZE = 16;
    public const int BLOCKS_AMOUNT = CHUNK_SIZE * CHUNK_SIZE * CHUNK_SIZE;

    private int blocks = 0;

    private readonly Stopwatch _stopwatch = new();
    public float TimeToGenerateChunk { get; private set; }

    public event Action OnChunkGeneratedEvent;

    public Block[,,] BlocksGrid = new Block[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];   

    // This List contains the Lists of tstrips of blocks.
    // The whole purpose of this List is to check by strip and not to
    // iterate the whole Chunk at once.
    public Strip[,] StripsContainer = new Strip[16, 16];

    private readonly GraphicsDevice _graphicsDevice;
    private readonly GameManager _gameManager;

    public Chunk(GameManager gameManager, GraphicsDevice graphicsDevice)
    {
        Tag = 0;

        _gameManager = gameManager;
        _graphicsDevice = graphicsDevice;
        OnChunkGeneratedEvent = OnFinishedGeneration;
    }

    public void GenerateChunk()
    {
        _stopwatch.Start();
        GenerateStrips();

        for (int x = 0; x < CHUNK_SIZE; x++)
        {
            for (int y = 0; y < CHUNK_SIZE; y++)
            {
                for (int z = 0; z < CHUNK_SIZE; z++)
                {
                    Block block = new(_graphicsDevice, this, new(x, y, z));
                    BlocksGrid[x, y, z] = block;

                    if (StripsContainer[x, z].Count <= StripsContainer[x, z].Height)
                        StripsContainer[x, z].Add(block);
                }
            }
        }

        OnChunkGeneratedEvent?.Invoke();
    }

    public void Draw()
    {
        DrawAllStrips();
    }

    public void Update()
    {
        UpdateAllStrips();  
    }

    private void OnFinishedGeneration()
    {
        _stopwatch.Stop();
        TimeToGenerateChunk = (float)_stopwatch.Elapsed.TotalMilliseconds;
        _stopwatch.Reset();
    }

    private void GenerateStrips()
    {
        // Gets called in init and initializes all of the strips
        for (int x = 0; x < StripsContainer.GetLength(0); x++)
        { 
            for (int z = 0; z < StripsContainer.GetLength(1); z++)
            {
                // GENERATE PERLIN NOISE, TAKE VALUE AND SET IT AS THE HEIGHT OF EACH STRIP
                StripsContainer[x, z] = new(StripsContainer, (int)(Noise.ActiveGeneration.Elements[x, z].Value * 16));
            }
        }
    }

    private void DrawAllStrips()
    {
        foreach (Strip strip in StripsContainer)
        {
            strip.Draw();
        }
    }
    private void UpdateAllStrips()
    {
        foreach (Strip strip in StripsContainer)
        {
            strip.Update();
        }
    }

    public static Block GetBlockAtPosition(Chunk chunk, Vector3Int position)
    {
        return chunk.BlocksGrid[position.X, position.Y, position.Z];
    }

}
