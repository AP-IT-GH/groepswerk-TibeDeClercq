using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Players { Player1, Player2 }

public class ObservationGrid : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private Transform start;
    [SerializeField] private float spacing = 0;
    [SerializeField] private float rotationX = 0;
    [SerializeField] private Field playerFieldToObserve;

    public Zeeslag game;
    public Players player = Players.Player1;

    private ObservationCube[,] grid = new ObservationCube[10,10];

    void Start()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                var newCube = Instantiate(cube, new Vector3(start.position.x + (x * (cube.transform.localScale.x + spacing)), start.position.y, start.position.z + (y * (cube.transform.localScale.z + spacing))), Quaternion.identity, start);
                newCube.GetComponent<ObservationCube>().coordinates = new Vector2(x, y);
                grid[x, y] = newCube.GetComponent<ObservationCube>();
            }
        }
        cube.SetActive(false);
        start.rotation = Quaternion.Euler(rotationX, 0, 0);
    }

    public void ResetGrid()
    {
        foreach (GameObject _cube in start)
        {
            Destroy(_cube);
        }
        cube.SetActive(true);

        Start();
    }

    void Update()
    {
        for (int x = 0; x < playerFieldToObserve.DiscoveredValues.GetLength(0); x++)
        {
            for (int y = 0; y < playerFieldToObserve.DiscoveredValues.GetLength(1); y++)
            {
                switch (playerFieldToObserve.DiscoveredValues[x, y])
                {
                    case 'W':
                        grid[x, y].state = CubeState.Water;
                        break;
                    case 'H':
                        grid[x, y].state = CubeState.Hit;
                        break;
                    case 'M':
                        grid[x, y].state = CubeState.Miss;
                        break;
                }
            }
        }
    }
}
