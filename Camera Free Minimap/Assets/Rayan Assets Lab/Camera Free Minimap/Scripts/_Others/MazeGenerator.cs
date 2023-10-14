using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditorInternal;
using UnityEngine;




namespace CameraFreeMinimap
{
    public class MazeGenerator : MonoBehaviour
    {
        [SerializeField]
        private MazeCell _mazeCellPrefab;

        private float _cellSize;

        [SerializeField]
        private int _mazeWidth;

        [SerializeField]
        private int _mazeDepth;

        private MazeCell[,] _mazeGrid;

        async void Start()
        {

            _cellSize = _mazeCellPrefab.transform.localScale.x;

            _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

            for (int x = 0; x < _mazeWidth; x++)
            {
                for (int z = 0; z < _mazeDepth; z++)
                {
                    await Task.Delay(50);

                    if (this == null) return;

                    Vector3 localPosition = new Vector3(x * _cellSize, 0, z * _cellSize);

                    _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, transform.position + localPosition, Quaternion.identity, transform);
                }
            }

            await GenerateMazeAsync(null, _mazeGrid[0, 0]);

        }

        private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
        {
            currentCell.Visit();
            ClearWalls(previousCell, currentCell);

            MazeCell nextCell;

            do
            {
                nextCell = GetNextUnvisitedCell(currentCell);

                if (nextCell != null)
                {
                    GenerateMaze(currentCell, nextCell);
                }

            } while (nextCell != null);

        }

        private async Task GenerateMazeAsync(MazeCell previousCell, MazeCell currentCell)
        {
            currentCell.Visit();
            ClearWalls(previousCell, currentCell);

            MazeCell nextCell;

            do
            {
                await Task.Delay(50);

                nextCell = GetNextUnvisitedCell(currentCell);

                if (nextCell != null)
                {
                    await GenerateMazeAsync(currentCell, nextCell);
                }
            } while (nextCell != null);
        }

        private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
        {
            var unvisitedCells = GetUnvisitedCells(currentCell);

            return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
        }

        private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
        {

            if (currentCell == null) yield break;

            int x = (int)(currentCell.transform.localPosition.x / _cellSize);
            int z = (int)(currentCell.transform.localPosition.z / _cellSize);

            if (x + 1 < _mazeWidth)
            {
                var cellToRight = _mazeGrid[x + 1, z];

                if (cellToRight.IsVisited == false)
                {
                    yield return cellToRight;
                }
            }

            if (x - 1 >= 0)
            {
                var cellToLeft = _mazeGrid[x - 1, z];

                if (cellToLeft.IsVisited == false)
                {
                    yield return cellToLeft;
                }
            }

            if (z + 1 < _mazeDepth)
            {
                var cellToFront = _mazeGrid[x, z + 1];

                if (cellToFront.IsVisited == false)
                {
                    yield return cellToFront;
                }
            }

            if (z - 1 >= 0)
            {
                var cellToBack = _mazeGrid[x, z - 1];

                if (cellToBack.IsVisited == false)
                {
                    yield return cellToBack;
                }
            }
        }

        private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
        {
            if (previousCell == null)
            {
                return;
            }

            if (previousCell.transform.position.x < currentCell.transform.position.x)
            {
                previousCell.ClearRightWall();
                currentCell.ClearLeftWall();
                return;
            }

            if (previousCell.transform.position.x > currentCell.transform.position.x)
            {
                previousCell.ClearLeftWall();
                currentCell.ClearRightWall();
                return;
            }

            if (previousCell.transform.position.z < currentCell.transform.position.z)
            {
                previousCell.ClearFrontWall();
                currentCell.ClearBackWall();
                return;
            }

            if (previousCell.transform.position.z > currentCell.transform.position.z)
            {
                previousCell.ClearBackWall();
                currentCell.ClearFrontWall();
                return;
            }
        }

    }

}