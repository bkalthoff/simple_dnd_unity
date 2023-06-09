using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

public class IPC : MonoBehaviour
{
    public List<UnitInfo> units;
    private Unit_Manager unitManager;



    private UdpClient client;
    private int port = 19238;

    private async void Start()
    {
        unitManager = FindObjectOfType<Unit_Manager>();
        units = unitManager.units;
        if (unitManager != null)
        {
            unitManager.OnUnitHasChanged += HandleUnitChanged;
        }
        client = new UdpClient(port);
        Debug.Log("UDP client is listening...");

        while (true)
        {
            UdpReceiveResult result = await client.ReceiveAsync();
            string message = Encoding.UTF8.GetString(result.Buffer);

            Debug.Log("Received message: " + message);
        }
    }

    private void OnApplicationQuit()
    {
        client.Close();
    }


    void HandleUnitChanged()
    {
        //send units
    }
}
