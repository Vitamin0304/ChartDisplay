
--print(wifi.sta.getip())
--wifi.setmode(wifi.STATION)
--cfg={}
--cfg.ssid="Xiaomi_8B3A"
--cfg.pwd="19741226"
--wifi.sta.config(cfg)
--print(wifi.sta.getip())


--body = [[{"time":8,"data":[4,6,7,8]}]]
--
--http.post('https://192.168.31.94:44376/api/chart',
--   'Content-Type: application/json\r\n',
--    body,
--    function(code, data)
--        if (code < 0) then
--            print("HTTP request failed")
--        else
--            print(code, data)
--        end
--    end
--)

--http服务器，接收数据
function start_server()
    dofile('httpServer.lua')
    httpServer:listen(80)
    
    httpServer:use('/welcome', function(req, res)
        res:send('Hello ' .. req.query.name)
    end)
    httpServer:use('/set', function(req, res)
        body = string.match(req.source,"%[.+%]")
        if(body ~= nil) then
            data = sjson.decode(body)
            print('body',body)
            print(data[1])
            res:send('{"success":true}')
        else
            res:send('{"success":false}')
        end
    end)
    print('HttpServer setup complete.')
end

tmr.create():alarm(1000,1,function(cb_timer)
    if wifi.sta.getip() == nil then 
        print("Connecting...")
    else
        cb_timer:unregister()
        tmr.create():alarm(1000,tmr.ALARM_SINGLE,start_server)
    end
end)
