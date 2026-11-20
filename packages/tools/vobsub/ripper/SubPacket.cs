using System;

namespace MeGUI.VobSubRip
{
    /// <summary>Analyse minimale d'un paquet de sous-image DVD (SPU), équivalent de <c>CVobSubImage::GetPacketInfo</c>.</summary>
    internal static class SubPacket
    {
        /// <summary>
        /// Parcourt les blocs de contrôle du SPU. <paramref name="forced"/> est vrai si la commande 0x00
        /// (« forced start displaying ») est présente. Retourne false si le paquet est invalide.
        /// </summary>
        public static bool GetInfo(byte[] data, int packetSize, int dataSize, out bool forced)
        {
            forced = false;
            bool valid = false;
            int next = dataSize;
            int i;

            do
            {
                i = next;
                if (i + 4 > packetSize) return false;

                i += 2;                                   // date du bloc (ignorée ici)
                next = (data[i] << 8) | data[i + 1];
                i += 2;

                if (next + 4 > packetSize || next < dataSize) return false;

                bool end = false;
                while (!end)
                {
                    if (i >= packetSize) break;
                    int len;
                    switch (data[i])
                    {
                        case 0x03: len = 2; break;
                        case 0x04: len = 2; break;
                        case 0x05: len = 6; break;
                        case 0x06: len = 4; break;
                        default: len = 0; break;
                    }
                    if (i + len >= packetSize) break;

                    switch (data[i++])
                    {
                        case 0x00: forced = true; valid = true; break;
                        case 0x01: forced = false; valid = true; break;
                        case 0x02: valid = true; break;
                        case 0x03: i += 2; break;
                        case 0x04: i += 2; break;
                        case 0x05: valid = true; i += 6; break;
                        case 0x06:
                        {
                            int o0 = (data[i] << 8) + data[i + 1];
                            int o1 = (data[i + 2] << 8) + data[i + 3];
                            i += 4;
                            if (o0 > o1 || o1 > dataSize) return false;
                            break;
                        }
                        default: end = true; break;   // 0xFF = fin du bloc, autre = bloc ignoré
                    }
                }
            } while (i <= next && i < packetSize);

            return valid;
        }
    }
}
