using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;

namespace DataAPI.Controller
{
    public class MQTTController : ApiController
    {
        const string mainTopic = "datn/pump/state";

        MqttClient m_mqttClient = new MqttClient("broker.emqx.io");
        string m_topic { get; set; }
        string m_lastSubMessage { get; set; }

        public MQTTController()
        {
            m_topic = mainTopic;
            m_lastSubMessage = string.Empty;

            Start();
        }

        private void Start()
        {
            string clientId = Guid.NewGuid().ToString();
            m_mqttClient.Connect(clientId);
            Debug.WriteLine("[MQTT] Connected successfully to {0} with clientID = {1}", "broker.emqx.io", clientId); ;

            if(m_topic == null) 
            {
                Debug.WriteLine("[MQTT] No topic to subcribe");
            }
            else
            {
                m_mqttClient.Subscribe(new string[] { m_topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
                Debug.WriteLine(String.Format("[MQTT] Subcribed to topic {0}", m_topic));
                m_mqttClient.MqttMsgPublishReceived += ReceiveMessageHandler;
            }
        }

        void ReceiveMessageHandler(object sender, MqttMsgPublishEventArgs e)
        {
            string _message = System.Text.Encoding.Default.GetString(e.Message);
            if(_message == null) 
            {
                // do nothing
            }
            else 
            {
                m_lastSubMessage = _message;
                Debug.WriteLine(String.Format("[MQTT] Received: {0}", m_lastSubMessage));    
            }

        }

    }

}
