using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using SpawnDev.BlazorJS;
using SpawnDev.BlazorJS.JSObjects;
using SpawnDev.BlazorJS.PeerJS;
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace pythonbackendgame.Models
{
    public class LeechPeerConnectionManager
    {
        [Inject] BlazorJSRuntime JS { get; set; }
        private Peer? peer;
        private List<DataConnection> connections = new();
        private string myId;
        private string connectId;
        private LeechSendModel LDM;

        public event Action<MainDataModel>? OnDataReceived;
        public event Action<string>? OnPeerConnected;
        public event Action? OnPeerDisconnected;

        public LeechPeerConnectionManager(string myId, string connectId, LeechSendModel LDM)
        {
            this.myId = myId;
            this.connectId = connectId;
            this.LDM = LDM;

            peer = new Peer(myId);
            peer.OnOpen += Peer_OnOpen;
            peer.OnConnection += Peer_OnConnection;
        }

        private void Peer_OnOpen()
        {
            ConnectToHost();
        }

        private void Peer_OnConnection(DataConnection conn)
        {
            conn.OnData += DataConnection_OnData;
            conn.OnClose += () => { OnPeerDisconnected?.Invoke(); };
            OnPeerConnected?.Invoke(conn.Peer);

        }

        private void DataConnection_OnData(JSObject msg)
        {
            string data = msg.JSRef!.As<string>(); // Receive raw JSON string
            try
            {
                MainDataModel? receivedData = JsonConvert.DeserializeObject<MainDataModel>(data);
                if (receivedData != null)
                {
                    OnDataReceived?.Invoke(receivedData); // Notify subscribers with the object
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deserializing received data: " + ex.Message);
            }
        }
        void DataConnection_OnOpen()
        {
            //SendData($"Hello from ");
        }
        public void ConnectToHost()
        {
            if (peer == null) return;
            var conn = peer.Connect(connectId);
            conn.OnData += DataConnection_OnData;
            connections.Add(conn);
            //SendData("0," + myId + ",0,0,0");

        }
        public void SendData(LeechSendModel data)
        {
            if (data == null) return;

            string jsonData = JsonConvert.SerializeObject(data); // Convert object to JSON
            foreach (var conn in connections)
            {
                conn.Send(jsonData);
            }
        }

        public void Dispose()
        {
            foreach (var conn in connections)
            {
                conn.OnData -= DataConnection_OnData;
                conn.Dispose();
            }

            peer?.Destroy();
            peer = null;
        }
    }
}
