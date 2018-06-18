package com.github.utransnet.utranscalc.server.data;

import com.github.utransnet.utranscalc.Protos;
import io.netty.channel.ChannelHandlerContext;
import io.netty.handler.codec.MessageToMessageDecoder;

import java.util.List;

/**
 * Created by Artem on 07.06.2018.
 */
public class EnvelopeEncoder extends MessageToMessageDecoder<Protos.Envelope> {
    @Override
    protected void decode(ChannelHandlerContext ctx, Protos.Envelope msg, List<Object> out) throws Exception {

    }
}
