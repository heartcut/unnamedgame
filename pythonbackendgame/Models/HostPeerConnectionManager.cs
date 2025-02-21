using Microsoft.AspNetCore.Components;
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

        public event Action<string>? OnDataReceived;
        public event Action<string>? OnPeerConnected;
        public event Action? OnPeerDisconnected;

        public HostPeerConnectionManager(string myId, string connectId, MainDataModel MDM, bool connecttoleech)
        {
            this.myId = myId;
            this.connectId = connectId;
            this.connectoleech = connecttoleech;

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
            string data = msg.JSRef!.As<string>();
            OnDataReceived?.Invoke(data);
        }
        
        public void ConnectToLeech(string leechplayernumber, string leechpeerid)
        {
            this.leechplayernumber = leechplayernumber;
            this.leechpeerid = leechpeerid;
            if (peer == null) return;
            var conn = peer.Connect(leechpeerid);
            conn.OnData += DataConnection_OnData;
            connections.Add(conn);
            //add logic to send player their number
            //SendData(leechplayernumber + "," + leechpeerid + ",0,0,0,0,0,0,0,0,0");


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
