using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Gen3MAF.Main;

namespace Gen3MAF
{

    public struct ReturnDataPoint
    {
        public uint   Frequency;
        public double Airflow;
        public double AdjustedAirflow;
        public bool   HasUpdatedAirflow;

    }

    public struct MafDataPoint
    {
        public uint Frequency;
        public bool HasUpdatedAirFlow;

        public double AirFlow;
        public double AirFlowAdjustment;
        public double AirFlowAdjusted;
#if DEBUG
        public uint FrequencyLeft;
#endif
        public double AirFlowLeft;
        public double AirFlowLeftAdjustment;
        public double AirFlowLeftAdjusted;

#if DEBUG
        public uint FrequencyRight;
#endif
        public double AirFlowRight;
        public double AirFlowRightAdjustment;
        public double AirFlowRightAdjusted;
    }

    internal class AdjustClass
    {

        MafDataPoint[] m_mafDataPoints = null;


        int m_MinMAFFrequency = 0;
        int m_MaxMAFFrequency = 0;
        int m_MAFFrequencyStep = 0;
        BucketStyleEnum m_BucketStyle = BucketStyleEnum.Double;

        uint m_BucketCount = 0;

        int m_mafFrequencyCount = 0;

        public AdjustClass(int MinMAFFrequency, int MaxMAFFrequency, int MAFFrequencyStep, BucketStyleEnum BucketStyle)
        {
            m_MinMAFFrequency = MinMAFFrequency;
            m_MaxMAFFrequency = MaxMAFFrequency;
            m_MAFFrequencyStep = MAFFrequencyStep;
            m_BucketStyle = BucketStyle;

            m_mafFrequencyCount = (m_MaxMAFFrequency - m_MinMAFFrequency) / m_MAFFrequencyStep + 1;

            if (m_BucketStyle == BucketStyleEnum.Single)
            {
                m_BucketCount = (uint)m_mafFrequencyCount;
            }
            else if (m_BucketStyle == BucketStyleEnum.Double)
            {
                m_BucketCount = (uint)m_mafFrequencyCount * 2;
            }
            else
            {
                Debug.Assert(true, "unknown bucket type");

                return;
            }

            m_mafDataPoints = new MafDataPoint[m_mafFrequencyCount];

            for (uint i = 0; i < m_mafFrequencyCount; i++)
            {
                m_mafDataPoints[i].Frequency = (uint)(m_MinMAFFrequency + (i * m_MAFFrequencyStep));
                m_mafDataPoints[i].AirFlow = 0.0f; // Placeholder for actual airflow values
            }
        }

        public void InitializeAirFlowFromTuneObject(
                    TuneCycle Tune
                    )

        {
            for (int i = 0; i < m_mafFrequencyCount; i++)
            {

                m_mafDataPoints[i].AirFlow = Tune.GetAirflowAtIndex(i);


            }

            if (m_BucketStyle == BucketStyleEnum.Double)
            {
                //  for double bucket, we initialize the left and right airflow values base on the slope to the previous and next data points
                for (int i = 0; i < (m_mafDataPoints.Length); i++)
                {

                    int Right = i + 1;
                    int Left = i - 1;

                    if (Right < m_mafFrequencyCount)
                    {

                        double slopeRight = (m_mafDataPoints[Right].AirFlow - m_mafDataPoints[i].AirFlow)
                                         / (m_mafDataPoints[Right].Frequency - m_mafDataPoints[i].Frequency);
#if DEBUG
                        m_mafDataPoints[i].FrequencyRight = m_mafDataPoints[i].Frequency + (uint)(m_MAFFrequencyStep / 4);
#endif

                        m_mafDataPoints[i].AirFlowRight = m_mafDataPoints[i].AirFlow + (slopeRight * (m_MAFFrequencyStep * 0.25f));
                    }

                    if (Left >= 0)
                    {
                        double slopeLeft = (m_mafDataPoints[i].AirFlow - m_mafDataPoints[Left].AirFlow)
                                         / (m_mafDataPoints[i].Frequency - m_mafDataPoints[Left].Frequency);
#if DEBUG
                        m_mafDataPoints[i].FrequencyLeft = m_mafDataPoints[i].Frequency - (uint)(m_MAFFrequencyStep / 4);
#endif
                        m_mafDataPoints[i].AirFlowLeft = m_mafDataPoints[i].AirFlow - (slopeLeft * (m_MAFFrequencyStep * 0.25f));
                    }
                }
            }

            // Since there is no data point before or after the endpoints of the array, we can't calculate a slope, we will set them to NaN.
            //  Since we are unlikely to get any data here it won't make any difference.
            //
            m_mafDataPoints[0].AirFlowLeft = double.NaN;
            m_mafDataPoints[m_mafFrequencyCount - 1].AirFlowRight = double.NaN;

            return;
        }

