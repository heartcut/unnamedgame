using Microsoft.AspNetCore.Components;
using SpawnDev.BlazorJS;
using SpawnDev.BlazorJS.JSObjects;
using SpawnDev.BlazorJS.PeerJS;
using System;
using System.Collections.Generic;

namespace pythonbackendgame.Models
{
    public class LeechPeerConnectionManager
    {
        [Inject] BlazorJSRuntime JS { get; set; }
        private Peer? peer;
        private List<DataConnection> connections = new();
        private string myId;
        private string connectId;

        public event Action<string>? OnDataReceived;
        public event Action<string>? OnPeerConnected;
        public event Action? OnPeerDisconnected;

        public LeechPeerConnectionManager(string myId, string connectId, MainDataModel MDM)
        {
            this.myId = myId;
            this.connectId = connectId;

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
            string data = msg.JSRef!.As<string>();
            OnDataReceived?.Invoke(data);
        }
        void DataConnection_OnOpen()
        {
            SendData($"Hello from ");
        }
        public void ConnectToHost()
        {
            if (peer == null) return;
            var conn = peer.Connect(connectId);
            conn.OnData += DataConnection_OnData;
            connections.Add(conn);
            //SendData("0," + myId + ",0,0,0");

        }
        public void SendData(string message)
        {
            foreach (var conn in connections)
            {
                conn.Send(message);
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
