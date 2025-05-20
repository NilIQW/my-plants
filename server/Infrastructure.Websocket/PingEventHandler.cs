using Fleck;
using WebSocketBoilerplate;

namespace Infrastructure.Websocket;

public class PingDto : BaseDto{}

public class PongDto : BaseDto;

public class PingEventHandler : BaseEventHandler<PingDto>
{
    public async override Task Handle(PingDto dto, IWebSocketConnection socket)
    {
        socket.SendDto(new PongDto());
        
    }
}