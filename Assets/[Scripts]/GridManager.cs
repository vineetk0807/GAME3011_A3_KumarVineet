using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Display the public struct in Inspector
    [System.Serializable]
    public struct GemPrefab
    {
        public GemType type;
        public GameObject gemPrefab;
    }

    // Grid dimensions
    [Header("Grid Dimensions of the board")]
    public int X_GridDimensions = 10;
    public int Y_GridDimensions = 10;
    public Vector2 GridStartPosition = Vector2.zero;
    public Vector2 GridFactor = Vector2.zero;

    [Header("Gem Prefab")] 
    public GemPrefab[] GemPrefabArray;

    public GameObject BorderPrefab;

    // Dictionary
    private Dictionary<GemType, GameObject> gemDictionary;

    // Gem Array
    private GemBehaviour[,] gemArray;

    // Start is called before the first frame update
    void Start()
    {
        // Take the array contents and put it in the dictionary
        gemDictionary = new Dictionary<GemType, GameObject>();

        foreach (var gem in GemPrefabArray)
        {
            if (!gemDictionary.ContainsKey(gem.type))
            {
                gemDictionary.Add(gem.type, gem.gemPrefab);
            }
        }

        // Gem Array
        gemArray = new GemBehaviour[X_GridDimensions, Y_GridDimensions];

        // Create grid elements - border and gem
        for (int i = 0; i < X_GridDimensions; i++)
        {
            for (int j = 0; j < Y_GridDimensions; j++)
            {
                // Border
                GameObject border = Instantiate(BorderPrefab, new Vector3(GridFactor.x * i, GridFactor.y * j, 0), Quaternion.identity);
                border.transform.SetParent(transform);

                // Gem
                GameObject tempGem = Instantiate(gemDictionary[GemType.NORMAL], Vector3.zero, Quaternion.identity);
                tempGem.transform.SetParent(transform);

                // Gem behaviour array
                gemArray[i, j] = tempGem.GetComponent<GemBehaviour>();
                gemArray[i,j].Initialize(i,j, this, GemType.NORMAL);

                // Moveable
                if (gemArray[i, j].IsMovable())
                {
                    gemArray[i,j].movableComponent.Move(GridFactor.x * i, GridFactor.y * j);
                }

                // Color
                if (gemArray[i, j].IsColored())
                {
                    gemArray[i, j].colorComponent.SetColor(
                        ((GemColor.ColorType)(Random.Range(0, gemArray[i, j].colorComponent.GetTotalColors())))
                        );
                }
            }
        }

        // Set Grid starting position
        transform.position = GridStartPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetWorldPosition(float x, float y)
    {
        return new Vector2(transform.position.x - X_GridDimensions / 2.0f + x,
            transform.position.y + Y_GridDimensions / 2.0f - y);
    }
}
