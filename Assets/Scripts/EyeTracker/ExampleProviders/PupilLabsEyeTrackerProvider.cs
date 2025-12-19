using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Voxon.EyeTracker;

namespace Voxon.EyeTracker.ExampleProviders
{
    /// <summary>
    /// Pupil Labs eye tracker provider
    /// Connects to Pupil Capture software via network (ZMQ/WebSocket)
    /// 
    /// Mac-compatible! Pupil Labs works great on macOS.
    /// 
    /// To use:
    /// 1. Install Pupil Capture software (https://pupil-labs.com/products/core/)
    /// 2. Start Pupil Capture and enable Network API
    /// 3. Configure IP address and port (default: 127.0.0.1:50020)
    /// 4. Calibrate in Pupil Capture
    /// 5. This provider will connect and receive gaze data
    /// </summary>
    public class PupilLabsEyeTrackerProvider : EyeTrackerProvider
    {
        [Header("Pupil Labs Settings")]
        [SerializeField] private string ipAddress = "127.0.0.1";
        [SerializeField] private int port = 50020;
        [SerializeField] private bool autoConnect = true;
        [SerializeField] private float connectionTimeout = 5f;

        private UdpClient udpClient;
        private IPEndPoint remoteEndPoint;
        private bool isReceivingData = false;
        private GazeData currentGazeData;
        private Coroutine connectionCoroutine;

        public override bool Initialize()
        {
            try
            {
                remoteEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
                isInitialized = true;
                Debug.Log($"PupilLabsEyeTrackerProvider initialized. Connecting to {ipAddress}:{port}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to initialize Pupil Labs eye tracker: {e.Message}");
                return false;
            }
        }

        public override bool Connect()
        {
            if (!isInitialized)
            {
                Debug.LogError("PupilLabsEyeTrackerProvider not initialized. Call Initialize() first.");
                return false;
            }

            try
            {
                // For Pupil Labs, we typically use ZMQ or WebSocket
                // This is a simplified UDP example - you may need to use ZMQ library
                // Install: https://github.com/zeromq/clrzmq or use WebSocket
                
                udpClient = new UdpClient();
                udpClient.Connect(remoteEndPoint);
                
                // Send connection request (Pupil Labs protocol)
                string connectMessage = "{\"subject\":\"gaze\",\"action\":\"subscribe\"}";
                byte[] data = Encoding.UTF8.GetBytes(connectMessage);
                udpClient.Send(data, data.Length);

                isConnected = true;
                isReceivingData = true;
                
                // Start receiving data
                if (connectionCoroutine == null)
                {
                    connectionCoroutine = StartCoroutine(ReceiveGazeData());
                }

                Debug.Log("PupilLabsEyeTrackerProvider connected successfully.");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to connect to Pupil Labs: {e.Message}");
                Debug.LogWarning("Make sure Pupil Capture is running and Network API is enabled.");
                return false;
            }
        }

        public override void Disconnect()
        {
            isReceivingData = false;
            isConnected = false;

            if (connectionCoroutine != null)
            {
                StopCoroutine(connectionCoroutine);
                connectionCoroutine = null;
            }

            if (udpClient != null)
            {
                udpClient.Close();
                udpClient = null;
            }

            Debug.Log("PupilLabsEyeTrackerProvider disconnected.");
        }

        public override GazeData GetGazeData()
        {
            if (!isConnected)
            {
                return new GazeData(Vector3.zero, Vector3.forward, false);
            }

            return currentGazeData;
        }

        private IEnumerator ReceiveGazeData()
        {
            while (isReceivingData && udpClient != null)
            {
                try
                {
                    if (udpClient.Available > 0)
                    {
                        byte[] receivedBytes = udpClient.Receive(ref remoteEndPoint);
                        string jsonData = Encoding.UTF8.GetString(receivedBytes);
                        
                        // Parse Pupil Labs gaze data
                        // Format: {"topic":"gaze","norm_pos":[x,y],"confidence":0.9,...}
                        ParsePupilGazeData(jsonData);
                    }
                }
                catch (System.Exception e)
                {
                    if (isReceivingData) // Only log if we're still supposed to be connected
                    {
                        Debug.LogWarning($"Error receiving Pupil Labs data: {e.Message}");
                    }
                }

                yield return new WaitForSeconds(0.01f); // ~100Hz
            }
        }

        private void ParsePupilGazeData(string jsonData)
        {
            // Simplified parser - you may want to use JSON library like Newtonsoft.Json
            // For now, this is a placeholder that shows the structure
            
            // TODO: Parse JSON properly
            // Example: {"topic":"gaze","norm_pos":[0.5,0.5],"confidence":0.9,"timestamp":1234567890}
            
            // Placeholder: convert normalized position to world space
            UnityEngine.Camera mainCam = UnityEngine.Camera.main;
            if (mainCam != null)
            {
                // For now, use center of screen (replace with actual parsing)
                Vector2 normalizedPos = new Vector2(0.5f, 0.5f);
                Vector3 screenPos = new Vector3(
                    normalizedPos.x * Screen.width,
                    normalizedPos.y * Screen.height,
                    0
                );
                
                Ray ray = mainCam.ScreenPointToRay(screenPos);
                
                currentGazeData = new GazeData();
                currentGazeData.gazeOrigin = ray.origin;
                currentGazeData.gazeDirection = ray.direction.normalized;
                currentGazeData.timestamp = Time.time;
                currentGazeData.isValid = true;
            }
        }

        protected override void OnDestroy()
        {
            Disconnect();
        }

        private void OnApplicationQuit()
        {
            Disconnect();
        }
    }
}

