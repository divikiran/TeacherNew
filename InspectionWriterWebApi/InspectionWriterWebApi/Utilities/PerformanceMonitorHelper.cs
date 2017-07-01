using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace InspectionWriterWebApi.Utilities
{

    public class PerformanceMonitorHelper
    {

        // http://stackoverflow.com/questions/8657470/how-to-know-the-network-bandwidth-used-at-a-given-time
        public double GetNetworkUsage(string networkCard)
        {
            const int numberOfIterations = 10;

            PerformanceCounter bandwidthCounter = new PerformanceCounter("Network Interface", "Current Bandwidth", networkCard);
            float bandwidth = bandwidthCounter.NextValue();//valor fixo 10Mb/100Mn/

            PerformanceCounter dataSentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", networkCard);

            PerformanceCounter dataReceivedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", networkCard);

            float sendSum = 0;
            float receiveSum = 0;

            for (int index = 0; index < numberOfIterations; index++)
            {
                sendSum += dataSentCounter.NextValue();
                receiveSum += dataReceivedCounter.NextValue();
            }
            float dataSent = sendSum;
            float dataReceived = receiveSum;


            double utilization = (8 * (dataSent + dataReceived)) / (bandwidth * numberOfIterations) * 100;
            return utilization;
        }

        // http://stackoverflow.com/questions/8657470/how-to-know-the-network-bandwidth-used-at-a-given-time
        public List<string> GetAllNetworkCardNames()
        {
            var result = new List<string>();

            PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");
            String[] instancename = category.GetInstanceNames();

            foreach (string name in instancename)
            {
                result.Add(name);
            }

            return result;
        }

        // http://stackoverflow.com/questions/105031/how-do-you-get-total-amount-of-ram-the-computer-has
        public double GetMemoryUsage()
        {
            // this is returning the memory "usage"
            // using VB is the easiest and safest way to get this info in C# ... :-(
            var ci = new Microsoft.VisualBasic.Devices.ComputerInfo();

            ulong physicalMemory = ulong.Parse(ci.TotalPhysicalMemory.ToString());
            ulong available = ulong.Parse(ci.AvailablePhysicalMemory.ToString());

            return (1f - ((float)available / (float)physicalMemory)) * 100f;
        }

        // http://stackoverflow.com/questions/278071/how-to-get-the-cpu-usage-in-c
        public double GetCPUUsage()
        {
            var cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            float value = 0f;

            for (int tries = 0; tries < 3; tries++)
            {
                value = cpuCounter.NextValue();

                if (value.ToString() != "0")
                    return value;
                else
                    System.Threading.Thread.Sleep(333);
            }

            // now matches task manager reading
            return value;
        }

    }

}