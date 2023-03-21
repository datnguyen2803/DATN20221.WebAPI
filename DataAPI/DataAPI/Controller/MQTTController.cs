using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using DataAPI.Models;
using DataAPI.Common;

namespace DataAPI.Controller
{
    public class MQTTController : ApiController
    {
        const string MQTTBroker = "broker.emqx.io";
        const string mainTopic = "DATN.Pumpmonitor/Pump";

        MqttClient m_mqttClient = new MqttClient(MQTTBroker);
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
            Debug.WriteLine("[MQTT] Connected successfully to {0} with clientID = {1}", MQTTBroker, clientId); ;

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

            PumpModel myPump = MessageToPump(_message);
            if(myPump == null) 
            {
                //do nothing
            }
            else
            {
                bool res = UpdateToDB(myPump);
                Debug.WriteLine("res = " + res);
            }
            
        }

        private PumpModel MessageToPump(string message)
        {
            //PumpModel retPump= new PumpModel();

            if(message.Length != 11)
            {
                return null;
            }

            // 4 first char must = "pump"
            string keyword = message.Substring(0, 4);
            Debug.WriteLine(keyword);
            string stationName = message.Substring(5, 1);
            string pumpPosition = message.Substring(7, 2);
            int pumpState = Int32.Parse(message.Substring(message.Length-1));
            Debug.WriteLine(String.Format("After solve: {0} {1} {2} {3}", keyword, stationName, pumpPosition, pumpState));
            if(keyword != "pump")
            {
                // do nothing
                return null;
            }

            Guid stationId = PumpModel.RetrieveStationId(stationName);
            if(stationId == null) 
            {
                // not found any station
                return null;
            }

            return new PumpModel()
            {
                StationName = stationName,
                Position = pumpPosition,
                State = pumpState
            };

        }

        private bool UpdateToDB(PumpModel checkPump)
        {
            Guid stationId = PumpModel.RetrieveStationId(checkPump.StationName);
            var myEntity = new DATN2022DBEntities();
            var oldPump = myEntity.PumpTables
                    .Where(pump => (pump.StationId == stationId && pump.Position == checkPump.Position))
                    .FirstOrDefault();

            if (oldPump != null)
            {
                // update to PumpTable
                oldPump.State = checkPump.State;
                myEntity.SaveChanges();
                Debug.WriteLine("Update successful!");

                // add record to HistoryTable
                HistoryModel newHistory = new HistoryModel
                {
                    StationName = checkPump.StationName,
                    PumpPosition = checkPump.Position,
                    Date = DateTime.Now.ToLocalTime(),
                    Time = DateTime.Now.ToLocalTime().TimeOfDay,
                    State = checkPump.State
                };
                Guid myPumpId = PumpModel.RetrievePumpId(newHistory.StationName, newHistory.PumpPosition);
                HistoryTable historyTable = new HistoryTable
                {
                    Id = Guid.NewGuid(),
                    PumpId = myPumpId,
                    Date = newHistory.Date,
                    Time = newHistory.Time,
                    State = newHistory.State
                };

                myEntity.HistoryTables.Add(historyTable);
                myEntity.SaveChanges();

                return true;
            }
            else
            {
                Debug.WriteLine("Update fail!!!");
                return false;
            }

        }

    }

}
