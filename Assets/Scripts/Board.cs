using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public Transform playerPos;
    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);
    public CountAux score;

    private int randomXPos = 0;

     public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }

        // Chame o método para atualizar o Bounds após as peças serem inicializadas
        UpdateBounds();
    }

    private void Start()
    {
        if (playerPos != null)
        {
            SpawnPiece();
        }
        else
        {
            Debug.LogError("Player position is not assigned to Board!");
        }
    }

    public void SpawnRandomTetromino()
    {
        int randomIndex = Random.Range(0, tetrominoes.Length);
        TetrominoData randomTetromino = tetrominoes[randomIndex];
        int newXpos = Random.Range(-5, 5);
        if (newXpos != randomXPos)
        {
            spawnPosition = new Vector3Int(newXpos, 10, 0);
        }
        else
        {
            if (newXpos == -5)
            {
                newXpos += 1;
            }
            else if (newXpos == 5)
            {
                newXpos -= 1;
            }

            spawnPosition = new Vector3Int(newXpos, 10, 0);
        }
        randomXPos = newXpos;

        activePiece.Initialize(this, spawnPosition, randomTetromino);

        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        else
        {
            GameOver();
        }
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];
        spawnPosition = new Vector3Int(Random.Range(-4, 4), (int)playerPos.position.y + 28, 0);
        activePiece.Initialize(this, spawnPosition, data);

        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        else
        {
            GameOver();
        }
    }

    private void UpdateBounds()
    {
        RectInt bounds = Bounds;

        // Deslocar a base do tabuleiro para a posição 0, mantendo a altura original
        int yOffset = -bounds.yMin;
        bounds.yMin += yOffset;
        bounds.yMax += yOffset;

        // Atualizar a posição de spawn
        spawnPosition.y += yOffset;

        // Atualizar a posição da peça ativa
        if (activePiece != null)
        {
            activePiece.position += new Vector3Int(0, yOffset, 0);
        }
    }

    public void GameOver()
    {
        tilemap.ClearAllTiles();

        // Do anything else you want on game over here..
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.customTile); // Substitua tile por customTile
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        // A posição é válida apenas se todas as células forem válidas
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            // Uma célula fora dos limites é inválida
            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            // Uma célula já ocupada por outra peça é inválida
            if (tilemap.HasTile(tilePosition))
            {
                return false;
            }

            // Uma célula abaixo da base fixa em 0 é inválida
            if (tilePosition.y < 0)
            {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        // Clear from bottom to top
        while (row < bounds.yMax)
        {
            Debug.Log(row);
            // Only advance to the next row if the current is not cleared
            // because the tiles above will fall down when a row is cleared
            if (IsLineFull(row))
            {
                score.PopCount(1000);
            }
            else
            {
                row++;
            }
        }
    }

    public bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            // The line is not full if a tile is missing
            if (!tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }

    public void LineClear(int row)
    {
        RectInt bounds = Bounds;

        // Clear all tiles in the row
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }

        // Shift every row above down one
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
    }
}
