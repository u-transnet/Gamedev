package com.github.utransnet.utranscalc.server;


import com.github.utransnet.utranscalc.Protos;
import com.github.utransnet.utranscalc.server.transport.NetworkFacade;
import io.netty.channel.ChannelInitializer;
import io.netty.channel.ChannelPipeline;
import io.netty.channel.socket.SocketChannel;
import io.netty.handler.codec.protobuf.ProtobufDecoder;
import io.netty.handler.codec.protobuf.ProtobufEncoder;
import io.netty.handler.codec.protobuf.ProtobufVarint32FrameDecoder;
import io.netty.handler.codec.protobuf.ProtobufVarint32LengthFieldPrepender;

import java.util.concurrent.ExecutorService;

/**
 * Created by Artem on 23.05.2018.
 */
public class ServerChannelInitializer extends ChannelInitializer<SocketChannel> {

    private final ExecutorService executorService;
    private final NetworkFacade networkFacade;

    ServerChannelInitializer(ExecutorService executorService, NetworkFacade networkFacade) {
        this.executorService = executorService;
        this.networkFacade = networkFacade;
    }

    @Override
    protected void initChannel(SocketChannel ch) throws Exception {
        ChannelPipeline p = ch.pipeline();
        p.addLast(new ProtobufVarint32FrameDecoder());
        p.addLast(new ProtobufDecoder(Protos.Request.getDefaultInstance()));

        p.addLast(new ProtobufVarint32LengthFieldPrepender());
        p.addLast(new ProtobufEncoder());

        p.addLast(new ProtocolServerHandler(executorService, networkFacade));
    }
}