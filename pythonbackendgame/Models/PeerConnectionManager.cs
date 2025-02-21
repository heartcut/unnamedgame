using Microsoft.AspNetCore.Components;
using SpawnDev.BlazorJS;
using SpawnDev.BlazorJS.JSObjects;
using SpawnDev.BlazorJS.PeerJS;
using System;
using System.Collections.Generic;

namespace pythonbackendgame.Models
{
    public class PeerConnectionManager : IDisposable
    {
        [Inject] BlazorJSRuntime JS { get; set; }
        private Peer? peer;
        private List<DataConnection> connections = new();
        private string myId;
        private string connectId;
        private bool isHost;

        public event Action<string>? OnDataReceived;
        public event Action<string>? OnPeerConnected;
        public event Action? OnPeerDisconnected;

        public PeerConnectionManager(string myId, string connectId, bool isHost)
        {
            this.myId = myId;
            this.connectId = connectId;
            this.isHost = isHost;

            peer = new Peer(myId);
            peer.OnOpen += Peer_OnOpen;
            peer.OnConnection += Peer_OnConnection;
        }

        private void Peer_OnOpen()
        {
            if (!isHost) ConnectToHost();
            else ConnectToLeech();
        }

        private void Peer_OnConnection(DataConnection conn)
        {
            connections.Add(conn);
            conn.OnData += DataConnection_OnData;
            conn.OnClose += () => { OnPeerDisconnected?.Invoke(); };
            OnPeerConnected?.Invoke(conn.Peer);
        }

        private void DataConnection_OnData(JSObject msg)
        {
            string data = msg.JSRef!.As<string>();
            OnDataReceived?.Invoke(data);
        }

        public void ConnectToHost()
        {
            if (peer == null || isHost) return;
            var conn = peer.Connect(connectId);
            conn.OnData += DataConnection_OnData;
            connections.Add(conn);
            SendData(myId+",0,0,0");
        }
        public void ConnectToLeech()
        {
            if (peer == null || isHost) return;
            var conn = peer.Connect(connectId);
            conn.OnData += DataConnection_OnData;
            connections.Add(conn);
            SendData("1,0,0,0,0,0,0,0");
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
