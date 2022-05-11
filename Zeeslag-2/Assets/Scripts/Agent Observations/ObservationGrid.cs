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

    private ObservationCube[,] grid;
    private bool cubeIsPlane = false;

    void Start()
    {
        cubeIsPlane = cube.GetComponent<ObservationCube>().isPlane;

        grid = new ObservationCube[playerFieldToObserve.Size, playerFieldToObserve.Size];
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (cubeIsPlane)
                {
                    var newCube = Instantiate(cube, new Vector3(start.position.x + (x * 10 * (cube.transform.localScale.x + spacing)), cube.transform.localPosition.y, start.position.z + (y * 10 * (cube.transform.localScale.z + spacing))), Quaternion.identity, start);
                    newCube.GetComponent<ObservationCube>().coordinates = new Vector2(x, y);
                    grid[x, y] = newCube.GetComponent<ObservationCube>();
                }
                else
                {
                    var newCube = Instantiate(cube, new Vector3(start.position.x + (x * (cube.transform.localScale.x + spacing)), start.position.y, start.position.z + (y * (cube.transform.localScale.z + spacing))), Quaternion.identity, start);
                    newCube.GetComponent<ObservationCube>().coordinates = new Vector2(x, y);
                    grid[x, y] = newCube.GetComponent<ObservationCube>();
                }                
            }
        }
        cube.SetActive(false);
        start.rotation = Quaternion.Euler(rotationX, 0, 0);
        game.GameRestarted = true;
    }

    public void ResetGrid()
    {
        foreach (Transform child in start)
        {
            Destroy(child.gameObject);
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
                        grid[x, y].gameObject.GetComponent<Renderer>().material.color = Color.blue;
                        grid[x, y].gameObject.tag = "W";
                        break;
                    case 'H':
                        grid[x, y].state = CubeState.Hit;
                        grid[x, y].gameObject.GetComponent<Renderer>().material.color = Color.red;
                        grid[x, y].gameObject.tag = "H";
                        break;
                    case 'M':
                        grid[x, y].state = CubeState.Miss;
                        grid[x, y].gameObject.GetComponent<Renderer>().material.color = Color.white;
                        grid[x, y].gameObject.tag = "M";
                        break;
                    default:
                        grid[x, y].state = CubeState.Water;
                        grid[x, y].gameObject.GetComponent<Renderer>().material.color = Color.blue;
                        grid[x, y].gameObject.tag = "W";
                        break;
                }
            }
        }
    }
}
