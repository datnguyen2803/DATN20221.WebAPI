﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using DataAPI.Models;

namespace DataAPI.Controller
{
    public class MQTTController : ApiController
    {
        const string MQTTBroker = "broker.hivemq.com";
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
            
            int stationId = RetrieveStationId(stationName);
            if(stationId == 0) 
            {
                // not found any station
                return null;
            }

            return new PumpModel()
            {
                Id = 0,
                StationId = stationId,
                Position = pumpPosition,
                State = pumpState
            };

        }

        private int RetrieveStationId(string StationName)
        {
            var myEntity = new DATNDBEntities();
            var retStation = myEntity.StationTables.Include("Id")
                                      .Where(station => station.Name == StationName)
                                      .Select(station => new StationModel()
                                      {
                                          Id = station.Id,
                                          Name = station.Name,
                                          Address = station.Address
                                      }).FirstOrDefault<StationModel>();

            if (retStation == null)
            {
                return 0;
            }
            else
            {
                return retStation.Id;
            }
        }

        private bool UpdateToDB(PumpModel checkPump)
        {
            var myEntity = new DATNDBEntities();
            var oldPump = myEntity.PumpTables
                    .Where(pump => (pump.StationId == checkPump.StationId && pump.Position == checkPump.Position))
                    .FirstOrDefault<PumpTable>();

            if (oldPump != null)
            {
                oldPump.State = checkPump.State;
                myEntity.SaveChanges();
                Debug.WriteLine("Update successful!");

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
