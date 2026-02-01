using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class HandUDPReceiver : MonoBehaviour
{
    UdpClient client;
    Thread thread;

    // NO allocations every frame
    public float throttle;
    public float brake;
    public float steer;

    volatile bool running = true;

    void Start()
    {
        client = new UdpClient(5055);

        thread = new Thread(ReceiveLoop);
        thread.IsBackground = true;
        thread.Start();
    }

    void ReceiveLoop()
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, 5055);

        while (running)
        {
            byte[] data = client.Receive(ref ep);
            string json = Encoding.UTF8.GetString(data);

            // parse manually (faster than JsonUtility allocations)
            var state = JsonUtility.FromJson<HandControlState>(json);

            throttle = state.throttle;
            brake = state.brake;
            steer = state.steer;
        }
    }

    void OnApplicationQuit()
    {
        running = false;
        client?.Close();
    }
}