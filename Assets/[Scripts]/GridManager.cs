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

    // Border Array
    private BorderScript[,] borderArray;

    public float fillInterval = 0.1f;

    // Diagonal filling
    private bool inverse = false;

    // Blocker
    public int[] difficultyBlockers;

    // Gem Swapping
    private GemBehaviour pressedGem;
    private GemBehaviour enteredGem;

    // Match number based on difficulty
    [Header("Match number on Difficulty")]
    public int matchNumber = 3;
    public int matchNumber_EasyNormal = 3;
    public int matchNumber_Hard = 4;

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

        // Border Array
        borderArray = new BorderScript[X_GridDimensions, Y_GridDimensions];

        // Create grid elements - border and gem
        for (int i = 0; i < X_GridDimensions; i++)
        {
            for (int j = 0; j < Y_GridDimensions; j++)
            {
                // Border
                GameObject border = Instantiate(BorderPrefab, GetWorldPosition(i,j), Quaternion.identity);
                borderArray[i, j] = border.GetComponent<BorderScript>();
                borderArray[i,j].Initialize(i,j,this);
                border.transform.SetParent(transform);
                
                // Empty Gem
                SpawnNewGem(i,j,GemType.EMPTY);

                //// Gem
                //GameObject tempGem = Instantiate(gemDictionary[GemType.NORMAL], Vector3.zero, Quaternion.identity);
                //tempGem.transform.SetParent(transform);
                //
                //// Gem behaviour array
                //gemArray[i, j] = tempGem.GetComponent<GemBehaviour>();
                //gemArray[i,j].Initialize(i,j, this, GemType.NORMAL);
                //
                //// Moveable
                //if (gemArray[i, j].IsMovable())
                //{
                //    gemArray[i,j].movableComponent.Move(GridFactor.x * i, GridFactor.y * j);
                //}
                //
                //// Color
                //if (gemArray[i, j].IsColored())
                //{
                //    gemArray[i, j].colorComponent.SetColor(
                //        ((GemColor.ColorType)(Random.Range(0, gemArray[i, j].colorComponent.GetTotalColors())))
                //        );
                //}
            }
        }

        // Set match number
        if (GameManager.GetInstance().difficulty == Difficulty.HARD)
        {
            matchNumber = matchNumber_Hard;
        }
        else
        {
            matchNumber = matchNumber_EasyNormal;
        }


        // Test block functionality <- this involves complexity/difficulty of the game

        // Difficulty Manager
        if (GameManager.GetInstance().difficulty != Difficulty.EASY)
        {
            SpawnBlockers();
        }
        
        // Spawn Blocking Gems

        StartCoroutine(Fill());

        // Set Grid starting position
        //transform.position = GridStartPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Get world position with x and y grid factor for offset
    public Vector2 GetWorldPosition(float x, float y)
    {
        return new Vector2(transform.position.x - (X_GridDimensions / 2.0f) * GridFactor.x + x * GridFactor.x,
            transform.position.y + (Y_GridDimensions / 2.0f) * GridFactor.y - y * GridFactor.y);
    }



    // Spawn blockers as per difficulty
    private void SpawnBlockers()
    {
        List<Vector2> blockedCoordinates1 = new List<Vector2>();
        blockedCoordinates1 = GameManager.GetInstance().blockedCoordinates;

        switch (GameManager.GetInstance().difficulty)
        {
            // 2 to 3 blockers
            case Difficulty.NORMAL:
                for (int i = 0; i < difficultyBlockers[0]; i++)
                {
                    int listIndex = Random.Range(0, blockedCoordinates1.Count);
                    Destroy(gemArray[(int)blockedCoordinates1[listIndex].x, (int)blockedCoordinates1[listIndex].y].gameObject);
                    SpawnNewGem((int)blockedCoordinates1[listIndex].x, (int)blockedCoordinates1[listIndex].y, GemType.BLOCK);
                    blockedCoordinates1.RemoveAt(listIndex);
                }
                break;

            // 5 to 6 blockers
            case Difficulty.HARD:
                for (int i = 0; i < difficultyBlockers[1]; i++)
                {
                    int listIndex = Random.Range(0, blockedCoordinates1.Count);
                    Destroy(gemArray[(int)blockedCoordinates1[listIndex].x, (int)blockedCoordinates1[listIndex].y].gameObject);
                    SpawnNewGem((int)blockedCoordinates1[listIndex].x, (int)blockedCoordinates1[listIndex].y, GemType.BLOCK);
                    blockedCoordinates1.RemoveAt(listIndex);
                }
                break;
        }
        
    }


    /// <summary>
    /// Spawn a new empty gem
    /// </summary>
    /// <param name="x">i index</param>
    /// <param name="y">j index</param>
    /// <param name="type">gem type: empty, normal or block</param>
    /// <returns></returns>
    public GemBehaviour SpawnNewGem(int x, int y, GemType type)
    {
        GameObject tempGem = Instantiate(gemDictionary[type], GetWorldPosition(x,y), Quaternion.identity);
        tempGem.transform.SetParent(transform);

        gemArray[x, y] = tempGem.GetComponent<GemBehaviour>();
        gemArray[x, y].Initialize(x, y, this, type);

        return gemArray[x, y];
    }

    /// <summary>
    /// Fill Coroutine
    /// </summary>
    public IEnumerator Fill()
    {
        // Every Fill gem has an interval time
        while (FillStep())
        {
            // toggle it for every call
            inverse = !inverse;
            yield return new WaitForSeconds(fillInterval);
        }
    }


    /// <summary>
    /// If any gems were moved, call fill step
    /// </summary>
    /// <returns></returns>
    public bool FillStep()
    {
        bool movedGem = false;

        // bottom row can't be moved down, that's why -2, and not -1
        for (int y = Y_GridDimensions - 2; y >= 0; y--)
        {
            for (int loopX = 0; loopX < X_GridDimensions; loopX++)
            {
                int x;

                if (!inverse)
                {
                    x = loopX;
                }
                else
                {
                    x = X_GridDimensions - 1 - loopX;
                }


                GemBehaviour gem = gemArray[x, y];

                if (gem.IsMovable())
                {
                    GemBehaviour gemBelow = gemArray[x, y + 1];

                    if (gemBelow.type == GemType.EMPTY)
                    {

                        // Clean up #1
                        Destroy(gemBelow.gameObject);

                        gem.movableComponent.Move(x , (y + 1) ,fillInterval);
                        gemArray[x, y + 1] = gem;
                        SpawnNewGem(x, y, GemType.EMPTY);
                        movedGem = true;
                    }
                    else
                    {
                        // For diagonal movement - loop 2 diagonals
                        for (int dIndex = -1; dIndex <= 1; dIndex++)
                        {
                            if (dIndex != 0)
                            {
                                // move diagonally right using diagX (x coordinate)
                                int diagX = x + dIndex;

                                if (inverse)
                                {
                                    // move diagonally left
                                    diagX = x - dIndex;
                                }

                                // Check if x coordinate is within the bounds
                                if (diagX >= 0 && diagX < X_GridDimensions)
                                {
                                    // Get reference to that gem
                                    GemBehaviour diagonalGem = gemArray[diagX, y + 1];

                                    // check for empty
                                    if (diagonalGem.type == GemType.EMPTY)
                                    {
                                        bool hasAPieceAbove = true;

                                        for (int aboveY = y; aboveY >= 0; aboveY--)
                                        {
                                            GemBehaviour gemAbove = gemArray[diagX, aboveY];

                                            // If movable, break, and fall down and fill the space
                                            if (gemAbove.IsMovable())
                                            {
                                                break;
                                            }
                                            else
                                            {

                                                // Blocked
                                                if (!gemAbove.IsMovable() && gemAbove.type != GemType.EMPTY)
                                                {
                                                    hasAPieceAbove = false;
                                                    break;
                                                }
                                            }
                                        }

                                        // Spawn normally
                                        if (!hasAPieceAbove)
                                        {
                                            Destroy(diagonalGem.gameObject);
                                            gem.movableComponent.Move(diagX, y + 1, fillInterval);
                                            gemArray[diagX, y + 1] = gem;
                                            SpawnNewGem(x, y, GemType.EMPTY);
                                            movedGem = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // top row at negative row
        for (int x = 0; x < X_GridDimensions; x++)
        {
            GemBehaviour gemBelow = gemArray[x, 0];
            if (gemBelow.type == GemType.EMPTY)
            {
                // Clean up #2
                Destroy(gemBelow.gameObject);
                GameObject tempGem = Instantiate(gemDictionary[GemType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity);
                tempGem.transform.SetParent(transform);

                gemArray[x, 0] = tempGem.GetComponent<GemBehaviour>();
                
                gemArray[x, 0].Initialize(x,-1, this, GemType.NORMAL);
                
                gemArray[x, 0].movableComponent.Move(x , 0 , fillInterval);
                
                gemArray[x, 0].colorComponent.SetColor(
                        ((GemColor.ColorType)(Random.Range(0, gemArray[x, 0].colorComponent.GetTotalColors())))
                        );

                movedGem = true;
            }
        }

        return movedGem;
    }



    // ------------------------------------- Match3 functionality ------------------------------------- //


    /// <summary>
    /// Check if 2 pieces are adjacent based on their x,y
    /// </summary>
    /// <param name="gem1"></param>
    /// <param name="gem2"></param>
    /// <returns></returns>
    public bool IsAdjacent(GemBehaviour gem1, GemBehaviour gem2)
    {
        // If x or y are same, check if the other is within 1 units
        return (gem1.X == gem2.X && (int)Mathf.Abs(gem1.Y - gem2.Y) == 1) ||
               (gem1.Y == gem2.Y && (int)Mathf.Abs(gem1.X - gem2.X) == 1);
    }


    /// <summary>
    /// Swap gem functionality to swap the coordinates of gem 1 and gem 2
    /// </summary>
    /// <param name="gem1"></param>
    /// <param name="gem2"></param>
    public void SwapGem(GemBehaviour gem1, GemBehaviour gem2)
    {
        if (gem1.IsMovable() && gem2.IsMovable())
        {
            // Swap them
            gemArray[gem1.X, gem1.Y] = gem2;
            gemArray[gem2.X, gem2.Y] = gem1;

            //--- Check for match first

            if (GetMatch(gem1, gem2.X, gem2.Y) != null || GetMatch(gem2, gem1.X, gem1.Y) != null)
            {
                int tempX = gem1.X;
                int tempY = gem1.Y;

                gem1.movableComponent.Move(gem2.X, gem2.Y, fillInterval);
                gem2.movableComponent.Move(tempX, tempY, fillInterval);
            }
            else
            {
                // return them back to their original position
                gemArray[gem1.X, gem1.Y] = gem1;
                gemArray[gem2.X, gem2.Y] = gem2;
            }


        }
    }


    /// <summary>
    /// Assign pressed gem
    /// </summary>
    /// <param name="gem"></param>
    public void PressGem(GemBehaviour gem)
    {
        pressedGem = gem;
    }

    /// <summary>
    /// Gem that we are hovering over
    /// </summary>
    /// <param name="gem"></param>
    public void EnteredGem(GemBehaviour gem)
    {
        enteredGem = gem;
    }


    /// <summary>
    /// On release click, if they are adjacent, then swap
    /// </summary>
    public void ReleaseGem()
    {
        if (IsAdjacent(pressedGem, enteredGem))
        {
            SwapGem(pressedGem,enteredGem);
        }
    }

    /// <summary>
    /// Call border enter function
    /// </summary>
    /// <param name="gem"></param>
    public void EnteredBorder(GemBehaviour gem)
    {
        borderArray[gem.X,gem.Y].BorderEnter();
    }


    /// <summary>
    /// Call border exit function
    /// </summary>
    /// <param name="gem"></param>
    public void ExitBorder(GemBehaviour gem)
    {
        borderArray[gem.X,gem.Y].BorderExit();
    }


    /// <summary>
    /// Check for matching gems in a straight line
    /// </summary>
    public List<GemBehaviour> GetMatch(GemBehaviour gem, int gemX, int gemY)
    {
        if (gem.IsColored())
        {
            GemColor.ColorType color = gem.colorComponent.Color;

            // take the gem's horizontal and vertical piece list and traverse accordingly
            List<GemBehaviour> horizontalGemsList = new List<GemBehaviour>();
            List<GemBehaviour> verticalGemsList = new List<GemBehaviour>();
            List<GemBehaviour> matchingGemsList = new List<GemBehaviour>();

            // --- HORIZONTAL ---
            horizontalGemsList.Add(gem);
            for (int direction = 0; direction <= 1; direction++)
            {
                for (int xOffset = 1; xOffset < X_GridDimensions; xOffset++)    // how far from the centre gem
                {
                    int x;

                    // Left Direction
                    if (direction == 0)
                    {
                        x = gemX - xOffset;
                    }
                    else    // Right Direction
                    {
                        x = gemX + xOffset;
                    }


                    // check for out of bounds horizontally
                    if (x < 0 || x >= X_GridDimensions)
                    {
                        break;
                    }

                    // matched, add it to the list
                    if (gemArray[x, gemY].IsColored() && gemArray[x, gemY].colorComponent.Color == color)
                    {
                        horizontalGemsList.Add(gemArray[x,gemY]);
                    }
                    else
                    {
                        break;
                    }

                }
            }

            // If the count of horizontal gem list is greater than matching number, add it to matching list
            if (horizontalGemsList.Count >= matchNumber)
            {
                foreach (var matchedGem in horizontalGemsList)
                {
                    matchingGemsList.Add(matchedGem);   
                }
            }

            // Return the matching list
            if (matchingGemsList.Count >= 3)
            {
                return matchingGemsList;
            }


            // --- VERTICAL ---
            verticalGemsList.Add(gem);
            for (int direction = 0; direction <= 1; direction++)
            {
                for (int yOffset = 1; yOffset < Y_GridDimensions; yOffset++)    // how far from the centre gem
                {
                    int y;

                    // Up Direction
                    if (direction == 0)
                    {
                        y = gemY - yOffset;
                    }
                    else    // Down Direction
                    {
                        y = gemY + yOffset;
                    }


                    // check for out of bounds vertically
                    if (y < 0 || y >= Y_GridDimensions)
                    {
                        break;
                    }

                    // matched, add it to the list
                    if (gemArray[gemX, y].IsColored() && gemArray[gemX, y].colorComponent.Color == color)
                    {
                        verticalGemsList.Add(gemArray[gemX, y]);
                    }
                    else
                    {
                        break;
                    }

                }
            }

            // If the count of vertical gem list is greater than matching number, add it to matching list
            if (verticalGemsList.Count >= matchNumber)
            {
                foreach (var matchedGem in verticalGemsList)
                {
                    matchingGemsList.Add(matchedGem);
                }
            }

            // Return the matching list
            if (matchingGemsList.Count >= 3)
            {
                return matchingGemsList;
            }
        }

        return null;
    }

    // ---------------------------------------- Game management ---------------------------------------- //


    /// <summary>
    /// Reset functionality
    /// </summary>
    private void ResetGame()
    {

    }
}
