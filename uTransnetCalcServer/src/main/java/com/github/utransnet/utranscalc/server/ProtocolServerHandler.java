package com.github.utransnet.utranscalc.server;

import com.github.utransnet.utranscalc.Protos;
import com.github.utransnet.utranscalc.server.transport.MissingArgumentException;
import com.github.utransnet.utranscalc.server.transport.NetworkFacade;
import com.github.utransnet.utranscalc.server.transport.UnsupportedTypeException;
import com.github.utransnet.utranscalc.server.transport.messages.Message;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.SimpleChannelInboundHandler;
import lombok.extern.slf4j.Slf4j;

import java.util.concurrent.ExecutorService;

import static com.github.utransnet.utranscalc.Protos.Envelope;
import static com.github.utransnet.utranscalc.Protos.Request;

/**
 * Created by Artem on 23.05.2018.
 */
@Slf4j
public class ProtocolServerHandler extends SimpleChannelInboundHandler<Request> {

    private final ExecutorService executorService;
    private final NetworkFacade networkFacade;

    ProtocolServerHandler(ExecutorService executorService, NetworkFacade networkFacade) {
        this.executorService = executorService;
        this.networkFacade = networkFacade;
    }

    @Override
    protected void channelRead0(ChannelHandlerContext ctx, Request msg)
            throws Exception {

        log.info("Processing request: " + msg.getType().name());
        // Don't block netty threads
//        executorService.execute(() -> {
            try {

                Envelope.Builder builder = Envelope.newBuilder();
                builder.setType(msg.getType());
                Request.OptionalId idArg = msg.getPresetId();
                switch (msg.getType()) {
                    case listPresets:
                        networkFacade.listPresets()
                                .stream().map(Message::toProto).forEach(builder::addPresets);
                        break;
                    case getPresetMaterials:
                        checkArg(msg.getType(), idArg, "PresetId");
                        networkFacade.getPresetMaterials(idArg.getId())
                                .stream().map(Message::toProto).forEach(builder::addPresetMaterials);
                        break;
                    case listBaseObjects:
                        networkFacade.listBaseObjects()
                                .stream().map(Message::toProto).forEach(builder::addBaseObjects);
                        break;
                    case getRequiredMaterialsForBaseObject:
                        checkArg(msg.getType(), idArg, "PresetId");
                        networkFacade.getRequiredMaterialsForBaseObject(idArg.getId())
                                .stream().map(Message::toProto).forEach(builder::addRequiredMaterials);
                        break;
                    case listMaterials:
                        networkFacade.listMaterials()
                                .stream().map(Message::toProto).forEach(builder::addMaterials);
                        break;
                    default:
                        throw new UnsupportedTypeException(msg.getType());
                }

                ctx.writeAndFlush(builder.build());

            } catch (UnsupportedTypeException e) {
                log.error(e.getMessage());
                //TODO: use errors enum
                ctx.writeAndFlush(Protos.Error.newBuilder().setId(1).setMessage(e.getMessage()));
            } catch (MissingArgumentException e) {
                log.error(e.getMessage());
                ctx.writeAndFlush(Protos.Error.newBuilder().setId(2).setMessage(e.getMessage()));
            }
//        });
    }

    @Override
    public void channelReadComplete(ChannelHandlerContext ctx) {
        ctx.flush();
    }

    @Override
    public void exceptionCaught(ChannelHandlerContext ctx, Throwable cause) {
        cause.printStackTrace();
        ctx.close();
    }

    private void checkArg(Protos.Type type, Request.OptionalId optionalId, String argName) throws MissingArgumentException {
        if (!optionalId.getIsSet()) {
            throw new MissingArgumentException(type, argName);
        }
    }

}