        public void ReadAdjustmentDataFromTuneObject(
                    TuneCycle Tune
                    )

        {
            for (int i = 0; i < m_mafDataPoints.Length; i++)
            {
                ref MafDataPoint Current = ref m_mafDataPoints[i];

                Current.AirFlowAdjusted = 0.0f;

                // read the adjustment data out of the tune cycle object and populate the MafDataPoint array with the adjustment values for each data point, based on the defined bucket style (single or double).
                //
                if (m_BucketStyle == BucketStyleEnum.Single)
                {
                    Current.AirFlowAdjustment = Tune.GetAdjustmentDataAtIndex(i);
                }
                else
                {
                    Current.AirFlowLeftAdjustment = Tune.GetAdjustmentDataAtIndex(i * 2);
                    Current.AirFlowRightAdjustment = Tune.GetAdjustmentDataAtIndex((i * 2) + 1);

                }
            }
        }

        public void ProcessAdjustmentData(
            double AdjustmentPercent
            )
        {
            int FirstUpdatedBucketIndex = -1;
            int LastUpdatedBucketIndex = -1;



            // This method can be used to process the adjustment data if needed, such as applying additional transformations or validations before updating the adjusted airflow values.
            // For now, the processing is done directly in the button1_Click event handler, but this method can be called from there if we want to separate concerns and keep the event handler cleaner.

            double[] AdjustedAirflowArray = new double[m_mafDataPoints.Length];

            for (int i = 0; i < m_mafDataPoints.Length; i++)
            {
                ref MafDataPoint Current = ref m_mafDataPoints[i];

                Current.AirFlowAdjusted = 0.0;
                Current.AirFlowLeftAdjusted = 0.0;
                Current.AirFlowRightAdjusted = 0.0;
                Current.HasUpdatedAirFlow = false;

                if (m_BucketStyle == BucketStyleEnum.Single)
                {
                    //
                    //   single bucket, just apply the adjustment to the orignal airflow
                    //
                    double ModifiedAdjustmentPercent = Current.AirFlowAdjustment * AdjustmentPercent;

                    Current.AirFlowAdjusted = Current.AirFlow * (1.0 + (ModifiedAdjustmentPercent / 100.0));
                }
                else
                {
                    //  For double bucket, we need to calculate the adjusted airflow for the left and right sides separately, and then average them to get the final adjusted airflow value for the bucket
                    //  This allows for more granular adjustments to the airflow values, as the user can specify different adjustments for the left and right sides of the bucket
                    //

                    double ModifiedAdjustmentPercentLeft = Current.AirFlowLeftAdjustment * AdjustmentPercent;
                    double ModifiedAdjustmentPercentRight = Current.AirFlowRightAdjustment * AdjustmentPercent;

                    Current.AirFlowLeftAdjusted = Current.AirFlowLeft + (Current.AirFlowLeft * (ModifiedAdjustmentPercentLeft / 100.0));
                    Current.AirFlowRightAdjusted = Current.AirFlowRight + (Current.AirFlowRight * (ModifiedAdjustmentPercentRight / 100.0));

                    //
                    // since the left and right values are an equal distance from the target frequency, just average them together
                    //
                    Current.AirFlowAdjusted = (Current.AirFlowLeftAdjusted + Current.AirFlowRightAdjusted) / 2.0f;

                }

                //  
                //  if the adjusted airflow is NaN, it means we got not updated data from the scanner app. THis means no datapoints were collected
                //  for this frequency. '
                //  
                //  we will scan through the array to find the first and last frequncies with valid data. No adjustments will be applied to the value before and after
                //
                Current.HasUpdatedAirFlow = !double.IsNaN(Current.AirFlowAdjusted);

                if ((FirstUpdatedBucketIndex == -1) && !double.IsNaN(Current.AirFlowAdjusted))
                {
                    //
                    //  Set the first updated bucket index to the first index where we have a valid adjusted airflow value
                    //
                    FirstUpdatedBucketIndex = i;
                }

                if (!double.IsNaN(Current.AirFlowAdjusted))
                {
                    //
                    // Set the last updated bucket index to the last index where we have a valid adjusted airflow value
                    //
                    LastUpdatedBucketIndex = i;
                }
            }

            //
            //  Ajusted Airflow has been computed for the whole array;
            //   
            //  Now we will deal with parts that did not get updated air flow data
            //  Basically, just use the original data upto the first update value, Same after the last update value.
            //  If there are missing data points inbetween udated data points, interpolate
            //
            if (FirstUpdatedBucketIndex != -1)
            {
                //  We have at least one updated bucket, so we can adjust the airflow values for all buckets between the first and last updated bucket
                //

                for (int i = 0; i < FirstUpdatedBucketIndex; i++)
                {
                    ref MafDataPoint Current = ref m_mafDataPoints[i];
                    //
                    //  If the current index is less than the first updated bucket index, set the adjusted airflow to the original airflow value of the first updated bucket
                    //
                    Current.AirFlowAdjusted = Current.AirFlow;

                }

            }

            if (LastUpdatedBucketIndex != -1)
            {
                //
                //  We have atleast one updated data point, and we are at that index, just put the original flow back.
                //
                for (int i = LastUpdatedBucketIndex + 1; i < m_mafFrequencyCount; i++)
                {
                    ref MafDataPoint Current = ref m_mafDataPoints[i];

                    //
                    //  If the current index is greater than the last updated bucket index, set the adjusted airflow to the original airflow value of the last updated bucket
                    //
                    Current.AirFlowAdjusted = Current.AirFlow;
                }
            }

            //  now we will go through the region with some updated data points
            //  if there are gaps, interpolate between updated data points
            //
            for (int i = (FirstUpdatedBucketIndex + 1); i <= LastUpdatedBucketIndex; i++)
            {

                if (!m_mafDataPoints[i].HasUpdatedAirFlow)
                {
                    //  If we have a bucket that doesn't have an updated airflow value, but is between the first and last updated bucket, we need to interpolate the adjusted airflow value for that bucket
                    //
                    int j = i;
                    //  Find the next bucket index that has an updated airflow value
                    //
                    while (j <= LastUpdatedBucketIndex && !m_mafDataPoints[j].HasUpdatedAirFlow)
                    {
                        j++;
                    }
                    if (j > LastUpdatedBucketIndex)
                    {
                        //  We didn't find any more buckets with updated airflow values, so we can break out of the loop
                        //
                        break;
                    }

                    //  i is the data point with missing updated airflow value, i-1 is the last data point with an updated airflow value, and j is the next data point with an updated airflow value. We can use these three points to interpolate the adjusted airflow value for the current bucket at index i.    
                    //  We will use linear interpolation to calculate the adjusted airflow value for the current bucket at index i, based on the adjusted airflow values of the buckets at index i-1 and j, and the frequencies of those buckets.   

                    double slope = (m_mafDataPoints[j].AirFlowAdjusted - m_mafDataPoints[i - 1].AirFlowAdjusted)
                                   / (m_mafDataPoints[j].Frequency - m_mafDataPoints[i - 1].Frequency);


                    for (int k = i; k < j; k++)
                    {
                        m_mafDataPoints[k].AirFlowAdjusted = m_mafDataPoints[i - 1].AirFlowAdjusted
                                                             + (slope * (m_mafDataPoints[k].Frequency - m_mafDataPoints[i - 1].Frequency));

                    }

                    //  Move the index i to the next bucket index with an updated airflow value, which is j, so that we can continue processing the next buckets with missing updated airflow values
                    //

                    i = j;

                }
            }



            return;
        }

        public ReturnDataPoint GetDataPointAtIndex(int i)
        {
            ReturnDataPoint ReturnValues;

            ReturnValues.Frequency = m_mafDataPoints[i].Frequency;
            ReturnValues.Airflow = m_mafDataPoints[i].AirFlow;
            ReturnValues.AdjustedAirflow = m_mafDataPoints[i].AirFlowAdjusted;
            ReturnValues.HasUpdatedAirflow = m_mafDataPoints[i].HasUpdatedAirFlow;

            return ReturnValues;
        }

        public int GetFrequencyCount()
        {
            return m_mafDataPoints.Length;
        }

        public int GetBucketCount()
        {
            return (int)m_BucketCount;
        }
    }
}
