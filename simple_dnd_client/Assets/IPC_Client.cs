using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

public class IPC_Client : MonoBehaviour
{
    public List<UnitInfo> units = new List<UnitInfo>();
    private UdpClient client;
    private int receivePort = 19238;
    private int sendPort = 19239;
    private Thread receiveThread;

    Stack<string> messageStack = new Stack<string>();
    public EntryManager entryManager;

    private void Start()
    {
        client = new UdpClient();
        receiveThread = new Thread(new ThreadStart(ReceivePackets));
        receiveThread.Start();
        Debug.Log("UDP client is listening...");
        SendString("update");
    }

    private void ReceivePackets()
    {
        client.Client.Bind(new IPEndPoint(IPAddress.Any, receivePort));

        while (true)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = client.Receive(ref endPoint);
            string message = Encoding.UTF8.GetString(data);
            Debug.Log("Received message: " + message);
            if (message != "") {
                messageStack.Push(message);
            }
        }
    }

    void Update()
    {
        if (messageStack.Count > 0)
        {
            ParseMessage(messageStack.Pop());
        }
    }

    private void SendString(string message)
    {
        print("Sending message: " + message);
        byte[] data = Encoding.UTF8.GetBytes(message);
        client.Send(data, data.Length, "127.0.0.1", sendPort);
    }

    private void OnApplicationQuit()
    {
        receiveThread.Abort();
        client.Close();
    }
    void ParseMessage(string message)
    {
        if (message == "empty") {
            print("Empty message");
            units.Clear();
            entryManager.ClearEntries();
            return;
        }

        string[] msgUnits = message.Split(';');
        foreach (string unit in msgUnits)
        {
            print(unit);
        }
        List<UnitInfo> newUnits = new List<UnitInfo>();
        foreach (string unit in msgUnits)
        {
            if (unit == "")
            {
                continue;
            }
            string[] unitInfo = unit.Split(',');
            string id = unitInfo[0];
            string name = unitInfo[1];
            UnitInfo newUnit = new UnitInfo(id, name);
            entryManager.setName(id, name);
            newUnits.Add(newUnit);
        }

        foreach (UnitInfo unit in newUnits)
        {
            bool exists = false;
            foreach (UnitInfo unit2 in units)
            {
                if (unit.instanceID == unit2.instanceID)
                {
                    exists = true;
                    unit2.unitName = unit.unitName;
                }
            }
            if (!exists)
            {
                entryManager.AddEntry(unit.instanceID, unit.unitName);
                units.Add(unit);
            }
        }

        List<UnitInfo> toRemove = new List<UnitInfo>();
        foreach (UnitInfo unit in units)
        {
            bool exists = false;
            foreach (UnitInfo unit2 in newUnits)
            {
                if (unit.instanceID == unit2.instanceID)
                {
                    exists = true;
                }
            }
            if (!exists)
            {
                toRemove.Add(unit);
            }
        }
        foreach (UnitInfo unit in toRemove)
        {
            entryManager.RemoveEntry(unit.instanceID);
            units.Remove(unit);
        }
    }
}
