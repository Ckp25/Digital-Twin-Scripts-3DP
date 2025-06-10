using UnityEngine;
using System.IO;
using System.Collections;
using System.Threading;
using TMPro;

public class MoveBedUnity : MonoBehaviour
{

    private FetchDataSsh3dP fetchDataSsh3dP;
//    public string filePath = "Assets/data.csv"; // Path to the CSV file
    public float time_interval = 0.3f;
    private float minZ = 1.850f;
    private float maxZ = 379.950f;
    private float minUnityZ = 0.0f;
    private float maxUnityZ = 0.35f;
//    private float lastZValue = float.MinValue;
    public Transform targetObject; // Object to be moved
//    private float target;

    void Start()
    {
        fetchDataSsh3dP = GameObject.Find("PanelMain3dP").GetComponent<FetchDataSsh3dP>();

        // Start the coroutine to read the CSV line
        StartCoroutine(checkData());
        StartCoroutine(BedMove());
    }

//    IEnumerator ReadCSVLine()
//    {
//        while (true)
//        {
//            float targetZ;
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
//                                if (float.TryParse(values[2], out float zValue))
//                                {
//                                    if (zValue != lastZValue)
//                                    {
//                                        lastZValue = zValue;
//                                        targetZ = Map(zValue, minZ, maxZ, minUnityZ, maxUnityZ);
//                                        target = targetZ;
//                                        UnityEngine.Debug.Log($"Parsed and mapped Z value: {targetZ}");
//
//                                        // Start the motion coroutine
//                                        StartCoroutine(BedMove(target));
//                                    }
//                                }
//                                else
//                                {
//                                    UnityEngine.Debug.LogWarning("Failed to parse Z value from CSV.");
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

    private IEnumerator BedMove()
    {
        while (true)
        {
            //       TextMeshProUGUI PosZ_text = PosZ.GetComponent<TextMeshProUGUI>();
            //       float targetZ = float.Parse(PosZ_text.text);
            float targetZ = float.Parse(fetchDataSsh3dP.Data_list[2]);
            targetZ = Map(targetZ, minZ, maxZ, maxUnityZ, minUnityZ);
//            UnityEngine.Debug.Log(targetZ);

            float elapsedTime = 0f;
            Vector3 startPosition = targetObject.localPosition;
            Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y, targetZ);

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
