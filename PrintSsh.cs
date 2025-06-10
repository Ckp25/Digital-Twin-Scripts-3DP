using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.IO;
using Renci.SshNet;
using System;

public class PrintSsh3dP : MonoBehaviour
{
    public Button runButton;  // Drag and drop your button here in the Inspector

    private static readonly string Host = "ikshana.local";
    private static readonly string Username = "ikshana";
    private static readonly string Password = "ikshana!123";  // Replace with the actual password
    private static readonly string Command = "python3 print.py";
    

    void Start()
    {
        // Assign the Button's onClick listener
        runButton.onClick.AddListener(OnButtonClick);
        UnityEngine.Debug.Log("Print Listner added");
    }

    private void OnButtonClick()
    {
        UnityEngine.Debug.Log("Print Button Triggered");
        RunSshScript();
    }

    private void RunSshScript()
    {
        try
        {
            // Create an SSH client instance
            using (var client = new SshClient(Host, Username, Password))
            {
                // Connect to the SSH server
                client.Connect();

                if (client.IsConnected)
                {
                    Console.WriteLine("Connected to the SSH server.");

                    // Execute the command
                    var commandResult = client.RunCommand(Command);

                    // Output the result
                    Console.WriteLine("Command output:");
                    Console.WriteLine(commandResult.Result);

                    // Check for any errors
                    if (!string.IsNullOrEmpty(commandResult.Error))
                    {
                        Console.WriteLine("Command error:");
                        Console.WriteLine(commandResult.Error);
                    }
                }
                else
                {
                    Console.WriteLine("Failed to connect to the SSH server.");
                }

                // Disconnect from the server
                client.Disconnect();
            }
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError("An error occurred while running the Ssh script:");
            UnityEngine.Debug.LogError(ex.Message);
        }
    }
}