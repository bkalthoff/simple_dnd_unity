using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using System.Collections.Generic;

public class IPC_Host : MonoBehaviour
{
    private Unit_Manager unitManager;

    private UdpClient server;
    private int receivePort = 19239;
    private int sendPort = 19238;
    private bool isRunning = true;
    private Thread receiveThread;

    private float sendTimer = 0f;
    private float sendInterval = 2f;


    private void Start()
    {
        unitManager = FindObjectOfType<Unit_Manager>();
        if (unitManager != null)
        {
            unitManager.OnUnitHasChanged += HandleUnitChanged;
        }

        server = new UdpClient();
        server.Client.Bind(new IPEndPoint(IPAddress.Any, receivePort));

        receiveThread = new Thread(new ThreadStart(ReceivePackets));
        receiveThread.Start();
        Debug.Log("UDP server is running...");
    }

    private void ReceivePackets()
    {
        try
        {
            while (isRunning)
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = server.Receive(ref endPoint);
                string message = Encoding.UTF8.GetString(data);
                print("Received message: " + message);
                if (message == "update")
                {
                    HandleUnitChanged();
                }
            }
        }
        catch (SocketException e)
        {
            Debug.LogError("SocketException: " + e.Message);
        }
    }

    private void OnApplicationQuit()
    {
        receiveThread.Abort();
        isRunning = false;
        server.Close();
    }

    private void SendString(string message)
    {
        print("Sending message: " + message);
        if (message == "") {
            message = "empty";
        }
        byte[] data = Encoding.UTF8.GetBytes(message);
        server.Send(data, data.Length, "127.0.0.1", sendPort);
    }

    void Update()
    {
        // sendTimer += Time.deltaTime;
        // if (sendTimer > sendInterval)
        // {
        //     sendTimer = 0f;
        //     HandleUnitChanged();
        // }
    }
    private void HandleUnitChanged()
    {
        StringBuilder sb = new StringBuilder();
        // fetch all gameobjects with tag "Unit"
        // sleep for a bit to make sure all units have been spawned
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("Unit");
        if (targetObjects.Length == 0)
        {
            SendString("empty");
            return;
        }
        foreach (GameObject unit in targetObjects)
        {
            UnitInfo unitInfo = unit.GetComponent<UnitInfo>();
            print(unitInfo.GetInstanceID().ToString() + " " + unitInfo.nameText.text);
            sb.Append(unitInfo.GetInstanceID().ToString());
            sb.Append(",");
            sb.Append(unitInfo.nameText.text);
            sb.Append(";");
        }

        string message = sb.ToString();
        SendString(message);
    }
}
