using UnityEngine;
using SFB;
public class OpenImage : MonoBehaviour
{
    private Texture2D loadedTexture;
    private GameObject imageObject;
    private SpriteRenderer spriteRenderer;
    public Unit_Manager unitManager;
    void Update()
    {
        if (unitManager.renameMode)
        {
            return;
        }
        // if 'O' is pressed, open the file explorer
        if (Input.GetKeyDown(KeyCode.O))
        {
            string path = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false)[0];
            if (!string.IsNullOrEmpty(path))
            {
                Debug.Log("Selected file path: " + path);

                // Load the image as a byte array
                byte[] imageData = System.IO.File.ReadAllBytes(path);

                // Create a new Texture2D and load the image data into it
                loadedTexture = new Texture2D(2, 2);
                loadedTexture.LoadImage(imageData);

                // Delete the old image if it exists
                DestroyImageObject();

                // Apply the loaded texture to a game object or UI element
                ApplyTextureToGameObject();
            }
        }
    }

    void ApplyTextureToGameObject()
    {
        // Create a new GameObject to display the loaded texture
        imageObject = new GameObject("LoadedImage");
        spriteRenderer = imageObject.AddComponent<SpriteRenderer>();

        // Create a sprite using the loaded texture
        Sprite sprite = Sprite.Create(loadedTexture, new Rect(0, 0, loadedTexture.width, loadedTexture.height), new Vector2(0.5f, 0.5f));

        // Assign the sprite to the SpriteRenderer
        spriteRenderer.sprite = sprite;

        // Set the position and scale of the image object as needed
        imageObject.transform.position = new Vector3(0, 0, 1);
        imageObject.transform.localScale = Vector3.one;
        // set order in layer to 1 so that it is rendered above the grid
        spriteRenderer.sortingOrder = -10;
    }

    void DestroyImageObject()
    {
        if (imageObject != null)
        {
            Destroy(imageObject);
        }
    }
}