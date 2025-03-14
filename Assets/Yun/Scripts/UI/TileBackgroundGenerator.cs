using UnityEngine;
using UnityEngine.Tilemaps;

namespace Yun.Scripts.UI
{
    public class TileBackgroundGenerator : MonoBehaviour
    {
        public Tilemap tilemap;
        public TileBase tile;
        public int width = 10;
        public int height = 10;
        public int spacing = 2; // Khoảng cách giữa các tile

        void Start()
        {
            GenerateBackground();
        }

        void GenerateBackground()
        {
            for (int x = 0; x < width; x += spacing)
            {
                for (int y = 0; y < height; y += spacing)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    tilemap.SetTile(tilePosition, tile);
                }
            }
        }

        // Phương thức để thay đổi khoảng cách trong runtime
        public void UpdateSpacing(int newSpacing)
        {
            spacing = newSpacing;
            tilemap.ClearAllTiles();
            GenerateBackground();
        }
    }
}