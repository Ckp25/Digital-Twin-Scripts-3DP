using UnityEngine;
using UnityEngine.UI;
using Renci.SshNet;
using System;
using System.Collections;
using Unity.VisualScripting;
using TMPro;



public class FetchDataSsh3dP : MonoBehaviour
{
    //public Button runButton;  // Drag and drop your button here in the Inspector
    public GameObject PosX;
    public GameObject PosY;
    public GameObject PosZ;
    public GameObject TempNoz;
    public GameObject TempBed;
    public GameObject TempCham;
    public GameObject Speed;
    public GameObject Duration;
    public GameObject Status;
    public GameObject Humid;
    public GameObject FilamentUsed;
    public GameObject FilamentWidth;

    private static readonly string Host = "ikshana.local";
    private static readonly string Username = "ikshana";
    private static readonly string Password = "ikshana!123";  // Replace with the actual password
    private static readonly string Command = "python3 fetch_data_completed.py";

    private SshClient client;
    public bool isDataFetched = false;
    public string[] Data_list;

    void Start()
    {
        RunSshScript();
        
    }

    public void RunSshScript()
    {
        try
        {
            // Create an SSH client instance
            client = new SshClient(Host, Username, Password);
            
            // Connect to the SSH server
            client.Connect();
            UnityEngine.Debug.Log("Client Connection Started");

            if (client.IsConnected)
                {
                    Console.WriteLine("Connected to the SSH server.");

                    StartCoroutine(FetchData());

                }
            else
                {
                    UnityEngine.Debug.Log("Failed to connect to the SSH server.");
                }

            // Disconnect from the server
            // client.Disconnect();
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError("An error occurred while running the Ssh script:");
            UnityEngine.Debug.LogError(ex.Message);
        }
    }

    IEnumerator FetchData()
    {
        while (true)
        {
            var commandResult = client.RunCommand(Command);
            //UnityEngine.Debug.Log("Command Started");

            // Output the result
            //UnityEngine.Debug.Log("Command Output:");
            //UnityEngine.Debug.Log(commandResult.Result);
            string data = commandResult.Result;
            data = data.Replace("[", "").Replace("]", "").Replace(",", "").Replace("'", "");
            Data_list = data.Split(" ");
            if (isDataFetched==false)
            {
                isDataFetched = true;
            }
            UpdateDashboard(Data_list);
            
            


            // Check for any errors
            if (!string.IsNullOrEmpty(commandResult.Error))
            {
                UnityEngine.Debug.Log("Command Error");
                UnityEngine.Debug.Log(commandResult.Error);
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void UpdateDashboard(string[] data)
    {
        TextMeshProUGUI PosX_text = PosX.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI PosY_text = PosY.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI PosZ_text = PosZ.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI TempNoz_text = TempNoz.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI TempBed_text = TempBed.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI TempCham_text = TempCham.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Speed_text = Speed.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Duration_text = Duration.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Status_text = Status.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Humid_text = Humid.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI FilamentUsed_text = FilamentUsed.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI FilamentWidth_text = FilamentWidth.GetComponent<TextMeshProUGUI>();


        PosX_text.text = data[0];
        PosY_text.text = data[1];
        PosZ_text.text = data[2];
        TempNoz_text.text = data[3];
        TempBed_text.text = data[4];
        TempCham_text.text = data[5];
        Speed_text.text = data[6];
        Duration_text.text = data[7];
        Status_text.text = data[8];
        Humid_text.text = data[9];
        FilamentUsed_text.text = data[10];
        FilamentWidth_text.text = data[11];
    }

    private void OnApplicationQuit()
    {
        UnityEngine.Debug.Log("OnApplicationQuit started");
        if (client != null && client.IsConnected)
        {
            client.Disconnect();
            client.Dispose();
            UnityEngine.Debug.Log("client disposed");
        }
    }
}