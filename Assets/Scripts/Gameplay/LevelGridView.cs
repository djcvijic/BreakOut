using UnityEngine;

public class LevelGridView : MonoBehaviour
{
    [SerializeField] private int bricksPerRow = 10;
    [SerializeField] private Brick brickPrefab;

    public void Initialize(LevelGrid levelGrid)
    {
        var gridTransform = transform;
        foreach (Transform child in gridTransform)
        {
            Destroy(child.gameObject);
        }

        var boundsScale = GameBounds.Instance.BoundsTransform.lossyScale;
        var brickTransform = brickPrefab.Shape;
        var brickScale = brickTransform.lossyScale;
        var gapSize = (boundsScale.x - 10 * brickTransform.lossyScale.x) / 11;
        for (var rowIndex = 0; rowIndex < levelGrid.Rows.Count; rowIndex++)
        {
            var row = levelGrid.Rows[rowIndex];
            var rowSize = row.Brick.Count;
            for (var columnIndex = 0; columnIndex < row.Brick.Count; columnIndex++)
            {
                var brick = row.Brick[columnIndex];
                if (brick == null) continue;

                var brickX = -boundsScale.x / 2 - brickScale.x / 2 + (brickScale.x + gapSize) * (1 + columnIndex);
                var brickY = boundsScale.y / 2 + brickScale.y / 2 - (brickScale.y + gapSize) * (1 + rowIndex);
                var brickPosition = new Vector3(brickX, brickY, 0);
                Instantiate(brickPrefab, brickPosition, Quaternion.identity, gridTransform)
                    .Initialize(brick);
            }
        }
    }
}