using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Gen3MAF.Form1;

namespace Gen3MAF
{
    public enum BucketStyleEnum
    {
        Single = 1,
        Double = 2
    }
    internal class SessionClass
    {
        public uint m_SchemaVersion = 1;
        public string m_VehicleName = "";
        public string m_ECU = "";
        public string m_OS = "";
        public int m_MinFrequency = 1500;
        public int m_MaxFrequency = 12000;
        public int m_FrequencyStep = 125;
        public BucketStyleEnum m_BucketStyle = BucketStyleEnum.Double;
        public uint m_TuneCycleSequenceNumber =1 ;

        public List<TuneCycle> m_TuneCycles = new();

        public SessionClass() { }
        public SessionClass(string vehicleName, string eCU, string oS, int minFrequency, int maxFrequency, int frequencyStep, BucketStyleEnum bucketStyle, uint tuneCycleSequenceNumber)
        {
            m_VehicleName = vehicleName;
            m_ECU = eCU;
            m_OS = oS;
            m_MinFrequency = minFrequency;
            m_MaxFrequency = maxFrequency;
            m_FrequencyStep = frequencyStep;
            m_BucketStyle = bucketStyle;
            m_TuneCycleSequenceNumber = tuneCycleSequenceNumber;
        }

        [JsonIgnore] public string VehicleName { get { return m_VehicleName; } }
        [JsonIgnore] public string ECU { get { return m_ECU; } }
        [JsonIgnore] public string OS { get { return m_OS; } }
        [JsonIgnore] public int MinFrequency { get { return m_MinFrequency; } }
        [JsonIgnore] public int MaxFrequency { get { return m_MaxFrequency; } }
        [JsonIgnore] public int FrequencyStep { get { return m_FrequencyStep; } }
        [JsonIgnore] public BucketStyleEnum BucketStyle { get { return m_BucketStyle; } }
        [JsonIgnore] public uint TuneCycleSequenceNumber { get { return m_TuneCycleSequenceNumber; } }

        public void IncrementSequenceNumber() {m_TuneCycleSequenceNumber++;}

        public void AddTuneCycle(TuneCycle tuneCycle)
        {
            m_TuneCycles.Add(tuneCycle);
        }
    }
}
