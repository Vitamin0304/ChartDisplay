using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using MQTTnet.Adapter;
using MQTTnet.Diagnostics;
using MQTTnet.Client.Receiving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using ChartDisplay.Hubs;
using System.Runtime.Serialization.Json;
using System.IO;
using ChartDisplay.Models;

namespace ChartDisplay
{
    public class MyMqttServer
    {
        private static MqttServer mqttServer = null;
        private static IHubContext<ChartDataHub> _hub = null;

        public static async void StartMqttServer(IHubContext<ChartDataHub> hub)
        {
            _hub = hub;

            if (mqttServer == null)
            {
                string hostIP = "127.0.0.1";
                int hostPort = 1883;
                int timeOut = 120;
                string userName = "NodeMCU";
                string password = "bohan0304";

                var optionsBuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithDefaultEndpointPort(hostPort)
                .WithDefaultCommunicationTimeout(new TimeSpan(0,0,timeOut))
                .WithConnectionValidator(
                    c =>
                    {
                        if (c.Username != userName)
                        {
                            c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                            return;
                        }

                        if (c.Password != password)
                        {
                            c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                            return;
                        }

                        c.ReasonCode = MqttConnectReasonCode.Success;
                    }).WithSubscriptionInterceptor(
                    c =>
                    {
                        c.AcceptSubscription = true;
                    }).WithApplicationMessageInterceptor(
                    c =>
                    {
                        c.AcceptPublish = true;
                    });

                mqttServer = new MqttFactory().CreateMqttServer() as MqttServer;
                mqttServer.StartedHandler = new MqttServerStartedHandlerDelegate(OnMqttServerStarted);
                mqttServer.StoppedHandler = new MqttServerStoppedHandlerDelegate(OnMqttServerStopped);

                mqttServer.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(OnMqttServerClientConnected);
                mqttServer.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandlerDelegate(OnMqttServerClientDisconnected);
                mqttServer.ClientSubscribedTopicHandler = new MqttServerClientSubscribedHandlerDelegate(OnMqttServerClientSubscribedTopic);
                mqttServer.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandlerDelegate(OnMqttServerClientUnsubscribedTopic);
                mqttServer.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(OnMqttServer_ApplicationMessageReceived);


                await mqttServer.StartAsync(optionsBuilder.Build());
            }

        }
        public static async void PublishMqttTopic(string topic, string payload)
        {
            var message = new MqttApplicationMessage()
            {
                Topic = topic,
                Payload = Encoding.UTF8.GetBytes(payload),
                QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce
            };
            await mqttServer.PublishAsync(message);
        }
        public static void OnMqttServerStarted(EventArgs e)
        {
            Debug.WriteLine("MQTT服务启动完成！");
        }
        public static void OnMqttServerStopped(EventArgs e)
        {
            Debug.WriteLine("MQTT服务停止完成！");
        }
        public static void OnMqttServerClientConnected(MqttServerClientConnectedEventArgs e)
        {
            Debug.WriteLine($"客户端[{e.ClientId}]已连接");
        }

        public static void OnMqttServerClientDisconnected(MqttServerClientDisconnectedEventArgs e)
        {
            Debug.WriteLine($"客户端[{e.ClientId}]已断开连接！");
        }

        public static void OnMqttServerClientSubscribedTopic(MqttServerClientSubscribedTopicEventArgs e)
        {
            Debug.WriteLine($"客户端[{e.ClientId}]已成功订阅主题[{e.TopicFilter}]！");
        }
        public static void OnMqttServerClientUnsubscribedTopic(MqttServerClientUnsubscribedTopicEventArgs e)
        {
            Debug.WriteLine($"客户端[{e.ClientId}]已成功取消订阅主题[{e.TopicFilter}]！");
        }

        public static async Task OnMqttServer_ApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            string topic = e.ApplicationMessage.Topic;
            if (topic == "/data/chart")
            {
                ChartDisplayData chartDisplayData = JsonToObject<ChartDisplayData>(payload);
                await _hub.Clients.All.SendAsync("ReceiveChartDisplayData", chartDisplayData);
            }
            Debug.WriteLine($"客户端[{e.ClientId}]>> 主题：{topic} 负荷：{payload} Qos：{e.ApplicationMessage.QualityOfServiceLevel} 保留：{e.ApplicationMessage.Retain}");
        }

        public static T JsonToObject<T>(string jsonText)
        {
            DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonText));
            T obj = (T)s.ReadObject(ms);
            ms.Dispose();
            return obj;
        }
    }
}
