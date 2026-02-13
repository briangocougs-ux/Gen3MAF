using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Gen3MAF.Form1;

namespace Gen3MAF
{
    public enum TuneCycleStateEnum
    {
        Created = 0,
        InitialAirflowPopulated = 1,
        AdjustmentAirflowPopulated = 2

    }

    internal class TuneCycle
    {
        DateTime m_Timestamp;

        public TuneCycleStateEnum m_State;
        int m_AdjustmentPercent;
        bool m_AverageWithOriginal;

        double[] m_InitialAirflow;
        double[] m_AdjustmentAirflowData;
        Double[] m_AdjustedAirflow;

        public TuneCycle(int AirflowCount, int AdjustmentAirflowCount)
        {
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

        public double GetAdjustmentDataAtIndex(int Index)
        {
            if ( (m_State != TuneCycleStateEnum.AdjustmentAirflowPopulated))
                throw new InvalidOperationException("Adjustment Airflow is not populated yet.");
            if (Index < 0 || Index >= m_AdjustmentAirflowData.Length)
                throw new ArgumentOutOfRangeException("Index is out of range.");
            return m_AdjustmentAirflowData[Index];
        }
    }
}
