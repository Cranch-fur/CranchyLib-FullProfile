using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Org.BouncyCastle.Utilities.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CranchyLib.fullProfile
{
    public static class FullProfile
    {
        public static class Structures
        {
            public static class BloodWeb
            {
                public class properties
                {
                    public string rarity { get; set; }
                }
                public class nodeData
                {
                    public properties properties { get; set; }
                    public string state { get; set; }
                    public string nodeId { get; set; }
                }
                public class ringData
                {
                    public List<nodeData> nodeData { get; set; }
                }
                public class bloodWebData
                {
                    public int versionNumber { get; set; }
                    public int level { get; set; }
                    public List<ringData> ringData { get; set; }
                }
            }
            public class CharacterData
            {
                public class data
                {
                    public int bloodWebLevel { get; set; }
                    public int prestigeLevel { get; set; }
                    public List<string> prestigeDates { get; set; }
                    public BloodWeb.bloodWebData bloodWebData { get; set; }
                    public CharacterLoadoutData.characterLoadoutData characterLoadoutData { get; set; }
                    public List<inventory> inventory { get; set; }
                    public List<string> uniquePerksAdded { get; set; }

                }
                public class characterData
                {
                    public string key { get; set; }
                    public data data { get; set; }
                }
            }

            public class CharacterLoadoutData
            {
                public class characterLoadoutData
                {
                    public string item { get; set; }
                    public List<string> itemAddOns { get; set; }
                    public List<string> camperPerks { get; set; }
                    public List<int> camperPerkLevels { get; set; }
                    public string camperFavor { get; set; }
                    public string power { get; set; }
                    public List<string> powerAddOns { get; set; }
                    public string slasherFavor { get; set; }
                }
            }

            public class pageVisited
            {
                public string key { get; set; }
                public bool value { get; set; }
            }
            public class chatVisible
            {
                public string key { get; set; }
                public bool value { get; set; }
            }
            public class inventory
            {
                public string i { get; set; }
            }

            public class lastLoadout
            {
                public string item { get; set; }
                public string camperFavor { get; set; }
                public string power { get; set; }
                public List<string> powerAddOns { get; set; }
                public string slasherFavor { get; set; }
            }
            public class playerStatProgression
            {
                public string name { get; set; }
                public int value { get; set; }
            }
            public class specialEvent
            {
                public string eventId { get; set; }
                public bool eventEntryScreenOpened { get; set; }
            }
            public class bloodStoreKillers
            {
                public int versionNumber { get; set; }
                public int level { get; set; }
            }
            public class bloodStoreSurvivors
            {
                public int versionNumber { get; set; }
                public int level { get; set; }
            }
            public class dailyRitualSaveData
            {
                public string lastRitualPopupDate { get; set; }
            }





            public class fullProfile
            {
                public string playerUId { get; set; }
                public int selectedCamperIndex { get; set; }
                public int selectedSlasherIndex { get; set; }
                public bool firstTimePlaying { get; set; }
                public int consecutiveMatchStreak { get; set; }
                public bool hasBeenGivenKillerTutorialEndReward { get; set; }
                public bool hasBeenGivenSurvivorTutorialEndReward { get; set; }
                public long currentSeasonTicks { get; set; }
                public int lastConnectedCharacterIndex { get; set; }
                public string disconnectPenaltyTime { get; set; }
                public string lastMatchEndTime { get; set; }
                public string lastMatchStartTime { get; set; }
                public string lastKillerMatchEndTime { get; set; }
                public string lastSurvivorMatchEndTime { get; set; }
                public string ongoingGameTime { get; set; }
                public int cumulativeMatches { get; set; }
                public List<pageVisited> pageVisited { get; set; }
                public List<chatVisible> chatVisible { get; set; }
                public int cumulativeMatchesAsSurvivor { get; set; }
                public int cumulativeMatchesAsKiller { get; set; }
                public int cumulativeMatchesAsSurvivorNoFriends { get; set; }
                public int cumulativeMatchesAsKillerNoFriends { get; set; }
                public string lastMatchTimestamp { get; set; }
                public string lastSessionTimestamp { get; set; }
                public int cumulativeSessions { get; set; }
                public string cumulativePlaytime { get; set; }
                public List<CharacterData.characterData> characterData { get; set; }
                public lastLoadout lastLoadout { get; set; }
                public List<playerStatProgression> playerStatProgression { get; set; }
                public List<specialEvent> specialEvent { get; set; }
                public bloodStoreKillers bloodStoreKillers { get; set; }
                public bloodStoreSurvivors bloodStoreSurvivors { get; set; }
                public bool hasBeginnerTooltipsBeenDisabledAtLevel { get; set; }
                public int versionNumber { get; set; }
            }
        }


        private static bool IsJson(this string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input == string.Empty)
                return false;

            int length = input.Length;
            if ((input[0] == '{' && input[length - 1] == '}') || // JObject
                (input[0] == '[' && input[length - 1] == ']'))   // JArray
            {
                try
                {
                    var serializer = new JavaScriptSerializer();
                    serializer.DeserializeObject(input);

                    return true;
                }
                catch { return false; }
            }
            else return false;
        }
        private static bool IsBase64(this string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input == string.Empty)
                return false;

            if (input.Length % 4 != 0) // Proper BASE64 string shouldn't have remainder when divided
                return false;

            try
            {
                Convert.FromBase64String(input);
                return true;
            }
            catch { return false; }
        }
        private static bool IsUnicodeString(this string input)
        {
            return input.Any(ch => ch > 128);
        }


        private static T[] Subset<T>(this T[] array, int start, int count)
        {
            T[] result = new T[count];
            Array.Copy(array, start, result, 0, count);
            return result;
        }
        private static byte[] fullProfilePadding(this MemoryStream stream, int length)
        {
            byte[] buffer = stream.ToArray();
            int bufferLength = buffer.Length;

            byte[] bytes = BitConverter.GetBytes(length);
            int bytesLength = bytes.Length;


            byte[] result = new byte[bytesLength + bufferLength];
            Buffer.BlockCopy(bytes, 0, result, 0, bytesLength);

            Buffer.BlockCopy(buffer, 0, result, bytesLength, bufferLength);
            return result;
        }


        private static readonly byte[] encryptionKey = Encoding.ASCII.GetBytes("5BCC2D6A95D4DF04A005504E59A9B36E");
        private const string encryptionInner = "DbdDAQEB";
        private const string encryptionOuter = "DbdDAgAC";


        public static string Decrypt(string fullProfile)
        {
            string secondLayer = DecryptFirstLayer(fullProfile);

            if (String.IsNullOrEmpty(secondLayer))
                return null;

            if (secondLayer.IsUnicodeString() == true)
                return null;


            string convertedString = "";
            foreach (char ch in secondLayer)
                convertedString += (char)(ch + '\u0001');

            if (convertedString.StartsWith(encryptionInner) == false)
                return null;

            convertedString = convertedString.Replace("\u0001", string.Empty);


            byte[] bytes = Convert.FromBase64String(convertedString.Remove(0, encryptionInner.Length));
            byte[] buffer = bytes.Subset(4, bytes.Length - 4);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                InflaterInputStream inflaterInputStream = new InflaterInputStream(new MemoryStream(buffer));
                inflaterInputStream.CopyTo(memoryStream);

                memoryStream.Position = 0L;
                using (StreamReader streamReader = new StreamReader(memoryStream))
                    return streamReader.ReadToEnd();
            };
        }
        public async static Task<string> Decrypt_Async(string fullProfile)
        {
            return await Task.Run(() => Decrypt(fullProfile));
        }
        private static string DecryptFirstLayer(string fullProfile)
        {
            if (String.IsNullOrEmpty(fullProfile))
                return null;

            if (fullProfile.IsBase64() == false)
                return null;

            if (fullProfile.StartsWith(encryptionOuter) == false)
                return null;



            byte[] bytes = Convert.FromBase64String(fullProfile.Remove(0, encryptionOuter.Length));
            MemoryStream memoryStream = new MemoryStream(bytes);

            ICryptoTransform cryptoTransform = new RijndaelManaged
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.Zeros
            }.CreateDecryptor(encryptionKey, null);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);

            int bytesLength = bytes.Length;
            byte[] cryptoArray = new byte[bytesLength];
            cryptoStream.Read(cryptoArray, 0, bytesLength);

            cryptoStream.FlushAsync();
            cryptoStream.Close();

            memoryStream.FlushAsync();
            memoryStream.Close();

            return Encoding.ASCII.GetString(cryptoArray);

        }








        public static string Encrypt(string fullProfile)
        {
            if (fullProfile.IsUnicodeString() == true)
                return null;

            byte[] bytes = Encoding.ASCII.GetBytes(fullProfile);
            int bytesLength = bytes.Length;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZOutputStream zOutputSteam = new ZOutputStream(memoryStream, -1))
                {
                    zOutputSteam.Write(bytes, 0, bytesLength);
                    zOutputSteam.Flush();
                }


                string formatedString = Convert.ToBase64String(memoryStream.fullProfilePadding(bytesLength));
                int formatedStringLength = formatedString.Length;

                int convertedStringPadding = 16 - ((formatedStringLength + encryptionInner.Length) % 16);
                string convertedString = encryptionInner + formatedString.PadRight(formatedStringLength + convertedStringPadding, '\u0001');


                int convertedStringLength = convertedString.Length;
                string secondLayerString = string.Empty;


                foreach (char ch in convertedString)
                    secondLayerString += (char)(ch - '\u0001');


                return encryptionOuter + EncryptWithFirstLayer(secondLayerString);
            }
        }
        public async static Task<string> Encrypt_Async(string fullProfile)
        {
            return await Task.Run(() => Encrypt(fullProfile));
        }
        private static string EncryptWithFirstLayer(string fullProfile)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(fullProfile);
            MemoryStream memoryStream = new MemoryStream(bytes);


            ICryptoTransform cryptoTransform = new RijndaelManaged
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.Zeros
            }.CreateEncryptor(encryptionKey, null);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);

            int bytesLength = bytes.Length;
            byte[] cryptoArray = new byte[bytesLength];
            cryptoStream.Read(cryptoArray, 0, bytesLength);

            cryptoStream.FlushAsync();
            cryptoStream.Close();

            memoryStream.FlushAsync();
            memoryStream.Close();

            return Convert.ToBase64String(cryptoArray);

        }








        public static string GetUserIdFromFullProfile(string fullProfile)
        {
            if (fullProfile.IsBase64() == true)
            {
                if (fullProfile.StartsWith(encryptionOuter))
                    fullProfile = Decrypt(fullProfile);

                else
                    return null;
            }

            if (fullProfile.IsJson() == false)
                return null;


            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            var deserialized = javaScriptSerializer.Deserialize<Structures.fullProfile>(fullProfile);

            return deserialized.playerUId;
        }


        public static string SetFullProfileUserId(string fullProfile, string userId)
        {
            if (fullProfile.IsBase64() == true)
            {
                if (fullProfile.StartsWith(encryptionOuter))
                    fullProfile = Decrypt(fullProfile);

                else
                    return null;
            }

            if (fullProfile.IsJson() == false)
                return null;

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            var deserialized = javaScriptSerializer.Deserialize<Structures.fullProfile>(fullProfile);

            deserialized.playerUId = userId;
            return javaScriptSerializer.Serialize(deserialized);

        }
        public static string SetFullProfileCurrentSeasonTicks(string fullProfile, long currentSeasonTicks)
        {
            if (fullProfile.IsBase64() == true)
            {
                if (fullProfile.StartsWith(encryptionOuter))
                    fullProfile = Decrypt(fullProfile);

                else
                    return null;
            }

            if (fullProfile.IsJson() == false)
                return null;

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            var deserialized = javaScriptSerializer.Deserialize<Structures.fullProfile>(fullProfile);

            deserialized.currentSeasonTicks = currentSeasonTicks;
            return javaScriptSerializer.Serialize(deserialized);
        }
        public static string SetFullProfileUserIdAndCurrentSeasonTicks(string fullProfile, string userId, long currentSeasonTicks)
        {
            if (fullProfile.IsBase64() == true)
            {
                if (fullProfile.StartsWith(encryptionOuter))
                    fullProfile = Decrypt(fullProfile);

                else
                    return null;
            }

            if (fullProfile.IsJson() == false)
                return null;

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            var deserialized = javaScriptSerializer.Deserialize<Structures.fullProfile>(fullProfile);

            deserialized.playerUId = userId;
            deserialized.currentSeasonTicks = currentSeasonTicks;
            return javaScriptSerializer.Serialize(deserialized);
        }

        public static string UpdateFullProfileContestToValidate(string fullProfile, string userId)
        {
            if (fullProfile.IsBase64() == true)
            {
                if (fullProfile.StartsWith(encryptionOuter))
                    fullProfile = Decrypt(fullProfile);

                else
                    return null;
            }

            if (fullProfile.IsJson() == false)
                return null;

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            var deserialized = javaScriptSerializer.Deserialize<Structures.fullProfile>(fullProfile);

            deserialized.playerUId = userId;
            deserialized.currentSeasonTicks = (long)((DateTime.Now.ToUniversalTime() - new DateTime()).TotalMilliseconds + 0.5);

            return javaScriptSerializer.Serialize(deserialized);
        }
    }
}
