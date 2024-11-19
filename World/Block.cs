using Minesharp.Graphics;
using Minesharp.Logic;
using Minesharp.Managers;
using System;
using System.Collections.Generic;

namespace Minesharp.World;

public enum BlockFaces
{
    FRONT,
    BACK,
    TOP,
    BOTTOM,
    LEFT,
    RIGHT,
}
public enum Visibility
{
    SHOWN,
    HIDDEN,
}

public class Block
{
    // It's the 3D Position Vector of the Block
    public Vector3Int Position;

    public BlockGenericData GenericData { get; init; }

    // Contains the graphic data necessary
    // to draw the Block on screen
    public BlockGraphicsData GraphicsData;

    // Checks if the Block is destroyed
    public bool Destroyed { get; private set; }

    public List<Block> AdjiacentBlocks { get; private set; } = new(6);

    // It's the dictionary that stores whether a face should be shown
    // or hidden. All faces are shown by default.
    public Dictionary<BlockFaces, Visibility> BlockFacesVisibility;

    // It's the Chunk of this Block
    public Chunk Chunk { get; }
    // It's the Strip of this Block
    public Strip Strip { get; private set; }

    public Block(GraphicsDevice graphics, Chunk chunk, Vector3Int position)
    {
        Chunk = chunk;
        Chunk.OnChunkGeneratedEvent += OnChunkGenerated;

        GenericData = new(Material.DIRT);

        Position = position;
        GraphicsData = new BlockGraphicsData(this, graphics);
        BlockFacesVisibility = new()
        {
            { BlockFaces.FRONT, Visibility.SHOWN },
            { BlockFaces.BACK, Visibility.SHOWN },
            { BlockFaces.TOP, Visibility.SHOWN },
            { BlockFaces.BOTTOM, Visibility.SHOWN },
            { BlockFaces.LEFT, Visibility.SHOWN },
            { BlockFaces.RIGHT, Visibility.SHOWN },
        };
    }

    public void Update()
    {
        
    }

    public void Draw()
    {
        GraphicsData.Draw();
    }

    public void Destroy()
    {
        Destroyed = true;
    }

    private void OnChunkGenerated()
    {
        Strip = Chunk.StripsContainer[Position.X, Position.Y];
    }

    public void ShowFace(BlockFaces face)
    {
        BlockFacesVisibility[face] = Visibility.SHOWN;
    }
    public void HideFace(BlockFaces face)
    {
        BlockFacesVisibility[face] = Visibility.HIDDEN;
    }

    // --- These voids are basically only used for debug purposes ---

    public void ShowAllFaces()
    {
        foreach (BlockFaces face in BlockFacesVisibility.Keys)
        {
            BlockFacesVisibility[face] = Visibility.SHOWN;
        }
    }
    public void HideAllFaces()
    {
        foreach (BlockFaces face in BlockFacesVisibility.Keys)
        {
            BlockFacesVisibility[face] = Visibility.HIDDEN;
        }
    }

    // --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---

}
