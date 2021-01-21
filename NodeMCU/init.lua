
print(wifi.sta.getip())
wifi.setmode(wifi.STATION)
cfg={}
cfg.ssid="xxxxx"
cfg.pwd="xxxx"
wifi.sta.config(cfg)
--print(wifi.sta.getip())
--Content-Type: application/x-www-form-urlencoded
--Content-Type: application/json

body = [[{"time":8,"data":[4,6,7,8]}]]

gpio.mode(4, gpio.OUTPUT)
gpio.write(4, gpio.HIGH)
--
--function PostHttp(body)
--    http.post('https://192.168.31.94:44376/api/chart',
--        'Content-Type: application/json\r\n',
--        body,
--        function(code, data)
--            if (code < 0) then
--                print("failed\r\n")
--            else
--                print("succeeded\r\n")
--            end
--        end)
--end

dofile('myMqtt.lua')
dofile('myUART.lua')

--http服务器，接收数据
--function start_server()
--    dofile('httpServer.lua')
--    httpServer:listen(80)
--    
--    httpServer:use('/welcome', function(req, res)
--        res:send('Hello ' .. req.query.name)
--    end)
--    httpServer:use('/set', function(req, res)
--        body = string.match(req.source,"%[.+%]")
--        if(body ~= nil) then
----            data = sjson.decode(body)
--            print(body)
--            res:send('{"success":true}')
--        else
--            res:send('{"success":false}')
--        end
--    end)
--    print('HttpServer setup complete.')
--end


