using Me.Shishioko.Msdl.Data;
using Me.Shishioko.Msdl.Data.Chat;
using Me.Shishioko.Msdl.Data.Entities;
using System.Drawing;

namespace Me.Shishioko.Msdl.Test
{
    public sealed class Player
    {
        public readonly Guid ID;
        public readonly Connection Connection;
        public readonly string Name;
        public readonly Property[] Properties;
        public readonly string OriginAddress;
        public readonly ushort OriginPort;
        public int EID = -1;
        public double X = 0.0;
        public double Y = 0.0;
        public double Z = 0.0;
        public float Yaw = 0.0f;
        public float Pitch = 0.0f;
        public float HeadYaw = 0.0f;
        public EntityPlayer Entity = new();
        public Item?[] Hotbar = new Item?[9];
        public readonly Color LightColor;
        public readonly Color MediumColor;
        public readonly Color DarkColor;
        public Player(Guid id, Connection connection, string name, Property[] properties, string originAddress, ushort originPort)
        {
            ID = id;
            Connection = connection;
            Name = name;
            Properties = properties;
            OriginAddress = originAddress;
            OriginPort = originPort;

            Color color = Color.FromArgb(id.GetHashCode() & 0xFFFFFF);
            DarkColor = Color.FromArgb(color.R / 2, color.G / 2, color.B / 2);
            MediumColor = Color.FromArgb(DarkColor.R + 64, DarkColor.G + 64, DarkColor.B + 64);
            LightColor = Color.FromArgb(DarkColor.R + 128, DarkColor.G + 128, DarkColor.B + 128);

            Hotbar[0] = new(443, 0);
            Hotbar[1] = new(35, 14);
            Hotbar[2] = new(57, 112);
            Hotbar[3] = new(27, 9);
            Hotbar[4] = new(45, 18666);
            Hotbar[5] = new(485, 5959);
            Hotbar[6] = new(216, 2061);
            Hotbar[7] = new(909, 95);
            Hotbar[8] = new(443, 0);
        }
        internal async Task PulseAsync()
        {
            long outgoing = 0;
            bool pulse = false;
            Connection.ReceiveHeartbeatAsync = async (long incoming) =>
            {
                if (incoming != outgoing) return;
                pulse = true;
            };
            while (true)
            {
                if (Connection.ProtocolState == ProtocolState.Disconnected) return;
                outgoing = Random.Shared.NextInt64();
                await Connection.SendHeartbeatAsync(outgoing);
                await Task.Delay(20000);
                if (pulse) continue;
                await Connection.DisconnectAsync(new ChatText("Timed out!"));
                return;
            }
        }
    }
}
