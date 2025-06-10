using UnityEngine;
using System.IO;
using System.Collections;
using System.Threading;

public class MoveBellowUnity : MonoBehaviour
{
    private FetchDataSsh3dP fetchDataSsh3dP;
//    public string filePath = "Assets/data.csv"; // Path to the CSV file
    public float time_interval = 0.3f;
    private float minX = -12.70f;
    private float maxX = 379.30f;
    private float minUnityX = 0.2f;
    private float maxUnityX = -0.2f;
//    private float lastXValue = float.MinValue;
    public Transform targetObject; // Object to be moved
//    private float targetX;

    void Start()
    {
        fetchDataSsh3dP = GameObject.Find("PanelMain3dP").GetComponent<FetchDataSsh3dP>();
        // Start the coroutine to read the CSV line
        StartCoroutine(checkData());
        fetchDataSsh3dP = GameObject.Find("PanelMain3dP").GetComponent<FetchDataSsh3dP>();
        // Start the coroutine to read the CSV line
        StartCoroutine(BellowMove());
    }

//    IEnumerator ReadCSVLine()
//    {
//        while (true)
//        {
//            float targetXValue;
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
//                            if (values.Length >= 2)
//                            {
//                                if (float.TryParse(values[1], out float xValue))
//                                {
//                                    if (xValue != lastXValue)
//                                    {
//                                        lastXValue = xValue;
//                                        targetXValue = Map(xValue, minX, maxX, minUnityX, maxUnityX);
//                                        targetX = targetXValue;
//                                        UnityEngine.Debug.Log($"Parsed and mapped X value: {targetXValue}");
//
//                                        // Start the motion coroutine
//                                        StartCoroutine(BedMove(targetX));
//                                    }
//                                }
//                                else
//                                {
//                                    UnityEngine.Debug.LogWarning("Failed to parse X value from CSV.");
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

    private IEnumerator BellowMove()
    {
        while (true)
        {
            float targetX = float.Parse(fetchDataSsh3dP.Data_list[0]);
            targetX = Map(targetX, minX, maxX, minUnityX, maxUnityX);

            float elapsedTime = 0f;
            Vector3 startPosition = targetObject.localPosition;
            Vector3 targetPosition = new Vector3(targetX, startPosition.y, startPosition.z); // Maintain the same Y and Z values

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
