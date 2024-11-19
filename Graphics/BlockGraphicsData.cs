using Minesharp.World;
using System.Collections.Generic;

namespace Minesharp.Graphics;

public sealed class BlockGraphicsData
{
    public const float DEFAULT_BLOCK_SIZE = 0.5f;

    public Texture2D Texture { get; }
    
    public bool HasTexture;

    public readonly Dictionary<BlockFaces, VertexPositionNormalTexture[]> FacesVertices;
    public readonly List<int> Indices;

    private readonly BasicEffect _basicEffect;
    private readonly GraphicsDevice _graphics;

    private readonly Block _block;

    public BlockGraphicsData(Block block, GraphicsDevice graphicsDevice)
    {
        _block = block;
        _graphics = graphicsDevice;
        _graphics.RasterizerState = RasterizerState.CullNone;
        Texture = block.GenericData.Texture;

        _basicEffect = new BasicEffect(graphicsDevice)
        {
            TextureEnabled = true,
            Texture = Texture,
        };

        FacesVertices = new Dictionary<BlockFaces, VertexPositionNormalTexture[]>
        {
            // FRONT face (using right half of the texture atlas)
            { BlockFaces.FRONT, new VertexPositionNormalTexture[] {
                new(new Vector3(-DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE), Vector3.Forward, new Vector2(0.5f, 1)),    // Bottom-left
                new(new Vector3(DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE), Vector3.Forward, new Vector2(1, 1)),       // Bottom-right
                new(new Vector3(-DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE), Vector3.Forward, new Vector2(0.5f, 0)),    // Top-left
                new(new Vector3(DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE), Vector3.Forward, new Vector2(1, 0)),        // Top-right
            }},
    
            // BACK face (using right half of the texture atlas)
            { BlockFaces.BACK, new VertexPositionNormalTexture[] {
                new(new Vector3(DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE), Vector3.Backward, new Vector2(0.5f, 1)),   // Bottom-left
                new(new Vector3(-DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE), Vector3.Backward, new Vector2(1, 1)),     // Bottom-right
                new(new Vector3(DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE), Vector3.Backward, new Vector2(0.5f, 0)),    // Top-left
                new(new Vector3(-DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE), Vector3.Backward, new Vector2(1, 0)),      // Top-right
            }},
    
            // TOP face (using left half of the texture atlas)
            { BlockFaces.TOP, new VertexPositionNormalTexture[] {
                new(new Vector3(-DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE), Vector3.Up, new Vector2(0, 1)),     // Bottom-left
                new(new Vector3(DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE), Vector3.Up, new Vector2(0.5f, 1)),   // Bottom-right
                new(new Vector3(-DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE), Vector3.Up, new Vector2(0, 0)),    // Top-left
                new(new Vector3(DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE), Vector3.Up, new Vector2(0.5f, 0)),  // Top-right
            }},
    
            // BOTTOM face (using right half of the texture atlas)
            { BlockFaces.BOTTOM, new VertexPositionNormalTexture[] {
                new(new Vector3(-DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE), Vector3.Down, new Vector2(0.5f, 1)),   // Bottom-left
                new(new Vector3(DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE), Vector3.Down, new Vector2(1, 1)),       // Bottom-right
                new(new Vector3(-DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE), Vector3.Down, new Vector2(0.5f, 0)),    // Top-left
                new(new Vector3(DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE), Vector3.Down, new Vector2(1, 0)),        // Top-right
            }},
    
            // LEFT face (using right half of the texture atlas)
            { BlockFaces.LEFT, new VertexPositionNormalTexture[] {
                new(new Vector3(-DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE), Vector3.Left, new Vector2(0.5f, 1)),    // Bottom-left
                new(new Vector3(-DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE), Vector3.Left, new Vector2(1, 1)),        // Bottom-right
                new(new Vector3(-DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE), Vector3.Left, new Vector2(0.5f, 0)),     // Top-left
                new(new Vector3(-DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE), Vector3.Left, new Vector2(1, 0)),         // Top-right
            }},
    
            // RIGHT face (using right half of the texture atlas)
            { BlockFaces.RIGHT, new VertexPositionNormalTexture[] {
                new(new Vector3(DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE), Vector3.Right, new Vector2(0.5f, 1)),   // Bottom-left
                new(new Vector3(DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE), Vector3.Right, new Vector2(1, 1)),     // Bottom-right
                new(new Vector3(DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE), Vector3.Right, new Vector2(0.5f, 0)),    // Top-left
                new(new Vector3(DEFAULT_BLOCK_SIZE, DEFAULT_BLOCK_SIZE, -DEFAULT_BLOCK_SIZE), Vector3.Right, new Vector2(1, 0)),      // Top-right
            }},
        };


        Indices = new List<int>
        {
            // Front face
            0, 1, 2,
            2, 1, 3,
            // Back face
            4, 5, 6,
            5, 7, 6,
            // Top face
            8, 9, 10,
            9, 11, 10,
            // Bottom face
            12, 13, 14,
            13, 15, 14,
            // Left face
            16, 17, 18,
            17, 19, 18,
            // Right face
            20, 21, 22,
            21, 23, 22,
        };
    }

    public void Draw()
    {
        _basicEffect.World = Matrix.CreateTranslation(_block.Position.ToVector3());
        _basicEffect.View = GraphicsManager.View;
        _basicEffect.Projection = GraphicsManager.Projection;

        foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();

            foreach (var kvp in FacesVertices)
            {
                var face = kvp.Key;
                var vertices = kvp.Value;

                if (_block.BlockFacesVisibility[face] == Visibility.SHOWN)
                {
                    _graphics.DrawUserIndexedPrimitives(
                        PrimitiveType.TriangleList,
                        vertices, 0, vertices.Length,
                        Indices.ToArray(), 0, Indices.Count / 3
                    );
                }
            }
        }
    }
}

