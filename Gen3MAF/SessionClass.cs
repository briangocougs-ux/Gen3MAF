using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Gen3MAF.Main;

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

        private bool m_IsDirty = false;

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
            m_IsDirty=true;
        }

        public TuneCycle GetLastTuneCycle()
        {
            if (m_TuneCycles.Count == 0)
            { 
                
                return null;
            }
            return m_TuneCycles.Last();
        }

        public TuneCycle RemoveLastTuneCycle()
        {
            TuneCycle LastTuneCycle = null;

            if (m_TuneCycles.Count != 0)
            {

                LastTuneCycle = m_TuneCycles.Last();

                m_TuneCycles.RemoveAt(m_TuneCycles.Count - 1);
                m_IsDirty = true;
            }

            
            return LastTuneCycle;
        }

        public TuneCycle CreateNewTuneCycle(int AirflowCount, int AdjustmentAirflowCount)
        {
            var newTuneCycle = new TuneCycle();

            newTuneCycle.InitTuneCycle(m_TuneCycleSequenceNumber, AirflowCount, AdjustmentAirflowCount);

            
            IncrementSequenceNumber();
            return newTuneCycle;
        }   

        public bool IsDirty()
        {
            return m_IsDirty;
        }

        public void SetClean()
        {
            if (m_IsDirty)
            {
                m_IsDirty = false;
            }
        }

        public int GetTuneCycleCount()
        {
            return m_TuneCycles.Count;
        }

        public TuneCycle GetTuneCycleAtIndex(int index)
        {
            if (index < 0 || index >= m_TuneCycles.Count)
                throw new IndexOutOfRangeException("Tune cycle index is out of range.");

            return m_TuneCycles[index];
        }
    }
}
