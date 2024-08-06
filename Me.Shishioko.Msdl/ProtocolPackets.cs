namespace Me.Shishioko.Msdl
{
    internal static class ProtocolPackets
    {
        public const int IncomingHandshakeHandshake = 0x00;

        public const int IncomingStatusStatusRequest = 0x00;
        public const int IncomingStatusPingRequest = 0x01;

        public const int OutgoingStatusStatusResponse = 0x00;
        public const int OutgoingStatusPingResponse = 0x01;

        public const int IncomingLoginStart = 0x00;
        public const int IncomingLoginEncryptionResponse = 0x01;
        public const int IncomingLoginPluginResponse = 0x02;
        public const int IncomingLoginEnd = 0x03;
        public const int IncomingLoginCookieResponse = 0x04;

        public const int OutgoingLoginDisconnect = 0x00;
        public const int OutgoingLoginEncryptionRequest = 0x01;
        public const int OutgoingLoginSuccess = 0x02;
        public const int OutgoingLoginCompression = 0x03;
        public const int OutgoingLoginPluginRequest = 0x04;
        public const int OutgoingLoginCookieRequest = 0x05;

        public const int IncomingConfigurationInformation = 0x00;
        public const int IncomingConfigurationCookieResponse = 0x01;
        public const int IncomingConfigurationPluginMessage = 0x02;
        public const int IncomingConfigurationEnd = 0x03;
        public const int IncomingConfigurationHeartbeat = 0x04;
        public const int IncomingConfigurationPingResponse = 0x05;
        public const int IncomingConfigurationResourcpackAdd = 0x06;
        public const int IncomingConfigurationDatapacks = 0x07;

        public const int OutgoingConfigurationCookieRequest = 0x00;
        public const int OutgoingConfigurationPluginMessage = 0x01;
        public const int OutgoingConfigurationDisconnect = 0x02;
        public const int OutgoingConfigurationEnd = 0x03;
        public const int OutgoingConfigurationHeartbeat = 0x04;
        public const int OutgoingConfigurationPingRequest = 0x05;
        public const int OutgoingConfigurationChatClear = 0x06;
        public const int OutgoingConfigurationRegistry = 0x07;
        public const int OutgoingConfigurationResourcepackRemove = 0x08;
        public const int OutgoingConfigurationResourcepackAdd = 0x09;
        public const int OutgoingConfigurationCookieStore = 0x0A;
        public const int OutgoingConfigurationTransfer = 0x0B;
        public const int OutgoingConfigurationFeatures = 0x0C;
        public const int OutgoingConfigurationTags = 0x0D;
        public const int OutgoingConfigurationDatapacks = 0x0E;

        public const int IncomingPlayCommand = 0x04;
        public const int IncomingPlayChat = 0x06;
        public const int IncomingPlayConfigure = 0x0C;
        public const int IncomingPlayHeartbeat = 0x18;
        public const int IncomingPlayLocation = 0x1A;
        public const int IncomingPlayPosition = 0x1B;
        public const int IncomingPlayRotation = 0x1C;
        public const int IncomingPlayActionGeneric = 0x24;
        public const int IncomingPlayActionMovement = 0x25;
        public const int IncomingPlayHotbar = 0x2F;
        public const int IncomingPlaySwing = 0x36;
        public const int IncomingPlayInteractionBlock = 0x38;

        public const int OutgoingPlayEntityAdd = 0x01;
        public const int OutgoingPlayEntityAnimation = 0x03;
        public const int OutgoingPlayBlockFeedback = 0x05;
        public const int OutgoingPlayBlockSingle = 0x09;
        public const int OutgoingPlayChunkBiomes = 0x0E;
        public const int OutgoingPlayContainerFull = 0x13;
        public const int OutgoingPlayContainerSingle = 0x15;
        public const int OutgoingPlayDisconnect = 0x1D;
        public const int OutgoingPlayChunkUnload = 0x21;
        public const int OutgoingPlayEvent = 0x22;
        public const int OutgoingPlayHeartbeat = 0x26;
        public const int OutgoingPlayChunkFull = 0x27;
        public const int OutgoingPlayChunkLight = 0x2A;
        public const int OutgoingPlayStart = 0x2B;
        public const int OutgoingPlayEntityLocationShort = 0x2E;
        public const int OutgoingPlayEntityPositionShort = 0x2F;
        public const int OutgoingPlayEntityRotation = 0x30;
        public const int OutgoingPlayTablistRemove = 0x3D;
        public const int OutgoingPlayTablistAction = 0x3E;
        public const int OutgoingPlayPlayerPoint = 0x3F;
        public const int OutgoingPlayPlayerPosition = 0x40;
        public const int OutgoingPlayEntityRemove = 0x42;
        public const int OutgoingPlayEffectRemove = 0x43;
        public const int OutgoingPlayEntityHead = 0x48;
        public const int OutgoingPlayHotbar = 0x53;
        public const int OutgoingPlayChunkCenter = 0x54;
        public const int OutgoingPlaySpawnpoint = 0x56;
        public const int OutgoingPlayEntityData = 0x58;
        public const int OutgoingPlayConfigure = 0x69;
        public const int OutgoingPlayTime = 0x64;
        public const int OutgoingPlayChatSystem = 0x6C;
        public const int OutgoingPlayTablistText = 0x6D;
        public const int OutgoingPlayEntityPositionFar = 0x70;
        public const int OutgoingPlayEffectAdd = 0x76;
        public const int OutgoingPlayTags = 0x78;
    }
}
