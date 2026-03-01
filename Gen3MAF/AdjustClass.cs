using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
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

    public struct AdjustmentDataPoint
    {
        public uint   TargetFrequency;
        public uint   BucketStart;
        public uint   BucketEnd;
        public double Airflow;
        public double AirFlowAdjustment;
        public double AirFlowAdjusted;
    }

    public struct MafDataPoint
    {
        public uint Frequency;
        public double AirFlow;
        public double Bias;
        public double LeftRightAverageAirflowAdjusted;
        public bool BelowThreshold;

        public uint LeftFrequency;
        public uint RightFrequency;
        public double LeftAirFlow;  // this is used for calculating the slope of the airflow values, which is used to determine the left and right bucket airflow values for the double and triple bucket styles. This field is used to store the original airflow value for the previous data point, which is needed to calculate the slope of the airflow values between the current data point and the previous data point. This slope is then used to calculate the left and right bucket airflow values for the double and triple bucket styles, based on the distance from the target frequency.    
        public double RightAirFlow; // this is used for calculating the slope of the airflow values, which is used to determine the left and right bucket airflow values for the double and triple bucket styles. This field is used to store the original airflow value for the next data point, which is needed to calculate the slope of the airflow values between the current data point and the next data point. This slope is then used to calculate the left and right bucket airflow values for the double and triple bucket styles, based on the distance from the target frequency.   
        public double LeftAirFlowSlope;
        public double RightAirFlowSlope;

        public AdjustmentDataPoint[] DataPoints;

        public bool HasUpdatedAirFlow;

        public double AirFlowAdjusted;

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
            else if (m_BucketStyle == BucketStyleEnum.Triple)
            {
                m_BucketCount = (uint)m_mafFrequencyCount * 3;
            }
            else
            {
                Debug.Assert(false, "unknown bucket type");

                return;
            }

            m_mafDataPoints = new MafDataPoint[m_mafFrequencyCount];

            for (uint i = 0; i < m_mafFrequencyCount; i++)
            {
                m_mafDataPoints[i].Frequency = (uint)(m_MinMAFFrequency + (i * m_MAFFrequencyStep));
                m_mafDataPoints[i].AirFlow = double.NaN; // Placeholder for actual airflow values
                m_mafDataPoints[i].DataPoints = new AdjustmentDataPoint[(int)m_BucketStyle];
            }
        }

        public void InitializeAirFlowFromTuneObject(
                    TuneCycle Tune
                    )

        {
            //
            //  fill out the original airflow values from the current tune.
            //
            for (int i = 0; i < m_mafFrequencyCount; i++)
            {
                ref MafDataPoint Current = ref m_mafDataPoints[i];

                Current.AirFlow = Tune.GetAirflowAtIndex(i);

            }

            for (int i = 0; i < m_mafFrequencyCount; i++)
            {
                if (i > 0)
                {
                    double AirflowDelta = 0;
                    double FrequencyDelta = 0;
                    int LeftFrequency = (int)m_mafDataPoints[i - 1].Frequency;
                    int CurrentFrequency = (int)m_mafDataPoints[i].Frequency;

                    m_mafDataPoints[i].LeftFrequency = (uint)LeftFrequency;
                    m_mafDataPoints[i].LeftAirFlow = m_mafDataPoints[i - 1].AirFlow;

                    AirflowDelta = (m_mafDataPoints[i - 1].AirFlow - m_mafDataPoints[i].AirFlow);
                    FrequencyDelta = (double)(LeftFrequency) - CurrentFrequency;

                    m_mafDataPoints[i].LeftAirFlowSlope = AirflowDelta / FrequencyDelta;
                }

                if (i < (m_mafFrequencyCount - 1))
                {
                    m_mafDataPoints[i].RightFrequency = m_mafDataPoints[i + 1].Frequency;
                    m_mafDataPoints[i].RightAirFlow = m_mafDataPoints[i + 1].AirFlow;

                    m_mafDataPoints[i].RightAirFlowSlope = (m_mafDataPoints[i + 1].AirFlow - m_mafDataPoints[i].AirFlow)
                                                        / (m_mafDataPoints[i + 1].Frequency - m_mafDataPoints[i].Frequency);
                }

            }

            //  at the edge there is no adjacent node
            //
            m_mafDataPoints[0].LeftAirFlow = double.NaN;
            m_mafDataPoints[0].LeftAirFlowSlope = double.NaN;

            m_mafDataPoints[m_mafFrequencyCount - 1].RightAirFlow = double.NaN;
            m_mafDataPoints[m_mafFrequencyCount - 1].RightAirFlowSlope = double.NaN;


            for (int i = 0; i < (m_mafDataPoints.Length); i++)
            {
                ref MafDataPoint Current = ref m_mafDataPoints[i];
                double Step = (double)m_MAFFrequencyStep;
                double BucketSize = Step / Current.DataPoints.Length;
                double Frequency = (double)Current.Frequency;

                for (int j = 0; j < (Current.DataPoints.Length); j++)
                {

                    double BucketStart = (Frequency - (Step / 2.0)) + (BucketSize * j);
                    double BucketEnd = BucketStart + (BucketSize - 1);

                    double TargetFrequency = BucketStart + (BucketSize / 2.0);

                    double Slope = double.NaN;
                    double FrequencyDifference = TargetFrequency - Frequency;

                    Current.DataPoints[j].BucketStart = (uint)Math.Round(BucketStart);
                    Current.DataPoints[j].BucketEnd = (uint)Math.Round(BucketEnd);
                    Current.DataPoints[j].TargetFrequency = (uint)Math.Round(TargetFrequency);

                    if (Current.DataPoints[j].TargetFrequency < Current.Frequency)
                    {
                        Slope = Current.LeftAirFlowSlope;

                    } else if (Current.DataPoints[j].TargetFrequency > Current.Frequency)
                    {
                        Slope = Current.RightAirFlowSlope;
                    }
                    else
                    {
                        Slope = 1.0;
                    }

                    Current.DataPoints[j].Airflow = m_mafDataPoints[i].AirFlow + (Slope * FrequencyDifference);

                }

            }



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

                for (int j = 0; j < Current.DataPoints.Length; j++)
                {
                    Current.DataPoints[j].AirFlowAdjustment = Tune.GetAdjustmentDataAtIndex((i * Current.DataPoints.Length) + j);
                }


            }
        }

        public void ReadRawAirflowData(
            ref double[] Frequency,
            ref double[] Airflow,
            ref double[] AdjustedAirflow
            )
        {
            Frequency = new double[ m_mafDataPoints.Length * m_mafDataPoints[0].DataPoints.Length ];
            Airflow   = new double[m_mafDataPoints.Length * m_mafDataPoints[0].DataPoints.Length];
            AdjustedAirflow = new double[m_mafDataPoints.Length * m_mafDataPoints[0].DataPoints.Length];

            for (int i = 0; i < m_mafDataPoints.Length; i++)
            {
                ref MafDataPoint Current = ref m_mafDataPoints[i];

                //
                // read the adjustment data out of the tune cycle object and populate the MafDataPoint array with the adjustment values for each data point, based on the defined bucket style (single or double).
                //

                for (int j = 0; j < Current.DataPoints.Length; j++)
                {
                    Frequency[i * Current.DataPoints.Length + j] = Current.DataPoints[j].TargetFrequency;
                    Airflow[i * Current.DataPoints.Length + j] = Current.DataPoints[j].Airflow;
                    AdjustedAirflow[i * Current.DataPoints.Length + j] = Current.DataPoints[j].AirFlowAdjusted;

                }


            }
        }
           


        public void ProcessAdjustmentData(
            double AdjustmentPercent,
            double AdjustmentThreshold,
            bool InterpolateMissingData,
            double MinFrequency,
            double MaxFrequency
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

                Current.BelowThreshold = false;
                Current.AirFlowAdjusted = double.NaN;
                Current.HasUpdatedAirFlow = false;

                //
                // Adjust the airflow for each bucket, applying overall reduction value'
                //
                for (int j = 0; j < Current.DataPoints.Length; j++)
                {
                    double ModifiedAdjustmentPercent = Current.DataPoints[j].AirFlowAdjustment * AdjustmentPercent;

                    Current.DataPoints[j].AirFlowAdjusted = Current.DataPoints[j].Airflow * (1.0 + (ModifiedAdjustmentPercent / 100.0));
                }

                if (m_BucketStyle == BucketStyleEnum.Single)
                {
                    //
                    //   single bucket, just apply the adjustment to the orignal airflow
                    //
                    Current.AirFlowAdjusted = Current.DataPoints[0].AirFlowAdjusted;

                }
                else if (m_BucketStyle == BucketStyleEnum.Double)
                {
                    //
                    // since the left and right values are an equal distance from the target frequency, just average them together
                    //
                    Current.AirFlowAdjusted = (Current.DataPoints[0].AirFlowAdjusted + Current.DataPoints[1].AirFlowAdjusted) / 2.0f;

                }
                else if (m_BucketStyle == BucketStyleEnum.Triple)
                {
                    ProcessTripleBucket(ref Current);

                }


                if (Current.Frequency < MinFrequency || Current.Frequency > MaxFrequency)
                {
                    //
                    // everything not in range is set to NaN
                    //
                    Current.AirFlowAdjusted = double.NaN;
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
                        if (InterpolateMissingData)
                        {
                            m_mafDataPoints[k].AirFlowAdjusted = m_mafDataPoints[i - 1].AirFlowAdjusted
                                                             + (slope * (m_mafDataPoints[k].Frequency - m_mafDataPoints[i - 1].Frequency));
                        }
                        else
                        {
                            m_mafDataPoints[k].AirFlowAdjusted = m_mafDataPoints[k].AirFlow;
                        }
                    }
                

                    //  Move the index i to the next bucket index with an updated airflow value, which is j, so that we can continue processing the next buckets with missing updated airflow values
                    //

                    i = j;
                }
            }
        
    
            

            //  
            //  now we will go through the list checking to see if the change was above the thresh hold value, it not we but the original airflow back;
            //


            for (int i = 0; i < m_mafDataPoints.Length; i++)
            {
                ref MafDataPoint Current = ref m_mafDataPoints[i];
                double ChangeAmountPercent = ((Current.AirFlowAdjusted - Current.AirFlow) / Current.AirFlow) * 100;

                if (Math.Abs(ChangeAmountPercent) < AdjustmentThreshold)
                {
                    Current.AirFlowAdjusted = Current.AirFlow;
                    Current.BelowThreshold = true;
                }
            }


                return;
        }

        void ProcessTripleBucket(
             ref MafDataPoint Current
            )
        {
            double Bias = (((Current.DataPoints[0].Airflow + Current.DataPoints[2].Airflow) / 2) - Current.DataPoints[1].Airflow) / Current.DataPoints[1].Airflow;
            Current.Bias = Bias;
            Current.LeftRightAverageAirflowAdjusted = (Current.DataPoints[0].AirFlowAdjusted + Current.DataPoints[2].AirFlowAdjusted) / 2;

            Current.LeftRightAverageAirflowAdjusted = Current.LeftRightAverageAirflowAdjusted - (Current.LeftRightAverageAirflowAdjusted * Bias);

            if (!double.IsNaN(Current.DataPoints[0].AirFlowAdjusted) &&
                !double.IsNaN(Current.DataPoints[1].AirFlowAdjusted) &&
                !double.IsNaN(Current.DataPoints[2].AirFlowAdjusted))
            {
                //
                //  we have all three buckets, take the average of the center and the adjusted average of the left and right values
                //
                Current.AirFlowAdjusted = (Current.DataPoints[1].AirFlowAdjusted + ( Current.LeftRightAverageAirflowAdjusted * 2 ) ) / 3;
            }
            else if (!double.IsNaN(Current.DataPoints[1].AirFlowAdjusted))
            {
                //
                // if we only have the center bucket, just use the center bucket adjustment value as the adjusted airflow value for the current data point
                //
                Current.AirFlowAdjusted = Current.DataPoints[1].AirFlowAdjusted;
            }
            else if (!double.IsNaN(Current.DataPoints[0].AirFlowAdjusted) && !double.IsNaN(Current.DataPoints[2].AirFlowAdjusted))
            {
                //
                // if we only have the left and right buckets, just average the left and right bucket adjustment values together to get the adjusted airflow value for the current data point
                //

                Current.AirFlowAdjusted = Current.LeftRightAverageAirflowAdjusted;
            }
            else
            {
                //
                // if we don't have any valid adjustment values for the left, center, or right buckets, then we will just leave the adjusted airflow value as 0 for now, and we will deal with this case later when we go through the array to check for any missing updated airflow values and fill them in accordingly.
                //
                Current.AirFlowAdjusted = double.NaN;
            }
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

        public MafDataPoint GetFullDataPointAtIndex(int i)
        {
            return m_mafDataPoints[i];
        }
    }
}
