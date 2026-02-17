using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Gen3MAF.Main;

namespace Gen3MAF
{
    public enum TuneCycleStateEnum
    {
        Created = 0,
        InitialAirflowPopulated = 1,
        AdjustmentAirflowPopulated = 2,
        AdjustedAirflowBuilt = 3,
        Completed = 5,
        Paused = 6


    }

    internal class TuneCycle
    {
        public uint m_SchemaVersion = 1;
        public DateTime m_Timestamp = DateTime.UtcNow;
        public uint m_SequenceNumber = 0;

        public TuneCycleStateEnum m_State = TuneCycleStateEnum.Created;
        public int m_AdjustmentPercent = 100;
        public bool m_AverageWithOriginal = true;

        public double[] m_InitialAirflow = Array.Empty<double>();
        public double[] m_AdjustmentAirflowData = Array.Empty<double>();
        public Double[] m_AdjustedAirflow = Array.Empty<double>();

        public TuneCycle()
        {
            m_SequenceNumber = 0;
            m_Timestamp = DateTime.Now;
            m_State = TuneCycleStateEnum.Created;
            m_AdjustmentPercent = 0;
            m_AverageWithOriginal = false;
        }

        public void InitTuneCycle(uint SequenceNumber, int AirflowCount, int AdjustmentAirflowCount)
        {
            m_SequenceNumber = SequenceNumber;
            m_Timestamp = DateTime.Now;
            m_State = TuneCycleStateEnum.Created;
            m_AdjustmentPercent = 0;
            m_AverageWithOriginal = false;
            m_InitialAirflow = new double[AirflowCount];
            m_AdjustmentAirflowData = new double[AdjustmentAirflowCount];
            m_AdjustedAirflow = new double[AirflowCount];

        }

        public void PopulateInitialAirflow(double[] Airflow)
        {
            if (Airflow.Length != m_InitialAirflow.Length)
                throw new ArgumentException("Airflow length does not match InitialAirflow length.");

            Array.Copy(Airflow, m_InitialAirflow, Airflow.Length);
            m_State = TuneCycleStateEnum.InitialAirflowPopulated;
        }

        public void PopulateAirflowAdjustment(double[] Airflow)
        {
            if (Airflow.Length != m_AdjustmentAirflowData.Length)
                throw new ArgumentException("Airflow length does not match AdjustmentAirflow length.");

            Array.Copy(Airflow, m_AdjustmentAirflowData, Airflow.Length);

            m_State = TuneCycleStateEnum.AdjustmentAirflowPopulated;
        }

        public double GetAirflowAtIndex(int Index)
        {
            if ((m_State != TuneCycleStateEnum.InitialAirflowPopulated) && (m_State != TuneCycleStateEnum.AdjustmentAirflowPopulated))
            {
                throw new InvalidOperationException("TuneCycle is not populated yet.");
            }

            if (Index < 0 || Index >= m_InitialAirflow.Length)
                throw new ArgumentOutOfRangeException("Index is out of range.");

            return m_InitialAirflow[Index];
        }

        public double GetAdjustedAirflowAtIndex(int Index)
        {
            if ((m_State != TuneCycleStateEnum.AdjustedAirflowBuilt) && (m_State != TuneCycleStateEnum.Completed))
                throw new InvalidOperationException("Adjusted Airflow is not built yet.");

            if (Index < 0 || Index >= m_AdjustedAirflow.Length)
                throw new ArgumentOutOfRangeException("Index is out of range.");

            return m_AdjustedAirflow[Index];
        }

        public double GetAdjustmentDataAtIndex(int Index)
        {
            if ((m_State != TuneCycleStateEnum.AdjustmentAirflowPopulated))
                throw new InvalidOperationException("Adjustment Airflow is not populated yet.");

            if (Index < 0 || Index >= m_AdjustmentAirflowData.Length)
                throw new ArgumentOutOfRangeException("Index is out of range.");

            return m_AdjustmentAirflowData[Index];
        }

        public void PopulatedAdjustedAirflow(double[] AdjustedAirflow)
        {
            if (AdjustedAirflow.Length != m_AdjustedAirflow.Length)
                throw new ArgumentException("Airflow length does not match AdjustmentAirflow length.");

            Array.Copy(AdjustedAirflow, m_AdjustedAirflow, AdjustedAirflow.Length);

            m_State = TuneCycleStateEnum.AdjustedAirflowBuilt;
        }

        public void MarkAsCompleted(int AdjustmentPecentage, bool AverageWithOriginal)
        {
            if (m_State != TuneCycleStateEnum.AdjustedAirflowBuilt)
                throw new InvalidOperationException("Adjusted Airflow must be built before marking complete.");

            m_AdjustmentPercent = AdjustmentPecentage;
            m_AverageWithOriginal = AverageWithOriginal;

            m_Timestamp = DateTime.UtcNow;
            m_State = TuneCycleStateEnum.Completed;
        }

        public void MarkAsPaused()
        {
            if (m_State != TuneCycleStateEnum.InitialAirflowPopulated)
                throw new InvalidOperationException("initial airflow must be populated before marking paused.");


            m_State = TuneCycleStateEnum.Paused;
        }

        public bool IsCompleted()
        {
            return m_State == TuneCycleStateEnum.Completed;
        }

        public bool IsPaused()
        {
            return m_State == TuneCycleStateEnum.Paused;
        }

        public void ChangePausedToAirflowPopulated()
        {
            if (m_State != TuneCycleStateEnum.Paused)
                throw new InvalidOperationException("TuneCycle is not paused.");

            m_State = TuneCycleStateEnum.InitialAirflowPopulated;
            return;
        }
    }
}
