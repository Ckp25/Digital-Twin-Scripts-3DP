using UnityEngine;
using System.IO;
using System.Collections;
using TMPro;
using System.Threading;

public class MoveExtruderUnity : MonoBehaviour
{

    private FetchDataSsh3dP fetchDataSsh3dP;
    //    public string filePath = "Assets/data.csv"; // Path to the CSV file
    public float time_interval = 0.3f;
    private float minX = -12.70f;
    private float maxX = 379.30f;
    private float minUnityX = 0.2f;
    private float maxUnityX = -0.2f;
    private float minY = -1.50f;
    private float maxY = 379.50f;
    private float minUnityY = -0.16f;
    private float maxUnityY = 0.16f;
//    private Vector2 lastXYValue = new Vector2(float.MinValue, float.MinValue);
    public Transform targetObject; // Object to be moved
//    private Vector2 target;

    void Start()
    {
        fetchDataSsh3dP = GameObject.Find("PanelMain3dP").GetComponent<FetchDataSsh3dP>();
        // Start the coroutine to read the CSV line
        StartCoroutine(checkData());
        StartCoroutine(ExtruderMove());
    }

//    IEnumerator ReadCSVLine()
//    {
//        while (true)
//        {
//            Vector2 targetXY;
//
//            // Check if the file exists
//            if (File.Exists(filePath))
//            {
//                try
//                {
//                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
//                    using (StreamReader reader = new StreamReader(fs))
//                    {
//                        UnityEngine.Debug.Log("CSV file read successfully.");
//                        string[] csvLines = reader.ReadToEnd().Split('\n');
//
//                        if (csvLines.Length > 1)
//                        {
//                            // Read the second line (first line is the header)
//                            string[] values = csvLines[1].Split(',');
//
//                            if (values.Length >= 3)
//                            {
//                                if (float.TryParse(values[1], out float xValue) && float.TryParse(values[2], out float yValue))
//                                {
//                                    Vector2 newXYValue = new Vector2(xValue, yValue);
//                                    if (newXYValue != lastXYValue)
//                                    {
//                                        lastXYValue = newXYValue;
//                                        targetXY = new Vector2(
//                                            Map(xValue, minX, maxX, minUnityX, maxUnityX),
//                                            Map(yValue, minY, maxY, minUnityY, maxUnityY)
//                                        );
//                                        target = targetXY;
//                                        UnityEngine.Debug.Log($"Parsed and mapped XY values: {targetXY}");
//
//                                        // Start the motion coroutine
//                                        StartCoroutine(BedMove(target));
//                                    }
//                                }
//                                else
//                                {
//                                    UnityEngine.Debug.LogWarning("Failed to parse X or Y value from CSV.");
//                                }
//                            }
//                            else
//                            {
//                                UnityEngine.Debug.LogWarning("CSV line does not have enough values.");
//                            }
//                        }
//                        else
//                        {
//                            UnityEngine.Debug.LogWarning("CSV file does not have enough lines.");
//                        }
//                    }
//                }
//                catch (IOException ex)
//                {
//                    UnityEngine.Debug.LogError("IOException: " + ex.Message);
//                }
//            }
//            else
//            {
//                UnityEngine.Debug.LogWarning($"File not found: {filePath}");
//            }
//
//            // Wait for the specified interval before reading the next line
//            yield return new WaitForSeconds(time_interval);
//        }
//    }

    private IEnumerator ExtruderMove()
    {
        while (true)
        {
            //TextMeshProUGUI PosX_text = PosX.GetComponent<TextMeshProUGUI>();
            //TextMeshProUGUI PosY_text = PosY.GetComponent<TextMeshProUGUI>();
            //float targetX = float.Parse(PosX_text.text);
            //float targetY = float.Parse(PosY_text.text);
            float targetX = float.Parse(fetchDataSsh3dP.Data_list[0]);
            float targetY = float.Parse(fetchDataSsh3dP.Data_list[1]);
            targetX = Map(targetX, minX, maxX, minUnityX, maxUnityX);
            targetY = Map(targetY, minX, maxY, minUnityY, maxUnityY);
//            UnityEngine.Debug.Log(targetX + "," + targetY);

            float elapsedTime = 0f;
            Vector3 startPosition = targetObject.localPosition;
            Vector3 targetPosition = new Vector3(targetX, targetY, startPosition.z); // Maintain the same Z value

            while (elapsedTime < time_interval)
            {
                targetObject.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / time_interval);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the final position is set to the target position
            targetObject.localPosition = targetPosition;
            yield return null;
        }
    }

    private IEnumerator checkData()
    {
        while (fetchDataSsh3dP.isDataFetched == false)
        {
            yield return new WaitForSeconds(time_interval);
        }
    }

    float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }
}
