using UnityEngine;
using System.Collections.Generic;



public class Unit_Manager : MonoBehaviour
{
    public GameObject unitPrefab; // Prefab of the unit to be spawned

    private GameObject hoveredUnit; // The unit that the mouse is hovering over
    private GameObject heldUnit; // The unit that the mouse is holding
    public List<UnitInfo> units = new List<UnitInfo>();

    //create an array to hold GameObject IDs    

    public delegate void HoverUnitHandler(GameObject unit);
    public event HoverUnitHandler OnHoverUnitChanged;

    public delegate void UnitHasChangedHandler();
    public event UnitHasChangedHandler OnUnitHasChanged;

    public Vector3 defaultScale = new Vector3(1, 1, 1);

    public bool renameMode = false;
    private string renameText = "";

    private GameObject renameObject;
    private GameObject renamedParent;


    void Update()
    {
        CheckHover();
        if (renameMode)
        {
            RenameMode();
        }
        else
        {
            CheckInput();
        }
    }

private float backspaceTimer = 0f;
private float backspaceDelay = 0.1f;

void RenameMode()
{
    if (renameMode)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Invoke("UnitListUpdated", 0.02f);
            renameMode = false;
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            backspaceTimer -= 0.2f;
            renameText = renameText.Substring(0, renameText.Length - 1);
            renameObject.GetComponent<TMPro.TMP_Text>().text = renameText;
        }
        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            backspaceTimer = 0f;
        }
        else if (Input.GetKey(KeyCode.Backspace))
        {
            backspaceTimer += Time.deltaTime;

            if (backspaceTimer >= backspaceDelay)
            {
                backspaceTimer = 0f;

                if (renameText.Length > 0)
                {
                    renameText = renameText.Substring(0, renameText.Length - 1);
                    renameObject.GetComponent<TMPro.TMP_Text>().text = renameText;
                }
            }
        }
        else
        {
            backspaceTimer = 0f;
            renameText += Input.inputString;
            renameObject.GetComponent<TMPro.TMP_Text>().text = renameText;
        }
    }
}

    void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // check if control is held

            if (hoveredUnit == null)
            {
                Color color;
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    color = Color.blue;
                }
                else {
                    color = Color.red;
                }
                Vector2 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SpawnUnit(spawnPosition, color);
            }
            else
            {
                heldUnit = hoveredUnit;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (heldUnit != null)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                heldUnit.transform.position = mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            heldUnit = null;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (hoveredUnit != null)
            {
                Destroy(hoveredUnit);
                SetHoveredUnit(null);
                //wait for the next frame to update the list
                Invoke("UnitListUpdated", 0.02f);
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (hoveredUnit != null)
            {
                hoveredUnit.transform.localScale *= 1.1f;
                defaultScale = hoveredUnit.transform.localScale;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (hoveredUnit != null)
            {
                hoveredUnit.transform.localScale /= 1.1f;
                defaultScale = hoveredUnit.transform.localScale;
            }
        }

        // if d is pressed find the child of the unit called isDead and toggle its visibility
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (hoveredUnit != null) {
                for (int i = 0; i < hoveredUnit.transform.childCount; i++)
                {
                    Transform child = hoveredUnit.transform.GetChild(i);
                    if (child.name == "isDead")
                    {
                        child.gameObject.SetActive(!child.gameObject.activeSelf);
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (hoveredUnit != null) {
                for (int i = 0; i < hoveredUnit.transform.childCount; i++)
                {
                    Transform child = hoveredUnit.transform.GetChild(i);
                    if (child.name == "sleepy")
                    {
                        child.gameObject.SetActive(!child.gameObject.activeSelf);
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (hoveredUnit != null) {
                for (int i = 0; i < hoveredUnit.transform.childCount; i++)
                {
                    Transform child = hoveredUnit.transform.GetChild(i);
                    if (child.name == "prone")
                    {
                        child.gameObject.SetActive(!child.gameObject.activeSelf);
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (hoveredUnit != null) {
                for (int i = 0; i < hoveredUnit.transform.childCount; i++)
                {
                    Transform child = hoveredUnit.transform.GetChild(i);
                    if (child.name == "cursed")
                    {
                        child.gameObject.SetActive(!child.gameObject.activeSelf);
                    }
                }
            }
        }
        // if N enable rename mode
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (hoveredUnit != null)
            {
                print("rename mode");
                for (int i = 0; i < hoveredUnit.transform.childCount; i++)
                {
                    Transform child = hoveredUnit.transform.GetChild(i);
                    if (child.name == "Name")
                    {
                        renamedParent = hoveredUnit;
                        renameObject = child.gameObject;
                    }
                }
                if (renameObject == null)
                {
                    print("no name object found");
                    return;
                }
                renameMode = true;
                renameText = "";
            }
        }
    }

    void SpawnUnit(Vector2 position, Color color)
    {
        GameObject newUnit = Instantiate(unitPrefab, position, Quaternion.identity);
        newUnit.transform.localScale = defaultScale;
        newUnit.GetComponentInChildren<SpriteRenderer>().color = color;
        Invoke("UnitListUpdated", 0.02f);
        heldUnit = newUnit;
        Debug.Log("Unit spawned at position: " + position);
    }

    void CheckHover()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);
        if (hitCollider != null)
        {
            if (hitCollider.CompareTag("Unit"))
            {
                SetHoveredUnit(hitCollider.gameObject);
            }
        }
        else
        {
            SetHoveredUnit(null);
        }
    }

    void SetHoveredUnit(GameObject unit)
    {
        if (unit != hoveredUnit)
        {
            hoveredUnit = unit;

            // Invoke the event to propagate the hover state change
            OnHoverUnitChanged?.Invoke(hoveredUnit);
        }
    }

    void UnitListUpdated()
    {
        print("unit list updated");
        OnUnitHasChanged?.Invoke();
    }
}
