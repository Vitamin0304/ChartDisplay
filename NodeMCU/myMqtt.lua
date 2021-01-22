m = mqtt.Client("node_mcu", 120, "NodeMCU", "bohan0304")
subTopic = "/data/set"
pubTopic = "/data/chart"

--MQTT连接
function handle_mqtt_error(client, reason)
    tmr.create():alarm(5 * 1000, tmr.ALARM_SINGLE, do_mqtt_connect)
    print("MQTT is connecting...")
end

function do_mqtt_connect()
    m:connect("192.168.31.94", function(client) 
        print("MQTT connected.") 
        m:subscribe(subTopic, 1, function(conn) print("subscribe success") end)
    end, handle_mqtt_error)
end

m:on("message", function(client, topic, data)
    if(topic == subTopic) then
        if data ~= nil then
        print(data)
        end
    end
end)

function mqttPublishChartData(data)
     m:publish(pubTopic, data, 1, 0, function(client) print("sent") end)
end

tmr.create():alarm(1000,1,function(cb_timer)
    if wifi.sta.getip() == nil then 
        print("Wifi is connecting...")
    else
        cb_timer:unregister()
        tmr.create():alarm(1000,tmr.ALARM_SINGLE,do_mqtt_connect)
    end
end)

