package com.github.utransnet.utranscalc.server;

import com.github.utransnet.utranscalc.EnvelopeOuterClass;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.SimpleChannelInboundHandler;
import lombok.extern.slf4j.Slf4j;

import java.util.concurrent.ExecutorService;

/**
 * Created by Artem on 23.05.2018.
 */
@Slf4j
public class ProtocolServerHandler  extends SimpleChannelInboundHandler<EnvelopeOuterClass.Envelope> {

    private final ExecutorService executorService;

    ProtocolServerHandler(ExecutorService executorService) {
        this.executorService = executorService;
    }

    @Override
    protected void channelRead0(ChannelHandlerContext ctx, EnvelopeOuterClass.Envelope msg)
            throws Exception {

        // Don't block netty threads
        executorService.execute(() -> {
            log.info(msg.getName());
            EnvelopeOuterClass.Envelope.Builder builder = EnvelopeOuterClass.Envelope.newBuilder();
            builder.setId(0);
            builder.setName("response");
            ctx.writeAndFlush(builder.build());
        });
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

}