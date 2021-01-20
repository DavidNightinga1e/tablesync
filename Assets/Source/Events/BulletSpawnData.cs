using System;
using ExitGames.Client.Photon;
using UnityEngine;

namespace TableSync
{
    [Serializable]
    public class BulletSpawnData
    {
        public Vector2 position;
        public float rotation;
        
        private const int SizeOfBulletSpawnData = 3 * 4; // 3 floats

        private static readonly byte[] MemBulletSpawnData = new byte[SizeOfBulletSpawnData];
        
        public static short Serialize(StreamBuffer outStream, object customObject)
        {
            var bsp = (BulletSpawnData) customObject;
            lock (MemBulletSpawnData)
            {
                var bytes = MemBulletSpawnData;
                var index = 0;
                Protocol.Serialize(bsp.position.x, bytes, ref index);
                Protocol.Serialize(bsp.position.y, bytes, ref index);
                Protocol.Serialize(bsp.rotation, bytes, ref index);
                outStream.Write(bytes, 0, SizeOfBulletSpawnData);
            }

            return SizeOfBulletSpawnData;
        }

        public static object Deserialize(StreamBuffer inStream, short length)
        {
            var bsp = new BulletSpawnData();
            lock (MemBulletSpawnData)
            {
                inStream.Read(MemBulletSpawnData, 0, SizeOfBulletSpawnData);
                var index = 0;
                Protocol.Deserialize(out bsp.position.x, MemBulletSpawnData, ref index);
                Protocol.Deserialize(out bsp.position.y, MemBulletSpawnData, ref index);
                Protocol.Deserialize(out bsp.rotation, MemBulletSpawnData, ref index);
            }

            return bsp;
        }
    }
}