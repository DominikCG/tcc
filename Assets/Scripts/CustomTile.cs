using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CustomTile", menuName = "Tiles/Custom Tile")]
public class CustomTile : Tile
{
    public GameObject tilePrefab;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        // Crie uma instância do GameObject do tilePrefab e coloque-o na posição correta.
        GameObject instantiatedPrefab = Instantiate(tilePrefab, position, Quaternion.identity);

        // Parenteie o objeto criado ao GameObject go para mantê-lo organizado na hierarquia.
        instantiatedPrefab.transform.SetParent(go.transform);

        return base.StartUp(position, tilemap, go);
    }
}
