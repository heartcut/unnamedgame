using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using SpawnDev.BlazorJS;
using SpawnDev.BlazorJS.JSObjects;
using SpawnDev.BlazorJS.PeerJS;
using System;
using System.Collections.Generic;
namespace pythonbackendgame.Models
{
    public class HostPeerConnectionManager
    {

        [Inject] BlazorJSRuntime JS { get; set; }
        private Peer? peer;
        private List<DataConnection> connections = new();
        private string myId;
        private string connectId;
        private bool connectoleech;
        private string leechplayernumber;
        private string leechpeerid;
        private MainDataModel MDM;
        public event Action<LeechSendModel>? OnDataReceived;
        public event Action<string>? OnPeerConnected;
        public event Action? OnPeerDisconnected;

        public HostPeerConnectionManager(string myId, string connectId, MainDataModel MDM, bool connecttoleech)
        {
            this.myId = myId;
            this.connectId = connectId;
            this.connectoleech = connecttoleech;
            this.MDM = MDM;

            peer = new Peer(myId);
            peer.OnOpen += Peer_OnOpen;
            peer.OnConnection += Peer_OnConnection;
        }
        private void Peer_OnOpen()
        {
            //if (connectoleech)
            //{
            //    ConnectToLeech();
            //}
        }
        private void Peer_OnConnection(DataConnection conn)
        {
            connections.Add(conn);
            conn.OnData += DataConnection_OnData;
            conn.OnClose += () => { OnPeerDisconnected?.Invoke(); };
            OnPeerConnected?.Invoke(conn.Peer);
            //SendData("2,"+ leechpeerid +",0,0,0,0,0,0,0,0,0");
        }
        private void DataConnection_OnData(JSObject msg)
        {
            string data = msg.JSRef!.As<string>(); // Receive raw JSON string
            try
            {
                LeechSendModel? receivedData = JsonConvert.DeserializeObject<LeechSendModel>(data);
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
        public void SendData(MainDataModel data)
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
