using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Manager
{
    public class TCPReceiver : MonoBehaviour
    {
        Socket SeverSocket = null;
        Thread Socket_Thread = null;
        bool Socket_Thread_Flag = false;

        SceneManager SceneManager;

        //string[] stringSeparators = new string[] { "*TOUCHEND*", "*MOUSEDELTA*", "*Tapped*", "*DoubleTapped*" };

        void Awake()
        {
            Socket_Thread = new Thread(Dowrk);
            Socket_Thread_Flag = true;
            Socket_Thread.Start();
            SceneManager = SceneManager.Instance;
        }

        private void Dowrk()
        {
            //receivedMSG = new string[10];
            SeverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 5000);
            SeverSocket.Bind(ipep);
            SeverSocket.Listen(10);

            Debug.Log("Socket Standby....");
            Socket client = SeverSocket.Accept(); //client
            Debug.Log("Socket Connected.");

            IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
            NetworkStream recvStm = new NetworkStream(client);
            //tick = 0;

            while (Socket_Thread_Flag)
            {
                byte[] receiveBuffer = new byte[1024 * 80];
                try
                {
                    if (recvStm.Read(receiveBuffer, 0, receiveBuffer.Length) == 0)
                    {
                        // when disconnected , wait for new connection.
                        client.Close();
                        SeverSocket.Close();

                        SeverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        ipep = new IPEndPoint(IPAddress.Any, 1000);
                        SeverSocket.Bind(ipep);
                        SeverSocket.Listen(10);
                        Debug.Log("Socket Standby....");
                        client = SeverSocket.Accept(); //client
                        Debug.Log("Socket Connected.");

                        clientep = (IPEndPoint)client.RemoteEndPoint;
                        recvStm = new NetworkStream(client);
                    }
                    else
                    {
                        //string Test = Encoding.Default.GetString(receiveBuffer);
                        //SceneManager.ReadStringMessageFromSocket(Test);

                        //MessageToUnity msg = new MessageToUnity();
                        //msg.Data = receiveBuffer;
                        byte[] msg = receiveBuffer;
                        //avi:SceneManager.OnMessageReceived(msg);
                    }
                }

                catch (Exception)
                {
                    Socket_Thread_Flag = false;
                    client.Close();
                    SeverSocket.Close();
                    continue;
                }

            }

        }

        void OnApplicationQuit()
        {
            try
            {
                Socket_Thread_Flag = false;
                Socket_Thread.Abort();
                SeverSocket.Close();
                Debug.Log("Unity was shut down. Closed all Socket connections.");
            }

            catch
            {
                Debug.Log("Error when finished...");
            }
        }
    }
}
