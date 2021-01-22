gpio.mode(1, gpio.INPUT, gpio.PULLUP)

tmr.create():alarm(5000, tmr.ALARM_SINGLE, function()
    if(gpio.read(1) == 0) then 
        uart.alt(1)
        uart.setup(0,115200,8,uart.PARITY_NONE, uart.STOPBITS_1, 1)
        uart.on("data","\r",function(data)
            mqttPublishChartData(data)
            --print("receive from uart:", data)
        end,0)
        --print("uart work")
        gpio.write(4, gpio.LOW)
    else
        uart.alt(0)
        uart.setup(0, 115200, 8, uart.PARITY_NONE, uart.STOPBITS_1, 1)
        print("debug")
        uart.on("data")
    end
end)
