using ChartDisplay.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Ports;
using RJCP.IO.Ports;
using ChartDisplay.Models;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;

namespace ChartDisplay.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SerialPortController : ControllerBase
    {
        public static SerialPortStream serialPort = null;
        private static IHubContext<ChartDataHub> _hub = null;

        public static void Init(IHubContext<ChartDataHub> hub)
        {
            _hub = hub;
            serialPort = new SerialPortStream("COM4", 115200, 8, Parity.None, StopBits.One);
            serialPort.DataReceived += DataReceivedHandler;
        }

        private static byte[] rxBuff = new byte[100];
        private static int rxLength = 0;
        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            int len = serialPort.BytesToRead;
            if (len > 0)
            {
                byte[] rec = new byte[len];
                serialPort.Read(rec, 0, len);

                foreach (byte b in rec)
                {
                    if(ReceiveJson(b) == -1)
                    {
                        ChartDisplayData chartDisplayData = JsonConvert.DeserializeObject<ChartDisplayData>(Encoding.UTF8.GetString(rxBuff, 0, rxLength));
                        _hub.Clients.All.SendAsync("ReceiveChartDisplayData", chartDisplayData);
                    }
                }
            }
        }
        static int step = 0;
        private static int ReceiveJson(byte rxChar)
        {
            switch (step)
            {
                case 0:
                    rxLength = 0;
                    if (rxChar == (byte)'{')
                    {
                        rxBuff[rxLength++] = rxChar;
                        step++;
                    }
                    break;
                case 1:
                    rxBuff[rxLength++] = rxChar;
                    if (rxChar == (byte)'}')
                    {
                        step = 0;
                        return -1;
                    }
                    break;
                default:
                    break;
            }
            return step;
        }

        [HttpGet]
        public ActionResult<string> Open()
        {
            if(serialPort.IsOpen)
            {
                return "{\"open\":true}";
            }
            else
            {
                try
                {
                    serialPort.Open();
                    return "{\"open\":true}";
                }
                catch(InvalidOperationException e)
                {
                    return "{\"open\":false}";
                }
            }
        }

        [HttpGet]
        public ActionResult<string> Close()
        {
            serialPort.Close();
            return "{\"open\":false}";
        }
        [HttpPost]
        public async Task<ActionResult<string>> SetData(DataSet dataSet)
        {
            if (serialPort.IsOpen)
            {
                string[] bodyArray = Array.ConvertAll(dataSet.Data, s => s.ToString());
                await serialPort.WriteAsync(Encoding.UTF8.GetBytes(string.Concat("[", string.Join(",", bodyArray), "]")));
                return await Task.FromResult("Data have been sent.");
            }
            else
            {
                return await Task.FromResult("Serial Port is closed.");
            }
        }
    }
}
