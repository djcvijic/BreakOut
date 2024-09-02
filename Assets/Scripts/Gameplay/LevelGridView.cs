using UnityEngine;

public class LevelGridView : MonoBehaviour
{
    [SerializeField] private int bricksPerRow = 10;
    [SerializeField] private Brick brickPrefab;

    public void Initialize(LevelGrid levelGrid)
    {
        ClearBricks();
        InstantiateBricks(levelGrid);
    }

    private void ClearBricks()
    {
        transform.ClearTransform();
    }

    private void InstantiateBricks(LevelGrid levelGrid)
    {
        var gridTransform = transform;
        var boundsScale = GameBounds.Instance.BoundsTransform.lossyScale;
        var brickTransform = brickPrefab.Shape;
        var brickScale = brickTransform.lossyScale;
        var gapSize = (boundsScale.x - bricksPerRow * brickScale.x) / (bricksPerRow + 1);
        for (var rowIndex = 0; rowIndex < levelGrid.Rows.Count; rowIndex++)
        {
            var row = levelGrid.Rows[rowIndex];
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