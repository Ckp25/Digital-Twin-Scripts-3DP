using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CloseDoorUnity3dP : MonoBehaviour
{
    public Button runButton;  // Drag and drop your button here in the Inspector
    public GameObject door;    // Drag and drop your door GameObject here in the Inspector

    public float minY = 35f;    // Minimum Z position (closed position)
    public float maxY = 0f;  // Maximum Z position (open position)
    public float moveSpeed = 70f; // Speed at which the door moves

    
    void Start()
    {
        // Assign the Button's onClick listener
        runButton.onClick.AddListener(OnButtonClick);
        UnityEngine.Debug.Log("Button Listener added");
        Vector3 startPosition = new Vector3(door.transform.position.x, minY, door.transform.position.z);
        door.transform.position = startPosition;

    }

    private void OnButtonClick()
    {
        UnityEngine.Debug.Log("Button Triggered");
        Vector3 startPosition = new Vector3(door.transform.position.x, minY, door.transform.position.z);
        if (door.transform.position == startPosition )
        {
            OpenDoorUnity();
        }
    }

    private void OpenDoorUnity()
    {
        try
        {
            // Start moving the door
            StartCoroutine(MoveDoor());
            UnityEngine.Debug.Log("Move Coroutine started");
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError("An error occurred while moving the door:");
            UnityEngine.Debug.LogError(ex.Message);
        }
    }

    private IEnumerator MoveDoor()
    {
        float elapsedTime = 0f;
        
        Vector3 targetPosition = new Vector3(door.transform.position.x, maxY, door.transform.position.z);

        while (elapsedTime < moveSpeed)
        {
            float newY = Mathf.Lerp(minY, maxY, elapsedTime/moveSpeed);
            door.transform.position = new Vector3(door.transform.position.x, newY, door.transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position is set correctly
        door.transform.position = targetPosition;
    }

}
